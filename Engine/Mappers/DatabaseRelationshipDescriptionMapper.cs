using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class DatabaseRelationshipDescriptionMapper
        : DatabaseDataMapper<RelationshipDescription>
    {
        public override RelationshipDescription MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            RelationshipDescription description = new RelationshipDescription();

            description.OwnerMigrationID = row.Field<Guid>("ownerid");
            description.OwnerContentType = row.Field<String>("ownercontenttype");

            description.DependentMigrationID = row.Field<Guid>("dependentid");
            description.DependentContentType = row.Field<String>("dependentcontenttype");

            description.SlotName = row.Field<String>("slot");
            description.TemplateName= row.Field<String>("template");

            CopyFields(row, description.Fields);

            return description;
        }
    }
}
