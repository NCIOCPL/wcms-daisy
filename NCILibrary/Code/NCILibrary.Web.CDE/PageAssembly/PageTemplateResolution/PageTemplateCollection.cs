using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    /// <summary>
    /// 
    /// </summary>
    public class PageTemplateCollection
    {
        /// <summary>
        /// Gets or sets the name of this CDE Page Template.
        /// </summary>
        /// <value>The name of the template.</value>
        [XmlAttribute(AttributeName = "Name", Form = XmlSchemaForm.Unqualified)]
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the collection of PageTemplateInfos which represent the aspx file and associated stylesheets for a DisplayVersion of 
        /// a CDE Page Template.
        /// </summary>
        /// <value>The page template infos.</value>
        [XmlArray(ElementName = "PageTemplateInfos", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName = "PageTemplateInfo", Form = XmlSchemaForm.Unqualified)]
        public PageTemplateInfo[] PageTemplateInfos { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTemplateCollection"/> class.
        /// </summary>
        public PageTemplateCollection()
        {
            PageTemplateInfos = new PageTemplateInfo[] { };
        }

        /// <summary>
        /// Gets the PageTemplateInfo for a given Display Version.
        /// </summary>
        /// <param name="version">The Display Version to get the PageTemplateInfo</param>
        /// <returns>The PageTemplateInfo for the given Display Version if defined, otherwise it defaults to the
        /// PageTemplateInfo for the Web Display Version.
        /// </returns>
        public PageTemplateInfo GetPageTemplateInfo(DisplayVersions version)
        {
            PageTemplateInfo rtnInfo = null;

            //Find the PageTemplateInfo for the requested version type.
            rtnInfo = PageTemplateInfos.FirstOrDefault(pti => pti.DisplayVersion == version);

            //If there was no PageTemplateInfo for the requested version type.
            if (rtnInfo == null)
            {                
                if (version == DisplayVersions.PrintAll)
                {
                    //For PrintAll default to Print if available
                    rtnInfo = PageTemplateInfos.FirstOrDefault(pti => pti.DisplayVersion == DisplayVersions.Print);

                    //If Print is not available default to ViewAll
                    if (rtnInfo == null)
                        rtnInfo = PageTemplateInfos.FirstOrDefault(pti => pti.DisplayVersion == DisplayVersions.ViewAll);
                }

                //If nothing is available default to Web
                if (rtnInfo == null)
                    rtnInfo = PageTemplateInfos.FirstOrDefault(pti => pti.DisplayVersion == DisplayVersions.Web);
            }

            return rtnInfo;
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
            PageTemplateCollection target = obj as PageTemplateCollection;

            if (target == null)
                return false;

            if (TemplateName != target.TemplateName)
                return false;

            if (
                (PageTemplateInfos == null && target.PageTemplateInfos != null) ||
                (PageTemplateInfos != null && target.PageTemplateInfos == null)
                )
            {
                return false;
            }

            if (PageTemplateInfos.Length != target.PageTemplateInfos.Length)
                return false;

            for (int i = 0; i < PageTemplateInfos.Length; i++)
            {
                if (PageTemplateInfos[i] == null)
                {
                    if (target.PageTemplateInfos[i] != null)
                        return false;

                    //If we did not return then we know that target.PageTemplateInfos[i] is also null
                }
                else
                {
                    if (!PageTemplateInfos[i].Equals(target.PageTemplateInfos[i]))
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
