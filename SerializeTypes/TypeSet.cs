using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Tasks;

namespace SerializeTypes
{
    public class TypeSet
    {
        public MigrationTask[] MigrationTaskList;

        public TypeSet()
        {
            MigrationTaskList = new MigrationTask[]
        {
            new FolderCreator(),
            new GeneralContentCreator(),
            new ContentUpdater(){ DataGetter = new DatabaseDataGetter<UpdateContentItem>() },
            new Transitioner()
        };
        }
    }
}
