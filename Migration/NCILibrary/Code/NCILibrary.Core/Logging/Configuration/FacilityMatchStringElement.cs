using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NCI.Logging.Configuration
{
    public class FacilityMatchStringElement : ConfigurationElement
    {

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }


    }
}
