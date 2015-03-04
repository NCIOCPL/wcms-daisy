using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gbu43.Configuration
{
    public class ContentTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("tabName")]
        public string TabName
        {
            get { return (string)base["tabName"]; }
            set { base["tabName"] = value; }
        }

        [ConfigurationProperty("extractFieldnames", IsDefaultCollection = false)]
        public FieldNameCollection extractFieldnames
        {
            get { return (FieldNameCollection)base["extractFieldnames"]; }
        }
    }
}
