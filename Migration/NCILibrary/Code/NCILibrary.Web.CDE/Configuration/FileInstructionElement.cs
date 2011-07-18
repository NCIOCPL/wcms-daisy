using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class FileInstructionElement : ConfigurationElement
    {
        [ConfigurationProperty("fileInstructionTypes")]
        public FileInstructionTypeElementCollection FileInstructionTypes
        {
            get { return (FileInstructionTypeElementCollection)base["fileInstructionTypes"]; }
        }
    }
}
