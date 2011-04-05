using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TemplateItemContainer runat=server></{0}:TemplateItemContainer>")]
    public class TemplateItemContainer : WebControl, IDataItemContainer, INamingContainer
    {
        protected string _templateType;
        protected object _data;

        /// <summary>
        /// Gets the template type of this TemplateItemContainer.
        /// </summary>
        public string TemplateType
        {
            get { return _templateType; }
        }

        public TemplateItemContainer(string templateType)
        {
            _templateType = templateType;
        }

        public TemplateItemContainer(string templateType, object data)
            : this(templateType)
        {
            _data = data;
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        /// <remarks>This does not render out any opening and closing tags.</remarks>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

        #region IDataItemContainer Members

        public object DataItem
        {
            get { return _data; }
        }

        public int DataItemIndex
        {
            get { return 0; }
        }

        public int DisplayIndex
        {
            get { return 0; }
        }

        #endregion

    }
}
