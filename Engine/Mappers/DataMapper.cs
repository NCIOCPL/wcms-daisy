using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Mappers
{
    /// <summary>
    /// Base class for all data mappers.
    /// </summary>
    /// <typeparam name="ReturnType">The type of object to be created by the mapper.</typeparam>
    /// <remarks>
    /// A data mapper takes a single instance of migration data (e.g. an XmlNode or a DataRow)
    /// and maps the various data fields into the members of a migration business object
    /// (a descriptor).
    /// 
    /// One DataMapper-derived class is required for each combination of businss objects
    /// and data storage types (database, XML file, etc).
    /// 
    /// New DataMappers are declared by inheriting from a constructed DataMapper
    /// or one of its subtypes (e.g. DataMapper&lt;FolderDesription&gt;).
    /// </remarks>
    public abstract class DataMapper<ReturnType>
    {
        // Common field names.
        protected const string MigIDField = "mig_id";
        protected const string ContentTypeField = "contenttype";
        protected const string CommunityNameField = "community";
        protected const string PathNameField = "folder";

        /// <summary>
        /// Maps one item of a data storage type into a single business object.
        /// </summary>
        /// <param name="dataItem">The data item.</param>
        /// <returns>The business object.</returns>
        public abstract ReturnType MapItem(object dataItem);
    }
}
