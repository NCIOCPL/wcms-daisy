using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.CDE.Configuration;
using NCI.Util;
using NCI.Logging;
using NCI.Web.Extensions;
using NCI.Web.CDE;
using NCI.Web.UI.WebControls;


namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// This class is derived from Page class. The template pages which are .aspx files will inherit from 
    /// this class.Template pages are used for displaying content delivered by CDE. The template pages generally contain 
    /// markup as well as template controls which are used as place holders for the content. This class identifies all 
    /// template slot and assigns the one or more snippet info object for each template slot. Snippet info objects contain the 
    /// actual content that should be rendered. The content is mostly html markup.
    /// </summary>
    public class WebPageAssembler : Page, IPageAssembler
    {
        #region Private Members

        /// <summary>
        /// Loads a collection of all template slots that are declaratively 
        /// defined  in the page template. The slots that are specified as being blocked
        /// in the assembly instructions are not processed.
        /// </summary>
        private void loadTemplateSlots()
        {
            foreach (TemplateSlot slot in this.FindControlByType<TemplateSlot>())
            {
                //Add to slot collection only if it is not marked as being blocked.
                if (!PageAssemblyInstruction.BlockedSlotNames.Contains(slot.ID))
                    TemplateSlots.Add(slot);
            }
        }

        /// <summary>
        /// Adds the snippet info from the PageAssembly instruction for each 
        /// page template slot. Use the ID of the template slot control to match 
        /// the snippet info slotname property.
        /// </summary>
        private void loadSnippetsIntoTemplateSlots()
        {
            if (TemplateSlots.Count > 0)
            {
                List<SnippetControl> supportingSnippets = new List<SnippetControl>();
                foreach (SnippetInfo snippet in PageAssemblyInstruction.Snippets)
                {
                    if (!String.IsNullOrEmpty(snippet.SlotName))
                    {
                        string slotName = snippet.SlotName.Trim();
                        if (TemplateSlots.ContainsSlotName(slotName))
                        {
                            try
                            {
                                SnippetControl snippetControl = (SnippetControl)Page.LoadControl(snippet.SnippetTemplatePath.Trim());

                                // Note this has to come before adding the template control to the control tree.  
                                // This way, we can be sure any event in the control lifecycle like OnInit() 
                                // already has the snippet info data.
                                snippetControl.SnippetInfo = snippet;

                                TemplateSlots[slotName].AddSnippet(snippetControl);

                                // Some snippetcontrol implement the ISupportingSnippet interface. 
                                // The controls which implement ISupportingSnippet will provide additional 
                                // snippet control that are required for the complete functionality.
                                // Add them to a collection that will be processed later.
                                if( snippetControl is ISupportingSnippet )
                                {
                                    SnippetControl[] supportingcontrols = ((ISupportingSnippet)snippetControl).GetSupportingSnippets();
                                    if( supportingcontrols != null )
                                        supportingSnippets.AddRange(supportingcontrols);
                                }
                            }
                            catch (Exception ex)
                            {
                                //Failed to load the slot template control. Log this error.
                                Logger.LogError("CDE:WebPageAssembler.cs:WebPageAssembler", "Failed to load snippet control-" + snippet.SnippetTemplatePath, NCIErrorLevel.Error, ex);
                            }
                        }
                    }
                }

                foreach (SnippetControl supportSnippet in supportingSnippets)
                {
                    try
                    {
                        if (TemplateSlots.ContainsSlotName(supportSnippet.SnippetInfo.SlotName))
                            TemplateSlots[supportSnippet.SnippetInfo.SlotName].AddSnippet(supportSnippet);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("CDE:WebPageAssembler.cs:WebPageAssembler", "Failed to add supporting snippet control",NCIErrorLevel.Error, ex);
                    }
                }

            }
        }


        /// <summary>
        /// Returns the metadata value for different types of metadata name.
        /// </summary>
        /// <param name="metaDataType"></param>
        /// <returns></returns>
        private string getMetaData(HtmlMetaDataType metaDataType)
        {
            IPageAssemblyInstruction asmInstr = PageAssemblyInstruction;
            string metaData = String.Empty;
            if (asmInstr != null)
            {
                switch (metaDataType)
                {
                    case HtmlMetaDataType.Description:
                        metaData = asmInstr.GetField(PageAssemblyInstructionFields.HTML_MetaDescription);
                        break;

                    case HtmlMetaDataType.KeyWords:
                        metaData = asmInstr.GetField(PageAssemblyInstructionFields.HTML_MetaKeywords);
                        break;

                    case HtmlMetaDataType.ContentLanguage:
                        metaData = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                        break;
                }

            }

            metaData = string.IsNullOrEmpty(metaData) ? "" : metaData.Trim();
            return metaData;
        }

        /// <summary>
        /// Every time this method is called, a single metadata element is added to the 
        /// head of the page.
        /// </summary>
        /// <param name="htmlHead">The html header control of the page.The head html tag should have runat="server"</param>
        /// <param name="htmlMetaDataType">The name of the metadata element you want to add</param>
        private void addMetaDataItem(HtmlHead htmlHead, HtmlMetaDataType htmlMetaDataType)
        {
            HtmlMeta hm = new HtmlMeta();
            hm.Name = htmlMetaDataType.ToString().ToLower();
            if (htmlMetaDataType == HtmlMetaDataType.ContentLanguage)
                hm.Name = "content-language";
            else if (htmlMetaDataType == HtmlMetaDataType.ContentType)
                hm.Name = "content-type";
            hm.Content = getMetaData(htmlMetaDataType);
            htmlHead.Controls.Add(hm);
        }

        #endregion

        #region Public Members
        /// <summary>
        /// Constructor. Initializes the object state. 
        /// </summary>
        public WebPageAssembler()
        {
            TemplateSlots = new TemplateSlotCollection();
        }
        #endregion

        #region Interface Members
        /// <summary>
        /// Gets the collection of Template Slots used by this page assembler.
        /// </summary>
        /// <value></value>
        /// <remarks>Since controls and apps might need to shove stuff into slots,
        /// this information should be available to controls.
        /// </remarks>
        public TemplateSlotCollection TemplateSlots { get; private set; }

        #endregion

        #region Page Overrides
        /// <summary>
        /// Identify the Templates on the Page. For each template add the Snippet info objects.
        /// </summary>
        /// <param name="e">Not Used</param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            loadTemplateSlots();
            loadSnippetsIntoTemplateSlots();            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            JSManager.AddExternalScript(this, "http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js");
        }

        /// <summary>
        /// Use this event handler to perform operations like update 
        /// the page meta tags , web analytics, etc
        /// </summary>
        /// <param name="e">Not used</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);            
            SetTitle();
            InsertStyleSheetsReferences();
            InsertCanonicalURL();
            InsertPageMetaData();

            //Set the form action so it does not post back to the page template path.
            if (this.Form != null)
            {
                NciUrl formAction = this.PageAssemblyInstruction.GetUrl("PostBackURL");

                if (formAction == null || string.IsNullOrEmpty(formAction.ToString()))
                {
                    //Log Error
                }
                else
                {
                    this.Form.Action = formAction.ToString();
                }                
            }
        }

        #endregion

        #region Protected Members
        /// <summary>
        /// This property returns the current page header control. 
        /// Use this in lieu of the Page.Header property when the Page.Header property is null. 
        /// </summary>
        protected HtmlHead CurrentPageHead
        {
            get
            {
                // Find the html head control on the template page
                HtmlHead currentPageHead = null;
                foreach (HtmlHead head in this.FindControlByType<HtmlHead>())
                {
                    currentPageHead = head;
                    break;
                }
                return currentPageHead;
            }
        }

        /// <summary>
        /// Sets the title of the page. Uses the GetField method of the Page instructions interface
        /// to get  the value.
        /// </summary>
        protected virtual void SetTitle()
        {
            if (CurrentPageHead != null)
            {
                string title = PageAssemblyInstruction.GetField(PageAssemblyInstructionFields.HTML_Title);
                CurrentPageHead.Title = title.Replace("<i>","").Replace("</i>","");
            }
        }

        /// <summary>
        /// Returns the IPageAssemblyInstruction interface associated with this request.
        /// </summary>
        protected IPageAssemblyInstruction PageAssemblyInstruction
        {
            get { return PageAssemblyContext.Current.PageAssemblyInstruction; }
        }
        /// <summary>
        /// Adds page meta data information like keyword, description, content type to the head
        /// section.
        /// </summary>
        protected virtual void InsertPageMetaData()
        {
            if (CurrentPageHead != null)
            {
                addMetaDataItem(CurrentPageHead, HtmlMetaDataType.KeyWords);
                addMetaDataItem(CurrentPageHead, HtmlMetaDataType.Description);
                addMetaDataItem(CurrentPageHead, HtmlMetaDataType.ContentLanguage);
            }
        }

        /// <summary>
        /// Adds the canonical URL information to the page.
        /// </summary>
        protected virtual void InsertCanonicalURL()
        {
            string CanonicalUrl = PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.CanonicalUrl).ToString();
            if (!string.IsNullOrEmpty(CanonicalUrl))
            {
                if (CurrentPageHead != null)
                {
                    HtmlLink hml = new HtmlLink();
                    hml.Attributes.Add("rel", "canonical");
                    hml.Href = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName + CanonicalUrl;
                    CurrentPageHead.Controls.Add(hml);
                }
            }
        }

        /// <summary>
        /// Inserts the .css resources to the page. There can be one or more 
        /// css resource for each page template. This information is found in 
        /// the PageTemplateInfo object of the PageAssembleyContext.Current
        /// </summary>
        protected virtual void InsertStyleSheetsReferences()
        {
            if (CurrentPageHead != null)
            {
                PageTemplateInfo pgTemplateInfo = PageAssemblyContext.Current.PageTemplateInfo;
                if (pgTemplateInfo != null)
                {
                    // Loop thru all css resources and add css resource
                    foreach (StyleSheetInfo ssInfo in pgTemplateInfo.StyleSheets)
                        NCI.Web.UI.WebControls.CssManager.AddStyleSheet(this, ssInfo.StyleSheetPath);
                }
            }
        }

        #endregion
    }
}


