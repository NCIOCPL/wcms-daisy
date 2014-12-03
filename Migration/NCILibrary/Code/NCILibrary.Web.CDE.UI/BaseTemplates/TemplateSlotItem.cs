using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// This class is a container for Snippet control. This class also renders a div tag around
    /// Snippet control.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TemplateSlotItem runat=server></{0}:TemplateSlotItem>")]
    public class TemplateSlotItem : WebControl
    {
        /// <summary>
        /// Returns the snippet control which is the first element 
        /// in the control collection.
        /// </summary>
        public SnippetControl SnippetControl
        {
            get {
                if (Controls.Count > 0)
                    return (SnippetControl)Controls[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Returns the HtmlTextWriterTag value that corresponds to TemplateSlotItem
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

    }
}
