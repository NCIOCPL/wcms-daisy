using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NCI.Web.CDE.WebAnalytics.Configuration;
using System.Text.RegularExpressions;
using NCI.Logging;


namespace NCI.Web.CDE.WebAnalytics
{
    public static class WebAnalyticsOptions
    {
        private static readonly string webAnalyticsPath = "nci/web/analytics";
        private static WebAnalyticsSection _webAnalyticsConfig;
        private static Dictionary<string, UrlPathChannelElement> urlPathChannelLookUp = new Dictionary<string, UrlPathChannelElement>();
        private static Dictionary<string, UrlPathChannelElement> urlPathChannelWithUrlMatchLookUp = new Dictionary<string, UrlPathChannelElement>();

        public static void Initialize()
        {
            if (_webAnalyticsConfig == null)
            {
                _webAnalyticsConfig = (WebAnalyticsSection)ConfigurationManager.GetSection(webAnalyticsPath);

                // When the analytics sectionGroup isn't defined, _webAnalyticsConfig comes back null.
                if (_webAnalyticsConfig == null)
                {
                    throw new ConfigurationErrorsException(String.Format("Could not load web analytics configuration from {0}.  Please ensure the web analytics section group and configuration element have been defined in the configuration file.", webAnalyticsPath));
                }

                if (_webAnalyticsConfig.UrlPathChannelMappings != null)
                {
                    var urlPathChannelMappings = from urlPathChannel in _webAnalyticsConfig.UrlPathChannelMappings.Cast<UrlPathChannelElement>()
                                                 orderby urlPathChannel.UrlPath descending
                                                 select urlPathChannel;

                    foreach (UrlPathChannelElement urlPathChannelElement in urlPathChannelMappings)
                    {
                        string key = urlPathChannelElement.UrlPath.ToLower();
                        if (string.IsNullOrEmpty(urlPathChannelElement.UrlMatch))
                        {
                            if (!urlPathChannelLookUp.ContainsKey(key))
                                urlPathChannelLookUp.Add(key, urlPathChannelElement);
                        }
                        else
                        {
                            if (!urlPathChannelWithUrlMatchLookUp.ContainsKey(key))
                                urlPathChannelWithUrlMatchLookUp.Add(key, urlPathChannelElement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This property determines if web analytics is enabled.
        /// Returns the value set for IsEnabled from the configuration. 
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                WebAnalyticsOptions.Initialize();

                if (_webAnalyticsConfig == null)
                    return false;
                else
                    return _webAnalyticsConfig.Enabled;
            }
        }

        /// <summary>
        /// This property determines if web analytics EnableNonJavaScriptTagging is enabled.
        /// </summary>
        public static bool EnableNonJavaScriptTagging
        {
            get
            {
                WebAnalyticsOptions.Initialize();

                if (_webAnalyticsConfig == null)
                    return false;
                else
                    return _webAnalyticsConfig.EnableNonJavaScriptTagging;
            }
        }

        /// <summary>
        /// This method returns a list of all reporting suites for a given channel.
        /// </summary>
        /// <param name="channelName">The name of the channel</param>
        /// <param name="language">The language of the related suite.</param>
        /// <returns>A string[] containing reporting suite.</returns>
        public static string[] GetSuitesForChannel(string channelName, string language)
        {
            List<string> rtnSuites = new List<string>();

            WebAnalyticsOptions.Initialize();

            if (_webAnalyticsConfig != null)
            {
                var suites = from reportingSuite in _webAnalyticsConfig.ReportingSuites.Cast<ReportingSuiteElement>()
                             where
                                (((reportingSuite.Language == "") || (reportingSuite.Language == language)) &&
                                (reportingSuite.EnabledForAllChannels ||
                                 //When looking for channel make it case insensitive.
                                reportingSuite.Channels.Cast<ChannelElement>().Any(c => string.Compare(c.Name, channelName, true) == 0)))
                             select reportingSuite.Name;

                rtnSuites.AddRange(suites);
            }

            return rtnSuites.ToArray();
        }

        /// <summary>
        /// This method returns a channel name for a given folderpath. It also matches occurence
        /// of certain text in the url.
        /// </summary>
        /// <param name="urlFolderPath">This value contains the complete url</param>
        /// <returns>The channel name which matches the urlFolderPath.</returns>
        public static string GetChannelForUrlPath(string urlFolderPath)
        {
            try
            {
                WebAnalyticsOptions.Initialize();

                if (_webAnalyticsConfig.UrlPathChannelMappings != null)
                {
                    string url = urlFolderPath.Substring(urlFolderPath.LastIndexOf('/') + 1).ToLower();
                    int urlDelimiter = urlFolderPath.LastIndexOf('/');
                    string currUrlPath = urlFolderPath;
                    while (!string.IsNullOrEmpty(currUrlPath))
                    {

                        var urlPathWithUrlChannelMappings = from urlPathChannel in urlPathChannelWithUrlMatchLookUp
                                                            where urlPathChannel.Key == currUrlPath && urlFolderPath.Contains(((UrlPathChannelElement)urlPathChannel.Value).UrlMatch)
                                                            select urlPathChannel;
                        if (urlPathWithUrlChannelMappings.Count() != 0)
                            return ((UrlPathChannelElement)urlPathWithUrlChannelMappings.FirstOrDefault().Value).ChannelName;

                        var urlPathChannelMappings = from urlPathChannel in urlPathChannelLookUp
                                                     where urlPathChannel.Key == currUrlPath
                                                     select urlPathChannel;

                        if (urlPathChannelMappings.Count() != 0)
                            return ((UrlPathChannelElement)urlPathChannelMappings.FirstOrDefault().Value).ChannelName;

                        // did not find, backtrack the url path to find the matching elements
                        urlDelimiter = currUrlPath.LastIndexOf('/');

                        if (currUrlPath == "/" && urlDelimiter == -1)
                        {
                            // shoud never come here, there should be the minumum '/' mapping in the web.config
                            currUrlPath = String.Empty;
                            continue;
                        }

                        currUrlPath = currUrlPath.Substring(0, urlDelimiter).ToLower();

                        //Reached the beginning of the url path or the request is for the home page.
                        //when this happens set the path '/' so the lookup can succeed. This 
                        //is the final fall back.
                        if (string.IsNullOrEmpty(currUrlPath))
                            currUrlPath = "/";
                    }

                    // if it reaches here then, no mapping could be found.
                    Logger.LogError("GetChannelForUrlPath", "No channel mapping exists for pretty url:" + urlFolderPath, NCIErrorLevel.Info);
                }
                else
                    Logger.LogError("GetChannelForUrlPath", "Url to channel mapping information not present in config file.", NCIErrorLevel.Info);

            }
            catch (Exception ex)
            {
                Logger.LogError("GetChannelForUrlPath", "Failed to process url to channel mapping", NCIErrorLevel.Error, ex);
            }

            // Should never be executed.
            return "";
        }

        public static class Language
        {
            public static readonly string Spanish = "spanish";
            public static readonly string English = "english";
        }

        /// <summary>
        /// Named constants for Event data point types.
        /// </summary>
        public enum Events
        {
            /// <summary>
            /// Ominture event triggered on any page load (value=1)
            /// </summary>
            PageView = 1,
            /// <summary>
            /// Ominture event triggered by a search using any site search tool (value=2)
            /// </summary>
            AllSiteSearch = 2,
            /// <summary>
            /// Ominture event triggered by click to printable version of content (value=3)
            /// </summary>
            Print = 3,
            /// <summary>
            /// Ominture event triggered by a view of a clinical trial that is the result of clinical trial search (value=4)
            /// </summary>
            ClinicalTrialViewFromSearch = 4,
            /// <summary>
            /// Ominture event triggered visitor loads a site help page (value=5)
            /// </summary>
            Help = 5,
            /// <summary>
            /// Ominture event triggered by a click to PDF version of content in Page Option (value=6)
            /// </summary>
            ArticleDownload = 6,
            /// <summary>
            /// Ominture event triggered by click to email link on a content page in Page Options (value=7)
            /// </summary>
            Email = 7,
            /// <summary>
            /// Ominture event triggered when a visitor clicks to the "All Pages" option (in Page Options) of a multiple-page document (value=8)
            /// </summary>
            AllPages = 8,
            /// <summary>
            /// Ominture event triggered by submission of email address to subscribe to NCI Cancer Bulletin (value=9)
            /// </summary>
            Subscription = 9,
            /// <summary>
            /// Ominture event triggered by Any search page-load which displays a "Best Best" result (value=10)
            /// </summary>
            BestBets = 10,
            /// <summary>
            /// Ominture event triggered when a visitor lands on a Drug Dictionary term view (value=11)
            /// </summary>
            DictionaryTermView = 11,
            /// <summary>
            /// Ominture event triggered A visitor lands on a Drug Dictionary result page (value=12)
            /// </summary>
            DrugDictionaryView = 12,
            /// <summary>
            /// Ominture event triggered by a view of a clinical trial (not result of clinical trial search) (value=13)
            /// </summary>
            ClinicalTrialViewNotSearch = 13,
            /// <summary>
            /// Ominture event triggered the "Send To Printer" link is clicked on the printable version of content (value=14)
            /// </summary>
            SendToPrinter = 14
        }

        /// <summary>
        /// Named constanst for Props data point types.
        /// </summary>
        public enum Props
        {
            /// <summary>
            /// Omniture custom traffic variable containing characters 1-100 of the full URL (value=1)
            /// </summary>
            FullURL_1thru100 = 1,
            /// <summary>
            /// Omniture custom traffic variable containing characters 101-200 of the full URL (value=2)
            /// </summary>
            FullURL_101thru200 = 2,
            /// <summary>
            /// Omniture custom traffic variable containing Root Pretty URL (value=3)
            /// </summary>
            RootPrettyURL = 3,
            /// <summary>
            /// Omniture custom traffic variable containing page coming from when link tracking (value=4)
            /// </summary>
            LinkTracking1 = 4,
            /// <summary>
            /// Omniture custom traffic variable containing link going to concatenated with coming from page name (LinkTracking1) when link tracking (value=5)
            /// </summary>
            LinkTracking2 = 5,
            /// <summary>
            /// Omniture custom traffic variable containing Short Title(value=6)
            /// </summary>
            ShortTitle = 6,
            /// <summary>
            /// Omniture custom traffic variable containing "heath processfessional" or "patient" (value=7)
            /// </summary>
            Professional_Patient = 7,
            /// <summary>
            /// Omniture custom traffic variable containing language of page (value=8)
            /// </summary>
            Language = 8,
            /// <summary>
            /// Omniture custom traffic variable containing the topic of a Clinical Trial, Cancer Topics, Statistics, or other area page. (value=9)
            /// </summary>
            Topic = 9,
            /// <summary>
            /// Omniture custom traffic variable containing Long Title (document.title) (value=10)
            /// </summary>
            LongTitle = 10,
            /// <summary>
            /// Omniture custom traffic variable containing name of search tool being used (value=11)
            /// </summary>
            SearchType = 11,
            /// <summary>
            /// Omniture custom traffic variable containing "best_bets" or "generic" (value=12)
            /// </summary>
            SearchModule = 12,
            /// <summary>
            /// Omniture custom traffic variable containing search result order (value=13)
            /// </summary>
            SearchRank = 13,
            /// <summary>
            /// Omniture custom traffic variable containing the keyword searched on in Site-Wide search (value=14)
            /// </summary>
            SiteSearchKeyword = 14,
            /// <summary>
            /// Omniture custom traffic variable containing the Protocol Search ID in Clinical Trials search [set in s_code.js] (value=15)
            /// </summary>
            ProtocolSearchID = 15,
            /// <summary>
            /// Omniture custom traffic variable containing the CdrID in Clinical Trials search [set in s_code.js] (value=16)
            /// </summary>
            CdrId = 16,
            /// <summary>
            /// Omniture custom traffic variable containing Cancer Type/Condition in Clinical Trials search (value=17)
            /// </summary>
            CancerType_Condition = 17,
            /// <summary>
            /// Omniture custom traffic variable containing type of Location searched for in Clinical Trials search (value=18)
            /// </summary>
            Location = 18,
            /// <summary>
            /// Omniture custom traffic variable containing type of Treatment searched for in Clinical Trials search (value=19)
            /// </summary>
            TrialTreatmentType = 19,
            /// <summary>
            /// Omniture custom traffic variable containing type of Status/Phase searched for in Clinical Trials search (value=20)
            /// </summary>
            TrialStatus_Phase = 20,
            /// <summary>
            /// Omniture custom traffic variable containing type of TrialID/Sponsor searched for in Clinical Trials search (value=21)
            /// </summary>
            TrialID_Sponsor = 21,
            /// <summary>
            /// Omniture custom traffic variable containing search keyword in non-Site-Wide searches (value=22)
            /// </summary>
            BoutiqueSearchKeywords = 22,
            /// <summary>
            /// Omniture custom traffic variable containing Specialty in Genetics Services search (value=23)
            /// </summary>
            Specialty = 23,
            /// <summary>
            /// Omniture custom traffic variable containing "starts with" or "contains" based on the type of search (value=24)
            /// </summary>
            StartsWith_Contains = 24,
            /// <summary>
            /// Omniture custom traffic variable containing Posted Date (value=25)
            /// </summary>
            PostedDate = 25,
            /// <summary>
            /// Omniture custom traffic variable containing time stamp on page load [set in s_code.js] (value=26)
            /// </summary>
            TimeStamp = 26,

            MultipageShortTile = 27
        }

        /// <summary>
        /// Named constants for eVars data point types
        /// </summary>
        public enum eVars
        {
            /// <summary>
            /// Omniture custom conversion variable containing the name of the page (as per Design Document) [set in s_code.js] (value=1)
            /// </summary>
            PageName = 1,
            /// <summary>
            /// Omniture custom conversion variable containing language of page (value=2)
            /// </summary>
            Language = 2,
            /// <summary>
            /// Omniture custom conversion variable containing topic of page (value=3)
            /// </summary>
            Topic = 3,
            /// <summary>
            /// Omniture custom conversion variable containing the number of search results returned (value=10)
            /// </summary>
            NumberOfSearchResults = 10,
            /// <summary>
            /// Omniture custom conversion variable containing containing name of search tool being used (value=11)
            /// </summary>
            SearchType = 11,
            /// <summary>
            /// Omniture custom conversion variable containing "best_bets" or "generic" (value=12)
            /// </summary>
            SearchModule = 12,
            /// <summary>
            /// Omniture custom conversion variable containing a counter variable that increments every time a site search is performed. (value=13)
            /// </summary>
            SearchCount = 13,
            /// <summary>
            /// Omniture custom conversion variable containing the keyword searched on in Site-Wide search (value=14)
            /// </summary>
            SiteSearchKeyword = 14,
            /// <summary>
            /// Omniture custom conversion variable containing the Protocol Search ID in Clinical Trials search [set in s_code.js] (value=15)
            /// </summary>
            ProtocolSearchID = 15,
            /// <summary>
            /// Omniture custom conversion variable containing the CdrID in Clinical Trials search [set in s_code.js] (value=16)
            /// </summary>
            CdrId = 16,
            /// <summary>
            /// Omniture custom conversion variable containing Cancer Type/Condition in Clinical Trials search (value=17)
            /// </summary>
            CancerType_Condition = 17,
            /// <summary>
            /// Omniture custom conversion variable containing type of Location searched for in Clinical Trials search (value=18)
            /// </summary>
            Location = 18,
            /// <summary>
            /// Omniture custom conversion variable containing type of Treatment searched for in Clinical Trials search (value=19)
            /// </summary>
            TrialTreatmentType = 19,
            /// <summary>
            /// Omniture custom conversion variable containing type of Status/Phase searched for in Clinical Trials search (value=20)
            /// </summary>
            TrialStatus_Phase = 20,
            /// <summary>
            /// Omniture custom conversion variable containing type of TrialID/Sponsor searched for in Clinical Trials search (value=21)
            /// </summary>
            TrialID_Sponsor = 21,
            /// <summary>
            /// Omniture custom conversion variable containing a counter variable that increments every time a Clinical Trial is viewed. (value=22)
            /// </summary>
            ClinicalTrialViewCount = 22,
            /// <summary>
            /// Omniture custom conversion variable containing user-entered start date for time period searched by News Search tool. (value=23)
            /// </summary>
            StartDate = 23,
            /// <summary>
            /// Omniture custom conversion variable containing user-entered end date for time period searched by News Search tool. (value=24)
            /// </summary>
            EndDate = 24,
            /// <summary>
            /// Omniture custom conversion variable containing user selections regarding specialty in Genetics Services Search. (value=25)
            /// </summary>
            Specialty = 25,
            /// <summary>
            /// Omniture custom conversion variable containing "starts with" or "contains" based on the type of search  (value=26)
            /// </summary>
            StartsWith_Contains = 26,
            /// <summary>
            /// Omniture custom conversion variable containing a counter variable that increments every time an article is downloaded. (value=30)
            /// </summary>
            ArticleDownload = 30
        }
    }
}