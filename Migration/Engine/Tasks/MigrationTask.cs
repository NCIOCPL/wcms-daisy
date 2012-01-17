﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using NCI.Util;
using NCI.CMS.Percussion.Manager.CMS;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;

using MigrationEngine.Configuration;
using MonikerProviders;

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

        [XmlIgnore()]
        private MonikerStore _monikerStoreField;
        protected MonikerStore MonikerStore
        {
            get
            {
                // Doing a lazy initialization in the getter guarantees that
                // the moniker store isn't loaded until the task is ready to use it.
                // This is important for the case where a task uses monikers which
                // were generated by an earlier task.
                if (_monikerStoreField == null)
                {
                    MigrationEngineSection config = (MigrationEngineSection)ConfigurationManager.GetSection("MigrationEngine");
                    string storeProvider = config.MonikerStoreProviderName.Value;
                    _monikerStoreField = new MonikerStore(storeProvider);
                }
                return _monikerStoreField;
            }
        }

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

        protected Moniker LookupMoniker(string name, CMSController controller)
        {
            Moniker moniker = null;

            // If the Moniker is in the store, return it.
            if (MonikerStore.Contains(name))
            {
                moniker = MonikerStore.Get(name);
            }
            // If the moniker starts with a /, assume it's a folder and return the Navon.
            else if (name.StartsWith("/"))
            {
                moniker = GetFolderMoniker(name, controller);
                MonikerStore.Add(moniker);
            }
            // If the moniker is a number, check whether it's a content ID.
            else if (Strings.ToLong(name) != -1)
            {
                moniker = GetContentItemMoniker(name, controller);
                MonikerStore.Add(moniker);
            }
            else
            {
                string fmt = "Unable to locate or create a moniker for \"{0}\".";
                throw new MonikerNotFoundException(string.Format(fmt, name));
            }

            return moniker;
        }

        #region Moniker look up helpers.

        private Moniker GetFolderMoniker(string name, CMSController controller)
        {
            Moniker moniker;

            string folder = name.Trim();
            string contentType = (folder == "/") ? Moniker.ContentTypes.NavTree : Moniker.ContentTypes.Navon;

            PercussionGuid[] ids = controller.SearchForContentItems(contentType, folder, new Dictionary<string, string> { });

            // Report any errors encontered while looking for a Navon.
            if (ids.Length != 1)
            {
                throw new MonikerNotFoundException(string.Format("Unable to locate exactly one Navon/NavTree for folder {0}.", folder));
            }

            // A Navon was found.  Create a moniker for it.
            moniker = new Moniker(name, ids[0].ID, contentType);

            return moniker;
        }

        private Moniker GetContentItemMoniker(string name, CMSController controller)
        {
            Moniker moniker=null;

            long contentID = Strings.ToLong(name);

            bool itemExists = controller.VerifySingleItemExists(contentID);

            // Report any errors encontered while looking for an existing content item.
            if (itemExists)
            {
                // An item was found. Create a moniker for it.
                moniker = new Moniker(name, contentID, Moniker.ContentTypes.Indeterminate);
            }
            else
            {
                throw new MonikerNotFoundException(string.Format("Unable to locate content item with ID {0}.", name));
            }

            return moniker;
        }

        #endregion
    }
}
