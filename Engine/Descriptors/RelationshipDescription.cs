using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    public class RelationshipDescription : MigrationData
    {
        public String DependentContentType { get; set; }
        public Guid DependentMigrationID { get; set; }
        public String OwnerContentType { get; set; }
        public Guid OwnerMigrationID { get; set; }
        public String SlotName { get; set; }
        public String TemplateName { get; set; }

        public override string ToString()
        {
            string fmt = @"OwnerContentType: {{{0}}}; OwnerMigrationID: {{{1}}}; DependentContentType: {{{2}}}; DependentMigrationID: {{{3}}}; SlotName: {{{4}}}; TemplateName: {{{5}}}";

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