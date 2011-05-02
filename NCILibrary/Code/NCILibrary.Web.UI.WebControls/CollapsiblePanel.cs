using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    public enum PanelState
    {
        Collapsed,
        Expanded
    }

    /// <summary>
    /// Provides a grouping mechanism for organizing controls, which is Collapsible
    /// </summary>
    [ToolboxData("<{0}:CollapsiblePanel runat=server></{0}:CollapsiblePanel>")]
    public class CollapsiblePanel : WebControl , INamingContainer
    {
        #region private variables
        private const string GRIDVIEW_JS = "NCI.Web.UI.WebControls.Resources.CollapsiblePanel.CollapsiblePanel.js";
        private string _title = string.Empty;
        private string _functionName = string.Empty;
        private string _errorMessage = string.Empty;
        private MenuControl _menu;
        private Control _actionControl;
        private bool hasActionControlBeenAdded = false;

        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Gets or sets function name
        /// </summary>
        public string FunctionName
        {
            get { return _functionName; }
            set { _functionName = value; }
        }

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// Sets menu bar for the panel
        /// </summary>
        public MenuControl MenuBar
        {
            get { return _menu; }
        }

        /// <summary>
        /// Gets or sets the width of the panel
        /// </summary>
        [Browsable(true), Category("NewDynamic")]
        [Description("Get/Set the width of the panel")]
        public new string Width
        {
            get
            {
                if (ViewState["Width"] != null)
                {
                    return ViewState["Width"].ToString();
                }
                else
                {
                    return "600px";
                }
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// Get/Set the state of panel
        /// </summary>
        [DefaultValue(PanelState.Collapsed), Category("NewDynamic")]
        [Description("Get/Set the state of panel")]
        public PanelState State
        {
            get
            {
                if (ViewState["State"] != null)
                    return (PanelState)Enum.Parse(typeof(PanelState), ViewState["State"].ToString());
                else
                    return PanelState.Collapsed;
            }
            set { ViewState["State"] = value; }
        }

        /// <summary>
        /// Gets Image Url for the expand button burried in this assembly.
        /// </summary>
        protected string ResourceExpandButtonUrl
        {
            get
            {
                string result = String.Empty;
                if (null != Page)
                {
                    result = Page.ClientScript.GetWebResourceUrl(this.GetType(),
                        "NCI.Web.UI.WebControls.Resources.CollapsiblePanel.icon-expand.jpg");
                }
                return result;
            }
        }

        /// <summary>
        /// Gets Image Url for the Collapse button burried in this assembly.
        /// </summary>
        protected string ResourceCollapseButtonUrl
        {
            get
            {
                string result = String.Empty;
                if (null != Page)
                {
                    result = Page.ClientScript.GetWebResourceUrl(this.GetType(),
                        "NCI.Web.UI.WebControls.Resources.CollapsiblePanel.icon-collapse.jpg");
                }
                return result;
            }
        }

        /// <summary>
        /// Gets or sets action control for the panel
        /// </summary>
        public Control ActionControl
        {
            set
            {
                _actionControl = value;
                if (!hasActionControlBeenAdded)
                {
                    this.Controls.Add(_actionControl);
                }
            }

            get { return _actionControl; }
        }
        #endregion

        #region public Constructor
        /// <summary>
        /// Initializes a new instance of the CollapsiblePanel class. Defines base tag as div.
        /// </summary>
        public CollapsiblePanel()
            : base("div")
        {
            _menu = new MenuControl();
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Overridden.  Defines menubar's ID and add menubar to its control collection
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            _menu.ID = "MenuBar";
            base.Controls.Add(_menu);

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the Load event. Adds Javascript and CSS to page header. 
        /// Registers panel states as a hidden field.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Type t = this.GetType();
            string url = Page.ClientScript.GetWebResourceUrl(t, GRIDVIEW_JS);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, GRIDVIEW_JS))
                Page.ClientScript.RegisterClientScriptInclude(t, GRIDVIEW_JS, url);

            string cssTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\"/>";
            string cssLocation = Page.ClientScript.GetWebResourceUrl(GetType(), "NCI.Web.UI.WebControls.Resources.CollapsiblePanel.CollapsiblePanel.css");
            LiteralControl css = new LiteralControl(String.Format(cssTemplate, cssLocation));

            if (!Page.Header.Controls.Contains(css))
                Page.Header.Controls.Add(css);

            if (!DesignMode && Page.Request[this.ClientID + "_State"] != null)
            {
                State = (PanelState)Enum.Parse(typeof(PanelState), Page.Request[this.ClientID + "_State"]);
            }
            Page.ClientScript.RegisterHiddenField(this.ClientID + "_" + "State", State.ToString());
        }

        /// <summary>
        /// Render control as a div and all its child controls
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {


            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "collapsible");
            if (this.ActionControl != null && this.ActionControl.GetType().BaseType == typeof(GridView) )
            {
                if (((GridView)this.ActionControl).Width != Unit.Empty)
                    this.Width = ((GridView)this.ActionControl).Width.ToString();
            }
            writer.WriteAttribute("style", "width:" + Width);
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("div");
            writer.WriteAttribute("style", "float: left;");
            writer.WriteAttribute("style", "font-family:Arial;font-size=14px; color:#000000");
            writer.Write(HtmlTextWriter.TagRightChar);

            //Title & function

            writer.WriteBeginTag("span");
            writer.WriteAttribute("style", "font-weight: bold;");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(_title);
            writer.WriteEndTag("span");

            if (!string.IsNullOrEmpty(_functionName))
            {
                writer.Write(" : ");
                writer.Write(_functionName);
            }
            writer.WriteEndTag("div");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("style", "float: right;");
            writer.Write(HtmlTextWriter.TagRightChar);

            if (this._actionControl != null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);

                writer.Write("{0}_cgridViewObject = new CollapsiblePanel('{0}','{1}','{2}','{3}') ;\n", this._actionControl.ClientID, "btnExpCol" + this.ClientID, ResourceExpandButtonUrl, ResourceCollapseButtonUrl);

                writer.RenderEndTag();

                writer.WriteBeginTag("a");

                writer.WriteAttribute("OnClick", string.Format("{0}_cgridViewObject.ShowHideGridView();", this._actionControl.ClientID));
                writer.Write(HtmlTextWriter.TagRightChar);

                string src = (State == PanelState.Collapsed ? ResourceExpandButtonUrl : ResourceCollapseButtonUrl);
                string alt = (State == PanelState.Collapsed ? "Expand" : "Collapse");
                writer.WriteBeginTag("img");
                writer.WriteAttribute("id", "btnExpCol" + this.ClientID);
                writer.WriteAttribute("src", src);
                writer.WriteAttribute("width", "13");
                writer.WriteAttribute("height", "13");
                writer.WriteAttribute("alt", alt);
                writer.WriteAttribute("border", "0");
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                writer.WriteEndTag("a");
            }

            writer.WriteEndTag("div");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("style", "clear: both;");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("div");


            //Gray line
            writer.WriteBeginTag("div");
            writer.WriteAttribute("style", "background-color: #E6E6E6;padding-bottom: 5px; font-size:2px; margin-top: 10px; margin-bottom: 10px;");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&nbsp;");
            writer.WriteEndTag("div");

            //Menu
            if (_menu != null)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("style", "margin-top: 5px; margin-bottom: 10px;");
                writer.Write(HtmlTextWriter.TagRightChar);
                _menu.RenderControl(writer);
                writer.WriteEndTag("div");
            }
            writer.WriteBreak();     

            if (!string.IsNullOrEmpty(_errorMessage))
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("style", "margin-top: 5px; margin-bottom: 10px;");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(_errorMessage);
                writer.WriteEndTag("div");
            }

            if (_actionControl != null)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("style", "font-family: Verdana, Lucida, Geneva, Helvetica, Arial, sans-serif; font-size: 11px;");
                writer.Write(HtmlTextWriter.TagRightChar);

                _actionControl.RenderControl(writer);

                writer.WriteEndTag("div");
            }
            writer.WriteEndTag("div");
        }
        #endregion
    }
}
