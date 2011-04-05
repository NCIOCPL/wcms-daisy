using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    public abstract class MigrationTask : IMigrateTask
    {
        [XmlAttribute()]
        public String Name { get; set; }

        abstract public void Doit(IMigrationLog logger);
    }
}
