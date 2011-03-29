using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class UpdaterForMigrationID : ContentUpdaterBase
    {
        public DataGetter<ItemWithMigrationID> DataGetter = new XmlDataGetter<ItemWithMigrationID>();

        public override void Doit()
        {
        }
    }
}
