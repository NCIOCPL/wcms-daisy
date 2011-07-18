using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using NCI.Web.CDE.Modules;
using NCI.DataManager;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This class is the base implemenation for Dynamic Search & display control and Content Search & 
    /// display control.
    /// </summary>
    public abstract class BaseSearchSnippet : SnippetControl
    {
        #region Private Members
        SearchList _searchList = null;

        /// <summary>
        /// The current page that is being used.
        /// </summary>
        private int CurrentPage
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["page"]))
                    return 1;
                return Int32.Parse(this.Page.Request.Params["page"]);
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Keyword is a search criteria used in searching
        /// </summary>
        virtual protected string KeyWords
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return string.Empty;
                return this.SearchList.SearchParameters.Keyword;
            }
        }

        /// <summary>
        /// Startdate is a search criteria used in searching.if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        /// </summary>
        virtual protected DateTime StartDate
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return DateTime.MinValue;
                return string.IsNullOrEmpty(this.SearchList.SearchParameters.StartDate) ? DateTime.MinValue : DateTime.Parse(this.SearchList.SearchParameters.StartDate);
            }
        }

        /// <summary>
        /// Enddate is a search criteria used in searching, if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        /// </summary>
        virtual protected DateTime EndDate
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return DateTime.MaxValue;
                return string.IsNullOrEmpty(this.SearchList.SearchParameters.EndDate) ? DateTime.MaxValue : DateTime.Parse(this.SearchList.SearchParameters.EndDate);
            }
        }

        protected virtual SearchList SearchList
        { get; set; }

        #endregion

        #region Public
        public void Page_Load(object sender, EventArgs e)
        {
            processData();
        }

        #endregion

        #region Private Methods
        private void processData()
        {
            try
            {
                if (this.SearchList != null)
                {
                    // Validate();

                    int actualMaxResult = this.SearchList.MaxResults;

                    DateTime startDate = StartDate, endDate = EndDate;
                    string keyWord = KeyWords;
                    if (this.SearchList.SearchType == "keyword")
                    {
                        startDate = DateTime.MinValue;
                        endDate = DateTime.MaxValue;
                    }
                    else if (this.SearchList.SearchType == "date")
                    {
                        keyWord = string.Empty;
                    }

                    // Call the  datamanger to perform the search
                    ICollection<SearchResult> searchResults =
                                SearchDataManager.Execute(CurrentPage, startDate, endDate, keyWord,
                                    this.SearchList.RecordsPerPage, this.SearchList.MaxResults, this.SearchList.SearchFilter,
                                    this.SearchList.ExcludeSearchFilter, this.SearchList.ResultsSortOrder, this.SearchList.Language, Settings.IsLive, out actualMaxResult);

                    DynamicSearch dynamicSearch = new DynamicSearch();
                    dynamicSearch.Results = searchResults;
                    dynamicSearch.StartDate = String.Format("{0:MM/dd/yyyy}", startDate);
                    dynamicSearch.EndDate = String.Format("{0:MM/dd/yyyy}", endDate);
                    dynamicSearch.KeyWord = keyWord;

                    if (actualMaxResult > 0)
                    {
                        if (CurrentPage > 1)
                            dynamicSearch.StartCount = (this.SearchList.RecordsPerPage * (CurrentPage - 1)) + 1;
                        else
                        {
                            dynamicSearch.StartCount = 1;
                        }

                        if (CurrentPage == 1)
                        {
                            dynamicSearch.EndCount = this.SearchList.RecordsPerPage;
                            if (searchResults.Count < this.SearchList.RecordsPerPage)
                                dynamicSearch.EndCount = actualMaxResult;
                        }
                        else
                        {
                            dynamicSearch.EndCount = dynamicSearch.StartCount + this.SearchList.RecordsPerPage - 1;
                            if (searchResults.Count < this.SearchList.RecordsPerPage)
                                dynamicSearch.EndCount = actualMaxResult;
                        }
                    }

                    int recCount = 0;
                    foreach (SearchResult sr in searchResults)
                        sr.RecNumber = dynamicSearch.StartCount + recCount++;

                    int validCount = this.SearchList.MaxResults;

                    if (actualMaxResult < this.SearchList.MaxResults || this.SearchList.MaxResults == 0)
                        validCount = actualMaxResult;
                    else
                        validCount = this.SearchList.MaxResults;

                    dynamicSearch.ResultCount = validCount;
                    LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResults(this.SearchList.ResultsTemplate, dynamicSearch));
                    Controls.Add(ltl);
                    SetupPager(this.SearchList.RecordsPerPage, validCount);
                }
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("this.SearchListSnippet:processData", NCI.Logging.NCIErrorLevel.Error, ex);
            }
        }
        /// <summary>
        /// Helper method to setup the pager
        /// </summary>
        private void SetupPager(int recordsPerPage, int totalRecordCount)
        {
            SimplePager pager = new SimplePager();
            pager.RecordCount = totalRecordCount;
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = CurrentPage;
            pager.PageParamName = "page";
            pager.PagerStyleSettings.SelectedIndexCssClass = "pager-SelectedPage";
            pager.BaseUrl = PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();
            
            string searchQueryParams = string.Empty;
            if (this.SearchList.SearchType.ToLower() == "keyword" || this.SearchList.SearchType.ToLower() == "keyword_with_date")
                searchQueryParams = "?keyword=" + Server.HtmlEncode(KeyWords);
            if (this.SearchList.SearchType.ToLower() == "date" || this.SearchList.SearchType.ToLower()=="keyword_with_date")
            {
                if (string.IsNullOrEmpty(searchQueryParams))
                    searchQueryParams = "?";
                else
                    searchQueryParams += "&";
                if (StartDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                    searchQueryParams += string.Format("startMonth={0}&startyear={1}&endMonth={2}&endYear={3}", StartDate.Month, StartDate.Year, EndDate.Month, EndDate.Year);
                else
                    searchQueryParams += "startMonth=&startyear=&endMonth=&endYear=";
             }

            pager.BaseUrl += searchQueryParams;

            Controls.Add(pager);
        }
        /// <summary>
        /// Validates the data received from the xml, throws an exception if the required 
        /// fields are null or empty.
        /// </summary>
        /// <param name="this.SearchList">The object whose properties are being validated.</param>
        private void Validate()
        {
            if (string.IsNullOrEmpty(this.SearchList.SearchFilter) ||
                string.IsNullOrEmpty(this.SearchList.ResultsTemplate) ||
                string.IsNullOrEmpty(this.SearchList.SearchType))
                throw new Exception("One or more of these fields SearchFilter,ResultsTemplate,SearchType cannot be empty, correct the xml data.");

        }
        #endregion

    }
}
