using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.BusinessObjects
{
    public class RelationshipDescriptionWithMigrationID : MigrationData
    {
        public String OwnerMigrationID { get; set; }
    }
}
