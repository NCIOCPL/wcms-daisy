using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlUpdateItemMapper
        : XmlDataMapper<UpdateContentItem>
    {
        public override UpdateContentItem MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            UpdateContentItem description = new UpdateContentItem();

            try
            {
                description.UniqueIdentifier = GetNamedFieldValue(item, UniqueIDField);
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
