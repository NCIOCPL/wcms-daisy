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
    public enum ContextMenuStyle
    {
        None = 1,
        Silver = 2,
        Blue = 3
    }

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ContextMenu runat=server></{0}:ContextMenu>")]
    [ParseChildren(false), PersistChildren(true)]
    public class ContextMenu : CompositeControl, INamingContainer
    {

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("100%")]
        [Localizable(true)]
        public override Unit Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("None")]
        [Localizable(true)]
        public ContextMenuStyle MenuStyle
        {
            get
            {
                object s = ViewState["MenuStyle"];
                return ((s == null) ? ContextMenuStyle.None : (ContextMenuStyle)s);
            }

            set
            {
                ViewState["MenuStyle"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        public ContextMenu() : base() { }

        protected override void OnLoad(EventArgs e)
        {
            //This control needs the CssBrowserSelector to get the list items correct.
            //Without it the styles bounce.
            CssBrowserSelectorManager.AddSelectorJS(this.Page);

            Page.ClientScript.RegisterClientScriptResource(typeof(ContextMenu), "NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.ContextMenu.js");
      
            if (!Page.ClientScript.IsStartupScriptRegistered(typeof(ContextMenu), "InstantiateItemMenu"))
                Page.ClientScript.RegisterStartupScript(
                    typeof(ContextMenu), 
                    "InstantiateItemMenu",
                    "var menuHandler = new ItemMenu();", 
                    true);

            SetupStyle();
        }

        private void SetupStyle()
        {
            if (MenuStyle != ContextMenuStyle.None)
            {

                this.CssClass = "ctxMenu"
                    + " ctxMenu-" + MenuStyle.ToString();

                //Load stylesheets into the head block
                CssManager.AddResource(Page, typeof(ContextMenu), "NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.CTXMenuBase.css");
                CssManager.AddResource(Page, typeof(ContextMenu), string.Format("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.{0}.css", MenuStyle.ToString()));
            }

        }
        
        protected override void RenderContents(HtmlTextWriter output)
        {
            
            output.AddAttribute("onMouseOver", "menuHandler.checkOpenHover(this);");
            output.AddAttribute(HtmlTextWriterAttribute.Onclick, "menuHandler.showMenu(this);");
            output.AddAttribute("onMouseOut", "menuHandler.startCountdownToHide(event, this);");
            output.RenderBeginTag(HtmlTextWriterTag.Ul); //Menu item ul
            output.RenderBeginTag(HtmlTextWriterTag.Li); //Menu item li
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.Write(Text);
            output.RenderEndTag();
            output.RenderBeginTag(HtmlTextWriterTag.Ul); //Menu items ul

            foreach (Control c in this.Controls)
            {
                c.RenderControl(output);
            }

            output.RenderEndTag(); //Menu items ul
            output.RenderEndTag(); //Menu item li
            output.RenderEndTag(); //Menu item ul
        }
    }
}
