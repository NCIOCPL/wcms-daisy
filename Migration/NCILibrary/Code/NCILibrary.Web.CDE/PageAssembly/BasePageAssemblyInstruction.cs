using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Web;

using NCI.Web.CDE.WebAnalytics;
using NCI.Core;
using NCI.Text;
using NCI.Util;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Abstract base class for both single page and multiple page classes to 
    /// encapsulate common members and functionality.
    /// </summary>
    abstract public class BasePageAssemblyInstruction
    {
        #region Private 
        /// <summary>
        /// Collection of FieldFilter delegates for Web analytics fields.
        /// </summary>
        private Dictionary<string, WebAnalyticsDataPointDelegate> _webAnalyticsFieldFilterDelegates = new Dictionary<string, WebAnalyticsDataPointDelegate>();
        private WebAnalyticsSettings webAnalyticsSettings = null;
        #endregion

        #region Private Methods
        #endregion

        #region Protected Members
        /// <summary>
        /// This method intialize the state of the base object or peform tasks that are 
        /// applicable to all derived class object.
        /// </summary>
        protected virtual void Initialize()
        {
            IPageAssemblyInstruction pgInst = ((IPageAssemblyInstruction)this);
            pgInst.AddUrlFilter("BookMarkShareUrl", (name, url) =>
            {
                if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.PrintAll)
                    url.SetUrl(pgInst.GetUrl("PrintAll").ToString());
                else if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.ViewAll)
                    url.SetUrl(pgInst.GetUrl("ViewAll").ToString());
                else if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.Print)
                    url.SetUrl(pgInst.GetUrl("Print").ToString());
                else 
                    url.SetUrl(pgInst.GetUrl("CurrentURL").ToString());

            });

            pgInst.AddUrlFilter("EmailUrl", (name, url) =>
            {
                if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.PrintAll)
                    url.SetUrl(pgInst.GetUrl("PrintAll").ToString());
                else if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.ViewAll)
                    url.SetUrl(pgInst.GetUrl("ViewAll").ToString());
                else if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.Print)
                    url.SetUrl(pgInst.GetUrl("Print").ToString());
                else 
                    url.SetUrl(pgInst.GetUrl("CurrentURL").ToString());
            });

            pgInst.AddFieldFilter("invokedFrom", (name, field) =>
            {
                field.Value = String.Empty;
            });
        }

        /// <summary>
        /// This protected method registers all delegates for web analytics.
        /// </summary>
        /// <param name="webAnalyticFieldName">The key or the name of the datapoint</param>
        /// <param name="filter">The actual delegate callback which will modify the FieldFilterData object</param>
        protected void SetWebAnalytics(string webAnalyticFieldName, WebAnalyticsDataPointDelegate filter)
        {
            if (string.IsNullOrEmpty(webAnalyticFieldName))
                throw new ArgumentException("The webAnalyticFieldName parameter may not be null or empty.");

            string fieldNameKey = webAnalyticFieldName;

            if (_webAnalyticsFieldFilterDelegates.ContainsKey(fieldNameKey) == false)
                _webAnalyticsFieldFilterDelegates.Add(fieldNameKey, filter);
            else
                _webAnalyticsFieldFilterDelegates[fieldNameKey] += filter;
        }

        protected WebAnalyticsSettings WebAnalyticsSettings
        {
            get 
            {
                if (webAnalyticsSettings == null)
                    webAnalyticsSettings = new WebAnalyticsSettings();
                return webAnalyticsSettings;
            }
        }

        protected virtual string GetEmailUrl()
        {
            string popUpemailUrl = "";

            string title = ((IPageAssemblyInstruction)this).GetField("long_title");
            title = System.Web.HttpUtility.UrlEncode(Strings.StripHTMLTags(title.Replace("&#153;", "__tm;")));

            string emailUrl = ((IPageAssemblyInstruction)this).GetUrl("EmailUrl").ToString();
            string invokedFrom = ((IPageAssemblyInstruction)this).GetField("invokedFrom");

            if (!string.IsNullOrEmpty(invokedFrom))
                invokedFrom = "&invokedFrom=" + invokedFrom;

            if ((Strings.Clean(emailUrl) != null) && (Strings.Clean(emailUrl) != ""))
            {
                popUpemailUrl = "/common/popUps/PopEmail.aspx?title=" + title + invokedFrom + "&docurl=" + System.Web.HttpUtility.UrlEncode(emailUrl.Replace("&", "__amp;")) + "&language=" + PageAssemblyContext.Current.PageAssemblyInstruction.Language;
                popUpemailUrl = popUpemailUrl + HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + emailUrl);
            }
            return popUpemailUrl;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// This method returns the web analytics settings for Event, Props and eVars data points.
        /// </summary>
        public virtual WebAnalyticsSettings GetWebAnalytics()
        {
            // Enumerate _webAnalyticsFieldFilterDelegates , so each delagate can be executed.
            foreach (KeyValuePair<string, WebAnalyticsDataPointDelegate> kvDel in _webAnalyticsFieldFilterDelegates)
            {
                FieldFilterData fieldData = null;
                if (Enum.IsDefined(typeof(WebAnalyticsOptions.Events), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    // Execute the delegate so we can get the value of the field
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Events.Add((WebAnalyticsOptions.Events)Enum.Parse(typeof(WebAnalyticsOptions.Events), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.eVars), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Evars.Add((WebAnalyticsOptions.eVars)Enum.Parse(typeof(WebAnalyticsOptions.eVars), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.Props), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Props.Add((WebAnalyticsOptions.Props)Enum.Parse(typeof(WebAnalyticsOptions.Props), kvDel.Key), fieldData.Value);
                }
            }

            return this.WebAnalyticsSettings;
        }

        /// <summary>
        /// Register all site wide field filters for Web Analytics.
        /// </summary>
        protected virtual void RegisterWebAnalyticsFieldFilters()
        {
            SetWebAnalytics(WebAnalyticsOptions.Props.RootPrettyURL.ToString(), wbField =>
            {
                wbField.Value = "";
            });

            //SetWebAnalytics(WebAnalyticsOptions.eVars.Language.ToString(), wbField =>
            //{
            //    string languageValue = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            //    switch (languageValue.ToLower())
            //    {
            //        case "en":
            //            wbField.Value = "english";
            //            break;
            //        case "es":
            //            wbField.Value = "spanish";
            //            break;
            //        default:
            //            wbField.Value = "english";
            //            break;
            //    }
            //});
        }
        #endregion

        #region Public 
        public ContentDates ContentDates
        {
            get
            {
                ContentDates contentDates = null;
                if (this is SinglePageAssemblyInstruction)
                    contentDates = ((SinglePageAssemblyInstruction)this).ContentDates;
                else if (this is MultiPageAssemblyInstruction)
                    contentDates = ((MultiPageAssemblyInstruction)this).ContentDates;
                return contentDates;
            }
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ContentItemInfo ContentItemInfo { get; set; }

        #endregion
    }
}
