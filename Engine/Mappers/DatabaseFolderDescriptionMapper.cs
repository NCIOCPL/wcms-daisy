using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;


using MigrationEngine.Descriptors;

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

            description.Path = row.Field<String>("folder");
            description.MigrationdID = row.Field<Guid>("migid");

            foreach (DataColumn column in row.Table.Columns)
            {
                string name = column.ColumnName;
                string value = row[name].ToString();
                description.Fields.Add(name, value);
            }


            return description;
        }
    }
}
