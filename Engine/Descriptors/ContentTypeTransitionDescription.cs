using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object destailing a workflow transition for
    /// a specific content type.
    /// </summary>
    public class ContentTypeTransitionDescription : MigrationData
    {
        public string ContentType { get; set; }
        public string TriggerName { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<ContentType value=""{0}""/><TriggerName value=""{1}""/>";
                return string.Format(fmt, ContentType, TriggerName);
            }
        }
    }
}
