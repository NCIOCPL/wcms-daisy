using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MigrationEngine.Configuration
{
    public class CommunityLookupSection : ConfigurationSection
    {
        [ConfigurationProperty("Community", IsRequired = true, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CommunityListElement), AddItemName="add", ClearItemsName="clear", RemoveItemName="remove")]
        public CommunityListElement Community
        {
            get { return (CommunityListElement)base["Community"]; }
        }
    }
}
