using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;


using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    /// <summary>
    /// Maps data rows to FolderDescription objects.
    /// </summary>
    public class DatabaseFolderDescriptionMapper
        : DatabaseDataMapper<FolderDescription>
    {
        /// <summary>
        /// Maps one DataRow into a single FolderDescription.
        /// </summary>
        /// <param name="dataItem">The data row.</param>
        /// <returns>A single FolderDescription.</returns>
        public override FolderDescription MapItem(object dataItem)
        {
            if (!(dataItem is DataRow))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected DataRow."),
                    dataItem.GetType().Name);

            DataRow row = (DataRow)dataItem;

            FolderDescription description = new FolderDescription();

            description.Path = row.Field<String>(PathNameField);
            description.MigrationID = row.Field<Guid>(MigIDField);

            CopyFields(row, description.Fields);

            return description;
        }
    }
}
