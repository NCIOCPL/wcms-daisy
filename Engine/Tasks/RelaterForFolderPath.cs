using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class RelaterForFolderPath : RelationshipCreatorBase
    {
        public DataGetter<RelationshipDescriptionWithPath> DataGetter = new XmlDataGetter<RelationshipDescriptionWithPath>();

        public override void Doit()
        {
        }
    }
}
