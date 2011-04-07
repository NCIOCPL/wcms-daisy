using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    public class ContentTypeTransitionDescription : MigrationData
    {
        public string ContentType { get; set; }
        public string TriggerName { get; set; }
    }
}
