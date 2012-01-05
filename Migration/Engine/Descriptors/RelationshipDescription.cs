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
        public String DependentUniqueIdentifier { get; set; }
        public String OwnerUniqueIdentifier { get; set; }
        public String SlotName { get; set; }
        public String TemplateName { get; set; }


        protected override string PropertyString
        {
            get
            {
                string fmt = @"<OwnerUniqueIdentifier value=""{0}""/><DependentUniqueIdentifier value=""{1}""/><SlotName value=""{2}""/><TemplateName value=""{3}""/>";

                return string.Format(fmt,
                    OwnerUniqueIdentifier,
                    DependentUniqueIdentifier,
                    SlotName,
                    TemplateName);
            }
        }
    }
}