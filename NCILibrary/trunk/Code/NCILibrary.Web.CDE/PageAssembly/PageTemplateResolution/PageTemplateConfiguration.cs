using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{

    /// <summary>
    /// Represents the CDE Page Templates available to the Page Assemblers.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("PageTemplateConfiguration", Namespace = "http://www.example.org/CDESchema", IsNullable = false)] 
    public class PageTemplateConfiguration
    {

        /// <summary>
        /// Gets or sets a collection of CDE Page Templates.
        /// </summary>
        /// <value>The page template collections.</value>
        [XmlArray(ElementName = "PageTemplateCollections", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName="PageTemplateCollection", Form=XmlSchemaForm.Unqualified)]
        public PageTemplateCollection[] PageTemplateCollections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTemplateConfiguration"/> class.
        /// </summary>
        public PageTemplateConfiguration()
        {
            PageTemplateCollections = new PageTemplateCollection[] { };
        }

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
            PageTemplateConfiguration target = obj as PageTemplateConfiguration;

            if (target == null)
                return false;

            if (
                (PageTemplateCollections == null && target.PageTemplateCollections != null) ||
                (PageTemplateCollections != null && target.PageTemplateCollections == null)
                )
            {
                return false;
            }

            if (PageTemplateCollections.Length != target.PageTemplateCollections.Length)
                return false;

            for (int i = 0; i < PageTemplateCollections.Length; i++)
            {
                if (PageTemplateCollections[i] == null)
                {
                    if (target.PageTemplateCollections[i] != null)
                        return false;

                    //If we did not return then we know that target.PageTemplateCollections[i] is also null
                }
                else
                {
                    if (!PageTemplateCollections[i].Equals(target.PageTemplateCollections[i]))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            //This object may change, so it would be bad for the hash code to change,
            //so use the base implementation
            return base.GetHashCode();
        }

    }
}
