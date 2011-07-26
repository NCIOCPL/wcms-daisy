using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.WebAnalytics
{
    /// <summary>
    /// Class which represets differtent types of data points used in CDE.
    /// An instance of this class is populated by objects that implement 
    /// IPageAssemblyInstruction
    /// </summary>
    public class WebAnalyticsSettings
    {
        /// <summary>
        /// Sets or get the prop attribute value
        /// </summary>
        public IDictionary<WebAnalyticsOptions.Props, string> Props { get; set; }
        /// <summary>
        /// Gets or Sets the evar attributes
        /// </summary>
        public IDictionary<WebAnalyticsOptions.eVars, string> Evars { get; set; }
        /// <summary>
        /// Gets or Sets the Events attributes
        /// </summary>
        public IDictionary<WebAnalyticsOptions.Events, string> Events { get; set; }

        public WebAnalyticsSettings()
        { 
            Props = new Dictionary<WebAnalyticsOptions.Props, string>();
            Evars = new Dictionary<WebAnalyticsOptions.eVars, string>();
            Events = new Dictionary<WebAnalyticsOptions.Events, string>();
        }
    }
}
