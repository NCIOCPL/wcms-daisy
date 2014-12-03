using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;


namespace NCI.Search.Endeca
{
    /// <summary>
    /// This is an abstraction around the Endeca.Navigation search classes.
    /// <remarks>This handles Navigation searches, or what we are now calling normal searches. It also has the
    /// begining code to handle Dimension searches.  It can even do both a Nav search and a Dim search which is
    /// how you handle search results with all that cool guided navigation stuff on the right and left.  What
    /// it does not handle, and will not until we get more experience is an Aggregated record search and 
    /// record rollups.
    /// </remarks>
    /// </summary>
    public class GenericEndecaSearch
    {

        #region Fields

        private string _serverIP = "";
        private string _serverPort = "";

        //This allows us to define a normal Nav search and a dimension search in a sane way
        //private EndecaNormalSearchDefinition _normalSearchDefinition;
        //private EndecaDimensionSearchDefinition _dimensionSearchDefinition;
        //private EndecaRecordSearchDefinition _recordSearchDefinition;
        private IEndecaSearchDefinition _searchDefinition;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the normal search definition for this Endeca search.  This would be what endeca calls a Navigation search.
        /// </summary>
        //public EndecaNormalSearchDefinition NormalSearchDefinition
        //{
        //    get { return _normalSearchDefinition; }
        //    set { _normalSearchDefinition = value; }
        //}

        ///// <summary>
        ///// Gets and sets the dimension search definition for this Endeca search.
        ///// </summary>
        //public EndecaDimensionSearchDefinition DimensionSearchDefinition
        //{
        //    get { return _dimensionSearchDefinition; }
        //    set { _dimensionSearchDefinition = value; }
        //}

        //public EndecaRecordSearchDefinition RecordSearchDefinition
        //{
        //    get { return _recordSearchDefinition; }
        //    set { _recordSearchDefinition = value; }
        //}


