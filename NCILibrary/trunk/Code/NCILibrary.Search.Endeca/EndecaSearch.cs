using System;
using System.Collections;
using System.Configuration;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
	/// <summary>
	/// Summary description for EndecaSearch.
	/// </summary>
	public class EndecaSearch
	{

		protected HttpENEConnection eneConnection;
		protected ENEQuery searchQuery;
		protected ENEQueryResults results;
		protected string searchInterface;

		public long TotalSearchResults {
			get {
				if (results != null) {
					if (results.ContainsNavigation()) {
						
						return (int)results.Navigation.TotalNumERecs;
						/*
						if (results.Navigation.ESearchReports.Contains(searchInterface)) {
							ESearchReport searchReport = (ESearchReport)results.Navigation.ESearchReports[searchInterface];
							return searchReport.NumMatchingResults;
						} */
					}
				}

				return 0;
			}
		}

		public string DidYouMean(){
			string str=null;
			if (results!=null &&  results.ContainsNavigation()){
				IDictionary recSrchRprts =   results.Navigation.ESearchReports;
				if (recSrchRprts.Count>0)
				{
					IDictionaryEnumerator ide = recSrchRprts.GetEnumerator();
					ide.MoveNext();
					ESearchReport searchRep = (ESearchReport)ide.Value;
					//get the list of did you mean objects
					IList dymList = searchRep.DYMSuggestions;
					if (dymList.Count > 0)
					{
						ESearchDYMSuggestion dymSug = (ESearchDYMSuggestion)dymList[0];
						str= dymSug.Terms;
					}

				}
			}
			return str;
		}

		public EndecaSearch(string searchTerms, long numberOfRecords, long firstRecord, string doc_type)
		{
			//Step 1.  Create a connection.
			string host = ConfigurationSettings.AppSettings["EndecaSearchIP"];
			string port = ConfigurationSettings.AppSettings["EndecaSearchPort"];
			CreateConnection(host,port);

			//Step 2.  Create a query
			searchQuery = new ENEQuery();

			//Step 2a.  Setup the ENEQuery
			
			//Get the default search interface
			searchInterface = ConfigurationSettings.AppSettings["EndecaSearchInterface"];
			//Get the default search mode
			string searchOptions = ConfigurationSettings.AppSettings["EndecaSearchMode"];

			//Setup the queries.  An ERecSearch defines the search query.  See the comment in GetSearchItem.
			//I will probably create a class for these, and pass this in to this constructor... but for now.
			ERecSearch searchItem = GetSearchItem(searchInterface, searchTerms, searchOptions);
			
			//Put the search query into the um, search query?
			searchQuery.NavERecSearches = new ERecSearchList();
			searchQuery.NavERecSearches.Add(0,searchItem); // <-- JUST IMPLIMENT ADD(object)!!!

			//enable did you mean feature
			searchQuery.NavERecSearchDidYouMean = true;
			
			//This is going to get tricky to explain.  Thier search works by searching through nodes in concept trees.
			//By default we search through 0, which is the root node.  But say I wanted to narrow my search results for
			//the search term "Breast Cancer" by seeing all the results in the fictious NCI Taxonomy:
			//  ROOT (This is the top node of all of the different taxonomies)   Value: 0
			//    |
			//  Types Of Cancer -- Value: 100
			//      |
			//       ----> Breast Cancer -- Value: 150
			//                  |
			//                   --------> Breast Cancer Treatment (BCT) -- Value: 151
			//                  |
			//                   --------> Breast Cancer Prevention (BCP) -- Value: 152
			//
			//   I want to see all documents that match the search terms in the Breast Cancer Treatment node, then we would do
			//   searchQuery.NavDescriptors = new DimValIdList("151");
			//
			//   Now, this is a list, so I could choose to see which documents matched the search term in the BCT and BCP
			//   nodes, then this list would have 151 and 152 in it.  There is also another option that says what set operator to
			//   use.  (Intersection or Union)  But that is for another comment.  BTW, these values come from a link that is setup
			//   when rendering the dimensions, they come from the dimensions themselves.  But that to is another comment.

			searchQuery.NavDescriptors = new DimValIdList(doc_type);  

			//This says how many records to return.
			searchQuery.NavNumERecs = numberOfRecords;

			//This says what record to start at.
			searchQuery.NavERecsOffset = firstRecord;

			//That is pretty much it for now.  There are a lot more options, and well that is beyond the scope of this class for now.

		}

		public EndecaSearch(string searchTerms, long numberOfRecords, string host, string port, string searchInterface, string searchOptions, string doc_type) {

			//This needs to be done so we can get number of results.
			this.searchInterface = searchInterface;

			//Step 1.  Create a connection.
			CreateConnection(host,port);

			//Step 2.  Create a query
			searchQuery = new ENEQuery();

			//Step 2a.  Setup the ENEQuery
			
			ERecSearch searchItem = GetSearchItem(searchInterface, searchTerms, searchOptions);
			
			//Put the search query into the um, search query?
			searchQuery.NavERecSearches = new ERecSearchList();
			searchQuery.NavERecSearches.Add(0,searchItem); // <-- JUST IMPLIMENT ADD(object)!!!
			
			//enable did you mean feature
			searchQuery.NavERecSearchDidYouMean = true;

			searchQuery.NavDescriptors = new DimValIdList(doc_type); 

			//This says how many records to return.
			searchQuery.NavNumERecs = numberOfRecords;

			//This says what record to start at.
			searchQuery.NavERecsOffset = 0;

			//That is pretty much it for now.  There are a lot more options, and well that is beyond the scope of this class for now.

		}

		private ERecSearch GetSearchItem(string searchFields, string searchTerms, string searchOptions) {
			//An ERecSearch defines the search query.  Since they did not document it, I will.
			//ERecSearch(string key, string searchTerms, string opts);
			//
			// key -->  This is the name of the search interface to use, or, optionally what fields
			// to search on.  So for us, we would choose ALL.  If it is just searching against some
			// fields(Properties), I.E. Keywords and Title, then it would look like Keywords|Title.  
			// The | character separates the different fields.  Note, a new search interface requires
			// a full indexing, so XMLSearch replacement we might just uses fields instead of creating
			// new interfaces.
			//
			// searchTerms --> This is pretty self explanitory
			//
			// opts --> These are the options to search with.  This is where you set the search modes.
			// We use "mode matchallpartial" for now, if there are more options, then they are separated
			// with the | character.  
			//
			// Note: This is not really documented in the search documentation, and where it is is in the
			// URL Search Parameters section of the appendicies.  (And then it is not exactly documented)
			//
			// I will show an example of what needs to be done for xmlsearch elsewhere.

			ERecSearch searchItem = new ERecSearch(searchFields, searchTerms, searchOptions);

			return searchItem;
		}


		public EndecaSearch(string searchTerms, long numberOfRecords, long firstRecord,string startDate,string endDate, string doc_type)
		{
			//This Endeca Search is for searching Cancer Bulletin individual pages
			//Step 1.  Create a connection.
			string host = ConfigurationSettings.AppSettings["EndecaSearchIP"];
			string port = ConfigurationSettings.AppSettings["EndecaSearchPort"];
			CreateConnection(host,port);

			//Step 2.  Create a query
			searchQuery = new ENEQuery();

			//Step 2a.  Setup the ENEQuery
			
			//Get the default search interface
			searchInterface = ConfigurationSettings.AppSettings["EndecaSearchInterface"];
			//Get the default search mode
			string searchOptions = ConfigurationSettings.AppSettings["EndecaSearchMode"];

			//Setup the queries.  An ERecSearch defines the search query.  See the comment in GetSearchItem.
			//I will probably create a class for these, and pass this in to this constructor... but for now.
			ERecSearch searchItem = GetSearchItem(searchInterface, searchTerms, searchOptions);
			
			//Put the search query into the um, search query?
			searchQuery.NavERecSearches = new ERecSearchList();
			searchQuery.NavERecSearches.Add(0,searchItem); // <-- JUST IMPLIMENT ADD(object)!!!

			//enable did you mean feature
			searchQuery.NavERecSearchDidYouMean = true;
			
			searchQuery.NavDescriptors = new DimValIdList(doc_type);

			//Set up date range filters
			if (startDate!=null && endDate!=null)
			{
				string rangeFilterStr = "NCI.UpdateDate|BTWN "+startDate+" "+endDate;
				RangeFilterList rfl = new RangeFilterList();
				rfl.Add(0,new RangeFilter(rangeFilterStr));
				searchQuery.NavRangeFilters = rfl;
			}

			//This says how many records to return.
			searchQuery.NavNumERecs = numberOfRecords;

			//This says what record to start at.
			searchQuery.NavERecsOffset = firstRecord;

			//That is pretty much it for now.  There are a lot more options, and well that is beyond the scope of this class for now.

		}

		private void CreateConnection(string hostname, string port) 
		{
			eneConnection = new HttpENEConnection(hostname, port);
		}

		/// <summary>
		/// Executes the search
		/// </summary>
		public void ExecuteSearch() {
			
			try {
				results = eneConnection.Query(searchQuery);
			} catch (Exception e){
				throw e;
			}

		}

		/// <summary>
		/// This fills a EndecaResultsCollection.
		/// </summary>
		public virtual void FillSearchResults(ArrayList resultsCollection) {
			if (results.ContainsNavigation()) {

				//JESUS!  I did it again.  DO NOT, I REPEAT DO NOT, mistake
				//results.ERecs with results.Navigation.ERecs.
				//results.ERecs is if you just search for record ids.
				//results.Navigation.ERecs are the search results from 
				//searching by query.
				
				foreach (ERec result in results.Navigation.ERecs) {
					resultsCollection.Add(new EndecaResult(result));
				}
			}
		}

		//Note, when we get hard core and setup taxonimies and stuff there should be other Fill*** methods here,
		//Like FillNavigation or something like that.  Obviously, I am not going to impliment that right now.
	}
}
