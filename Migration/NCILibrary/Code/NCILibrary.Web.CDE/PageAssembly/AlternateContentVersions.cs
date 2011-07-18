using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Linq;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This class represent the information about an file needed in the Page display options. 
    /// like file resource URL and the content MIME Type of file.
    /// </summary>
    public class AlternateContentFile
    {
        /// <summary>
        /// URL of the file.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Url { get; set; }

        /// <summary>
        /// Mime type of the file.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string MimeType { get; set; }
    }

    public class AlternateContentVersions
    {
        /// <summary>
        /// If true display the Page Options Print This Page and Print This Document.
        /// If multi page this would also display View Entire Document and Print All.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool IsPrintAvailable { get; set; }

        /// <summary>
        /// If true displays the Page Options 'Email This Document'.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool IsEmailAvailable { get; set; }

        /// <summary>
        /// If true displays the Page Options 'Bookmark and Share'.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool IsShareBookmarkAvailable { get; set; }

        /// <summary>
        /// If true displays the order copy Page Option
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string OrderCopyURL { get; set; }

        /// <summary>
        /// Used to represent display options for different file types like 
        /// word,pdf,etc.
        /// </summary>
        [XmlArray(ElementName = "Files", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("File", typeof(AlternateContentFile), Form = XmlSchemaForm.Unqualified)]
        public AlternateContentFile[] Files { get; set; }

    }
}
