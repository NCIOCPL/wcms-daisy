using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class UpdaterForFolderPath : ContentUpdaterBase
    {
        public DataGetter<ItemWithPath> DataGetter;

        public override void Doit()
        {
        }
    }
}
