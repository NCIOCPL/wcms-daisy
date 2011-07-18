using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using NCI.Logging;

namespace NCI.DataManager
{
    /// <summary>
    /// This class is a datamanager class and deals with all data that resides 
    /// in the database and used maily for content and dynamic searches.
    /// </summary>
    public class SearchDataManager
    {
        /// <summary>
        /// Connects to the database , and executes the stored proc with the required parameter. The 
        /// results are processed as SearchResult object.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWords"></param>
        /// <param name="recordsPerPage"></param>
        /// <param name="maxResults"></param>
        /// <param name="searchFilter"></param>
        /// <param name="excludeSearchFilter"></param>
        /// <param name="language"></param>
        /// <returns>A collection which contains Search Result objects.</returns>
        public static ICollection<SearchResult> Execute(
        int         currentPage, 
        DateTime    startDate, 
        DateTime    endDate, 
        string      keyWords, 
        int         recordsPerPage,
        int         maxResults, 
        string      searchFilter, 
        string      excludeSearchFilter, 
        int      resultsSortOrder, 
        string      language, 
        bool        isLive, 
        out int     actualMaxResult
            )
        { 
            try
            {
                ICollection<SearchResult> searchResults = new Collection<SearchResult>();
                actualMaxResult = 0;

                string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                
                if (!string.IsNullOrEmpty(connString))
                {
                    using (SqlConnection conn = SqlHelper.CreateConnection(connString))
                    {
                        using (SqlDataReader reader =                                    
                                    SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "dbo.searchFilterKeywordDate",
                                    new SqlParameter("@Keyword ", string.IsNullOrEmpty(keyWords) ? null : keyWords),
                                    new SqlParameter("@StartDate", startDate == DateTime.MinValue ? null : String.Format("{0:MM/dd/yyyy}", startDate)),
                                    new SqlParameter("@EndDate", endDate == DateTime.MaxValue ? null : String.Format("{0:MM/dd/yyyy}", endDate)),
                                    new SqlParameter("@SearchFilter", searchFilter),
                                    new SqlParameter("@excludeSearchFilter", excludeSearchFilter),
                                    new SqlParameter("@ResultsSortOrder", resultsSortOrder),
                                    new SqlParameter("@language", language),
                                    new SqlParameter("@maxResults", maxResults),
                                    new SqlParameter("@recordsPerPage", recordsPerPage),
                                    new SqlParameter("@StartPage", currentPage),
                                    new SqlParameter("@isLive", isLive ? 1 : 0)
                            ))
                        {
                            while (reader.Read())
                            {
                                actualMaxResult = reader.GetInt32(0);
                            }

                            if (reader.NextResult())
                            {
                                SqlFieldValueReader sqlFVReader = new SqlFieldValueReader(reader);

                                while (sqlFVReader.Read())
                                {
                                    SearchResult searchResult = new SearchResult();
                                    searchResult.HRef = sqlFVReader.GetString("prettyurl");
                                    searchResult.LongTitle = sqlFVReader.GetString("Long_Title");
                                    searchResult.LongDescription = sqlFVReader.GetString("Long_Description");
                                    searchResult.ShortDescription = sqlFVReader.GetString("Short_Description");
                                    searchResult.ShortTitle = sqlFVReader.GetString("Short_Title");

                                    searchResult.QandAUrl = sqlFVReader.GetString("news_qanda_url");
                                    searchResult.ImageUrl = sqlFVReader.GetString("imageurl");
                                    searchResult.VideoUrl = sqlFVReader.GetString("videourl");
                                    searchResult.AudioUrl = sqlFVReader.GetString("audiourl");
                                    searchResult.Language = sqlFVReader.GetString("language");
                                    searchResult.OtherlanguageUrl = sqlFVReader.GetString("otherlanguageURL");
                                    searchResult.DateDisplayMode = sqlFVReader.GetString("Date_Display_Mode");

                                    DateTime dt = sqlFVReader.GetDateTime("Date_first_published");
                                    if (dt != DateTime.MinValue)
                                        searchResult.PostedDate = String.Format("{0:MM/dd/yyyy}", dt);

                                    dt = sqlFVReader.GetDateTime("date_last_modified");
                                    if (dt != DateTime.MinValue)
                                        searchResult.UpdatedDate = String.Format("{0:MM/dd/yyyy}", dt);

                                    dt = sqlFVReader.GetDateTime("date_last_reviewed");
                                    if (dt != DateTime.MinValue)
                                        searchResult.ReviewedDate = String.Format("{0:MM/dd/yyyy}", dt);

                                    searchResults.Add(searchResult);
                                }
                            }
                        }
                    }
                }
                else
                    throw new Exception("Configuration Missing:Connection string is null, update the web config with connection string");

                return searchResults;
            }
            catch (Exception ex)
            {
                Logger.LogError("SearchDataManager:Execute", "Failed in DataManager", NCIErrorLevel.Error);
                throw ex;
            }
        }
    }
}
