using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine.Tasks;

namespace MigrationEngine
{
    /// <summary>
    /// Encapsulates a migration run.  This class is not intended to be
    /// instantiated directly. Instead, it is created by the deserialization
    /// process in Migrator.Run().
    /// </summary>
    public class Migration
    {
        /// <summary>
        /// List of migration tasks to be performed.  The list is populated
        /// with concrete objects at runtime via deserialization.
        /// </summary>
        public MigrationTask[] MigrationTaskList;// = new MigrationTask[]
        //{
        //    new FolderCreator(),
        //    new GeneralContentCreator(),
        //    new UpdaterForMigrationID(),
        //    new UpdaterForFolderPath(),
        //    new RelaterForMigrationID(),
        //    new RelaterForFolderPath(),
        //    new Transitioner()
        //};

        /// <summary>
        /// Performs the Migration.
        /// </summary>
        /// <param name="migLog">IMigrationLog object for recording progress through
        /// the list of tasks.</param>
        public void Run(IMigrationLog migLog)
        {
            if (MigrationTaskList != null)
            {
                int index = 1;
                int taskCount = MigrationTaskList.Length;

                foreach (MigrationTask task in MigrationTaskList)
                {
                    try
                    {
                        migLog.BeginTask(task.Name, index++, taskCount);
                        task.Doit(migLog);
                    }
                    catch (Exception ex)
                    {
                        migLog.LogUnhandledException(task.Name, ex);
                    }
                    finally
                    {
                        migLog.EndTask();
                    }
                }
            }
        }
    }
}
