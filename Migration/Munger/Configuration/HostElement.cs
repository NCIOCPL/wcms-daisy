using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Munger.Configuration
{
    public class HostElement : ConfigurationElement
    {
        /// <summary>
        /// The host name.  e.g. www.cancer.gov.
        /// Does not include a protocol, or path
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Designates a specific host name as the recognized or accepted primary name for the host.
        /// </summary>
        [ConfigurationProperty("isCanonical", IsRequired = false, DefaultValue = false)]
        public bool IsCanonical
        {
            get { return (bool)this["isCanonical"]; }
            set { this["isCanonical"] = value; }
        }
    }
}
