using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Blu82
{
    public class DaisyCollection<T>
    {
        [XmlElement("item")]
        public T[] Items { get; set; }
    }
}
