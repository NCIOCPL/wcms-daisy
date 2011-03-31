using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class RelaterForMigrationID : RelationshipCreatorBase
    {
        public DataGetter<RelationshipDescriptionWithMigrationID> DataGetter;

        public override void Doit()
        {
            List<RelationshipDescriptionWithMigrationID> relationships = DataGetter.LoadData();

            // TODO: Actual task code goes here.
            Console.WriteLine("Creating {0} relationships.", relationships.Count);
            relationships.ForEach(relation => Console.WriteLine("OwnerContentType: {0}", relation.OwnerContentType));
        }
    }
}
