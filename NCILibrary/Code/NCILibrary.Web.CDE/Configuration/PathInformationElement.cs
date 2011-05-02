using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class PathInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("pagePathFormat")]
        public PathElement PagePathFormat
        {
            get { return (PathElement)base["pagePathFormat"]; }
        }

        [ConfigurationProperty("filePathFormat")]
        public PathElement FilePathFormat
        {
            get { return (PathElement)base["filePathFormat"]; }
        }

        [ConfigurationProperty("sectionPathFormat")]
        public PathElement SectionPathFormat
        {
            get { return (PathElement)base["sectionPathFormat"]; }
        }

        [ConfigurationProperty("sectionInfoTreePath")]
        public PathElement SectionInfoTreePath
        {
            get { return (PathElement)base["sectionInfoTreePath"]; }
        }

        [ConfigurationProperty("pageTemplateConfigurationPath")]
        public PathElement PageTemplateConfigurationPath
        {
            get { return (PathElement)base["pageTemplateConfigurationPath"]; }
        }

        [ConfigurationProperty("promoUrlMappingPath")]
        public PathElement PromoUrlMappingPath
        {
            get { return (PathElement)base["promoUrlMappingPath"]; }
        }

        [ConfigurationProperty("bestBetsResultPath")]
        public PathElement BestBetsResultPath
        {
            get { return (PathElement)base["bestBetsResultPath"]; }
        }

    }
}
