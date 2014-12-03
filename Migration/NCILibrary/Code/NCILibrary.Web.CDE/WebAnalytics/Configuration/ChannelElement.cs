using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    public class ChannelElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }
    }
}
