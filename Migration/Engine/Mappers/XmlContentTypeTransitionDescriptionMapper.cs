using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlContentTypeTransitionDescriptionMapper
        : XmlDataMapper<ContentTypeTransitionDescription>
    {
        public override ContentTypeTransitionDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            ContentTypeTransitionDescription description = new ContentTypeTransitionDescription();

            try
            {
                description.ContentType = GetNamedFieldValue(item, Constants.Fields.CONTENT_TYPE);
                description.Community = GetNamedFieldValue(item, Constants.Fields.COMMUNITY_NAME);
                description.TriggerName = GetNamedFieldValue(item, "trigger");

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
