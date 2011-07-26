using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MigrationEngine.Configuration
{
    public class SiteHostNameElement : ConfigurationElement
    {
        /// <summary>
        /// The key for looking up a community.
        /// </summary>
        /// <value>The key.</value>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }
}
