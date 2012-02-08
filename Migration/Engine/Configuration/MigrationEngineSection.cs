using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MigrationEngine.Configuration
{
    public class MigrationEngineSection : ConfigurationSection
    {
        [ConfigurationProperty("Community", IsRequired = true, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CommunityListElement), AddItemName="add", ClearItemsName="clear", RemoveItemName="remove")]
        public CommunityListElement Community
        {
            get { return (CommunityListElement)base["Community"]; }
        }

        [ConfigurationProperty("siteHostName", IsRequired = true)]
        public SiteHostNameElement SiteHostName
        {
            get { return (SiteHostNameElement)base["siteHostName"]; }
        }

        [ConfigurationProperty("monikerStoreProvider", IsRequired = true)]
        public SingleValueElement MonikerStoreProviderName
        {
            get { return (SingleValueElement)base["monikerStoreProvider"]; }
        }
    }
}
