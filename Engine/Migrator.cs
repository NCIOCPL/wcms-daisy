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
        public MigrationTask[] MigrationTaskList =
        {
            new FolderCreator(),
            new GeneralContentCreator(),
            new UpdaterForMigrationID(),
            new UpdaterForFolderPath(),
            new RelaterForMigrationID(),
            new RelaterForFolderPath(),
            new Transitioner()
        };

        public Migrator()
        {
        }

        public void Save(string filePath)
        {
            Type[] typeList = GetHierarchyTypeList();

            XmlSerializer serializer = new XmlSerializer(typeof(MigrationTask[]), typeList);
            TextWriter writer = new StreamWriter(filePath);

            serializer.Serialize(writer, MigrationTaskList);
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
            // TODO: Need a way to figure out which ones are actually
            // derived from DataGetter.  IsAssignableFrom doesn't seem to work here.
            Type dataGetterBase = typeof(DataGetter<>);
            Type[] dataGetterTypes = Array.FindAll(allTypes, testType =>
            {
                return !testType.IsAbstract && testType.IsGenericTypeDefinition;
            });

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
    }
}
