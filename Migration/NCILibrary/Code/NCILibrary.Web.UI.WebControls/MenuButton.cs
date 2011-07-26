
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NCI.Web.UI.WebControls
{
    public abstract class MenuButton : WebControl, INamingContainer
    {
        private string _text;

        public string ButtonText
        {
            get { return _text; }
            set { _text = value; }
        }

        public MenuButton()
            : base("li")
        {

        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.RenderControl(output);
            }
        }

    }
}
