using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Tasks
{
    public abstract class TransitionerBase : MigrationTask
    {
        public abstract override void Doit(IMigrationLog logger);
    }
}
