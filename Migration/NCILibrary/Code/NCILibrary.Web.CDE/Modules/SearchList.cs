using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents search parameter fields.
    /// </summary>
    public class SearchParameters
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Keyword { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string StartDate { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string EndDate { get; set; }
    }

    /// <summary>
    /// This class represents parameter fields, results template used in search.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DynamicList", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SearchList
    {
        /// <summary>
        /// Search title field
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchTitle { get; set; }

        /// <summary>
        /// Records per page field.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int RecordsPerPage { get; set; }

        /// <summary>
        /// Max results that should be returned
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int MaxResults { get; set; }

        /// <summary>
        /// Search filter field
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchFilter { get; set; }

        /// <summary>
        /// Fields that will be excluded in the search
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExcludeSearchFilter { get; set; }

        /// <summary>
        /// Field on which the results should be sorted
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ResultsSortOrder { get; set; }

        /// <summary>
        /// Language for which the search will be performed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>
        /// Type of Search
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchType { get; set; }

        /// <summary>
        /// Search parameters like Keyword, StartDate, EndDate
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public SearchParameters SearchParameters { get; set; }

        /// <summary>
        /// The template which will be used to render the results on the CDE
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultsTemplate { get; set; }
    }
}
