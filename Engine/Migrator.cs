using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Mappers;
using MigrationEngine.Tasks;

namespace MigrationEngine
{
    public class Migrator
    {
        public Migrator()
        {
        }

        //public void Save(string filePath)
        //{
        //    Migration migration = new Migration();

        //    Type[] typeList = GetTypeListForSerialization();

        //    XmlSerializer serializer = new XmlSerializer(typeof(Migration), typeList);
        //    using (TextWriter writer = new StreamWriter(filePath))
        //    {
        //        serializer.Serialize(writer, migration);
        //    }
        //}

        public void Run(string filePath, IMigrationLog logger)
        {
            // By providing the Serializer a list of classes, we're able to allow for the
            // use of subclasses without having to add a new XmlInclude attribute to the
            // superclass every time a new subclass is derived.  Determining the list
            // of classes to allow requires some work in reflection.
            Type[] typeList = GetTypeListForSerialization();

            Migration migration =null;

            XmlSerializer serializer = new XmlSerializer(typeof(Migration), typeList);
            using (TextReader reader = new StreamReader(filePath))
            {
                migration = (Migration)serializer.Deserialize(reader);
            }

            migration.Run(logger);
        }

        #region Here there be dragons.

        /// <summary>
        /// Uses reflection to traverse the list of types present in the Migration engine
        /// and discover the sub-classes which may be used for serialization.
        /// </summary>
        /// <returns></returns>
        private Type[] GetTypeListForSerialization()
        {
            Assembly assembly = typeof(Migrator).Module.Assembly;
            Type[] allTypes = assembly.GetTypes();

            List<Type> serializableTypes = new List<Type>();

            // Collect all the concrete types derived from MigrationTask.
            Type taskBase = typeof(MigrationTask);
            Array.ForEach(allTypes, testType =>
            {
                if (!testType.IsAbstract
                    && taskBase.IsAssignableFrom(testType))
                {
                    serializableTypes.Add(testType);
                }
            });

            // Collect all the concrete DataMapper types.
            Type mapperBase = typeof(DataMapper<>);
            Array.ForEach(allTypes, testType =>
            {
                // All the DataMappers in the initial implementation are actually
                // concrete classes derived from completed generics.  So technically,
                // they *could* be validated with IsAssignableFrom().  But there's no
                // reason a mapper couldn't be done with a generic (whether it would
                // make *sense* is another question), so we use IsDerivedFromGenericType()
                // in order future-proof this bit of code.
                if (!testType.IsAbstract
                    && IsDerivedFromGenericType(mapperBase, testType))
                {
                    serializableTypes.Add(testType);
                }
            });


            // Collect the list of concrete types derived from MigrationData.
            // These are the types returned by LoadData when a DataGetter type is
            // created, e.g. DatabaseDataGetter<FolderDescription>.
            Type migDataBase = typeof(MigrationData);
            Type[] migDataTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract
                    && migDataBase.IsAssignableFrom(testType);
            });

            // Get the list of classes which can be assigned to DataGetter<>.
            // This is complicated. See comments for IsDerivedFromGenericType().
            Type dataGetterBase = typeof(DataGetter<>);
            Type[] dataGetterTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract && testType.IsGenericTypeDefinition
                    && IsDerivedFromGenericType(dataGetterBase, testType);
            });

            // For every migratation data type
            // Create the cross-product of all the DataGetter generics with
            // all the MigrationData subclasses.
            foreach (Type dataType in migDataTypes)
            {
                foreach (Type getter in dataGetterTypes)
                {
                    Type constructedType = getter.MakeGenericType(new Type[] { dataType });
                    serializableTypes.Add(constructedType);
                }
            }

            return serializableTypes.ToArray();
        }


        // For non-generic types, finding out whether class B is a subclass class A
        // is a matter of simply calling IsAssignableFrom() on their types.  So for
        // example, given these declarations:
        // 
        //             Type a = typeof(Object);
        //             Type b = typeof(String);
        //             
        // Then a.IsAssignableFrom(b) will always return true, because everything
        // can be assigned to Object.
        // 
        // It isn't as simple for generics.
        // 
        // For these declarations:
        // 
        //              class generic1<T> { }
        //              class generic2<T> : generic1<T> { }
        //              
        //              Type g1 = typeof(generic1<>);
        //              Type g2 = typeof(generic2<>);
        //              
        // Calling g1.IsAssignableFrom(g2) will return false.  Completely non-intuitive
        // since generic2 is a subclass of generic1, but it always comes down to the details.
        // 
        // A *specific* use of generic1 might not use the parameter type as a specific
        // use of generic2.
        // 
        // So this case will work:
        // 
        //         class MyType { }
        //         generic1<MyType> var1;
        //         generic2<MyType> var2 = new generic2<MyType>();
        //         var1 = var2;
        //         
        // On the other hand, this case will fail at compile time.
        // 
        //         class MyType { }
        //         class MyOtherType { }
        //         generic1<MyType> var1;
        //         generic2<MyOtherType> var2 = new generic2<MyOtherType>();
        //         var1 = var2; //-----  This will break!
        // 
        // So we can't just use IsAssignableFrom().  Instead, we end up with this little
        // gem which checks whether the would-be suclass has the desired parent class.
        // 
        // Code derived from: http://stackoverflow.com/questions/5461295/using-isassignablefrom-with-generics
        // 
        // Also worth reading:
        // 
        //     Generics and Reflection (C# Programming Guide)
        //         ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/dv_csref/html/162fd9b4-dd5b-4abb-8c9b-e44e21e2f451.htm
        // 
        //     Overview of Reflection and Generics
        //         ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/dv_fxadvance/html/f7180fc5-dd41-42d4-8a8e-1b34288e06de.htm
        // 

        /// <summary>
        /// Determines whether testType contains a Type object which was
        /// derived from desiredBaseType. 
        /// </summary>
        /// <param name="desiredBaseType">Type object representing the desired
        /// base type.</param>
        /// <param name="testType">Type object representing a generic type, or a type
        /// derived from a generic type.</param>
        /// <returns>True if testType is derived from desiredBaseType. False otherwise</returns>
        private bool IsDerivedFromGenericType(Type desiredBaseType, Type testType)
        {
            var interfaceTypes = testType.GetInterfaces();

            // Is it a generic?
            // Is its Generic Type Definition the desired base type?
            // Yes to both: Return true.
            foreach (var it in interfaceTypes)
                if (it.IsGenericType)
                    if (it.GetGenericTypeDefinition() == desiredBaseType) return true;

            // Get the base type of the type being tested.
            // If there is no base type, we've gone all the way up to Object without
            // finding a match.  Return false.
            Type baseType = testType.BaseType;
            if (baseType == null) return false;

            // Recursively test the base type.
            return baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == desiredBaseType ||
                IsDerivedFromGenericType(desiredBaseType, baseType);
        }

        #endregion
    }
}
