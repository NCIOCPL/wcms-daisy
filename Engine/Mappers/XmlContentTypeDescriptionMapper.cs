using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlContentTypeDescriptionMapper
         : DataMapper<ContentTypeDescription>
    {
        public override ContentTypeDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            ContentTypeDescription description = new ContentTypeDescription();

            description.ContentType = item.SelectSingleNode("contentType").InnerText;
            description.Community = item.SelectSingleNode("community").InnerText;

            // Non-data fields not copied into the description's Fields collection.

            return description;
        }
    }
}