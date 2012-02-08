using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Blu82
{
    [XmlRootAttribute("item")]
    public class DaisyFolderLink
    {
        public String folder { get; set; }
        public String mig_id { get; set; }
    }
}
