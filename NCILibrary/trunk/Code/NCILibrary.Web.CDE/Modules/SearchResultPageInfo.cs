using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class defines the properties of search result. Like the prettyUrl of the 
    /// search results page. This information should be made avaliable in the instruction 
    /// that defines the search page.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_SearchResultPageInfo", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SearchResultPageInfo
    {
        /// <summary>
        /// The page that displays the search results.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchResultsPrettyUrl { get; set; }

        /// <summary>
        /// The page that displays the search conditions for selection by the user.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchPagePrettyUrl { get; set; }

        /// <summary>
        /// The page displays the detailed view of a single result.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DetailedViewSearchResultPagePrettyUrl { get; set; }

        /// <summary>
        /// The page displays the detailed view of a single result.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PrintSearchResultPagePrettyUrl { get; set; }

        /// <summary>
        /// The help page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchHelpPrettyUrl { get; set; }

    }
}
    