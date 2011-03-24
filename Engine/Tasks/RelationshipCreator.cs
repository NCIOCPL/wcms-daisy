using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    [XmlInclude(typeof(RelaterForMigrationID)),
        XmlInclude(typeof(RelaterForFolderPath))]
    public abstract class RelationshipCreator : MigrationTask, IRelationshipCreator
    {
        public abstract override void Doit();
    }
}
