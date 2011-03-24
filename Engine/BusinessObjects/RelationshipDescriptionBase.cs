using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    [XmlInclude(typeof(RelationshipDescriptionWithPath)),
    XmlInclude(typeof(RelationshipDescriptionWithMigrationID))]
    public abstract class RelationshipDescriptionBase : IMigrationData
    {
        public String DependentContentType;
        public String DependentMigrationID;
        public String OwnerContentType;
        public String SlotName;
        public String TemplateName;
    }
}