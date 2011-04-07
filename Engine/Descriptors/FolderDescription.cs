using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    public class FolderDescription : MigrationData
    {
        [XmlIgnore()]
        public String Path { get; set; }
        public Guid MigrationdID { get; set; }

        [XmlIgnore()]
        public Dictionary<string, string> Fields { get; private set; }

        public FolderDescription()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}