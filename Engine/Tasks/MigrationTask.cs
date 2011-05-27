using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine.Configuration;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Base class for all migration tasks.
    /// </summary>
    public abstract class MigrationTask : IMigrateTask
    {
        /// <summary>
        /// Descriptive name of an individual instance of a migration task.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute()]
        public String Name { get; set; }

        /// <summary>
        /// This is the main method for implementing a migration task.
        /// An instance of IMigrationLog is provided for logging the
        /// progress through the list of task items.
        /// </summary>
        /// <param name="logger">IMigrationLog object for recording task progress.</param>
        abstract public void Doit(IMigrationLog logger);

        protected string LookupCommunityName(string key)
        {
            string community = null;

            if (string.IsNullOrEmpty(key))
                throw new InvalidCommunityNameException("Community lookup key must not be null or empty.");

            MigrationEngineSection lookup = (MigrationEngineSection)ConfigurationManager.GetSection("MigrationEngine");
            if (lookup == null)
                throw new ConfigurationException("Unable to load MigrationEngine settings section.");

            community = lookup.Community[key].Name;
            if (lookup == null)
                throw new InvalidCommunityNameException(string.Format("No community found for key '{0}'.", key));

            return community;
        }
    }
}
