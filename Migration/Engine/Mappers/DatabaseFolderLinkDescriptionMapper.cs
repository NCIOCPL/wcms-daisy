using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class DatabaseFolderLinkDescriptionMapper
        : DatabaseDataMapper<FolderLinkDescription>
    {
        public override FolderLinkDescription MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            FolderLinkDescription description = new FolderLinkDescription();

            try
            {
                description.Path = row.Field<string>(PathNameField);
                description.ObjectMonikerName = row.Field<String>(UniqueIDField);

                CopyFields(row, description.Fields);
            }
            finally
            {
                CheckForRecordedErrors();
            }

            return description;
        }
    }
}
