using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This class represents a collection of TemplateItems.  This can be used for repeater
    /// controls so that you can define different ITemplates to be used when databinding 
    /// different types.
    /// </summary>
    public class TemplateItemCollection : List<TemplateItem>
    {
        //private List<TemplateItem> _items = new List<TemplateItem>();

        public TemplateItem this[string templateType]
        {
            get
            {
                return this.SingleOrDefault(p => string.Compare(p.TemplateType, templateType, true) == 0);
                
                // The above LINQ code expands to the following code.
                //foreach (TemplateItem p in this)
                //{
                //    if (string.Compare(p.TemplateType, key, true) == 0)
                //        return p;
                //}
                //return null;
            }
        }

        /// <summary>
        /// Checks to see if this TemplateItemCollection contains a TemplateItem with the specified
        /// type.
        /// </summary>
        /// <param name="templateType">The type to look for.</param>
        /// <returns>
        /// true if the collections contains a TemplateItem with the type 
        /// templateType, false if it does not
        /// </returns>
        public bool ContainsType(string templateType)
        {
            return this.Exists(p => p.TemplateType == templateType);
        }

    }
}
