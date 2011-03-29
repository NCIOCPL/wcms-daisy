using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class RelaterForMigrationID : RelationshipCreatorBase
    {
        public DataGetter<RelationshipDescriptionWithMigrationID> DataGetter = new XmlDataGetter<RelationshipDescriptionWithMigrationID>();

        public override void Doit()
        {
        }
    }
}
