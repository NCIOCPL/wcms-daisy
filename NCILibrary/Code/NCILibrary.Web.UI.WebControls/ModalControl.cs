using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NCI.Web.UI.WebControls.Menus;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// 1. "modalPage" is set to cover the entire page, and it's set as "display: none"
    /// so that initially you can't see it. 
    /// 2. "modalBackground" sets up a gray screen over the existing page. 
    /// Note the hacks to get opacity to work in all of the browsers (works in Safari too). 
    /// The z-index is one of several we'll set so that it's layered correctly. 
    /// 3. "modalContainer" is next and is set up further in the z-index, 
    /// with its top left corner positioned at the center of the page. 
    /// 4. "modal" is set in the container, and will be the same size, 
    /// so to make it appear in the right place, we need to set its dimensions, 
    /// but make its position relative to the container. 
    /// Since the container's top-left is dead center of the page, 
    /// we want to go half the width and height from that spot, in a negative direction. 
    /// This z-index is highest because it's on top. 
    /// 5. The other elements are to setup the content in the "window" that will sit in the middle of the page. 
    /// </summary>
    [ToolboxData("<{0}:ModalControl runat=server></{0}:ModalControl>")]
    public class ModalControl : WebControl, INamingContainer
    {
        #region private variables
        private TextBox _txtNotes = new TextBox();
        private RequiredFieldValidator _rfv = new RequiredFieldValidator();
        string _transitionID = string.Empty;
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets notes
        /// </summary>
        public string Notes
        {
            get
            {
                return _txtNotes.Text;
            }
            set
            {
                _txtNotes.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets notes client ID
        /// </summary>
        public string NotesClientID
        {
            get
            {
                return _txtNotes.ClientID;
            }
        }
        
        #endregion

        #region public constructor
        /// <summary>
        /// Initializes a new instance of the ModalControl class. Defines base tag as div
        /// </summary>
        /// <param name="transitionID"></param>
        public ModalControl(string transitionID): base("div")
        {
            _transitionID = transitionID;
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Overridden.  Defines child controls' IDs and its own styles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //_txtNotes = new TextBox();
            _txtNotes.ID = "txtNotes_" + _transitionID ; 

            //_rfv = new RequiredFieldValidator();
            _rfv.ID = "rfvNotes_" + _transitionID; 

            this.Style.Add("display", "none");
            this.Style.Add("position", "absolute");
            this.Style.Add("width", "100%");
            this.Style.Add("height", "100%");
            this.Style.Add("top", "0px");
            this.Style.Add("left", "0px");

            base.OnInit(e);
            EnsureChildControls();
        }

        /// <summary>
        /// Add child controls to its control collection
        /// </summary>
        protected override void CreateChildControls()
        {            
            // CreateGridView();
            Controls.Add(_txtNotes);
            Controls.Add(_rfv);
            _txtNotes.TextMode = TextBoxMode.MultiLine;
            _txtNotes.EnableViewState = true;
            _txtNotes.Width = (Unit)250;
            _txtNotes.Height = (Unit)100;
            _txtNotes.ValidationGroup = "ValidationGroup_" + _transitionID;

            _rfv.ErrorMessage = "Notes must be entered before clicking OK.";
            _rfv.EnableClientScript = true;
            _rfv.EnableViewState = true;
            _rfv.ControlToValidate = _txtNotes.ID;
            _rfv.ValidationGroup = "ValidationGroup_" + _transitionID;         
            base.CreateChildControls();
        }

        /// <summary>
        /// Renders control as a div and all its child controls
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //Draw modalBackground
            output.WriteBeginTag("div");
            output.WriteAttribute("class", "modalBackground");
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEndTag("div");

            //Draw modalContainer
            output.WriteBeginTag("div");
            output.WriteAttribute("class", "modalContainer");
            output.Write(HtmlTextWriter.TagRightChar);

            //Draw modal
            output.WriteBeginTag("div");
            output.WriteAttribute("class", "modal");
            output.Write(HtmlTextWriter.TagRightChar);

            //Draw modalBody
            output.WriteBeginTag("div");
            output.WriteAttribute("class", "modalBody");
            output.Write(HtmlTextWriter.TagRightChar);

            //Draw content inside modal box
            output.Write("Notes:");
            output.Write("<br />");
            output.Write("<br />");

            _txtNotes.RenderControl(output);

            output.Write("<br />");
            output.Write("<br />");

            //required field
            _rfv.RenderControl(output);

            output.Write("<br />");
            output.WriteBeginTag("p");
            output.WriteAttribute("style", "text-align:center");
            output.Write(HtmlTextWriter.TagRightChar);

            //buttons            //editor menu button for submit
            output.WriteBeginTag("ul");
            output.WriteAttribute("class", "modaltabs");
            output.Write(HtmlTextWriter.TagRightChar);

            //contents
            foreach (WebControl control in this.Controls)
            {
                MenuBarButton button = control as MenuBarButton;
                if (button != null)
                {
                    output.WriteBeginTag("li");
                    output.Write(HtmlTextWriter.TagRightChar);
                    button.RenderControl(output);
                    output.WriteEndTag("li");
                }
            }

            output.WriteEndTag("ul");

            output.WriteEndTag("p");
            //End of content

            output.WriteEndTag("div");
            output.WriteEndTag("div");
            output.WriteEndTag("div");

        }

        /// <summary>
        /// Raises the Load event. Adds Javascript and CSS to page header. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Add Javascript

            Type t = this.GetType();
            string ModalControl_JS = "NCI.Web.UI.WebControls.Resources.ModalControl.ModalControl.js";
            string url = Page.ClientScript.GetWebResourceUrl(t, ModalControl_JS);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(t, ModalControl_JS))
                Page.ClientScript.RegisterClientScriptInclude(t, ModalControl_JS, url);

            //Add CSS
            string includeTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
            string includeLocation = Page.ClientScript.GetWebResourceUrl(this.GetType(), "NCI.Web.UI.WebControls.Resources.ModalControl.ModalControl.css");
            LiteralControl include = new LiteralControl(String.Format(includeTemplate, includeLocation));
            ((HtmlHead) Page.Header).Controls.Add(include);

        }
         #endregion
    }
}
