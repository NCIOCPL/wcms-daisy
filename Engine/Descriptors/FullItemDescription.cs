using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    public class FullItemDescription : ContentDescriptionBase
    {
        public String Path { get; set; }
        public String Community { get; set; }
    }
}