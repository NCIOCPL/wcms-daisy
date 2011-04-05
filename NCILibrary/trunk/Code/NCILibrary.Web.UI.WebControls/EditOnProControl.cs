using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// It is a compostie control having a third party editonpro embeded in it.
    /// It serves an WYSISYG HTML editor with powerful editing functions.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EditOnProControl runat=server></{0}:EditOnProControl>")]
    public class EditOnProControl : CompositeControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]

        #region Fields
        private const string _jsFileName = "editonpro.js";

        private HiddenField _HtmlText = new HiddenField();

        private string _editOnProBase = string.Empty;
        private string _jsFileLocation = string.Empty;       
        private string _UIConfigFile = string.Empty;
        private string _configFile = string.Empty;
        private string _styleSheetURL = string.Empty;
        private string _additionalCss = string.Empty;
        private string _bodyClassName = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the path to the editonpro.js javascript file.
        /// <remarks>Normally this will be _editOnProBase + "/editonpro.js"  
        /// </remarks>
        /// </summary>
        public string JSFileLocation
        {
            get { return _jsFileLocation; }
            set { _jsFileLocation = Strings.Clean(value,string.Empty); }
        }

        public string EditOnProBase
        {
            get { return _editOnProBase; }
            set { _editOnProBase = Strings.Clean(value,string.Empty); }
        }

        /// <summary>
        /// Gets or sets the name of the Config file
        /// </summary>
        public string ConfigFile
        {
            get { return _configFile; }
            set { _configFile = Strings.Clean(value,string.Empty); }
        }

        /// <summary>
        /// Gets or sets the name of the UI config file
        /// </summary>
        public string UIConfigFile
        {
            get { return _UIConfigFile; }
            set { _UIConfigFile = Strings.Clean(value,string.Empty); }
        }

        /// <summary>
        /// Gets or sets the URL of a style sheet to be used by EOP
        /// </summary>
        public string StyleSheetURL
        {
            get { return _styleSheetURL; }
            set { _styleSheetURL =Strings.Clean( value,string.Empty); }
        }

        /// <summary>
        /// Gets or sets additional css styles to be used by EOP
        /// </summary>
        public string AdditionalCss
        {
            get { return _additionalCss; }
            set { _additionalCss = Strings.Clean(value,string.Empty); }
        }

        /// <summary>
        /// Gets or sets the html content of the editor 
        /// </summary>
        public string Text
        {
            get
            {
                String s = (String)_HtmlText.Value;
                return s;
            }

            set
            {
                String s =(string)value;
                _HtmlText.Value = s;
            }
        }

        /// <summary>
        /// Gets or sets the class name of the body tag
        /// </summary>
        public string BodyClassName
        {
            get { return _bodyClassName; }
            set { _bodyClassName = value; }
        }

        #endregion


        public EditOnProControl()
        {
            this.Height = Unit.Pixel(420);
            this.Width = Unit.Pixel(710);
        }

        /// <summary>
        /// Create editoronpro control layout
        /// </summary>
        protected override void CreateChildControls()
        {            
            _HtmlText.ID = "HTMLText";
            this.Controls.Add(_HtmlText);
        }

        /// <summary>
        /// Register the javascript block of Form.Onsubmit and Form.OnLoad
        /// Put the content from hiddenfield to editonpro in Form.Onload
        /// Put the contrent from editonpro to hiddenfield in Form.Onsubmit
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            if (string.IsNullOrEmpty(_jsFileLocation))
                if (_editOnProBase.EndsWith("/"))
                    _jsFileLocation = _editOnProBase + _jsFileName;
                else
                    _jsFileLocation = _editOnProBase + "/" + _jsFileName;
            else
                if (_jsFileLocation.EndsWith("/"))
                    _jsFileLocation += _jsFileName;
                else
                    _jsFileLocation += "/" + _jsFileName;
 
            //Load EOP Javascript
            Page.ClientScript.RegisterClientScriptInclude("EditProJS", _jsFileLocation);

            //Load our wrapper around the EOP Javascript
            Page.ClientScript.RegisterClientScriptResource(typeof(EditOnProControl), "NCI.Web.UI.WebControls.Resources.EditOnPro.NCILibraryEOP.js");

            //Register an onSubmit handler for copying the text from the EOP applet to the hidden input field
            Page.ClientScript.RegisterOnSubmitStatement(typeof(EditOnProControl), this.ClientID, this.ClientID + "_eopObject.saveData();");
        }


        /// <summary>
        /// Renders an EOP object property call.
        /// </summary>
        /// <param name="output">A HtmlTextWriter</param>
        /// <param name="propName">The name of the property</param>
        /// <param name="value">The value to set it to</param>
        private void RenderEOPProperty(HtmlTextWriter output, string propName, string value)
        {
            output.Write("{0}_eopObject.{1} = '{2}';\n", this.ClientID, propName, value);
        }

        /// <summary>
        /// Render the contents of this control
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {            
            //Render out the hidden html input field
            _HtmlText.RenderControl(output);

            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);

            output.Write("{0}_eopObject = new NCILibraryEOP('{0}');\n", this.ClientID);

            if (_editOnProBase!=string.Empty)
                RenderEOPProperty(output, "codeBase", _editOnProBase);
            
            RenderEOPProperty(output, "height", Height.ToString());

            RenderEOPProperty(output, "width", Width.ToString());

            if (_UIConfigFile != string.Empty)
                RenderEOPProperty(output, "uiConfigURL", _UIConfigFile);

            if (_configFile!=string.Empty)
                RenderEOPProperty(output, "configURL", _configFile);

            if (_bodyClassName!=string.Empty)
                RenderEOPProperty(output, "bodyClassName", _bodyClassName);

            if (_styleSheetURL!=string.Empty)
                RenderEOPProperty(output, "styleSheetURL", _styleSheetURL);

            if (_additionalCss != string.Empty)
                RenderEOPProperty(output, "additionalCSS", _additionalCss);
            

            output.Write("{0}_eopObject.load();\n", this.ClientID);
            output.RenderEndTag();
        }
    }
}

