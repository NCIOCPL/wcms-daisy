using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gbu43.Configuration
{
    public class SlotItemElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }

        [ConfigurationProperty("includeUnextractedDependent", DefaultValue=false)]
        public bool IncludeUnextractedDependent
        {
            get { return (bool)base["includeUnextractedDependent"]; }
            set { base["includeUnextractedDependent"] = value; }
        }

    }
}
