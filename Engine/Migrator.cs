using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;
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

            Type[] typeList = GetHierarchyTypeList();

            XmlSerializer serializer = new XmlSerializer(typeof(MigrationTask[]), typeList);
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, MigrationTaskList);
            }
            MigrationTaskList = null;
        }

        public void Load(string filePath)
        {
            Type[] typeList = GetHierarchyTypeList();

            XmlSerializer serializer = new XmlSerializer(typeof(MigrationTask[]), typeList);
            using (TextReader reader = new StreamReader(filePath))
            {
                MigrationTaskList = (MigrationTask[])serializer.Deserialize(reader);
            }
        }

        private Type[] GetHierarchyTypeList()
        {
            Assembly engine = typeof(Migrator).Module.Assembly;
            Type[] allTypes = engine.GetTypes();

            Type taskBase = typeof(MigrationTask);
            Type[] taskTypes = Array.FindAll(allTypes, testType => {
                return !testType.IsAbstract
                    && taskBase.IsAssignableFrom(testType);
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
            List<Type> generics = new List<Type>();
            foreach (Type dataType in migDataTypes)
            {
                foreach (Type getter in dataGetterTypes)
                {
                    Type constructedType = getter.MakeGenericType(new Type[] { dataType });
                    generics.Add(constructedType);
                }
            }

            List<Type> rollup = new List<Type>();
            rollup.AddRange(taskTypes);
            rollup.AddRange(generics);
            return rollup.ToArray();
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
    }
}
