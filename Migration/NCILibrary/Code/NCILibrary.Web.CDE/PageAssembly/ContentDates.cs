using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
/// <summary>
    /// Defines the content dates for an item.
    /// </summary>
    public class ContentDates
    {
        /// <summary>
        /// Gets the date the item was first published.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime FirstPublished { get; set; }

        /// <summary>
        /// Gets the date the item was last modified. 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets the date the item was last reviewed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime LastReviewed { get; set; }

        /// <summary>
        /// Gets the date the item should be reviewed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateTime NextReview { get; set; }

        /// <summary>
        /// Gets the display mode when dates are displayed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public DateDisplayModes DateDisplayMode { get; set; }

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
            ContentDates target = obj as ContentDates;

            if (target == null)
                return false;

            if (LastModified != target.LastModified)
                return false;


            if (FirstPublished != target.FirstPublished)
                return false;

            if (LastReviewed != target.LastReviewed)
                return false;

            if (NextReview != target.NextReview)
                return false;


            if (DateDisplayMode != target.DateDisplayMode)
                return false;

            return true;
        }
    }
}
