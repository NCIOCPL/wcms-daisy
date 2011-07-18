using System;
using System.Configuration;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.Test.Configuration
{
    public class PathInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("physicalApplicationPath")]
        public PathElement PhysicalApplicationPath
        {
            get { return (PathElement)base["physicalApplicationPath"]; }
        }

        [ConfigurationProperty("applicationPath")]
        public PathElement ApplicationPath
        {
            get { return (PathElement)base["applicationPath"]; }
        }
    }
}
