using System;
using System.Collections;
using System.Configuration;
using Endeca.Navigation;
using NCI.Util;

namespace NCI.Search.Endeca
{
	/// <summary>
	/// Summary description for EndecaBestBetsSearch.
	/// </summary>
	public class EndecaBestBetsSearch : EndecaSearch
	{


		public EndecaBestBetsSearch(string searchTerms, string doc_type) : base(
			searchTerms, 
			10000, 
			ConfigurationSettings.AppSettings["EndecaBBIP"], 
			ConfigurationSettings.AppSettings["EndecaBBPort"], 
			ConfigurationSettings.AppSettings["EndecaBBSearchInterface"], 
			ConfigurationSettings.AppSettings["EndecaBBSearchMode"],
			doc_type
			) 
		{

		}

		private ArrayList GetStopWords() 
		{
			ArrayList rtnList = new ArrayList();
			string stw = ConfigurationSettings.AppSettings["EndecaBBStopWords"];

			if (stw != null) 
			{
				string[] starr = stw.Split(new char[]{ ','});
				foreach (string s in starr) 
				{
					string s1 = Strings.Clean(s);
					if (s1 != null)
						rtnList.Add(s1);
				}
			}

			return rtnList;
		}

		private ArrayList GetSplitTerms(string terms) 
		{
			ArrayList rtnList = new ArrayList();

			if (terms != null) {
				string[] starr = terms.Split(new char[]{ ' '});
				foreach (string s in starr) {
					string s1 = Strings.Clean(s);
					if (s1 != null)
						rtnList.Add(s1);
				}
			}

			return rtnList;
		}

		public override void FillSearchResults(ArrayList resultsCollection) {

			Hashtable matchedErecs = new Hashtable();
			ArrayList matches = new ArrayList();
			ArrayList excludeMatches = new ArrayList();
			
			if (results.ContainsNavigation()) {

				int keywordCount = 0;

				//Get the number of terms in the search. (We need this for exact matches)
				if (results.Navigation.ESearchReports.Count > 0 && results.Navigation.ESearchReports.Contains(this.searchInterface)) {
					
					//Get stop words. //Test the collection
					ArrayList stopWords = GetStopWords();

					string terms = ((ESearchReport)results.Navigation.ESearchReports[this.searchInterface]).Terms;
					ArrayList termsList = GetSplitTerms(terms);

					//Loop through the terms to get the count not counting the stop words.
					if (stopWords.Count > 0) 
					{
						foreach (string term in termsList)
							if (!stopWords.Contains(term))
								keywordCount++;
					} 
					else 
					{
						keywordCount = termsList.Count;
					}
				}
				
				//Loop through the records and find valid matches and excludes
				foreach (ERec result in results.Navigation.ERecs) {

					//Get the wdim records back
					ArrayList wdim = result.Properties.GetValues("DGraph.WhyDidItMatch");

					//Get the number of words back
					int numWords = Strings.ToInt(result.Properties["NumberOfWords"].ToString());

					//If wdim == numWords then this item is either a Keyword match or an Exclude match
					if (numWords == wdim.Count) {

						//If the record is an exact match category or synonym, then we need to see if the number of keywords
                        //the user typed in matches the number of words in the term.  If it does not match then we need to skip
                        //this record.  Otherwise it works the exact same way as everything else.
                        if (result.Properties.Contains("IsExactMatch") && (Strings.ToBoolean(result.Properties["IsExactMatch"].ToString()) && keywordCount != numWords))
                            continue;

						string categoryID = result.Properties["CategoryID"].ToString();

                        if (!Strings.ToBoolean(result.Properties["IsExclude"].ToString()))
                        { //This is a keyword match							
                            if ((!excludeMatches.Contains(categoryID)) && (!matchedErecs.Contains(categoryID))) {
								//As long as this erec is not in matches already and not in excludes, then add it
								//into matchedErecs.  It will take less time to loop through matches than 
								//looping through all of the erecs again
								matchedErecs.Add(categoryID,result);
								matches.Add(categoryID);
							}
						} else { //This is an exclude match							
							if (!excludeMatches.Contains(categoryID)) { 
								excludeMatches.Add(categoryID);
							}
						}
					}
				}

				//Now loop through the matched best bets and fill the resultsCollection
				foreach (string categoryID in matches) {
					if (!excludeMatches.Contains(categoryID)) {
						//This is a good erec.
						if (matchedErecs.Contains(categoryID)) {
							resultsCollection.Add(new EndecaBestBetResult((ERec)matchedErecs[categoryID]));
						}
					}
				}
			}
		}

		
	}
}
