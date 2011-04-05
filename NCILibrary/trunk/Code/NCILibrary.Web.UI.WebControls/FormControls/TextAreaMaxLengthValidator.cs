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
    /// Extended TextBox control that applies javascript validation to Multiline TextBox
    /// Source -- http://www.codeproject.com/KB/aspnet/Textarea_Length_Validator.aspx
    /// </summary>
    [ToolboxData("<{0}:TextAreaMaxLengthValidator runat=\"server\" " + "ErrorMessage=\"TextAreaMaxLengthValidator\" " + "Text=\"*\"></{0}:TextAreaMaxLengthValidator>")]
    public class TextAreaMaxLengthValidator : CustomValidator
    {
        [Bindable(true), Category("Behavior"), DefaultValue(-1), Description("The maximum length of a valid value")]
        public virtual int MaximumLength
        {
            get
            {
                object i = ViewState["maxLength"];
                return (i == null) ? -1 : (int)i;
            }
            set
            {
                ViewState["maxLength"] = value;
            }
        }

        protected override bool EvaluateIsValid()
        {
            if (MaximumLength == 0)
                return true;

            string controlValueToValidate = base.GetControlValidationValue(base.ControlToValidate);
            return (controlValueToValidate.Length <= MaximumLength);
        }
        
        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

        //    if (RenderUplevel)
        //    {
        //        Page.ClientScript.RegisterClientScriptInclude(
        //                  typeof(TextAreaMaxLengthControl),
        //                  "TextArea",
        //                  "NCI.Web.UI.WebControls.FormControls.Resources.TextAreaMaxLengthValidator.js");
        //    }
        //}
    }
}
