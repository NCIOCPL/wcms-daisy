using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents parameter fields, results template used in dynamic search.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DynamicList", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DynamicList : SearchList
    { 
    }
}
