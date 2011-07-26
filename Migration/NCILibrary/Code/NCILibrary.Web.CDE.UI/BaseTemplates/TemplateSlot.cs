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
    /// ASP.Net control which represents a Slot in a Percussion Global/Local template.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TemplateSlot runat=server></{0}:TemplateSlot>")]
    public class TemplateSlot : WebControl
    {
        /// <summary>
        /// Removes the HTML output of this control if there are no contents.
        /// </summary>
        [DefaultValue(false)]
        public bool RenderIfEmpty { get; set; }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets or Sets HTML to be rendered before this TemplateSlot.
        /// </summary>
        /// <remarks>
        ///   This is only rendered if the TemplateSlot has child controls or if 
        ///   RenderIfEmpty is true.  The HTML should be plain HTML with no ASP.Net controls.
        /// </remarks>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string HeaderHtml { get; set; }

        /// <summary>
        /// Gets or Sets HTML to be rendered after this TemplateSlot.
        /// </summary>
        /// <remarks>
        ///   This is only rendered if the TemplateSlot has child controls or if 
        ///   RenderIfEmpty is true.  The HTML should be plain HTML with no ASP.Net controls.
        ///   
        /// </remarks>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string FooterHtml { get; set; }


        /// <summary>
        /// Adds a Snippet Control to this TemplateSlot.
        /// </summary>
        /// <param name="snippetInfo">The snippet info.</param>
        public void AddSnippet(SnippetControl snippetControl)
        {
            TemplateSlotItem item = new TemplateSlotItem();
            this.Controls.Add(item);
            item.Controls.Add(snippetControl);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // If the control count is empty the template slot then do not draw 
            // unless forced to draw using RenderIfEmpty set to true.
            if (this.Controls.Count == 0 && !this.RenderIfEmpty)
                return;
            
            if (!String.IsNullOrEmpty(HeaderHtml))
                writer.Write(HeaderHtml);

            base.Render(writer);

            if (!String.IsNullOrEmpty(FooterHtml))
                writer.Write(FooterHtml);
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //Currently I am assuming this will only contain template slot items
            foreach (Control c in this.Controls)
            {
                TemplateSlotItem templateslotItem = c as TemplateSlotItem;
                if (templateslotItem != null)
                {
                    //Cannot have the snippet info object to be null.
                    if (templateslotItem.SnippetControl != null && templateslotItem.SnippetControl.SnippetInfo != null)
                    {
                        //Additional Information is added to the class attribute  
                        //to help QA team to identify the slots.
                        templateslotItem.CssClass += templateslotItem.SnippetControl.SnippetInfo.SlotName
                            + templateslotItem.SnippetControl.SnippetInfo.ContentID == null ? "" : "contentid-" +templateslotItem.SnippetControl.SnippetInfo.ContentID;
                    }
                }

                if (c is TemplateSlotItem)
                {
                    ((TemplateSlotItem)c).CssClass += " slot-item";

                    if (this.Controls.Count == 1)
                    {
                        ((TemplateSlotItem)c).CssClass += " only-SI";
                    }
                    else if (c == this.Controls[0])
                    {
                        ((TemplateSlotItem)c).CssClass += " first-SI";
                    }
                    if (c == this.Controls[this.Controls.Count - 1] && this.Controls.Count!=1)
                    {
                        ((TemplateSlotItem)c).CssClass += " last-SI";
                    }
                }

                c.RenderControl(writer);
            }
        }
    }
}
