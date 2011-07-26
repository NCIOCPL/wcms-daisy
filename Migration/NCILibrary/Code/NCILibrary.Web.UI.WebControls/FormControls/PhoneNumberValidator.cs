using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.FormControls
{
    /// <summary>
    /// Phone Validator derived from CustomValidator class
    /// </summary>
    [ToolboxData("<{0}:PhoneNumberValidator runat=\"server\" " + "ErrorMessage=\"PhoneNumberValidator\" " + "Text=\"*\"></{0}:PhoneNumberValidator>")]
    [
    System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")
    ]
    public class PhoneNumberValidator : CustomValidator
    {
        [Category("Behavior"), DefaultValue(true), Description("Whether a value is required.")]
        public virtual bool IsRequired
        {
            get
            {
                object b = ViewState["isRequired"];
                return (b == null) ? true : (bool)b;
            }
            set
            {
                ViewState["isRequired"] = value;
            }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(0), Description("The maximum length of a valid value")]
        public virtual int MaximumLength
        {
            get
            {
                object i = ViewState["maxLength"];
                return (i == null) ? 0 : (int)i;
            }
            set
            {
                ViewState["maxLength"] = value;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (RenderUplevel)
            {
                writer.AddAttribute("isRequired", IsRequired.ToString(CultureInfo.InvariantCulture));
                writer.AddAttribute("maxLength", MaximumLength.ToString(CultureInfo.InvariantCulture));
            }
        }

        protected override bool OnServerValidate(string value)
        {
            //string value = GetControlValidationValue(ControlToValidate);

            if (IsRequired == false && String.IsNullOrEmpty(value))
            {
                return true;
            }
            else if (IsRequired == true && String.IsNullOrEmpty(value))
            {
                return false;
            }
            value = value.Trim().Replace("-", "").Replace(",", "");
            string pattern = @"^(\d{3})(\d{3})(\d{4})$";

            return Regex.IsMatch(value, pattern);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (RenderUplevel)
            {

                this.Page.ClientScript.RegisterClientScriptResource(
                    typeof(PhoneNumberControl)
                    , "myNCI.FormControls.Resources.PhoneNumberValidator.js");
            }
            base.OnPreRender(e);
        }

    }  
}
