using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.BusinessObjects
{
    public class RelationshipDescriptionWithMigrationID
        : RelationshipDescriptionBase

    {
        public Guid OwnerMigrationID { get; set; }
    }
}
