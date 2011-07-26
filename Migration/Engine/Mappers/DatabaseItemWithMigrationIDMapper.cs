using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.Mappers
{
    public class DatabaseItemWithMigrationIDMapper
        : DataMapper<ItemWithMigrationID>
    {
        public override ItemWithMigrationID MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            ItemWithMigrationID description = new ItemWithMigrationID();

            description.MigrationID = row.Field<Guid>("migid");

            // This desparately wants to be shared code, but inheriting from a
            // generic breaks the override of MapItem.  Possibly a special
            // DatabaseContentDescriptionBaseMapper could be added? Its MapItem
            // method couldn't do anything, but it could take care of common mapping.

            description.ContentType = row.Field<string>("contenttype");

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
