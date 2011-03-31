using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.BusinessObjects
{
    public class RelationshipDescriptionWithPath : RelationshipDescriptionBase
    {
        public String OwnerPath { get; set; }
    }
}