using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    abstract public class ContentDescriptionBase : MigrationData
    {
        public String Community;
        public String ContentType;

        [XmlIgnore()]
        public Dictionary<string, string> Fields;
    }
}
