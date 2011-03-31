using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    abstract public class ContentDescriptionBase : MigrationData
    {
        public String Community { get; set; }
        public String ContentType { get; set; }

        [XmlIgnore()]
        public Dictionary<string, string> Fields { get; private set; }

        public ContentDescriptionBase()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}