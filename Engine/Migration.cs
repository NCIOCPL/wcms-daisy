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

        public void Run()
        {
            if (MigrationTaskList != null)
            {
                foreach (MigrationTask task in MigrationTaskList)
                {
                    task.Doit();
                }
            }
        }
    }
}
