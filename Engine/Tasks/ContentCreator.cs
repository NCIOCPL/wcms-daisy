using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    [XmlInclude(typeof(GeneralContentCreator))]
    public abstract class ContentCreator : MigrationTask, IContentCreator
    {
        public abstract override void Doit();
    }
}
