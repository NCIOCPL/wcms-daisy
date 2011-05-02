using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NCI.Logging.Configuration
{
    public class LoggingSinkElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("providerName")]
        public string ProviderName
        {
            get { return (string)base["providerName"]; }
            set { base["providerName"] = value; }
        }


        [ConfigurationProperty("errorLevels")]
        public string ErrorLevels
        {
            get { return (string)base["errorLevels"]; }
            set { base["errorLevels"] = value; }
        }

        [ConfigurationProperty("facilityMatchStrings", IsDefaultCollection = false)]
        public FacilityMatchStringsCollection FacilityMatchStrings
        {
            get { return (FacilityMatchStringsCollection)base["facilityMatchStrings"]; }
        }


    }
}
