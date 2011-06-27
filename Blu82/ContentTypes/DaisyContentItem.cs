using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Blu82
{
    [XmlRootAttribute("item")]
    public class DaisyContentItem : IXmlSerializable
    {
        public String mig_id { get; set; }
        public String community { get; set; }
        public String contenttype { get; set; }
        public String folder { get; set; }
        [XmlIgnore()]
        public Dictionary<string, string> Fields = new Dictionary<string, string>();

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException("This is not done yet");
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("mig_id", this.mig_id);
            writer.WriteElementString("community", this.community);
            writer.WriteElementString("contenttype", this.contenttype);
            writer.WriteElementString("folder", this.folder);
            foreach (String key in Fields.Keys)
            {
                writer.WriteStartElement(key);
                writer.WriteCData(Fields[key]);
                writer.WriteEndElement();
            }            
        }

        #endregion
    }
}
