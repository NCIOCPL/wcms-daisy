using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    [ConfigurationCollection(typeof(ChannelElement),
     AddItemName = "channel",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ChannelElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ChannelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ChannelElement)element).Name;
        }
    }
}
