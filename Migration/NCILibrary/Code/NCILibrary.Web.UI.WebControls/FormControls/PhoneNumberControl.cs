using System;
using System.Configuration;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace NCI.Web.UI.WebControls.FormControls
{
    /// <summary>
    /// Phone Number Custom Server Control which implements the IPostBackDataHandler interface
    /// </summary>
    [
    System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust"), ValidationProperty("PhoneNumber")
    ]
    public class PhoneNumberControl : WebControl, IPostBackDataHandler
    {
        #region Fields

        #endregion Fields

        /// <summary>
        /// An enum to decide if the label should be placed left or top to the input box
        /// </summary>
        public enum LabelPositions
        {
            Top = 1
            , Left = 0

        }

        /// <summary>
        /// sets/returns the Area Code
        /// </summary>
        public String AreaCode
        {
            get
            {
                string s = (String)ViewState["AreaCode"];
                return (s == null) ? String.Empty : s;
            }

            set
            {
                ViewState["AreaCode"] = value;
            }
        }

        /// <summary>
        /// sets/returns Exchange code
        /// </summary>
        public String ExchangeCode
        {
            get
            {
                string s = (String)ViewState["ExchangeCode"];
                return (s == null) ? String.Empty : s;
            }

            set
            {
                ViewState["ExchangeCode"] = value;
            }
        }

        /// <summary>
        /// sets/returns rest of the number
        /// </summary>
        public String LocalNumber
        {
            get
            {
                string s = (String)ViewState["LocalNumber"];
                return (s == null) ? String.Empty : s;
            }

            set
            {
                ViewState["LocalNumber"] = value;
            }
        }

        /// <summary>
        /// Sets/retrieves label for Phone number control from the view state
        /// </summary>
        public String LabelText
        {
            get
            {
                String s = (String)ViewState["LabelText"];
                return (s == null) ? "Phone Number" : s;
            }
            set
            {
                ViewState["LabelText"] = value;
            }
        }

        /// <summary>
        /// Label Position
        /// </summary>
        public LabelPositions LabelPosition
        {
            get
            {
                object o = ViewState["LabelPosition"];
                return (o == null) ? LabelPositions.Left : (LabelPositions)o;
            }
            set
            {
                ViewState["LabelPosition"] = value;
            }
        }

        /// <summary>
        /// Gets/sets PhoneNumber (10 digit phone number)
        /// </summary>
        public String PhoneNumber
        {
            get
            {
                string s = string.Empty;
                if (string.IsNullOrEmpty(AreaCode + ExchangeCode + LocalNumber) == false)
                {
                    s = string.Format("{0}{1}{2}", AreaCode, ExchangeCode, LocalNumber);
                }
                return s;
            }
        }

        /// <summary>
        /// sets/returns display number
        /// </summary>
        public String DisplayNumber
        {
            get
            {
                string displayFormat = "{0}-{1}-{2}";
                string o = ConfigurationManager.AppSettings["PhoneDisplayFormat"];
                displayFormat = (o != null) ? o.ToString() : displayFormat;

                string s = string.Empty;
                if (string.IsNullOrEmpty(AreaCode + ExchangeCode + LocalNumber) == false)
                {
                    s = string.Format(displayFormat, AreaCode, ExchangeCode, LocalNumber);
                }
                return s;
            }
        }

        /// <summary>
        /// Sets the Phone Number
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void SetPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return;
            if (phoneNumber.Length.Equals(10))
            {
                AreaCode = phoneNumber.Substring(0, 3);
                ExchangeCode = phoneNumber.Substring(3, 3);
                LocalNumber = phoneNumber.Substring(6, 4);
            }

        }

        /// <summary>
        /// collect posted data to the server
        /// </summary>
        /// <param name="postDataKey">The key identifier for the control</param>
        /// <param name="postCollection">The collection of name/value pairs posted to the server</param>
        /// <returns></returns>
        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool rtnVal = true;
            String postedValue = postCollection[ClientID];
            if (string.IsNullOrEmpty(postedValue) == false)
            {
                SetPhoneNumber(postedValue.Replace(",", ""));
            }
            return rtnVal;
        }

        public virtual void RaisePostDataChangedEvent()
        {
            OnPhoneNumberChanged(EventArgs.Empty);
        }

        public event EventHandler PhoneNumberChanged;

        protected virtual void OnPhoneNumberChanged(EventArgs e)
        {
            if (PhoneNumberChanged != null)
                PhoneNumberChanged(this, e);
        }

        #region Overriden methods & properties

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Page.RegisterRequiresPostBack(this);
            RegisterClientScriptBlock();
            base.OnPreRender(e);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //0.0 add initial label for screen reader users            

            if (string.IsNullOrEmpty(LabelText) == false)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(LabelText);
                writer.RenderEndTag();
            }

            if (LabelPosition == LabelPositions.Top && String.IsNullOrEmpty(LabelText) == false)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Br);
            }

            //1.0 add area code label for screen reader users
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden-label");
            writer.AddAttribute(HtmlTextWriterAttribute.For, this.ClientID + "_AreaCode");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(string.Format("{0} {1}", LabelText, "Area Code"));
            writer.RenderEndTag();

            //1.1 Render Area code - (301)   
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write("(");
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_AreaCode");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
            writer.AddAttribute("onkeyup", "CheckAndJump(event, this,'" + this.ClientID + "_Exchange')");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "24px");
            writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "3");
            writer.AddAttribute("value", this.AreaCode);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(")");
            writer.RenderEndTag();

            //add a space
            writer.Write(HtmlTextWriter.SpaceChar);

            //2.0 add label for screen reader users
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden-label");
            writer.AddAttribute(HtmlTextWriterAttribute.For, this.ClientID + "_Exchange");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(string.Format("{0} {1}", LabelText, "first 3 digits"));
            writer.RenderEndTag();

            //2.1 Render Exchange Code
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_Exchange");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
            writer.AddAttribute("onkeyup", "CheckAndJump(event, this,'" + this.ClientID + "_LocalNumber')");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "24px");
            writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "3");
            writer.AddAttribute("value", this.ExchangeCode);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            //add a space
            //writer.Write(HtmlTextWriter.SpaceChar);

            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0px 2px 0px 2px");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write("-");
            writer.RenderEndTag();

            //add a space
            //writer.Write(HtmlTextWriter.SpaceChar);

            //3.0 add label for screen reader users
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden-label");
            writer.AddAttribute(HtmlTextWriterAttribute.For, this.ClientID + "_LocalNumber");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(string.Format("{0} {1}", LabelText, "last 4 digits"));
            writer.RenderEndTag();

            //3.1 Render Rest of the number
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_LocalNumber");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
            writer.AddAttribute("onkeyup", "CheckAndJump(event, this, null)");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "32px");
            writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "4");
            writer.AddAttribute("value", this.LocalNumber);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion Overriden methods & properties

        /// <summary>
        /// Registers Jump To Next Field Client Script Block
        /// </summary>
        private void RegisterClientScriptBlock()
        {
            this.Page.ClientScript.RegisterClientScriptResource(
                typeof(PhoneNumberControl)
                , "myNCI.FormControls.Resources.PhoneNumberValidator.js");
        }
    }
}
