using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NCI.Web.UI.WebControls.Menus
{
    /// <summary>
    /// Anchor link button for menubar which only provides client-side javascript click.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EditorMenuJSButton runat=server></{0}:EditorMenuJSButton>")]
    public class MenuBarJSButton : MenuBarButton
    {
        #region private variables
        private string _url;
        private HtmlAnchor _link = new HtmlAnchor();
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets client clickable URL
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OnClickURL
        {
            get { return "javascript:"+ _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Gets or sets the client click
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public override string OnClientClick
        {
            get
            {
                string str = (string)this.ViewState["OnClientClick"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OnClientClick"] = value;
            }
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Render child anchor control.
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //This might interfere with the javascript link...
            if (!string.IsNullOrEmpty(OnClientClick))
                output.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);

            output.AddAttribute(HtmlTextWriterAttribute.Href, OnClickURL);
            output.AddAttribute(HtmlTextWriterAttribute.Id, "MenuButtonJS");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write(this.Text);
            output.RenderEndTag();
            output.RenderEndTag();
        }
        #endregion
    }
}
