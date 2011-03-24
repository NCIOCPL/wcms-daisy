using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
