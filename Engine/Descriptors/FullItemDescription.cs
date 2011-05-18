using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object fully describing a content item.
    /// </summary>
    public class FullItemDescription : ContentDescriptionBase
    {
        public String Path { get; set; }
        public String Community { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"{0}<Path value=""{1}""/><Community value=""{2}""/>";

                return string.Format(fmt, base.PropertyString, Path, Community);
            }

        }
    }
}