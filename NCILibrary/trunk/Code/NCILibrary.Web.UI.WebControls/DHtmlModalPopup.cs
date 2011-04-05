using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web.UI.WebControls.JSLibraries;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DHtmlModalPopup runat=server></{0}:DHtmlModalPopup>")]
    public class DHtmlModalPopup : WebControl
    {

        /// <summary>
        /// The Opacity value of the overlay.  This must be a value between 0 and 1.
        /// The default is .5.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("0.5")]
        [Localizable(true)]
        public double OverlayOpacity
        {
            get
            {
                object s = ViewState["OverlayOpacity"];
                return ((s == null) ? 0.5 : (double)s);
            }

            set
            {
                if ((value > 0) && (value < 1))
                    ViewState["OverlayOpacity"] = value;
                else
                    throw new Exception("The OverlayOpacity value must be a value between 0 and 1");
            }
        }

        /// <summary>
        /// The HTML color of the overlay
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "")]
        [TypeConverter(typeof(WebColorConverter))] //So that we can take in #ffffff and have it work.
        [Localizable(true)]
        public Color OverlayColor
        {
            get
            {                
                object c = ViewState["OverlayColor"];
                return ((c == null) ? Color.Empty : (Color)c);                
            }

            set
            {
                ViewState["OverlayColor"] = value;
            }
        }

        /// <summary>
        /// The Z-Index of the overlay/modal popup.
        /// The default is 9990
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("9990")]
        [Localizable(true)]
        public int ZIndex
        {
            get
            {
                object s = ViewState["ZIndex"];
                return ((s == null) ? 9990 : (int)s);
            }

            set
            {
                ViewState["ZIndex"] = value;
            }
        }

        /// <summary>
        /// Gets and Sets the Client ID of the element that will open the popup when it is clicked
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OpenElementClientID
        {
            get
            {
                string str = (string)this.ViewState["OpenElementClientID"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OpenElementClientID"] = value;
            }
        }

        /// <summary>
        /// Gets and Sets the Client ID of the element that will close the popup.  This is for something like a cancel button.
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CloseElementClientID
        {
            get
            {
                string str = (string)this.ViewState["CloseElementClientID"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["CloseElementClientID"] = value;
            }
        }

        /// <summary>
        /// Client side script that is called before the modal popup is opened.
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OnClientBeforeOpen
        {
            get
            {
                string str = (string)this.ViewState["OnClientBeforeOpen"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OnClientBeforeOpen"] = value;
            }
        }

        /// <summary>
        /// Client side script that is called after the modal popup is opened.
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OnClientAfterOpen
        {
            get
            {
                string str = (string)this.ViewState["OnClientAfterOpen"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OnClientAfterOpen"] = value;
            }
        }

        /// <summary>
        /// Client side script that is called before the modal popup is closed.
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OnClientBeforeClose
        {
            get
            {
                string str = (string)this.ViewState["OnClientBeforeClose"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OnClientBeforeClose"] = value;
            }
        }

        /// <summary>
        /// Client side script that is called after the modal popup is closed.
        /// </summary>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string OnClientAfterClose
        {
            get
            {
                string str = (string)this.ViewState["OnClientAfterClose"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["OnClientAfterClose"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets the javascript that can be used to close the current window.
        /// </summary>
        /// <returns></returns>
        public string GetCloseCommand()
        {
            return "ModalPopup.closePopupIfOpen('" + this.ClientID + "');";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            PrototypeManager.Load(this.Page);            
            JSManager.AddResource(this.Page, typeof(DHtmlModalPopup), "NCI.Web.UI.WebControls.Resources.DHtmlModalPopup.js");

            //Remove any styles that we do not want! Must be done here because control state gets stored right after prerender.
            this.Style.Remove(HtmlTextWriterStyle.Display);
            this.Style.Remove(HtmlTextWriterStyle.Position);
            this.Style.Remove(HtmlTextWriterStyle.Top);
            this.Style.Remove(HtmlTextWriterStyle.Left);
            this.Style.Remove(HtmlTextWriterStyle.ZIndex);            

            //This popup should not show up by default.
            this.Style.Add(HtmlTextWriterStyle.Display, "none");            
        }

        protected override void Render(HtmlTextWriter output)
        {            
            base.RenderBeginTag(output); //I really would like it if height and width did not get
                                         //rendered out here, but it will cause more trouble than it is worth.

            RenderContents(output); //Actually this is ok because we should not have to mess with the contents.

            base.RenderEndTag(output);
            RenderScriptBlock(output);
        }

        private void RenderScriptBlock(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);

            output.Write("var ");
            output.Write(ClientID);
            output.Write("_obj = new ModalPopup('");
            output.Write(ClientID);
            output.Write("', '");
            output.Write(OpenElementClientID);
            output.Write("', ");
            if (!string.IsNullOrEmpty(CloseElementClientID))
            {
                output.Write("'");
                output.Write(CloseElementClientID);
                output.Write("', ");
            }
            else
            {
                output.Write("false, ");
            }

            //Options
            output.Write("{");
            if (Width != Unit.Empty)
                output.Write(String.Format("width: {0},", Width.ToString()));
            if (Height != Unit.Empty)
                output.Write(String.Format("height: {0},", Height.ToString()));
            
            output.Write(String.Format("overlayOpacity: {0}, ", OverlayOpacity.ToString()));
            output.Write(String.Format("zIndex: {0}", ZIndex.ToString()));
            
            if (OverlayColor != Color.Empty)
                output.Write(String.Format(", overlayBackgroundColor: '{0}'", ColorTranslator.ToHtml(OverlayColor)));

            if (!string.IsNullOrEmpty(OnClientBeforeOpen))
                output.Write(RenderCallback("beforeOpen", OnClientBeforeOpen));

            if (!string.IsNullOrEmpty(OnClientAfterOpen))
                output.Write(RenderCallback("afterOpen", OnClientAfterOpen));

            if (!string.IsNullOrEmpty(OnClientBeforeClose))
                output.Write(RenderCallback("beforeClose", OnClientBeforeClose));

            if (!string.IsNullOrEmpty(OnClientAfterClose))
                output.Write(RenderCallback("afterClose", OnClientAfterClose));

            output.Write("});");

            output.RenderEndTag();
        }

        private string RenderCallback(string callbackName, string callbackCode)
        {
            /*
             * Ok, here is how this works, the callback should be a function call,
             * so it should look like: 
             * afterClose: FunctionName
             * however if you want your own function call or code then it needs to be in a closure
             * afterClose: function() { function(a, b, c) { some code here}}
             * or
             * afterClose: function() { some code here }
             * then after close is called it calls that function that you already passed values in
             * to.  Now if you want real closures, then you cannot use this.
             * 
             * Our problem is taking in whatever is thrown at us and allowing this to work.
             */

            if (callbackCode.IndexOfAny(new char[] { ';', '{', '(', ' ', '=' }) != -1)
            {
                //This is most likely regualar code and not a function name,
                //so we need to wrap it.
                callbackCode = "function() { " + callbackCode + " }";
            }

            return String.Format(", {0}: {1}", callbackName, callbackCode);
        }

    }
}
