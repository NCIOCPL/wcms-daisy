using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class ContentDeliveryEngineSection : ConfigurationSection
    {
        [ConfigurationProperty("pageAssembly")]
        public PageAssemblyElement PageAssembly
        {
            get { return (PageAssemblyElement)base["pageAssembly"]; }
        }

        [ConfigurationProperty("fileInstruction")]
        public FileInstructionElement FileInstruction
        {
            get { return (FileInstructionElement)base["fileInstruction"]; }
        }

        [ConfigurationProperty("pathInformation")]
        public PathInformationElement PathInformation
        {
            get { return (PathInformationElement)base["pathInformation"]; }
        }
        [ConfigurationProperty("pageTitle")]
        public PageTitleInformationElement PageTitle
        {
            get { return (PageTitleInformationElement)base["pageTitle"]; }
        }

        [ConfigurationProperty("canonicalHostName")]
        public CanonicalHostNameInformationElement CanonicalHostName
        {
            get { return (CanonicalHostNameInformationElement)base["canonicalHostName"]; }
        }

        [ConfigurationProperty("defaultHomePage")]
        public DefaultHomePageElement DefaultHomePage
        {
            get { return (DefaultHomePageElement)base["defaultHomePage"]; }
        }

    }
}
