using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    public class ReportingSuiteElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("enabledForAllChannels", DefaultValue = false)]
        public bool EnabledForAllChannels
        {
            get
            {
                return (bool)base["enabledForAllChannels"];
            }
        }

        [ConfigurationProperty("language", DefaultValue = "")]
        public string Language 
        {
            get
            {
                return (string)base["language"];
            }
        }

        [ConfigurationProperty("channels")]
        public ChannelElementCollection Channels
        {
            get { return (ChannelElementCollection)base["channels"]; }
        }
    }
}
