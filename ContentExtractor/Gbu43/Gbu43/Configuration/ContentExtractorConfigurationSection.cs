using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gbu43.Configuration
{
    public class ContentExtractorConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("contentTypes", IsDefaultCollection = false)]
        public ContentTypesCollection ContentTypes
        {
            get { return (ContentTypesCollection)base["contentTypes"]; }
        }

        [ConfigurationProperty("sheetConfiguration", IsDefaultCollection = false)]
        public SheetConfigurationElement SheetConfiguration
        {
            get { return (SheetConfigurationElement)base["sheetConfiguration"]; }
        }

    }
}
