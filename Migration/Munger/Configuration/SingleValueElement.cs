using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Munger.Configuration
{
    /// <summary>
    /// Represents a configuration element with a single attribute named value.
    /// </summary>
    public class SingleValueElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
