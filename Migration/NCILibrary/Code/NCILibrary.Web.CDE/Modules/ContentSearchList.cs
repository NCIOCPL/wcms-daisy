using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents parameter fields, results template used in content search.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_ContentSearch", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class ContentSearchList: SearchList
    {
    }
}
