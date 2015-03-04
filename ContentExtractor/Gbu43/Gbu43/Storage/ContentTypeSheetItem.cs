using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbu43.Storage
{
    public class ContentTypeSheetItem: Dictionary<string, string>
    {
        public string Folder { get; internal set; }
        public string MigID { get; internal set; }
        public string ContentType { get; internal set; }
    }
}
