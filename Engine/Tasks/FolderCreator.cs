using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class FolderCreator : FolderCreatorBase
    {
        public DataGetter<FolderDescription> DataGetter;

        override public void Doit()
        {
            List<FolderDescription> folders = DataGetter.LoadData();

            // TODO: Actual task code goes here.
            Console.WriteLine("Creating {0} folders.", folders.Count);
            folders.ForEach(folder => Console.WriteLine("Folder: {0}", folder.Path));
        }
    }
}