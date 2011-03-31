using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.BusinessObjects
{
    public class FullContentItemDescription : ContentDescriptionBase
    {
        public Guid MigrationID { get; set; }
        public String Path { get; set; }
    }
}