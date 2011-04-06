using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Collections;

namespace NCI.CMS.Percussion.Manager.Configuration
{
    public class PDQCancerInfoSummaryPageElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "pdqCancerInfoSummaryPage", IsRequired = false)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
        }
    }
}
