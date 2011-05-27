using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlUpdateFileMapper
        : XmlDataMapper<UpdateFileItem>
    {
        public override UpdateFileItem MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            UpdateFileItem description = new UpdateFileItem();

            description.MigrationID = new Guid(item.SelectSingleNode(MigIDField).InnerText);
            description.ContentType = item.SelectSingleNode(ContentTypeField).InnerText;
            description.OriginalUrl = item.SelectSingleNode("url").InnerText;

            CopyFields(item, description.Fields);

            return description;
        }
    }
}
