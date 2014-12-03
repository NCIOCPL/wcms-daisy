using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlFullItemDescriptionMapper
        : XmlDataMapper<FullItemDescription>
    {
        public override FullItemDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            FullItemDescription description = new FullItemDescription();

            try
            {
                description.Path = GetNamedFieldValue(item, PathNameField);
                description.UniqueIdentifier = GetNamedFieldValue(item, UniqueIDField);
                description.Community = GetNamedFieldValue(item, CommunityNameField);
                description.ContentType = GetNamedFieldValue(item, ContentTypeField);

                CopyFields(item, description.Fields);
            }
            finally
            {
                CheckForRecordedErrors();
            }

            return description;
        }
    }
}
