using System;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    public class LocalField
    {
        //[XmlElement(Form = XmlSchemaForm.Unqualified)]
        [XmlAttribute(Form = XmlSchemaForm.Unqualified, AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }
    }
}
