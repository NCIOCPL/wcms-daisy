using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlRelationshipDescriptionMapper
        : XmlDataMapper<RelationshipDescription>
    {
        public override RelationshipDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            RelationshipDescription description = new RelationshipDescription();

            description.OwnerMigrationID = new Guid(GetNamedFieldValue(item, "ownerid", FieldMappingErrorHandler));
            description.OwnerContentType = GetNamedFieldValue(item, "ownercontenttype", FieldMappingErrorHandler);

            description.DependentMigrationID = new Guid(GetNamedFieldValue(item, "dependentid", FieldMappingErrorHandler));
            description.DependentContentType = GetNamedFieldValue(item, "dependentcontenttype", FieldMappingErrorHandler);

            description.SlotName = GetNamedFieldValue(item, "slot", FieldMappingErrorHandler);
            description.TemplateName = GetNamedFieldValue(item, "template", FieldMappingErrorHandler);

            CheckForRecordedErrors();

            return description;
        }
    }
}
