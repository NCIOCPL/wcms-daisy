using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.Modules
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DictionaryURL", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DictionaryURL
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DictionaryEnglishURL { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DictionarySpanishURL { get; set; }

    }
}
