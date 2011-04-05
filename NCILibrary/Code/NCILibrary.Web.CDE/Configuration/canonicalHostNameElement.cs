using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class canonicalHostNameElement : ConfigurationElement
    {
        [ConfigurationProperty("canonicalHostName")]
        public string CanonicalHostName
        {
            get { return (string)base["canonicalHostName"]; }
        }
    }
}
