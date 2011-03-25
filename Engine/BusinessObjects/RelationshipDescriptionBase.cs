using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    public abstract class RelationshipDescriptionBase : MigrationData
    {
        public String DependentContentType;
        public String DependentMigrationID;
        public String OwnerContentType;
        public String SlotName;
        public String TemplateName;
    }
}