using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    public abstract class RelationshipDescriptionBase : MigrationData
    {
        public String DependentContentType { get; set; }
        public Guid DependentMigrationID { get; set; }
        public String OwnerContentType { get; set; }
        public String SlotName { get; set; }
        public String TemplateName { get; set; }
    }
}