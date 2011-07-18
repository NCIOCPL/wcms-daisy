using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;


using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class DatabaseFullItemDescriptionMapper
        : DatabaseDataMapper<FullItemDescription>
    {
        public override FullItemDescription MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            FullItemDescription description = new FullItemDescription();

            description.Path = row.Field<string>(PathNameField);
            description.MigrationID = row.Field<Guid>(MigIDField);
            description.Community = row.Field<string>(CommunityNameField);
            description.ContentType = row.Field<string>(ContentTypeField);

            CopyFields(row, description.Fields);

            return description;
        }
    }
}
