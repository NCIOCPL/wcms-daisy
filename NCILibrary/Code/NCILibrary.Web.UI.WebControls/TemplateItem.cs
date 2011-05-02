using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.UI;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplateItem : ITemplate, INamingContainer
    {
        #region fields
        private string _templateType;
        private ITemplate _template;
        #endregion

        #region public properties
        // The Template
        // Since it isn't "known" what the exact type of container
        // will be used to instantiate a template, I just assume all
        // of them will implement the IDataItemContainer interface,
        // which is a good idea anyway for containers.
        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(IDataItemContainer))]
        public ITemplate Template
        {
            get { return _template; }
            set { _template = value; }
        }

        public string TemplateType
        {
            get { return _templateType; }
            set { _templateType = value; }
        }
        #endregion

        // This is just to help reduce one level of dereferencing 
        // needed to get a handle on the template object.
        #region ITemplate Members

        /// <summary>
        /// When implemented by a class, defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
        /// </summary>
        /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template.</param>
        public void InstantiateIn(Control container)
        {
            Template.InstantiateIn(container);
        }

        #endregion

    }
}
