using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.Globalization;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.WebAnalytics;
using NCI.Util;
using NCI.Core;


namespace NCI.Web.CDE
{
    /// <summary>
    /// Implements the IPageAssemblyInstruction interface.An Instance of this class is created by deserializing the percussion published XML file.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("SinglePageAssemblyInstruction", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SinglePageAssemblyInstruction : BasePageAssemblyInstruction, IPageAssemblyInstruction
    {

        #region Private Member Variables

        /// <summary>
        /// Dictionary holds the registered multiple Url filters.
        /// </summary>
        private Dictionary<string, UrlFilterDelegate> _UrlFilterDelegates = new Dictionary<string, UrlFilterDelegate>();
        /// <summary>
        /// Dictionary holds the registered multiple Field Filters
        /// </summary>
        private Dictionary<string, FieldFilterDelegate> _FieldFilterDelegates = new Dictionary<string, FieldFilterDelegate>();

        /// <summary>
        /// A collection of the snippets to be displayed on the page.
        /// </summary>
        private SnippetInfoCollection _snippets;
        private LocalFieldCollection _localFields;

        #endregion

        public SinglePageAssemblyInstruction()
        {
            // Initialize sub objects.
            _snippets = new SnippetInfoCollection();
            PageMetadata = new PageMetadata();
            _localFields = new LocalFieldCollection();
            RegisterFieldFilters();
            RegisterWebAnalyticsFieldFilters();

            AddFieldFilter(PageAssemblyInstructionFields.HTML_Title, (name, data) =>
            {
                //Site Name should be a configuration setting.
                data.Value = GetField("short_title") + ContentDeliveryEngineConfig.PageTitle.AppendPageTitle.Title;
            });

            AddFieldFilter(PageAssemblyInstructionFields.HTML_MetaDescription, (name, data) =>
            {
                string metaDescription = GetMetaDescription();
                data.Value = metaDescription;
            });

            AddFieldFilter(PageAssemblyInstructionFields.HTML_MetaKeywords, (name, data) =>
            {
                data.Value = GetField("meta_keywords");
            });

            //Register URL Filters
            AddUrlFilter(PageAssemblyInstructionUrls.PrettyUrl, new UrlFilterDelegate(FilterCurrentUrl));
            AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, new UrlFilterDelegate(CanonicalUrl));

            AddUrlFilter("CurrentURL", (name, url) =>
                { 
                    url.SetUrl(GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString());
                    if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.Print)
                    {
                        url.UriStem += "/print";
                    }
                });

            AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl( GetUrl("CurrentURL").ToString() + "/print");
            });

            AddUrlFilter("Email", (name, url) =>
            {
                url.SetUrl(GetEmailUrl());
            });

            AddUrlFilter("free", (name, url) =>
            {
                string freeCopyUrl = string.Empty; ;
                if (!string.IsNullOrEmpty(AlternateContentVersions.OrderCopyURL))
                    freeCopyUrl = AlternateContentVersions.OrderCopyURL.Trim();
                url.SetUrl(freeCopyUrl, true);
            });

            AddUrlFilter("PostBackURL", (name, url) =>
            {
                url.SetUrl(GetUrl("CurrentURL").ToString() + "?" + HttpContext.Current.Request.QueryString);
            });

            base.Initialize();
        }

        #region Properties

        /// <summary>
        /// Gets the URL filter delegates.
        /// </summary>
        /// <value>The URL filter delegates.</value>
        private Dictionary<string, UrlFilterDelegate> UrlFilterDelegates
        {
            get
            {
                return _UrlFilterDelegates;
            }
        }

        /// <summary>
        /// Gets the field filter delegates.
        /// </summary>
        /// <value>The field filter delegates.</value>
        private Dictionary<string, FieldFilterDelegate> FieldFilterDelegates
        {
            get
            {
                return _FieldFilterDelegates;
            }
        }

        #endregion

        #region IPageAssemblyInstruction Members

        [System.Xml.Serialization.XmlElement(ElementName = "LocalFields", Form = XmlSchemaForm.Unqualified)]
        public LocalFieldCollection LocalFields
        {
            get
            {
                return _localFields;
            }
            set
            {
                _localFields = value;
            }
        }

        /// <summary>
        /// BlockedSlots contain information about the blocked slot which should not be displayed on the page rendered.
        /// </summary>
        /// <value>The blocked slot names.</value>
        public string[] BlockedSlotNames
        {
            get
            {
                var names = from slot in BlockedSlots
                            select slot.Name;

                return names.ToArray();
            }
        }

        /// <summary>
        /// Gets the name of the page template i.e the actual aspx page to be loaded.
        /// </summary>
        /// <value>The name of the page template.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PageTemplateName { get; set; }

        /// <summary>
        /// Gets or sets the language for the page displayed.
        /// </summary>
        /// <value>The language.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>
        /// The path of all parent folders of the page assembly instruction.
        /// </summary>
        /// <value></value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SectionPath { get; set; }

        /// <summary>
        /// Gets or sets the pretty URL.
        /// </summary>
        /// <value>The pretty URL.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the page metadata.
        /// </summary>
        /// <value>The page metadata.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PageMetadata PageMetadata { get; set; }

        /// <summary>
        /// Gets or sets the content dates for the page.
        /// </summary>
        /// <value>The page metadata.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ContentDates ContentDates { get; set; }

        /// <summary>
        /// Gets the collections of the snippets
        /// </summary>
        /// <value>The snippet infos.</value>
        [System.Xml.Serialization.XmlArray(ElementName = "Snippets", Form = XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("SnippetInfo", Form = XmlSchemaForm.Unqualified)]
        public SnippetInfoCollection SnippetInfos
        {

            get { return _snippets; }
        }

        /// <summary>
        /// A collection of SnippetInfo objects for the page assembly Instruction which are needed
        /// to render a page.
        /// </summary>
        /// <value></value>
        [System.Xml.Serialization.XmlIgnore()]
        public IEnumerable<SnippetInfo> Snippets
        {
            get
            {
                List<SnippetInfo> snippets = new List<SnippetInfo>();

                // Add all local snippets to the list to return.
                foreach (SnippetInfo snipt in _snippets)
                {
                    if (snipt.OnlyDisplayFor.Count() == 0 || snipt.OnlyDisplayFor.Contains(PageAssemblyContext.Current.DisplayVersion))
                    {
                        snippets.Add(snipt);
                    }
                }
                ////Find all of the Slots on the page which are not blocked and where those Slots do not have associated SnippetInfos in the SinglePageAssemblyInstruction XML file.
                IEnumerable<string> filledTemplateSlots = (from snippet in _snippets select snippet.SlotName).Distinct<string>().Except(BlockedSlotNames);

                SectionDetail sectionDetail = SectionDetailFactory.GetSectionDetail(SectionPath);
                if (sectionDetail != null)
                {
                    List<SnippetInfo> snippetsFromParent = sectionDetail.GetSnippetsNotAssociatedWithSlots(filledTemplateSlots);
                    foreach (SnippetInfo sniptParents in snippetsFromParent)
                    {
                        if (sniptParents.OnlyDisplayFor.Count() == 0 || sniptParents.OnlyDisplayFor.Contains(PageAssemblyContext.Current.DisplayVersion))
                        {
                            snippets.Add(sniptParents);
                        }

                    }
                }
                return snippets;
            }
        }

        /// <summary>
        /// Gets or sets the blocked slots which should not be displayed on the page rendered.
        /// </summary>
        /// <value>The blocked slots.</value>
        [System.Xml.Serialization.XmlArray(ElementName = "BlockedSlots", Form = XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("Slot", Form = XmlSchemaForm.Unqualified)]
        public BlockedSlot[] BlockedSlots { get; set; }


        /// <summary>
        /// Gets the value of the field referenced by "String" with all field filter applied.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="ArgumentException">Thrown when the fieldName field is null or empty.</exception>
        /// <returns></returns>
        public string GetField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("The field name may not be null or empty.");

            string rtnValue = string.Empty;

            FieldFilterDelegate del = FieldFilterDelegates[fieldName.ToLower()];
            if (del != null)
            {
                //Initialize the field data to empty field data
                FieldFilterData data = new FieldFilterData();

                //Call delegate, all delegates will modify the FieldData string of the
                //FieldFilterData object we are passing in.
                del(fieldName, data);

                //set the return value to the processed value of the FieldFilterData
                rtnValue = data.Value;
            }

            return rtnValue;
        }
        /// <summary>
        /// Adds a field filter which modifies the value of the field referenced by "string"
        /// when GetField is called.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="filter"></param>
        public void AddFieldFilter(string fieldName, FieldFilterDelegate filter)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("The fieldName parameter may not be null or empty.");

            string fieldNameKey = fieldName.ToLower();

            if (FieldFilterDelegates.ContainsKey(fieldNameKey) == false)
            {
                FieldFilterDelegates.Add(fieldNameKey, filter);
            }
            else
            {
                //Note, this must be called this way and cannot use an intermediate value, I.E.
                //  FieldFilterDelegate foo = FieldFilterDelegates[fieldName.ToLower()];
                //  foo += filter;
                //will not work.  The reason is, Delegates are Immutable!!
                FieldFilterDelegates[fieldNameKey] += filter;
            }
        }

        /// <summary>
        /// Gets the URL referenced by "String" with all URL filters.
        /// </summary>
        /// <param name="urlType"></param>
        /// <returns></returns>
        public NciUrl GetUrl(string urlType)
        {
            NciUrl nciUrl = new NciUrl();
            string linkTypeKey = urlType.ToLower();

            if (UrlFilterDelegates.ContainsKey(linkTypeKey) == true)
            {
                UrlFilterDelegate UrlfilterLinkDelegate = UrlFilterDelegates[linkTypeKey];
                UrlfilterLinkDelegate(linkTypeKey, nciUrl);
            }
            else
            {
                throw new PageAssemblyException(String.Format("Unknown link type \"{0}\"", urlType));
            }

            return nciUrl;
        }


        /// <summary>
        /// Adds a URL filter which modifies the URL referenced by "string" when GetUrl is called.
        /// </summary>
        /// <param name="urlType">Pretty URL or Cannonical URL</param>
        /// <param name="fieldFilter"></param>
        public void AddUrlFilter(string urlType, UrlFilterDelegate fieldFilter)
        {
            if (string.IsNullOrEmpty(urlType))
                throw new ArgumentException("The urlType parameter may not be null or empty.");

            string linkTypeKey = urlType.ToLower();

            if (UrlFilterDelegates.ContainsKey(linkTypeKey) == false)
            {
                UrlFilterDelegates.Add(linkTypeKey, fieldFilter);
            }
            else
            {
                UrlFilterDelegates[linkTypeKey] += fieldFilter;
            }
        }

        /// <summary>
        /// This property returns the keys which represent the available content versions. 
        /// </summary>
        /// <value>A string array which are the keys to the alternate content versions.</value>
        public string[] AlternateContentVersionsKeys
        {
            get
            {
                ArrayList keysList = new ArrayList();
                if (AlternateContentVersions.IsPrintAvailable)
                    keysList.Add("print");
                if (AlternateContentVersions.IsShareBookmarkAvailable)
                    keysList.Add("bookmarkshare");
                if (AlternateContentVersions.IsEmailAvailable)
                    keysList.Add("email");
                if (!string.IsNullOrEmpty(AlternateContentVersions.OrderCopyURL))
                    keysList.Add("free");

                // Enumerate the Files and set an URL filter.
                foreach (AlternateContentFile acFile in AlternateContentVersions.Files)
                {
                    keysList.Add(acFile.MimeType.ToLower());
                    AddUrlFilter(acFile.MimeType.ToLower(), (name, url) =>
                    {
                        foreach (AlternateContentFile file in AlternateContentVersions.Files)
                        {
                            if (file.MimeType.ToLower() == name.ToLower())
                                url.SetUrl(file.Url);
                        }
                    });
                }

                return (string[])keysList.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// This method returns the web analytics settings for Event, Props and eVars data points.
        /// </summary>
        public override WebAnalyticsSettings GetWebAnalytics()
        { 
            return base.GetWebAnalytics();
        }

        /// <summary>
        /// A web analytics data point(props, eVars, Events) value is set using this method. The FieldFilter delegate 
        /// on this method allows registered callback with the same data point name to modfiy the 
        /// same value.
        /// </summary>
        /// <param name="webAnalyticType">The enum which defines all Events data points.</param>
        /// <param name="filter">The callback method which will set the value of the fieldfilter object</param>
        public void SetWebAnalytics(WebAnalyticsOptions.Events webAnalyticType, WebAnalyticsDataPointDelegate filter)
        {
            base.SetWebAnalytics(webAnalyticType.ToString(), filter);
        }

        /// <summary>
        /// A web analytics data point(props, eVars, Events) value is set using this method. The FieldFilter delegate 
        /// on this method allows registered callback with the same data point name to modfiy the 
        /// same value.
        /// </summary>
        /// <param name="webAnalyticType">The enum which defines all eVars data points.</param>
        /// <param name="filter">The callback method which will set the value of the fieldfilter object</param>
        public void SetWebAnalytics(WebAnalyticsOptions.eVars webAnalyticType, WebAnalyticsDataPointDelegate filter)
        {
            base.SetWebAnalytics(webAnalyticType.ToString(), filter);
        }

        /// <summary>
        /// A web analytics data point(props, eVars, Events) value is set using this method. The FieldFilter delegate 
        /// on this method allows registered callback with the same data point name to modfiy the 
        /// same value.
        /// </summary>
        /// <param name="webAnalyticType">The enum which defines all Props data point.</param>
        /// <param name="filter">The callback method which will set the value of the FieldFilterData object</param>
        public void SetWebAnalytics(WebAnalyticsOptions.Props webAnalyticType, WebAnalyticsDataPointDelegate filter)
        {
            base.SetWebAnalytics(webAnalyticType.ToString(), filter);
        }
        #endregion

        #region Private Methods

        private void FilterCurrentUrl(string name, NciUrl url)
        {
            //This should always be the first delegate for the CurrentUrl link type
            //so we can just overwrite whatever has come before.
            url.SetUrl(PrettyUrl);
        }

        private void CanonicalUrl(string name, NciUrl url)
        {
            //This should always be the first delegate for the CurrentUrl link type
            //so we can just overwrite whatever has come before.
            url.SetUrl(GetUrl("PrettyUrl").ToString());

        }

        //private string GetEmailUrl()
        //{
        //    string popUpemailUrl = "";

        //    string title = GetField("long_title");
        //    title = System.Web.HttpUtility.UrlEncode(Strings.StripHTMLTags(title.Replace("&#153;", "__tm;")));

        //    string emailUrl = GetUrl("EmailUrl").ToString();

        //    if ((Strings.Clean(emailUrl) != null) && (Strings.Clean(emailUrl) != ""))
        //    {
        //        popUpemailUrl = "/common/popUps/PopEmail.aspx?title=" + title + "&docurl=" + System.Web.HttpUtility.UrlEncode(emailUrl.Replace("&", "__amp;")) + "&language=" + PageAssemblyContext.Current.PageAssemblyInstruction.Language;
        //        popUpemailUrl = popUpemailUrl + HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + emailUrl);
        //    }
        //    return popUpemailUrl;
        //}

        private void RegisterMarkupExtensionFieldFilters()
        {
            //Register Field Filters

            foreach (LocalField localField in _localFields)
            {
                AddFieldFilter(localField.Name, (name, data) =>
                {
                    data.Value = _localFields[name].Value;
                });
            }

        }

        /// <summary>
        /// Registers the field filters.
        /// </summary>
        private void RegisterFieldFilters()
        {
            //Register Field Filters
            AddFieldFilter("long_title", (name, data) =>
            {
                data.Value = this.PageMetadata.LongTitle;
            });


            AddFieldFilter("short_title", (name, data) =>
            {
                data.Value = this.PageMetadata.ShortTitle;
            });

            AddFieldFilter("short_description", (name, data) =>
            {
                data.Value = this.PageMetadata.ShortDescription;
            });

            AddFieldFilter("long_description", (name, data) =>
            {
                data.Value = this.PageMetadata.LongDescription;
            });

            AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = this.PageMetadata.MetaDescription;
            });

            AddFieldFilter("meta_keywords", (name, data) =>
            {
                data.Value = this.PageMetadata.MetaKeywords;
            });

            AddFieldFilter("channelName", (name, data) =>
            {
                data.Value = this.SectionPath;
            });
        }

        /// <summary>
        /// Gets the meta description.
        /// </summary>
        /// <returns>meta description string which can be used for the meta tag on the page</returns>
        private string GetMetaDescription()
        {
            string meta = GetField("meta_description");

            if (string.IsNullOrEmpty(meta))
            {
                meta = GetField("short_description");

                if (!string.IsNullOrEmpty(meta))
                {
                    //string meta tags from HTML.
                    meta = Regex.Replace(meta, @"<(.|\n)*?>", String.Empty);
                }

                else
                {
                    meta = GetField("long_description");
                    if (!string.IsNullOrEmpty(meta))
                    {
                        //string meta tags from HTML.
                        meta = Regex.Replace(meta, @"<(.|\n)*?>", String.Empty);
                    }

                }
            }
            return meta;
        }

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            SinglePageAssemblyInstruction target = obj as SinglePageAssemblyInstruction;

            if (target == null)
                return false;

            if (
                (PageMetadata != null && target.PageMetadata == null) ||
                (PageMetadata == null && target.PageMetadata != null)
                )
            {
                return false;
            }

            if (PageMetadata != null && target.PageMetadata != null)
                if (!PageMetadata.Equals(target.PageMetadata))
                    return false;


            if (
                (_snippets != null && target._snippets == null) ||
                (_snippets == null && target._snippets != null)
                )
            {
                return false;
            }

            if (_snippets != null && target._snippets != null)
            {
                if (!_snippets.Equals(target._snippets))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns Alternate content versions object which contains information necessary to display 
        /// the page options.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public AlternateContentVersions AlternateContentVersions { get; set; }
        #endregion

        #region Protected
        /// <summary>
        /// Override this method to add any page specifc web analytics data points.
        /// </summary>
        protected override void RegisterWebAnalyticsFieldFilters()
        {
            base.RegisterWebAnalyticsFieldFilters();

            SetWebAnalytics(WebAnalyticsOptions.Props.RootPrettyURL.ToString(), wbField =>
            {
                // This is  hack to fix the rooturl for web analytics. If  this content type is 
                // rx:pdqCancerInfoSummary then remove the 'patient' or 'healthprofessional' from
                // the pretty url
                string prettyUrl = PrettyUrl;
                if (ContentItemInfo != null && ContentItemInfo.ContentItemType == "rx:pdqCancerInfoSummary")
                {
                    prettyUrl = prettyUrl.ToLower().Replace("/patient", "");
                    prettyUrl = prettyUrl.ToLower().Replace("/healthprofessional", "");
                }

                wbField.Value = prettyUrl;
            });

            SetWebAnalytics(WebAnalyticsOptions.Props.ShortTitle.ToString(), wbField =>
            {
                wbField.Value = GetField("short_title");
            });


            SetWebAnalytics(WebAnalyticsOptions.Props.PostedDate.ToString(), wbField =>
            {
                wbField.Value = String.Format("{0:MM/dd/yyyy}", this.ContentDates.FirstPublished);
            });
        }        
        #endregion

        public void Initialize()
        {
            RegisterMarkupExtensionFieldFilters();
        }
    }
}
