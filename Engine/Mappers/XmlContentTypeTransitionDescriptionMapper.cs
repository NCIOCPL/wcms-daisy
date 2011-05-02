using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlContentTypeTransitionDescriptionMapper
        : DataMapper<ContentTypeTransitionDescription>
    {
        public override ContentTypeTransitionDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            ContentTypeTransitionDescription description = new ContentTypeTransitionDescription();

            description.ContentType = item.SelectSingleNode("contentType").InnerText;
            description.TriggerName = item.SelectSingleNode("trigger").InnerText;

            // Non-data fields not copied into the description's Fields collection.

            return description;
        }
    }
}
