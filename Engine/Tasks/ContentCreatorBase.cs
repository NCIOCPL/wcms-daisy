using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    public abstract class ContentCreatorBase : ContentFieldTask
    {
        /// <summary>
        /// This is the main method for implementing a migration task.
        /// An instance of IMigrationLog is provided for logging the
        /// progress through the list of task items.
        /// </summary>
        /// <param name="logger">IMigrationLog object for recording task progress.</param>
        public abstract override void Doit(IMigrationLog logger);
    }
}
