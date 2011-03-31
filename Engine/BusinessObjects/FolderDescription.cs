using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    public class FolderDescription : MigrationData
    {
        [XmlIgnore()]
        public String Path { get; set; }
    }
}