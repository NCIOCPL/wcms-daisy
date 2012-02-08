using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlFolderDescriptionMapper
        : XmlDataMapper<FolderDescription>
    {
        public override FolderDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            FolderDescription description = new FolderDescription();

            try
            {
                // For folders, the Path *is* the unique identifier
                description.Path = GetNamedFieldValue(item, PathNameField);
                description.UniqueIdentifier = GetNamedFieldValue(item, PathNameField);
                description.ContentType = Constants.Types.NAVON;

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
