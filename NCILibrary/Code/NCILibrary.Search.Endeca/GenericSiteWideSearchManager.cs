using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using NCI.Search;
using NCI.Util;
using System.Collections;
using Endeca.Navigation;
using System.Text.RegularExpressions;

namespace NCI.Search.Endeca
{
    public static class GenericSiteWideSearchManager
    {
        public static ISiteWideSearchResultCollection GetSearchResults(string keyword, int page, int numPerPage, long dimFilter)
        {
            return GetSearchResults(keyword, page, numPerPage, dimFilter, false);
        }
        
        public static ISiteWideSearchResultCollection GetSearchResults(string keyword, int page, int numPerPage, long dimFilter, bool enableDidYouMean)
        {
            string ip = Strings.Clean(ConfigurationManager.AppSettings["EndecaSWSearchIP"]);
            string port = Strings.Clean(ConfigurationManager.AppSettings["EndecaSWSearchPort"]);
            string searchInterface = "All";
            return GetSearchResults(keyword, page, numPerPage, dimFilter, ip, port, searchInterface, enableDidYouMean);
        }

        public static ISiteWideSearchResultCollection GetSearchResults(string keyword, int page, int numPerPage, long dimFilter, string searchIP, string searchPort, string searchInterface)
        {
            return GetSearchResults(keyword, page, numPerPage, dimFilter, searchIP, searchPort, searchInterface, false);
        }

        public static ISiteWideSearchResultCollection GetSearchResults(string keyword, int page, int numPerPage, long dimFilter, string searchIP, string searchPort, string searchInterface, bool enableDidYouMean)
        {
            GenericSiteWideSearchResultCollection rtnResults = null;

            //string ip = Strings.Clean(ConfigurationManager.AppSettings["EndecaSWSearchIP"]);
            //string port = Strings.Clean(ConfigurationManager.AppSettings["EndecaSWSearchPort"]);
            //string searchInterface = "All";

            if ((searchIP == null) || (searchPort == null))
            {
                //TODO: Throw error
            }

            //Get offset and number of results
            int offset = (page - 1) * numPerPage;

            EndecaNormalSearchDefinition searchDef = new EndecaNormalSearchDefinition();
            EndecaNavigationSearchParameter searchParam = new EndecaNavigationSearchParameter(keyword, searchInterface, EndecaMatchModes.MatchAllPartial);


            searchDef.DimensionFilters.Add(dimFilter);
            searchDef.ResultsPerPage = numPerPage;
            searchDef.StartRecord = offset;
            searchDef.SearchParameters.Add(searchParam);
            searchDef.EnableDidYouMean = enableDidYouMean;

            GenericEndecaSearch search = new GenericEndecaSearch(searchIP, searchPort, searchDef);

            ENEQueryResults results = search.GetSearchResults();

            rtnResults = GetResultsFromENEQueryResults(results);

            return rtnResults;
        }

        private static GenericSiteWideSearchResultCollection GetResultsFromENEQueryResults(ENEQueryResults results)
        {
            GenericSiteWideSearchResultCollection rtnResults = new GenericSiteWideSearchResultCollection();
            ArrayList highlightFormList = new ArrayList();
            string dym = GetDYMFromENEQueryResults(results);

            if (results.ContainsNavigation())
            {
                /**** Beginning of Search Highlighting Code Step 1 (gather word forms) ****/
                foreach (DictionaryEntry de in results.Navigation.ESearchReports)
                {
                    IList rawTerms = new ArrayList();
                    ESearchReport report = (ESearchReport)de.Value;

                    // 1: get search term; if auto corrected, use that, otherwise use original search term
                    if (report.AutoSuggestions.Count == 0)
                    {
                        // no suggestions, use orig. term
                        String term = (report.TruncatedTerms == "") ? report.Terms : report.TruncatedTerms;
                        rawTerms.Add(term);
                    }
                    else
                    {
                        // phrasing or spelling autocorrect changed the term from the original, capture that
                        foreach (ESearchAutoSuggestion autoSugg in report.AutoSuggestions)
                        {
                            rawTerms.Add(autoSugg.Terms);
                        }
                    }

                    // 2: clean up raw term list gathered above
                    // the following Pattern will match dbl-quote enclosures
                    Regex rx = new Regex("\"([^\"]*)\"", RegexOptions.Compiled);

                    foreach (String rt in rawTerms)
                    {
                        String rawTerm = rt;

                        // 2a: check for the corner-case of unclosed quotes, which the engine
                        // closes automatically.. this code should follow suit so the
                        // term highlighting reflects what the engine has done
                        if (!rx.IsMatch(rawTerm) && rawTerm.IndexOf("\"") >= 0)
                        {
                            rawTerm += "\"";
                        }

                        // 2b: for any quoted text enclosures, add these quoted terms to the highlight list
                        Match m = rx.Match(rawTerm);
                        while (m.Success)
                        {
                            Group g = m.Groups[1];
                            Capture c = g.Captures[0];
                            if (c.Value.Length > 0)
                            {
                                highlightFormList.Add(c.Value);
                            }
                            m = m.NextMatch();
                        }

                        // 2c: strip out quoted text from terms and proceed with normal tokenization
                        rawTerm = rx.Replace(rawTerm, "");

                        // 2d: split term on greedy non-word char's, just like engine does
                        // add it to the list of terms to scan for
                        foreach (String origWord in Regex.Split(rawTerm, "\\W+"))
                        {
                            // check for any empty tokens just in case...
                            if (origWord.Length > 0)
                            {
                                highlightFormList.Add(origWord);
                            }
                        }
                    }

                    // 3: add word forms from Word Interpretation feature
                    PropertyMap wiMap = report.WordInterps;
                    foreach (ICollection wordInterpColl in wiMap.Values)
                    {
                        // wordInterp provides a PropertyMap, which has Collection-typed values
                        // each Collection value is the set of synonyms for one of the terms
                        foreach (String wordInterp in wordInterpColl)
                        {
                            highlightFormList.Add(wordInterp);
                        }
                    }
                    highlightFormList.Sort(new DescStrLengthComparer());
                }


                foreach (ERec result in results.Navigation.ERecs)
                {

                    string title = Strings.Clean(result.Properties["Title"]);
                    string description = Strings.Clean(result.Properties["Description"]);
                    string url = Strings.Clean(result.Properties["URL"]);

                    GenericSiteWideSearchResult sr = new GenericSiteWideSearchResult(title, description, url);
                    rtnResults.Add(sr);
                }
            }

            rtnResults.DidYouMean = dym;
            rtnResults.WordForms = highlightFormList;
            rtnResults.TotalNumResults = results.Navigation.TotalNumERecs;

            return rtnResults;
        }

        private static string GetDYMFromENEQueryResults(ENEQueryResults results)
        {
            string str = null;
            if (results != null && results.ContainsNavigation())
            {
                IDictionary recSrchRprts = results.Navigation.ESearchReports;
                if (recSrchRprts.Count > 0)
                {
                    //Need to figure out what this wackyness is.
                    IDictionaryEnumerator ide = recSrchRprts.GetEnumerator();
                    ide.MoveNext();
                    ESearchReport searchRep = (ESearchReport)ide.Value;

                    //get the list of did you mean objects
                    IList dymList = searchRep.DYMSuggestions;
                    if (dymList.Count > 0)
                    {
                        ESearchDYMSuggestion dymSug = (ESearchDYMSuggestion)dymList[0];
                        str = dymSug.Terms;
                    }

                }
            }

            return str;
        }

        
    }
}
