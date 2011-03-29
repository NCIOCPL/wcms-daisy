using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.Mappers
{
    public class XmlFolderDescriptionMapper
        : DataMapper<FolderDescription>
    {
        public override FolderDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            FolderDescription description = new FolderDescription();

            description.Path = item.SelectSingleNode("folder").InnerText;

            return description;
        }
    }
}
