using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    [XmlInclude(typeof(FullContentItemDescription)),
    XmlInclude(typeof(ItemWithMigrationID)),
    XmlInclude(typeof(ItemWithPath))]
    abstract public class ContentDescriptionBase : MigrationData
    {
        public String Community;
        public String ContentType;
        public Dictionary<string, string> Fields;
    }
}
