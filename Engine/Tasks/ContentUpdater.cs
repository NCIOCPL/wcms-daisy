using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    [XmlInclude(typeof(UpdaterForMigrationID)),
        XmlInclude(typeof(UpdaterForFolderPath))]
    public abstract class ContentUpdater : MigrationTask, IContentUpdater
    {
        public abstract override void Doit();
    }
}
