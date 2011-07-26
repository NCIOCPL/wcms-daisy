using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;


namespace NCI.Web.CDE
{
    /// <summary>
    /// Represents the metadata of a file published by Percussion.  An Instance of this class is created by deserializing the percussion published XML file.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("GenericFileInstruction", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class GenericFileInstruction : IFileInstruction
    {
        /// <summary>
        /// Gets or sets the language for the page displayed.
        /// </summary>
        /// <value>The language.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>
        /// The path of all parent folders of the page assembly instruction.
        /// </summary>
        /// <value></value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SectionPath { get; set; }

        /// <summary>
        /// Gets or sets the pretty URL.
        /// </summary>
        /// <value>The pretty URL.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        /// <value>The path of the file.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the page metadata.
        /// </summary>
        /// <value>The page metadata.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PageMetadata PageMetadata { get; set; }

        /// <summary>
        /// Gets or sets the content dates for the page.
        /// </summary>
        /// <value>The page metadata.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ContentDates ContentDates { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            GenericFileInstruction target = obj as GenericFileInstruction;

            if (target == null)
                return false;

            if (PrettyUrl != target.PrettyUrl)
                return false;

            if (FilePath != target.FilePath)
                return false;

            if (Language != target.Language)
                return false;

            if (SectionPath != target.SectionPath)
                return false;

            if (
                (PageMetadata != null && target.PageMetadata == null) ||
                (PageMetadata == null && target.PageMetadata != null)
                )
            {
                return false;
            }

            if (PageMetadata != null && target.PageMetadata != null)
                if (!PageMetadata.Equals(target.PageMetadata))
                    return false;

            if (
                (ContentDates != null && target.ContentDates == null) ||
                (ContentDates == null && target.ContentDates != null)
                )
            {
                return false;
            }

            if (ContentDates != null && target.ContentDates != null)
                if (!ContentDates.Equals(target.ContentDates))
                    return false;
        

            return true;
        }

    }
}
