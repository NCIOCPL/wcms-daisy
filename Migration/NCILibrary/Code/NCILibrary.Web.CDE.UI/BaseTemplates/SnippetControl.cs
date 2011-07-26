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
    //TODO: Document
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BaseSnippetTemplate runat=server></{0}:BaseSnippetTemplate>")]
    public abstract class SnippetControl : UserControl
    {
        #region Public Properties
		[Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public SnippetInfo SnippetInfo { get; set; }
        public IPageAssemblyInstruction PageInstruction
        {
            get
            {
              return PageAssemblyContext.Current.PageAssemblyInstruction;
            }
        }
 
	#endregion    
    }
}
