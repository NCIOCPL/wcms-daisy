using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;
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
            Assembly engine = typeof(Migrator).Module.Assembly;
            Type[] allTypes = engine.GetTypes();

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
                if (!testType.IsAbstract
                    && IsAssignableToGenericType(mapperBase, testType))
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
            // This is complicated. See comments for IsAssignableToGenericType().
            Type dataGetterBase = typeof(DataGetter<>);
            Type[] dataGetterTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract && testType.IsGenericTypeDefinition
                    && IsAssignableToGenericType(dataGetterBase, testType);
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

        private bool IsAssignableToGenericType(Type parentType, Type testType)
        {
            var interfaceTypes = testType.GetInterfaces();

            foreach (var it in interfaceTypes)
                if (it.IsGenericType)
                    if (it.GetGenericTypeDefinition() == parentType) return true;

            Type baseType = testType.BaseType;
            if (baseType == null) return false;

            return baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == parentType ||
                IsAssignableToGenericType(parentType, baseType);
        }

        #endregion
    }
}
