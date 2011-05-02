using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MigrationEngine.Mappers
{
    /// <summary>
    /// Base class for mapping database rows to business objects.
    /// </summary>
    /// <typeparam name="ReturnType">The type of the returned business object.</typeparam>
    /// <remarks>This class provides common functionality and constants.
    /// User objects should not be referenced as type DatabaseDataMapper.</remarks>
    public abstract class DatabaseDataMapper<ReturnType>
        : DataMapper<ReturnType>
    {
        // Common field names.
        protected const string MigIDField = "migid";
        protected const string ContentTypeField = "contenttype";
        protected const string CommunityNameField = "community";
        protected const string PathNameField = "folder";

        // List of fields which shouldn't be copied into a descriptor's field set.
        private string[] ommittedFields = { ContentTypeField, CommunityNameField, PathNameField };

        protected void CopyFields(DataRow row, Dictionary<String, String> fieldset)
        {

            foreach (DataColumn column in row.Table.Columns)
            {
                // It is assumed that fields in Percussion will always be
                // lowercase, otherwise it becomes more difficult to map
                // them from database column names.
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