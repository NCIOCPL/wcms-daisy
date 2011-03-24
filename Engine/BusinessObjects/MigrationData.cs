using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.BusinessObjects
{
    [XmlInclude(typeof(FolderDescription)),
    XmlInclude(typeof(ContentDescriptionBase)),
    XmlInclude(typeof(RelationshipDescriptionBase))]
    public abstract class MigrationData : IMigrationData
    {
    }
}
