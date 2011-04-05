using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    public class UrlPathChannelElement : ConfigurationElement
    {
        [ConfigurationProperty("urlPath", IsRequired = true)]
        public string UrlPath
        {
            get
            {
                return (string)base["urlPath"];
            }
        }

        [ConfigurationProperty("channelName", IsRequired = true)]
        public string ChannelName
        {
            get
            {
                return (string)base["channelName"];
            }
        }

        [ConfigurationProperty("urlMatch", IsRequired = false)]
        public string UrlMatch
        {
            get
            {
                return (string)base["urlMatch"];
            }
        }
    }
}
