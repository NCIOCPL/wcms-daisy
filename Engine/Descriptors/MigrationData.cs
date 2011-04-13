using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    public abstract class MigrationData : IMigrationData
    {
        [XmlIgnore()]
        public Dictionary<string, string> Fields { get; private set; }

        public MigrationData()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}
