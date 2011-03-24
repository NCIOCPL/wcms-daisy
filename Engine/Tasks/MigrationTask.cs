using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    [XmlInclude(typeof(FolderCreator)),
    XmlInclude(typeof(ContentCreator)),
    XmlInclude(typeof(ContentUpdater)),
    XmlInclude(typeof(RelationshipCreator)),
    XmlInclude(typeof(Transitioner))]
    public abstract class MigrationTask : IMigrateTask
    {
        abstract public void Doit();
    }
}
