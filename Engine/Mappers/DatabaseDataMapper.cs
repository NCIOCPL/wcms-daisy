using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MigrationEngine.Mappers
{
    public abstract class DatabaseDataMapper<ReturnType>
        : DataMapper<ReturnType>
    {
        // Common field names.
        protected const string MigIDField = "migid";
        protected const string ContentTypeField = "contenttype";
        protected const string CommunityNameField = "community";
        protected const string PathNameField = "folder";

        // List of fields which shouldn't be copied into a descriptor's field set.
        private string[] ommittedFields = { MigIDField, ContentTypeField, CommunityNameField, PathNameField };

        protected void CopyFields(DataRow row, Dictionary<String, String> fieldset)
        {

            foreach (DataColumn column in row.Table.Columns)
            {
                string name = column.ColumnName.ToLower();

                // Skip copying certain fields.
                if (Array.Exists(ommittedFields, fieldName => fieldName == name))
                    continue;

                string value = row[name].ToString();
                fieldset.Add(name, value);
            }

        }
    }
}