using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.Modules;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying page options based on Alternate content versions.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PageOptionsDisplay runat=server></{0}:PageOptionsDisplay>")]
    public class PageOptionsDisplay : SnippetControl
    {

        public void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            // In this case the snippet info data is not HTML(which is often the case)
            // but xml data which contains rendering properties for each page option item 
            processPageOptionsData(SnippetInfo.Data);

            JSManager.AddExternalScript(this.Page, "/JS/popEvents.js");

        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// Process the page options information in the xml and creates 
        /// the page options items.
        /// </summary>
        /// <param name="snippetXmlData">The xml fragment which contains pageoptions information.</param>
        private void processPageOptionsData(string snippetXmlData)
        {
            // The snippet CDATA may contain CDATA as part of the data but percussion replaces the CDATA 
            // close tag with Replace ']]>' with ']]ENDCDATA' this ']]ENDCDATA' should be replaced with 
            // valid CDATA close tag ']]>' before it can be deserialized
            snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");

            IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;

            // If AlternateContentVersions information is not in the instructions then do not create 
            // the PageOptions box.
            string[] acvKeys = pgInstruction.AlternateContentVersionsKeys;

            if (acvKeys != null)
            {

                Module_PageOptionsBox mPBO = ModuleObjectFactory<Module_PageOptionsBox>.GetModuleObject(snippetXmlData);

                if (mPBO != null)
                {
                    // Create the Page Options box control.
                    PageOptionsBox pageOptionsBox = new PageOptionsBox();
                    pageOptionsBox.BoxTitle = mPBO.Title;
                    pageOptionsBox.CssClass = "po-box";

                    foreach (PageOption pgOptionItem in mPBO.PageOptions)
                    {
                        try
                        {
                            // Check if the Pageoptions are recognized in the Alternate Content Version keys
                            string key = pgOptionItem.Key;
                            
                            if(!string.IsNullOrEmpty(key))
                                key = key.ToLower();

                            if (acvKeys.Contains<string>(key))
                            {
                                NCI.Web.UI.WebControls.PageOption pgoBase = null;

                                if (String.Compare(pgOptionItem.OptionType, PageOptionType.Link.ToString()) == 0)
                                {
                                    pgoBase = new LinkPageOption();
                                    ((LinkPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                                    ((LinkPageOption)pgoBase).Href = pgInstruction.GetUrl(key).ToString();
                                }
                                else if (String.Compare(pgOptionItem.OptionType, PageOptionType.Email.ToString()) == 0)
                                {
                                    pgoBase = new LinkPageOption();
                                    ((LinkPageOption)pgoBase).Href = pgInstruction.GetUrl("email").ToString();
                                    ((LinkPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                                    ((LinkPageOption)pgoBase).OnClick += " " + "dynPopWindow('" + ((LinkPageOption)pgoBase).Href.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29") + "', 'emailPopUp', 'height=365,width=525'); return false;";
                                }
                                else if (String.Compare(pgOptionItem.OptionType, PageOptionType.BookMarkShare.ToString()) == 0)
                                {
                                    pgoBase = new AddThisPageOption();
                                    ((AddThisPageOption)pgoBase).Settings.Language = pgInstruction.Language;
                                    ((AddThisPageOption)pgoBase).PageTitle = pgInstruction.GetUrl("BookMarkShareUrl").ToString();
                                    ((AddThisPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                                }

                                if (pgoBase != null)
                                {
                                    pgoBase.CssClass = pgOptionItem.CssClass;
                                    pgoBase.LinkText = pgOptionItem.LinkText;
                                    pageOptionsBox.PageOptions.Add(pgoBase);
                                }
                            }
                        }
                        catch
                        { 
                            //TODO, log exception
                        }
                    }

                    if( pageOptionsBox.PageOptions.Count > 0 )
                        this.Controls.Add(pageOptionsBox);
                }
            }
        }
    }
}
