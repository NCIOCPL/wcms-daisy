using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object representing an active assembly relationship
    /// between two content items.
    /// </summary>
    public class RelationshipDescription : MigrationData
    {
        public String DependentContentType { get; set; }
        public Guid DependentMigrationID { get; set; }
        public String OwnerContentType { get; set; }
        public Guid OwnerMigrationID { get; set; }
        public String SlotName { get; set; }
        public String TemplateName { get; set; }


        protected override string PropertyString
        {
            get
            {
                string fmt = @"<OwnerContentType value=""{0}""/><OwnerMigrationID value=""{1}""/><DependentContentType value=""{2}""/><DependentMigrationID value=""{3}""/><SlotName value=""{4}""/><TemplateName value=""{5}""/>";

                return string.Format(fmt,
                    OwnerContentType,
                    OwnerMigrationID,
                    DependentContentType,
                    DependentMigrationID,
                    SlotName,
                    TemplateName);
            }
        }
    }
}