using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbu43.Storage
{
    public class ContentTypeSheet : List<ContentTypeSheetItem>
    {
        public string SheetName { get; internal set; }

        public ContentTypeSheet(string sheetName)
        {
            SheetName = sheetName;
        }
    }
}
