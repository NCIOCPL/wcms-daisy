using System;
using System.Collections.Generic;
using System.Configuration;

namespace NCI.CMS.Percussion.Manager.Configuration
{
    public class RepublishEditionListElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get { return (String)this["value"]; }
        }
    }
}