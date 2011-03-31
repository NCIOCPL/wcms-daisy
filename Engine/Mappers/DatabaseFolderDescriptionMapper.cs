using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;


using MigrationEngine.BusinessObjects;

namespace MigrationEngine.Mappers
{
    public class DatabaseFolderDescriptionMapper
        : DataMapper<FolderDescription>
    {
        public override FolderDescription MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            FolderDescription description = new FolderDescription();

            description.Path = row["folderpath"].ToString();

            return description;
        }
    }
}
