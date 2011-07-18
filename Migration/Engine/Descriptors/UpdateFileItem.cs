using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object describing a content item to be updated.
    /// </summary>
    public class UpdateFileItem : ContentDescriptionBase
    {
        public string OriginalUrl { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"{0}<OriginalUrl value=""{1}""/>";
                return string.Format(fmt, base.PropertyString, OriginalUrl);
            }

        }
    }
}