        public IEndecaSearchDefinition SearchDefinition
        {
            get { return _searchDefinition; }
            set { _searchDefinition = value; }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new endeca search with no search definition 
        /// </summary>
        /// <param name="ip">The server host/ip</param>
        /// <param name="port">The port number for the dgraph</param>
        public GenericEndecaSearch(string ip, string port)
        {
            _serverIP = ip;
            _serverPort = port;
        }

        public GenericEndecaSearch(string ip, string port, IEndecaSearchDefinition searchDef)
        {
            _serverIP = ip;
            _serverPort = port;
            _searchDefinition = searchDef;
        }

        /* -- For now we have not really done the full coding for the dimension stuff
         * so this is going to be commented out until it is used.
        public GSSEndecaSearch(string ip, string port, GSSEndecaDimensionSearchDefinition dimSearchDef)
        {
            _serverIP = ip;
            _serverPort = port;
            _dimSearchDefinition = dimSearchDef;
        }
       
        public GSSEndecaSearch(string ip, string port, GSSEndecaNormalSearchDefinition normalSearchDef, GSSEndecaDimensionSearchDefinition dimSearchDef)
        {
            _serverIP = ip;
            _serverPort = port;
            _normalSearchDefinition = normalSearchDef;
            _dimSearchDefinition = dimSearchDef;
        }
        */

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new ENEQuery to be executed
        /// </summary>
        /// <returns>ENEQuery</returns>
        //private ENEQuery CreateQuery()
        //{

        //    //Step 2.  Create a query
        //    ENEQuery searchQuery = new ENEQuery();

        //    //Step 2a.  Setup the ENEQuery

        //    #region long description that might not be relevant anymore
        //    //Since we now have to handle both property searching (versus SearchInterfaces) and possibly dimension             
        //    //searching (diminsion searching is not searching by dimensions, but searching the dimensions)
        //    //We will have to be a bit more clever about this all.

        //    //This is going to get tricky to explain.  Thier search works by searching through nodes in concept trees.
        //    //By default we search through 0, which is the root node.  But say I wanted to narrow my search results for
        //    //the search term "Breast Cancer" by seeing all the results in the fictious NCI Taxonomy:
        //    //  ROOT (This is the top node of all of the different taxonomies)   Value: 0
        //    //    |
        //    //  Types Of Cancer -- Value: 100
        //    //      |
        //    //       ----> Breast Cancer -- Value: 150
        //    //                  |
        //    //                   --------> Breast Cancer Treatment (BCT) -- Value: 151
        //    //                  |
        //    //                   --------> Breast Cancer Prevention (BCP) -- Value: 152
        //    //
        //    //   I want to see all documents that match the search terms in the Breast Cancer Treatment node, then we would do
        //    //   searchQuery.NavDescriptors = new DimValIdList("151");
        //    //
        //    //   Now, this is a list, so I could choose to see which documents matched the search term in the BCT and BCP
        //    //   nodes, then this list would have 151 and 152 in it.  There is also another option that says what set operator to
        //    //   use.  (Intersection or Union)  But that is for another comment.  BTW, these values come from a link that is setup
        //    //   when rendering the dimensions, they come from the dimensions themselves.  But that to is another comment.
        //    #endregion


        //    //If there is a normal search def then setup the query with the nav search details
        //    if (_normalSearchDefinition != null)
        //    {
        //        _normalSearchDefinition.SetupQuery(searchQuery);
        //    }

        //    //If there is a nav search def then setup the query with the nav search details
        //    if (_dimensionSearchDefinition != null)
        //    {
        //        //TODO: Add dimsearch stuff
        //    }

        //    return searchQuery;
        //}

        private ENEQuery CreateQuery()
        {

            //Step 2.  Create a query
            ENEQuery searchQuery = new ENEQuery();

            //Step 2a.  Setup the ENEQuery

            #region long description that might not be relevant anymore
            //Since we now have to handle both property searching (versus SearchInterfaces) and possibly dimension             
            //searching (diminsion searching is not searching by dimensions, but searching the dimensions)
            //We will have to be a bit more clever about this all.

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
            #endregion


            //If there is a normal search def then setup the query with the nav search details
            if (_searchDefinition != null)
            {
                _searchDefinition.SetupQuery(searchQuery);
            }
            return searchQuery;
        }

        /// <summary>
        /// Gets the search results
        /// </summary>
        /// <returns>ENEQueryResults</returns>
        public ENEQueryResults GetSearchResults()
        {

            //Create the connection
            HttpENEConnection connection = new HttpENEConnection(_serverIP, _serverPort);

            //Create the search query
            ENEQuery searchQuery = CreateQuery();

            // Run the query
            ENEQueryResults results = null;
            results = connection.Query(searchQuery); //maybe put a try here

            return results;

        }

        #endregion

        /// <summary>
        /// Gets a list of all refinements available for a given root dimension
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="dimID"></param>
        /// <returns></returns>
        public static List<DimVal> GetDimensionRefinements(string ip, string port, long dimID)
        {
            List<DimVal> dimVals = new List<DimVal>();

            EndecaDimensionSearchParameter searchParam = new EndecaDimensionSearchParameter("NCIRefinement");
            EndecaDimensionSearchDefinition dimDef = new EndecaDimensionSearchDefinition(searchParam);

            dimDef.DimensionFilters.Add(dimID);

            GenericEndecaSearch search = new GenericEndecaSearch(ip, port, dimDef);
            ENEQueryResults results = search.GetSearchResults();

            if (results.ContainsDimensionSearch())
            {
                if (results.DimensionSearch.Results.Count > 0)
                {
                    DimensionSearchResultGroup resultGroup = (DimensionSearchResultGroup)results.DimensionSearch.Results[0];
                    for (int i = 0; i < resultGroup.Count; i++ )
                    {
                        DimLocationList dimLocList = (DimLocationList)resultGroup[i];
                        foreach (DimLocation dimLoc in dimLocList)
                        {
                            dimVals.Add(dimLoc.DimValue);
                        }
                    } 
                }
            }
            return dimVals;
        }

        /// <summary>
        /// Returns a record's details
        /// </summary>
        /// <param name="recordID">Either the endeca generated record id or the user assigned record spec id</param>
        /// <returns></returns>
        public static ERec GetRecord(string ip, string port, string recordID)
        {
            ERec endecaRecord = null;

            EndecaRecordSearchDefinition recDef = new EndecaRecordSearchDefinition();
            recDef.RecordID = recordID;

            GenericEndecaSearch search = new GenericEndecaSearch(ip, port, recDef);
            ENEQueryResults results = search.GetSearchResults();

            if (results.ContainsERec())
            {
                endecaRecord = results.ERec;
            }


            return endecaRecord;
        }
    }
}
