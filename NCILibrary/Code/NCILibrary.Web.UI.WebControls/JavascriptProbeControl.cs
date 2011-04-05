using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This control can be used to tell if the client has javascript enabled.  This only works on a postback.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:JavascriptProbeControl runat=server></{0}:JavascriptProbeControl>")]
    public class JavascriptProbeControl : WebControl, IPostBackDataHandler
    {
        private bool _hasJavascript = false;

        /// <summary>
        /// Determines if the client has javascript enabled.  This only works when Page.IsPostback == true.
        /// </summary>
        public bool HasJavascript
        {
            get { return _hasJavascript; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresPostBack(this);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + this.IdSeparator + "hasJS");
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + this.ClientIDSeparator + "hasJS");
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.AddAttribute(HtmlTextWriterAttribute.Value, "0");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();

            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);
            output.Write(@"
                {var hasJSInput = document.getElementById('" + this.ClientID + this.ClientIDSeparator + "hasJS" + @"');
                    if (hasJSInput) {
                        hasJSInput.value = '1';
                }}");
            output.RenderEndTag();
        }

        #region IPostBackDataHandler Members

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            _hasJavascript = Strings.ToBoolean((string)postCollection[postDataKey + this.IdSeparator + "hasJS"]);
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
        }

        #endregion
    }
}
