using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Munger.Configuration
{
    public class MungerConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("HostList", IsRequired = true, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(HostElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HostListElement HostList
        {
            get { return (HostListElement)base["HostList"]; }
        }
    }
}
