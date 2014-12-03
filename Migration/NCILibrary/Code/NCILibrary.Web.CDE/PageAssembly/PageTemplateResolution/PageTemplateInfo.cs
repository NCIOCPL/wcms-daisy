using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Represents an instance of a CDE Page Template for a DisplayVersion.
    /// </summary>
    public sealed class PageTemplateInfo
    {
        /// <summary>
        /// Gets or sets the <see cref="DisplayVersions"/> of this Page Template Info.
        /// </summary>
        /// <value>The display version.</value>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public DisplayVersions DisplayVersion { get; set; }

        /// <summary>
        /// Gets or sets the path to the aspx for this PageTemplateInfo.
        /// </summary>
        /// <value>The page template path.</value>
        [XmlElement(ElementName="PageTemplatePath", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string PageTemplatePath { get; set; }

        /// <summary>
        /// Gets or sets the style sheets of this PageTemplateInfo.
        /// </summary>
        /// <value>The style sheets.</value>
        [XmlArray(ElementName = "StyleSheets", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName = "StyleSheetInfo", Form = XmlSchemaForm.Unqualified)]
        public StyleSheetInfo[] StyleSheets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTemplateInfo"/> class.
        /// </summary>
        public PageTemplateInfo()
        {
            StyleSheets = new StyleSheetInfo[]{};
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
            PageTemplateInfo target = obj as PageTemplateInfo;

            if (target == null)
                return false;

            if (DisplayVersion != target.DisplayVersion)
                return false;

            if (PageTemplatePath != target.PageTemplatePath)
                return false;

            if (
                (StyleSheets == null && target.StyleSheets != null) ||
                (StyleSheets != null && target.StyleSheets == null)
                )
            {
                return false;
            }

            if (StyleSheets != null && target.StyleSheets != null)
            {

                if (StyleSheets.Length != target.StyleSheets.Length)
                    return false;

                for (int i = 0; i < StyleSheets.Length; i++)
                {
                    if (StyleSheets[i] == null)
                    {
                        if (target.StyleSheets[i] != null)
                            return false;

                        //If we did not return then we know that target.Stylesheets[i] is also null
                    }
                    else
                    {
                        if (!StyleSheets[i].Equals(target.StyleSheets[i]))
                            return false;
                    }
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
