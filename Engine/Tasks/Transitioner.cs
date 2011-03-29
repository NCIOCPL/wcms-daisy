using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class Transitioner : TransitionerBase
    {
        public DataGetter<TransitionDescription> DataGetter = new XmlDataGetter<TransitionDescription>();

        public override void Doit()
        {
        }
    }
}
