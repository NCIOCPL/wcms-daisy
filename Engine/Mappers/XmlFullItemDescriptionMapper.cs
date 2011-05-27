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

            description.Path = item.SelectSingleNode(PathNameField).InnerText;
            description.MigrationID = new Guid(item.SelectSingleNode(MigIDField).InnerText);
            description.Community = item.SelectSingleNode(CommunityNameField).InnerText;
            description.ContentType = item.SelectSingleNode(ContentTypeField).InnerText;

            CopyFields(item, description.Fields);

            return description;
        }
    }
}
