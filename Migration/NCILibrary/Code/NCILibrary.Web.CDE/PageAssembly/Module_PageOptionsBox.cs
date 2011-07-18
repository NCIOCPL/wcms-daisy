using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This class represents the properties of a page option item.
    /// </summary>
    public class PageOption
    {

        /// <summary>
        /// The key for this page option.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified, AttributeName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// The class name used for styling.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CssClass { get; set; }

        /// <summary>
        /// The text for the link.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string LinkText { get; set; }

        /// <summary>
        /// The WebAnalytics function to be used during onclick
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WebAnalyticsFunction { get; set; }

        /// <summary>
        /// The type of page option
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string OptionType { get; set; }
    }

    /// <summary>
    /// This class encapsulates all the information related to PageOption Box and 
    /// individual items in the page options box.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_PageOptionsBox", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class Module_PageOptionsBox
    {
        /// <summary>
        /// The text that will be used for title of the Page options box. Page Options, etc.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        /// <summary>
        /// A collection of all page options item.
        /// </summary>
        [XmlArray(ElementName = "PageOptions", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("PageOption", typeof(PageOption), Form = XmlSchemaForm.Unqualified)]
        public Collection<PageOption> PageOptions { get; set; }
    }
}
