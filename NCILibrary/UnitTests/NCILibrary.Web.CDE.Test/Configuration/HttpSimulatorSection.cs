using System;
using System.Configuration;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.Test.Configuration
{
    public class HttpSimulatorSection : ConfigurationSection
    {
        [ConfigurationProperty("pathInformation")]
        public PathInformationElement PathInformation
        {
            get { return (PathInformationElement)base["pathInformation"]; }
        }
    }
}
