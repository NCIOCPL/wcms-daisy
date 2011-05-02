using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_CDRDefinition", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class CDRDefinition
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CDRId { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CDRDefinitionName { get; set; }

    }
}
