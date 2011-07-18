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
    [ToolboxData("<{0}:MenuLinkButton runat=server></{0}:MenuLinkButton>")]
    public class MenuLinkButton : MenuButton
    {
        private string _url;        
        private HtmlAnchor _link = new HtmlAnchor();

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            _link.ID = "MenuButtonLink";
            this.Controls.Add(_link);
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (string.IsNullOrEmpty(_url))
                _url = "#";
            _link.HRef = _url;
            _link.InnerHtml = "<span>" + ButtonText + "</span>";

            base.OnPreRender(e);
        }
    }
}
