using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class CanonicalHostNameInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("canonicalUrlHostName")]
        public canonicalHostNameElement CanonicalUrlHostName
        {
            get { return (canonicalHostNameElement)base["canonicalUrlHostName"]; }
        }
    }
}
