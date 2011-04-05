using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    [ConfigurationCollection(typeof(UrlPathChannelElement),
         AddItemName = "urlPathChannelElement",
         CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class UrlPathChannelElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlPathChannelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }
    }
}
