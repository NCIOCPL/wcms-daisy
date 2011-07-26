using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.Menus
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ContextMenuLinkItem ></{0}:ContextMenuLinkItem>")]
    public class ContextMenuLinkItem : ContextMenuItem
    {

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NavigateUrl
        {
            get
            {
                String s = (String)ViewState["NavigateUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Href, NavigateUrl);

            if (!string.IsNullOrEmpty(IconSrc))
            {
                output.AddAttribute(HtmlTextWriterAttribute.Style,
                    String.Format("background-image: url({0}); background-repeat: no-repeat;", IconSrc));
            }

            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write(this.Text);
            output.RenderEndTag();
        }

    }
}
