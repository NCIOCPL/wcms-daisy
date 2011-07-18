using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlContentTypeDescriptionMapper
         : XmlDataMapper<ContentTypeDescription>
    {
        public override ContentTypeDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            ContentTypeDescription description = new ContentTypeDescription();

            try
            {
                description.ContentType = GetNamedFieldValue(item, ContentTypeField);
                description.Community = GetNamedFieldValue(item, CommunityNameField);

                // Non-data fields not copied into the description's Fields collection.
            }
            finally
            {
                CheckForRecordedErrors();
            }

            return description;
        }
    }
}