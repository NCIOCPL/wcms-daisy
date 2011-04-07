using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.BusinessObjects
{
    public class RelationshipDescription : MigrationData
    {
        public String DependentContentType { get; set; }
        public Guid DependentMigrationID { get; set; }
        public String OwnerContentType { get; set; }
        public Guid OwnerMigrationID { get; set; }
        public String SlotName { get; set; }
        public String TemplateName { get; set; }
    }
}