﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics
{

    /// <summary>
    /// WebAnalyticsPageLoad is used to create the necessary JavaScript code to create Omniture page load metrics.  
    /// It contains methods for setting different types of page view metrics including: 
    /// channel, custom variables (props), custom conversion variables (eVars), and events.    
    /// </summary>
    public class WebAnalyticsPageLoad
    {
        private const string DELIMITER = "'";
        private const string WEB_ANALYTICS_COMMENT_START = "<!-- ***** NCI Web Analytics ***** -->";
        private const string WEB_ANALYTICS_COMMENT_END = "<!-- ***** End NCI Web Analytics ***** -->";
        private const bool TEST_MODE = false;  // When true, Omniture image request is not sent 

        private StringBuilder pageLoadPreTag = new StringBuilder();
        private StringBuilder pageLoadPostTag = new StringBuilder();
        private bool pageWideLinkTracking = false;
        private Dictionary<int, string> props = new Dictionary<int, string>();
        private Dictionary<int, string> evars = new Dictionary<int, string>();
        private List<string> events = new List<string>();
        private string channel = "";
        private string pageName = null;
        private string pageType = "";
        private string language = "";


        /// <summary>When true, page-wide link tracking is enabled.</summary>
        public bool DoPageWideLinkTracking
        {
            get { return pageWideLinkTracking; }
            set { pageWideLinkTracking = value; }
        }

        /// <summary>the constructor builds base Omniture page load code.   
        /// Also sets the default custom variables (props), custom conversion variables (eVars), and events. .</summary>
        public WebAnalyticsPageLoad()
        {
            pageLoadPreTag.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\" src=\"/JS/Omniture/NCIAnalyticsFunctions.js\"></script>");
            pageLoadPreTag.AppendLine("<!-- SiteCatalyst code version: H.20.3. Copyright 1997-2009 Omniture, Inc. More info available at http://www.omniture.com -->");
            pageLoadPreTag.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\" src=\"/JS/Omniture/s_code.js\"></script>");
            pageLoadPreTag.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\">");
            pageLoadPreTag.AppendLine("<!--");

            // Default props, eVars, and/or events
            AddProp(WebAnalyticsOptions.Props.LongTitle, "document.title", true);  //prop10
            AddEvent(WebAnalyticsOptions.Events.PageView);

            // The following comment comes with the sample page-load tag from Omniture - it really has no relevance in this context 
            //pageLoadPostTag.AppendLine("/************* DO NOT ALTER ANYTHING BELOW THIS LINE ! **************/");

            if (!TEST_MODE)
            {
                pageLoadPostTag.AppendLine("var s_code=s.t();");
                pageLoadPostTag.AppendLine("if(s_code)");
                pageLoadPostTag.AppendLine("   document.write(s_code);");
            }

            pageLoadPostTag.AppendLine("-->");
            pageLoadPostTag.AppendLine("</script>");
            if (WebAnalyticsOptions.EnableNonJavaScriptTagging)
                pageLoadPostTag.Append(NoScriptTag().ToString());
            pageLoadPostTag.AppendLine("<!-- End SiteCatalyst code version: H.20.3. -->");
            pageLoadPostTag.AppendLine(WEB_ANALYTICS_COMMENT_END);
        }

        /// <summary>Builds the Page-wide link tracking JavaScript code inserted into the Omniture page load code.</summary>
        private StringBuilder LinkTrackPageLoadCode()
        {
            //Page-wide link tracking is currently not used 

            //This should be moved into a function in the NCIAnalytics.js file.
            StringBuilder linkTrackerPageLoadCode = new StringBuilder();

            linkTrackerPageLoadCode.AppendLine("// Page-wide click tracking");
            linkTrackerPageLoadCode.AppendLine("if (document.addEventListener)");
            linkTrackerPageLoadCode.AppendLine("   document.addEventListener('click',NCIAnalytics.LinkTrackTagBuilder,false);");
            linkTrackerPageLoadCode.AppendLine("else if (document.attachEvent)");
            linkTrackerPageLoadCode.AppendLine("   document.attachEvent('onclick',NCIAnalytics.LinkTrackTagBuilder);");
            linkTrackerPageLoadCode.AppendLine("// End Page-wide click tracking");

            return linkTrackerPageLoadCode;
        }

        private StringBuilder NoScriptTag()
        {
            StringBuilder noScriptTag = new StringBuilder();

            noScriptTag.AppendLine("<noscript>");
            noScriptTag.AppendLine("<a href='http://www.omniture.com' title='Web Analytics'>");
            noScriptTag.AppendLine("<img src='http://metrics.cancer.gov/b/ss/nciglobal/1/H.20.3–NS/0' height='1' width='1' border='0' alt='' />");
            noScriptTag.AppendLine("</a>");
            noScriptTag.AppendLine("</noscript>");

            return noScriptTag;
        }

        /// <summary>When DoWebAnalytics is true, this method renders the Omniture page load JavaScript code.</summary>
        public string Tag()
        {
            StringBuilder output = new StringBuilder();
            string reportSuites = "";

            if (WebAnalyticsOptions.IsEnabled)
            {
                output.AppendLine(WEB_ANALYTICS_COMMENT_START);

                // Report Suites JavaScript variable (s_account) must be set before the s_code file is loaded
                foreach (string suite in WebAnalyticsOptions.GetSuitesForChannel(channel, language))
                {
                    if (reportSuites.Length > 0)
                        reportSuites += ",";
                    reportSuites += suite;
                }
                output.AppendLine("<script language=\"JavaScript\" type=\"text/javascript\">");
                output.AppendLine("<!--");
                output.AppendLine("var s_account=" + DELIMITER + reportSuites + DELIMITER + ";");
                output.AppendLine("-->");
                output.AppendLine("</script>");

                output.Append(pageLoadPreTag.ToString());

                if (pageWideLinkTracking)
                {
                    // Page-wide link tracking is current not used - this may be implemented at a future date
                    // This however should just output a JS function call that lives in the NCIAnalytics.js
                    // file instead of the inline code.
                    //output.AppendLine(LinkTrackPageLoadCode().ToString());
                }

                if (channel != "") // if channel is set, output them to the tag
                    output.AppendLine("s.channel=" + DELIMITER + channel + DELIMITER + ";");

                if (pageName != null) // if pageName is not null (emptry string ok), output them to the tag
                    output.AppendLine("s.pageName=" + DELIMITER + pageName + DELIMITER + ";");

                if (pageType != "") // if pageType is set, output them to the tag
                    output.AppendLine("s.pageType=" + DELIMITER + pageType + DELIMITER + ";");

                if (props.Count > 0) // if props are set, output them to the tag
                {

                    foreach (var k in props.Keys.OrderBy(k => k))
                    {
                        output.AppendLine("s.prop" + k.ToString() + "=" + props[k] + ";");
                    }
                }

                if (evars.Count > 0) // if eVars are set, output them to the tag
                {
                    var items = from k in evars.Keys
                                orderby k ascending
                                select k;
                    foreach (int k in items)
                    {
                        output.AppendLine("s.eVar" + k.ToString() + "=" + evars[k] + ";");
                    }
                }

                if (events.Count > 0)  // if events have been defined, output then to the tag
                {
                    output.AppendLine("s.events=" + DELIMITER + string.Join(",", events.ToArray<string>()) + DELIMITER + ";");
                }

                output.AppendLine("");
                output.AppendLine(pageLoadPostTag.ToString());

            }
            return output.ToString();
        }

        /// <summary>Adds an Omniture custom variable (prop) to the Omniture page load JavaScript code 
        /// with delimiters attached to value parameter.</summary>
        /// <param name="propNumber">Omniture custom variable (prop) number</param>
        /// <param name="value">Value assigned to Omniture custom variable (prop)</param>
        public void AddProp(int propNumber, string value)
        {
            AddProp(propNumber, value, false);
        }

        /// <summary>Adds an Omniture custom variable (prop) to the Omniture page load JavaScript code 
        /// with delimiters attached to value parameter.</summary>
        /// <param name="propNumber">Omniture custom variable (prop) number</param>
        /// <param name="value">Value assigned to Omniture custom variable (prop)</param>
        public void AddProp(WebAnalyticsOptions.Props propNumber, string value)
        {
            AddProp((int)propNumber, value, false);

        }

        /// <summary>Adds Omniture custom variable (prop) to the Omniture page load JavaScript code.</summary>
        /// <param name="propNumber">Omniture custom variable (prop) number</param>
        /// <param name="value">Value assigned to Omniture custom variable (prop)</param>
        /// <param name="NoDelimiters">If true, delimiters are added to the beginning and end of the value parameter.  If false,
        /// no delimiters are added (used when value parameter already contains delimiters)</param>
        public void AddProp(WebAnalyticsOptions.Props propNumber, string value, bool NoDelimiters)
        {
            AddProp((int)propNumber, value, NoDelimiters);
        }

        /// <summary>Adds Omniture custom variable (prop) to the Omniture page load JavaScript code.</summary>
        /// <param name="propNumber">Omniture custom variable (prop) number</param>
        /// <param name="value">Value assigned to Omniture custom variable (prop)</param>
        /// <param name="NoDelimiters">If true, delimiters are added to the beginning and end of the value parameter.  If false,
        /// no delimiters are added (used when value parameter already contains delimiters)</param>
        public void AddProp(int propNumber, string value, bool NoDelimiters)
        {
            // if value is null set to empty string 
            value = value ?? string.Empty;
            string newValue = NoDelimiters ? value : DELIMITER + value.Replace("'", "\\'") + DELIMITER;

            if (props.ContainsKey(propNumber))
                props[propNumber] = newValue;
            else
                props.Add(propNumber, newValue);
        }

        /// <summary>Adds an Omniture custom conversion variable (eVar) to the Omniture page load JavaScript code with delimiters.</summary>
        /// <param name="eVarNumber">Omniture custom conversion variable (eVar) number</param>
        /// <param name="value">Value assigned to Omniture custom conversion variable (eVar)</param>
        public void AddEvar(int eVarNumber, string value)
        {
            AddEvar(eVarNumber, value, false);
        }

        /// <summary>Adds an Omniture custom conversion variable (eVar) to the Omniture page load JavaScript code with delimiters.</summary>
        /// <param name="eVarNumber">Omniture custom conversion variable (eVar) number</param>
        /// <param name="value">Value assigned to Omniture custom conversion variable (eVar)</param>
        public void AddEvar(WebAnalyticsOptions.eVars eVarNumber, string value)
        {
            AddEvar((int)eVarNumber, value, false);
        }

        /// <summary>Adds Omniture custom conversion variable (eVar) to the Omniture page load JavaScript code.</summary>
        /// <param name="eVarNumber">Omniture custom conversion variable (eVar) number</param>
        /// <param name="value">Value assigned to Omniture custom conversion variable (eVar)</param>
        /// <param name="NoDelimiters">If true, delimiters are added to the beginning and end of value parameter.  If false,
        /// no delimiters are added (used when value param already contains delimiters)</param>
        public void AddEvar(WebAnalyticsOptions.eVars eVarNumber, string value, bool NoDelimiters)
        {
            AddEvar((int)eVarNumber, value, NoDelimiters);
        }

        /// <summary>Adds Omniture custom conversion variable (eVar) to the Omniture page load JavaScript code.</summary>
        /// <param name="eVarNumber">Omniture custom conversion variable (eVar) number</param>
        /// <param name="value">Value assigned to Omniture custom conversion variable (eVar)</param>
        /// <param name="NoDelimiters">If true, delimiters are added to the beginning and end of value parameter.  If false,
        /// no delimiters are added (used when value param already contains delimiters)</param>
        public void AddEvar(int eVarNumber, string value, bool NoDelimiters)
        {
            // if value is null set to empty string 
            value = value ?? string.Empty;
            string newValue = NoDelimiters ? value : DELIMITER + value.Replace("'", "\\'") + DELIMITER;

            if (evars.ContainsKey(eVarNumber))
                evars[eVarNumber] = newValue;
            else
                evars.Add(eVarNumber, newValue);
        }

        /// <summary>Adds Omniture event to the Omniture page load JavaScript code.</summary>
        /// <param name="eventNumber">Omniture event number</param>
        public void AddEvent(WebAnalyticsOptions.Events eventNumber)
        {
            AddEvent((int)eventNumber);
        }

        /// <summary>Adds Omniture event to the Omniture page load JavaScript code.</summary>
        /// <param name="eventNumber">Omniture event number</param>
        public void AddEvent(int eventNumber)
        {
            if (eventNumber > 0)
            {
                string eventString = "event" + eventNumber.ToString();
                if (!events.Contains(eventString))
                    events.Add(eventString);
            }
        }


        /// <summary>Sets the value of the Omniture channel variable in the Omniture page load JavaScript code.</summary>
        /// <param name="channelValue">Value assigned to Omniture channel variable</param>
        public void SetChannel(string channelValue)
        {
            channel = channelValue.Replace("'", "\\'");

        }

        /// <summary>Sets the language in the Omniture page load JavaScript code.</summary>
        /// <param name="languageValue">Value assigned to Omniture language variable: english, spanish</param>
        public void SetLanguage(string languageValue)
        {
            switch (languageValue.ToLower())
            {
                case "en":
                    languageValue = "english";
                    break;
                case "es":
                    languageValue = "spanish";
                    break;
                default:
                    languageValue = "english";
                    break;
            }
            this.AddProp(WebAnalyticsOptions.Props.Language, languageValue); // prop8
            this.AddEvar(WebAnalyticsOptions.eVars.Language, languageValue); // eVar2
            language = languageValue;
        }

        /// <summary>Sets the value of the Omniture pageName variable in the Omniture page load JavaScript code.</summary>
        /// <param name="pageNameValue">Value assigned to Omniture pageName variable</param>
        public void SetPageName(string pageNameValue)
        {
            pageName = pageNameValue.Replace("'", "\\'");
            this.AddEvar(WebAnalyticsOptions.eVars.PageName, pageName);
        }

        /// <summary>Sets the value of the Omniture pageType  variable in the Omniture page load JavaScript code.</summary>
        /// <param name="pageTypeValue">Value assigned to Omniture pageType variable</param>
        public void SetPageType(string pageTypeValue)
        {
            pageType = pageTypeValue;
        }

        /// <summary>Clears all previously set props, eVars, events, channel, pageName, and pageType.</summary>
        public void ClearAll()
        {
            props.Clear();
            evars.Clear();
            events.Clear();
            channel = "";
            pageName = null;
            pageType = "";
        }

    }
}
