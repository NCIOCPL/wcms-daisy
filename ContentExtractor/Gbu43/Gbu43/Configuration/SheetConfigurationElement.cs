using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace Gbu43.Configuration
{
    /// <summary>
    /// Defines the configuration for the spreadsheet output.
    /// </summary>
    public class SheetConfigurationElement : ConfigurationElement
    {

        [ConfigurationProperty("contentTabPrefix",
            DefaultValue = "2 CT ")]
        public string ContentTabPrefix
        {
            get { return (string)base["contentTabPrefix"]; }
            set { base["contentTabPrefix"] = value; }
        }

        [ConfigurationProperty("folderTab",
            DefaultValue = "1 Folders")]
        public string FolderTab
        {
            get { return (string)base["folderTab"]; }
            set { base["folderTab"] = value; }
        }

        [ConfigurationProperty("relationsTab",
            DefaultValue = "4 Relationships")]
        public string RelationsTab
        {
            get { return (string)base["relationsTab"]; }
            set { base["relationsTab"] = value; }
        }

        [ConfigurationProperty("translationsTab",
            DefaultValue = "4 Relationships")]
        public string TranslationsTab
        {
            get { return (string)base["translationsTab"]; }
            set { base["translationsTab"] = value; }
        }

    }
}
