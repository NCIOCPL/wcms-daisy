using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Blu82
{
    [XmlRootAttribute("item")]
    public class DaisyFolder
    {
        public Guid mig_id { get; set; }
        public string folder { get; set; }
        public int show_in_nav { get; set; }
    }
}
