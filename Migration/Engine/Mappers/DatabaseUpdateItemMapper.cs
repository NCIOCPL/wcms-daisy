using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class DatabaseUpdateItemMapper
        : DatabaseDataMapper<UpdateContentItem>
    {
        public override UpdateContentItem MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            UpdateContentItem description = new UpdateContentItem();

            description.MigrationID = row.Field<Guid>(MigIDField);
            description.ContentType = row.Field<string>(ContentTypeField);

            CopyFields(row, description.Fields);

            return description;
        }
    }
}
