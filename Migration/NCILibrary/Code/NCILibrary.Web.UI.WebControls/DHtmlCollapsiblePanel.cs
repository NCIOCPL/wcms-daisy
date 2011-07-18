using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web.UI.WebControls.JSLibraries;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DHtmlCollapsiblePanel runat=server></{0}:DHtmlCollapsiblePanel>")]
    [ParseChildren(false), PersistChildren(true)]
    public class DHtmlCollapsiblePanel : CompositeControl, IPostBackDataHandler
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                String s = (String)ViewState["Title"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Title"] = value;
            }
        }



        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(1)]
        [Localizable(true)]
        public float EffectDuration
        {
            get
            {
                object s = ViewState["EffectDuration"];
                return ((s == null) ? 1 : (float)s);
            }

            set
            {
                ViewState["EffectDuration"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool IsCollapsed
        {
            get
            {
                object s = ViewState["IsCollapsed"];
                return ((s == null) ? false : (bool)s);
            }

            set
            {
                ViewState["IsCollapsed"] = value;       
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool AllowCollapsing
        {
            get
            {
                object s = ViewState["AllowCollapsed"];
                return ((s == null) ? true : (bool)s);
            }

            set
            {
                ViewState["AllowCollapsed"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CollapseIconUrl
        {
            get
            {
                String s = (String)ViewState["CollapseIconUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["CollapseIconUrl"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ExpandIconUrl
        {
            get
            {
                String s = (String)ViewState["ExpandIconUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ExpandIconUrl"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CollapseIconAltTag
        {
            get
            {
                String s = (String)ViewState["CollapseIconAltTag"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["CollapseIconAltTag"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ExpandIconAltTag
        {
            get
            {
                String s = (String)ViewState["ExpandIconAltTag"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ExpandIconAltTag"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("13px")]
        [Localizable(true)]
        public Unit IconWidth
        {
            get
            {
                object s = ViewState["IconWidth"];

                if (s == null)
                    return Unit.Pixel(13);
                else
                    return (Unit)s;
            }

            set
            {
                ViewState["IconWidth"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("13px")]
        [Localizable(true)]
        public Unit IconHeight
        {
            get
            {
                object s = ViewState["IconHeight"];

                if (s == null)
                    return Unit.Pixel(13);
                else
                    return (Unit)s;
            }

            set
            {
                ViewState["IconHeight"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {

            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (AllowCollapsing)
            {
                //Load the scriptaculous library for the blind effect
                ScriptaculousManager.Load(Page);
                JSManager.AddResource(this.Page, typeof(DHtmlCollapsiblePanel), "NCI.Web.UI.WebControls.Resources.DHtmlCollapsiblePanel.js");

                Page.RegisterRequiresPostBack(this);
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            //Render the main div
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            output.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass + " panelContainer");
            output.RenderBeginTag(HtmlTextWriterTag.Div);

            output.AddAttribute(HtmlTextWriterAttribute.Class, "panelHeaderText");
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "-header-text");
            output.RenderBeginTag(HtmlTextWriterTag.H3);
            output.Write(Title);
            output.RenderEndTag();

            //Render div for contents
            output.AddAttribute(HtmlTextWriterAttribute.Class, "panelBody");
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "-body");
            output.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (Control c in this.Controls)
                c.RenderControl(output);

            output.RenderEndTag();
            output.RenderEndTag();

            //Hidden var for keeping status
            if (AllowCollapsing)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                output.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID + "-collapsedStatus");
                output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "-collapsedStatus");
                output.AddAttribute(HtmlTextWriterAttribute.Value, IsCollapsed.ToString());
                output.RenderBeginTag(HtmlTextWriterTag.Input);
                output.RenderEndTag();

                //Script to init the box.
                output.AddAttribute("type", "text/javascript");
                output.RenderBeginTag(HtmlTextWriterTag.Script);
                output.Write(GetJSObjCreationString());
                output.RenderEndTag();
            }
        }

        private string GetJSObjCreationString()
        {
            StringBuilder s = new StringBuilder();
            //var panel2_Obj = new DHtmlCollapsiblePanel('panel2', 'True', 'Test 2', 'images/expand.gif', 'images/collapse.gif', 'Expand Test 2 Panel', 'Collapse Test 2 Panel', '13px', '13px');

            s.Append("var ");
            s.Append(this.ClientID);
            s.Append("_Obj = new DHtmlCollapsiblePanel(");

            //Param 1, ClientID
            s.Append("'");
            s.Append(this.ClientID);
            s.Append("', ");

            //OPtions
            s.Append(" { ");

            //Option 1, IsCollapsed           
            if (this.IsCollapsed)
                s.Append(GetOption("isCollapsed", "true", false));
            else
                s.Append(GetOption("isCollapsed", "false", false));

            //Option 2, Title
            if (!string.IsNullOrEmpty(this.Title))
            {
                s.Append(", ");
                s.Append(GetOption("title", this.Title, true));
            }

            //Param 4, Effect Duration            
            if (this.EffectDuration > 0) {
                s.Append(", ");
                s.Append(GetOption("effectDuration", this.EffectDuration.ToString(), false));
            }

            //Param 3, Expand Icon
            if (!string.IsNullOrEmpty(ExpandIconUrl))
            {
                s.Append(", ");
                s.Append(GetOption("expandIconUrl", this.ExpandIconUrl, true));
            }

            //Param 4, Collapse Icon
            if (!string.IsNullOrEmpty(CollapseIconUrl))
            {
                s.Append(", ");
                s.Append(GetOption("collapseIconUrl", this.CollapseIconUrl, true));                
            }

            //Param 5, Expand Icon Alt Tag
            s.Append(", ");
            if (!string.IsNullOrEmpty(ExpandIconAltTag))
                s.Append(GetOption("expandIconAltTag", this.ExpandIconAltTag, true));
            else
                s.Append(GetOption("expandIconAltTag", "Expand " + this.Title + " Panel", true));

            //Param 5, Collapse Icon Alt Tag
            s.Append(", ");
            if (!string.IsNullOrEmpty(CollapseIconAltTag))
                s.Append(GetOption("collapseIconAltTag", this.CollapseIconAltTag, true));
            else
                s.Append(GetOption("collapseIconAltTag", "Collapse " + this.Title + " Panel", true));

            //Param 7, Height
            s.Append(", ");            
            s.Append(GetOption("iconHeight", this.IconHeight.ToString(), true));

            //Param 7, Width
            s.Append(", "); 
            s.Append(GetOption("iconWidth", this.IconWidth.ToString(), true));

            s.Append(" });");

            return s.ToString();
        }

        private string GetOption(string optName, string value, bool isString)
        {
            string s = optName + ": ";
            if (isString)
                s += "'";
            s += value;
            if (isString)
                s += "'";
            return s;
        }

        #region IPostBackDataHandler Members

        public virtual bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool presentValue = IsCollapsed;

            bool postedValue = Strings.ToBoolean(postCollection[postDataKey.Replace(this.IdSeparator, this.ClientIDSeparator) + "-collapsedStatus"]);

            if (!presentValue.Equals(postedValue))
            {
                IsCollapsed = postedValue;
                return true;
            }

            return false;
        }

        public virtual void RaisePostDataChangedEvent()
        {        
        }

        #endregion
    }
}
