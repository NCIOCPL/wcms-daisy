using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using NCI.Text;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:GenericHtmlContentSnippet runat=server></{0}:GenericHtmlContentSnippet>")]
    public class GenericHtmlContentSnippet : SnippetControl
    {
        string _htmlData = String.Empty;

        public string HtmlData
        {
            get { return _htmlData; }
            set { _htmlData = value; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            HtmlData = SnippetInfo.Data;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            HtmlData=MarkupExtensionProcessor.Instance.Process(HtmlData);
            LiteralControl lit = new LiteralControl(HtmlData);
            lit.RenderControl(writer);
        }
    }
}
