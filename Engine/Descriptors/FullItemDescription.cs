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

        public override string ToString()
        {
            string fmt = @" {0} Path: {{{1}}}; Community: {{{2}}}";

            return string.Format(fmt, base.ToString(), Path, Community);
        }
    }
}