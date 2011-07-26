using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE
{
    /// <summary>
    /// IPageAssemblyInstruction interface provides access to the components,layout information and fields of a page published by percussion.
    /// </summary>
    public interface IPageAssemblyInstruction
    {

        /// <summary>
        /// Gets the name of the page template i.e the actual aspx page to be loaded.
        /// </summary>
        /// <value>The name of the page template.</value>
        string PageTemplateName { get; }

        /// <summary>
        /// The path of all parent folders of the page assembly instruction.        
        /// </summary>
        string SectionPath { get; }
        /// <summary>
        /// Gets a collection of SnippetInfo objects for the page assembly instruction which are needed to render a page.
        /// </summary>
        IEnumerable<SnippetInfo> Snippets { get; }
        /// <summary>
        /// Provides the Metadata information
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string GetField(string fieldName);
        /// <summary>
        /// Provides components of system with a URL for a page.
        /// </summary>
        /// <param name="urlType"></param>
        /// <returns></returns>
        NciUrl GetUrl(string urlType);

        /// <summary>        
        ///  When a component needs to modify the metadata of a page a field filter is added for that field name using AddFieldFilter.
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="filter"></param>
        void AddFieldFilter(string fieldname,FieldFilterDelegate filter);

        /// <summary>
        ///  When a component needs to modify the URL of a page a URL filter is added for that URL name using AddUrlFilter.
        /// </summary>
        /// <param name="urlType"></param>
        /// <param name="fieldFilter"></param>
        void AddUrlFilter(string urlType, UrlFilterDelegate fieldFilter);
        
        /// <summary>
        /// Gets or sets the language for the page displayed.
        /// </summary>
        /// <value>The language.</value>
        string Language { get; set; }

        /// <summary>
        /// BlockedSlots contain information about the blocked slot which should not be displayed on the page rendered. 
        /// </summary>
        /// <value>The blocked slot names.</value>
        string[] BlockedSlotNames { get; }

        /// <summary>
        /// This property returns the keys which represent the available content versions. 
        /// </summary>
        /// <value>A string array which are the keys to the alternate content versions.</value>
        string[] AlternateContentVersionsKeys { get; }

        /// <summary>
        /// This method returns the web analytics settings.
        /// </summary>
        WebAnalyticsSettings GetWebAnalytics();

        /// <summary>
        /// When a data point related to web anlytics is to be modified it is done using this method. 
        /// </summary>
        /// 

        void SetWebAnalytics(WebAnalyticsOptions.Events webAnalyticType, WebAnalyticsDataPointDelegate filter);
        void SetWebAnalytics(WebAnalyticsOptions.eVars webAnalyticType, WebAnalyticsDataPointDelegate filter);
        void SetWebAnalytics(WebAnalyticsOptions.Props webAnalyticType, WebAnalyticsDataPointDelegate filter);
        void Initialize();
    }
}
