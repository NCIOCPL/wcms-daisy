using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class Transitioner : TransitionerBase
    {
        public DataGetter<ContentTypeTransitionDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            List<ContentTypeTransitionDescription> transitionTypes = DataGetter.LoadData();

            // TODO: Actual task code goes here.
            Console.WriteLine("Performing {0} transitions.", transitionTypes.Count);
            transitionTypes.ForEach(transition => Console.WriteLine("Transition all items of type {0}, via transition {1}.", transition.ContentType, transition.TriggerName));
        }
    }
}
