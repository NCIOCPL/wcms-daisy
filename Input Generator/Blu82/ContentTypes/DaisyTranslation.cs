using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Blu82
{
    [XmlRootAttribute("item")]
    public class DaisyTranslation
    {
        public String english_id { get; set; }
        public String spanish_id { get; set; }
    }
}
