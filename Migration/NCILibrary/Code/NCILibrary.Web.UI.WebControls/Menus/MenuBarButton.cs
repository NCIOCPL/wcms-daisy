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
    [ToolboxData("<{0}:MenuBarButton runat=server></{0}:MenuBarButton>")]
    public abstract class MenuBarButton : CompositeControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string Text
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

        public abstract string OnClientClick
        {
            get;
            set;
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Li;
            }
        }
    }
}
