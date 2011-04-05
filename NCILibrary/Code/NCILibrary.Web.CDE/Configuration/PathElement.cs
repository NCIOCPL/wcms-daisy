using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class PathElement : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (string)base["path"]; }
        }
    }
}
