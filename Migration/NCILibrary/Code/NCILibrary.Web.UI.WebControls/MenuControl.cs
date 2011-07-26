using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [ToolboxData("<{0}:MenuControl runat=server></{0}:MenuControl>")]
    public class MenuControl : WebControl, INamingContainer
    {

        private MenuStyles _menuStyle = MenuStyles.Normal;

        public MenuStyles MenuStyle
        {
            get { return _menuStyle; }
            set { _menuStyle = value; }
        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            output.WriteBeginTag("div");
            if (_menuStyle == MenuStyles.Reverse)
            {
                output.WriteAttribute("style", "float: right");
            }
            output.Write(HtmlTextWriter.TagRightChar);

            output.WriteBeginTag("ul");
            output.WriteAttribute("class", "MenuControlTabs");
            output.Write(HtmlTextWriter.TagRightChar);

            foreach (MenuButton button in this.Controls)
            {

                button.RenderControl(output);
                
            }

            output.WriteEndTag("ul");
           // output.WriteEndTag("div");
            if (_menuStyle == MenuStyles.Reverse)
            {
                output.WriteBeginTag("div");
                output.WriteAttribute("style", "clear: right");
                output.Write(HtmlTextWriter.TagRightChar);
                output.WriteEndTag("div");
            }
        }


        public enum MenuStyles
        {
            None = -1,
            Normal = 1,
            Reverse =2
        }
    }
}
