using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine.Tasks;

namespace MigrationEngine
{
    public class Migration
    {
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
