using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class FileInstructionTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
        }
    }
}
