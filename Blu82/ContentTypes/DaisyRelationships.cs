using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Blu82
{
    [XmlRootAttribute("item")]
    public class DaisyRelationships
    {
        public Guid ownerid { get; set; }
        public String ownercontenttype { get; set; }
        public Guid dependentid { get; set; }
        public String dependentcontenttype {get; set;}
        public String slot {get; set;}
        public String template { get; set; }
    }
}
