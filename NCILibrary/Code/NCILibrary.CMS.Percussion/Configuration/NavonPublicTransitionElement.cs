using System;
using System.Collections.Generic;
using System.Configuration;

namespace NCI.CMS.Percussion.Manager.Configuration
{
    public class NavonPublicTransitionElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = false, DefaultValue = "DirectToPublicWithoutActions")]
        public String Value
        {
            get { return (String)this["value"]; }
        }
    }
}
