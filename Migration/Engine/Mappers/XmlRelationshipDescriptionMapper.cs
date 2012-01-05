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

            try
            {
                description.OwnerUniqueIdentifier = GetNamedFieldValue(item, "ownerid");
                description.OwnerContentType = GetNamedFieldValue(item, "ownercontenttype");

                description.DependentUniqueIdentifier = GetNamedFieldValue(item, "dependentid");
                description.DependentContentType = GetNamedFieldValue(item, "dependentcontenttype");

                description.SlotName = GetNamedFieldValue(item, "slot");
                description.TemplateName = GetNamedFieldValue(item, "template");
            }
            finally
            {
                CheckForRecordedErrors();
            }

            return description;
        }
    }
}
