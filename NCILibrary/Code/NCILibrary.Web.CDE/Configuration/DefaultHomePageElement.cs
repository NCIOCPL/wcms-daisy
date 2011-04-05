using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class DefaultHomePageElement : ConfigurationElement
    {
        [ConfigurationProperty("homepage")]
        public string Homepage
        {
            get { return (string)base["homepage"]; }
        }
    }
}
