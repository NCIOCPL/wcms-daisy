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
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MenuBar runat=server></{0}:MenuBar>")]
    [ParseChildren(false), PersistChildren(true)]
    public class MenuBar : CompositeControl
    {

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("Normal")]
        [Localizable(true)]
        public MenuStyles MenuStyle
        {
            get
            {
                object s = (object)ViewState["MenuStyle"];
                return ((s == null) ? MenuStyles.None : (MenuStyles)s);
            }

            set
            {
                ViewState["MenuStyle"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetupStyle();
        }

        private void SetupStyle()
        {
            if (MenuStyle != MenuStyles.None)
            {
                if (Page.Header == null)
                    throw new NullReferenceException("The head html element for the page requires the runat=server property.");

                this.CssClass = "MBMenu"
                    + " MBMenu-" + MenuStyle.ToString();

                if (!HttpContext.Current.Items.Contains("MBMenuBase"))
                {
                    HtmlLink link = new HtmlLink();

                    Page.Header.Controls.Add(link);

                    link.Href = Page.ClientScript.GetWebResourceUrl(
                        typeof(MenuBar),
                        "NCI.Web.UI.WebControls.Menus.Resources.MenuBar.MBMenuBase.css");

                    link.Attributes.Add("rel", "stylesheet");



                    //Mark that the stylesheet has been added so no other 
                    //context menus will add this...
                    HttpContext.Current.Items.Add("MBMenuBase", true);

                }

                //If we have not added the stylesheet for this style to the
                //Head block for this request then add it
                if (!HttpContext.Current.Items.Contains(MenuStyle))
                {
                    HtmlLink link = new HtmlLink();
                    Page.Header.Controls.Add(link);

                    link.Href = Page.ClientScript.GetWebResourceUrl(
                        typeof(MenuBar),
                        string.Format("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.{0}.css", MenuStyle.ToString()));

                    link.Attributes.Add("rel", "stylesheet");



                    //Mark that the stylesheet has been added so no other 
                    //context menus will add this...
                    HttpContext.Current.Items.Add(MenuStyle, true);
                }
            }

        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (Control ctrl in this.Controls)
            {
                ctrl.RenderControl(output);
            }

            output.RenderEndTag();

            //Stop the floating
            output.AddAttribute(HtmlTextWriterAttribute.Style, "clear: both");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();
        }


        public enum MenuStyles
        {
            None = -1,
            Silver = 1
        }
    }
}
