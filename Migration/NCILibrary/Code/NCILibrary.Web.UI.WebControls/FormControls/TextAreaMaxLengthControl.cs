using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.FormControls
{
    /// <summary>
    /// Extended TextBox control that applies javascript validation to Multiline TextBox
    /// Source -- http://www.codeproject.com/KB/aspnet/Textarea_Length_Validator.aspx
    /// </summary>
    public class TextAreaMaxLengthControl : TextBox
    {
        /// <summary>
        /// Override PreRender to include custom javascript
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (MaxLength > 0 && TextMode == TextBoxMode.MultiLine)
            {
                // Add javascript handlers for paste and keypress
               // Attributes.Add("onkeypress", "textAreaMaxLengthDoKeypress(this);");
               // Attributes.Add("onbeforepaste", "textAreaMaxLengthDoBeforePaste(this);");
               // Attributes.Add("onpaste", "textAreaMaxLengthDoPaste(this);");
                Attributes.Add("onmousemove", "TextMaxLenValidator.countChars(event, this, '"+ this.ClientID+ "_count')");
                Attributes.Add("onpropertychange", "TextMaxLenValidator.countChars(event, this, '" + this.ClientID + "_count')");

                // Add attribute for access of maxlength property on client-side
                Attributes.Add("maxLength", this.MaxLength.ToString());

                // Register client side include - only once per page
                JSManager.AddResource(this.Page, typeof(TextAreaMaxLengthControl),
                        "NCI.Web.UI.WebControls.FormControls.Resources.TextAreaMaxLengthValidator.js");

                this.Page.ClientScript.RegisterStartupScript(typeof(TextAreaMaxLengthControl), "SetMaxLength",
                    "<script type=\"text/javascript\">TextMaxLenValidator.setMaxLength(" + this.MaxLength.ToString() + ", '" + this.ClientID + "_count') </script>");
            }
        }

        public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);

            writer.Write("<br><div id=\""+ this.ClientID+ "_count\"></div>");
        }
    }
}
