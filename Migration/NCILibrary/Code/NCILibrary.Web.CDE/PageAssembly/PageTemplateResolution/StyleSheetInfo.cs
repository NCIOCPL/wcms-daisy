using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;


namespace NCI.Web.CDE
{
    /// <summary>
    /// Represents the location and processing rules for a style sheet of a PageTemplateInfo.
    /// </summary>
    public class StyleSheetInfo
    {
        /// <summary>
        /// Gets or sets the style sheet path.
        /// </summary>
        /// <value>The style sheet path.</value>
        [XmlElement(Form=XmlSchemaForm.Unqualified, IsNullable=false)]
        public string StyleSheetPath { get; set; }

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
            StyleSheetInfo target = obj as StyleSheetInfo;

            if (target == null)
                return false;

            if (StyleSheetPath != target.StyleSheetPath)
                return false;

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
