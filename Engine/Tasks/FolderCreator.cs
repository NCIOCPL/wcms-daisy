using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class FolderCreator : MigrationTask, IFolderCreator
    {
        public DataGetter<FolderDescription> DataGetter = new XmlDataGetter<FolderDescription>();

        override public void Doit()
        {
        }
    }
}