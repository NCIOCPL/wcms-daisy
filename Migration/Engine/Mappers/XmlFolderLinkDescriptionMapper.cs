using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlFolderLinkDescriptionMapper
        :XmlDataMapper<FolderLinkDescription>
    {
        public override FolderLinkDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            FolderLinkDescription description = new FolderLinkDescription();

            try
            {
                description.Path = GetNamedFieldValue(item, PathNameField);
                description.ObjectMonikerName = GetNamedFieldValue(item, UniqueIDField);

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
