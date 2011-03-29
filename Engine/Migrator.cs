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
        private MigrationTask[] MigrationTaskList;

        public Migrator()
        {
        }

        public void Save(string filePath)
        {
            MigrationTaskList = new MigrationTask[] 
            {
                new FolderCreator(),
                new GeneralContentCreator(),
                new UpdaterForMigrationID(),
                new UpdaterForFolderPath(),
                new RelaterForMigrationID(),
                new RelaterForFolderPath(),
                new Transitioner()
            };

            Type[] typeList = GetTypeListForSerialization();

            XmlSerializer serializer = new XmlSerializer(typeof(MigrationTask[]), typeList);
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, MigrationTaskList);
            }
            MigrationTaskList = null;
        }

        public void Load(string filePath)
        {
            Type[] typeList = GetTypeListForSerialization();

            XmlSerializer serializer = new XmlSerializer(typeof(MigrationTask[]), typeList);
            using (TextReader reader = new StreamReader(filePath))
            {
                MigrationTaskList = (MigrationTask[])serializer.Deserialize(reader);
            }

            foreach (MigrationTask task in MigrationTaskList)
            {
                task.Doit();
            }
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

            // Collect all the MigrationTask types.
            Type taskBase = typeof(MigrationTask);
            Array.ForEach(allTypes, testType =>
            {
                if (!testType.IsAbstract
                    && taskBase.IsAssignableFrom(testType))
                {
                    serializableTypes.Add(testType);
                }
            });

            // Collect all the DataMapper types.
            Type mapperBase = typeof(DataMapper<>);
            Array.ForEach(allTypes, testType =>
            {
                if (!testType.IsAbstract
                    && IsAssignableToGenericType(mapperBase, testType))
                {
                    serializableTypes.Add(testType);
                }
            });


            // Get the return data types for the various LoadData signatures.
            Type migDataBase = typeof(MigrationData);
            Type[] migDataTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract
                    && migDataBase.IsAssignableFrom(testType);
            });

            // Get the list of DataGetters.
            Type dataGetterBase = typeof(DataGetter<>);
            Type[] dataGetterTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract && testType.IsGenericTypeDefinition
                    && IsAssignableToGenericType(dataGetterBase, testType);
            });

            // Create the cross-product of all the DataGetter generics with
            // all the MigrationData subclasses.
            foreach (Type dataType in migDataTypes)
            {
                foreach (Type getter in dataGetterTypes)
                {
                    Type constructedType = getter.MakeGenericType(new Type[] { dataType });
                    //generics.Add(constructedType);
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
