using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    /// <summary>
    /// This is an abstraction of the fields used for an endeca navigation search.
    /// </summary>
    public class EndecaNormalSearchDefinition : IEndecaSearchDefinition
    {
        private int _resultsPerPage = 10;
        private int _startRecord = 0;

        private List<EndecaNavigationSearchParameter> _searchParameters;
        private List<long> _dimensionFilters;
        private List<EndecaSortParam> _sortFields;
        private List<EndecaRangeFilter> _rangeFilters;

        private bool _enableDidYouMean = false;
        private bool _enableRefinements = false;

        private List<long> _refinementDimIDList;
        

        /// <summary>
        /// Gets and sets the first record the results should start with.  1 is the first record, not 0.  (No parameter, page 512)
        /// </summary>
        public int StartRecord
        {
            get { return _startRecord; }
            set { _startRecord = value; }
        }

        /// <summary>
        /// Gets and sets the number of results to retrieve. (There is no query parameter for this option)
        /// </summary>
        public int ResultsPerPage
        {
            get { return _resultsPerPage; }
            set { _resultsPerPage = value; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GSS.Search.EndecaSearch.GSSEndecaNavigationSearchParameter"/>s.  This is what you use for a navigation
        /// search. (Ntk, Ntt, Ntx parameters, page 519-521)
        /// </summary>
        public List<EndecaNavigationSearchParameter> SearchParameters
        {
            get { return _searchParameters; }
        }

        /// <summary>
        /// Gets a list of dimension ids for Navigation search reduction.  (N parameter, page 507)
        /// </summary>
        public List<long> DimensionFilters
        {
            get { return _dimensionFilters; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GSS.Search.EndecaSearch.GSSEndecaSortParam"/>s to define a sort order for a Navigation search. 
        /// (Ns parameter, page 515)
        /// </summary>
        public List<EndecaSortParam> SortFields
        {
            get { return _sortFields; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GSS.Search.EndecaSearch.GSSEndecaRangeFilter"/>s to be used to filter records.
        /// (Nf parameter, page 510)
        /// </summary>
        public List<EndecaRangeFilter> RangeFilters
        {
            get { return _rangeFilters; }
        }

        /// <summary>
        /// Enable Did You Mean? (Nty parameter, page 522)
        /// </summary>
        public bool EnableDidYouMean
        {
            get { return _enableDidYouMean; }
            set { _enableDidYouMean = value; }
        }

        /// <summary>
        /// Enable Refinements? (N=0&amp;Ne=6+2+9+..., page 237)
        /// Note: Used by itself, this is the most expensive query. Use with RefinementDimIDList when possible to return
        /// only a subset of all the dimensions. If set to false, RefinementDimIDList is ignored.
        /// </summary>
        public bool EnableRefinements
        {
            get { return _enableRefinements; }
            set { _enableRefinements = value; }
        }

        /// <summary>
        /// Enable only these refinements. (N=0&amp;Ne=6, page 237)
        /// Note: If EnableRefinements is false, this is ignored.
        /// </summary>
        public List<long> RefinementDimIDList
        {
            get { return _refinementDimIDList; }
            set{ _refinementDimIDList = value; }
        }

        /// <summary>
        /// Creates a new NormalSearchDefinition
        /// </summary>
        public EndecaNormalSearchDefinition()
        {

            _searchParameters = new List<EndecaNavigationSearchParameter>();
            _dimensionFilters = new List<long>();
            _sortFields = new List<EndecaSortParam>();
            _rangeFilters = new List<EndecaRangeFilter>();
            _refinementDimIDList = new List<long>();
        }


        #region IEndecaSearchDefinition Members

        /// <summary>
        /// Sets up the ENEQuery for this search definition
        /// </summary>
        /// <param name="searchQuery"></param>
        public void SetupQuery(ENEQuery searchQuery)
        {
            //Add in the record search parameters.
            if (_searchParameters.Count > 0)
            {
                searchQuery.NavERecSearches = new ERecSearchList();

                int paramCount = 0;
                foreach (EndecaNavigationSearchParameter param in _searchParameters)
                {
                    searchQuery.NavERecSearches.Add(paramCount, param.GetERecSearch()); // <-- JUST IMPLEMENT ADD(object)!!!
                    paramCount++; // So yea I could use a for loop but maybe someday endeca will fix the stuipd add.
                }


                //Set did you mean.  This can only happen if there is at least one ERecSearch
                searchQuery.NavERecSearchDidYouMean = _enableDidYouMean;

            }

            //now add the dimension filters.   
            searchQuery.NavDescriptors = new DimValIdList();
            if (_dimensionFilters.Count > 0)
            {
                foreach (long dimID in _dimensionFilters)
                {
                    searchQuery.NavDescriptors.AddDimValueId(dimID);
                }
            }
            else
            {
                //Search the root
                searchQuery.NavDescriptors.AddDimValueId(0);
            }

            //Set refinement options
            if (_enableRefinements)
            {
                if (_refinementDimIDList.Count == 0)
                {
                    searchQuery.NavAllRefinements = true;//This is the most expensive query!
                }
                else
                {
                    DimValIdList valIdList = new DimValIdList();
                    foreach (long id in _refinementDimIDList)
                        valIdList.AddDimValueId(id);
                    searchQuery.NavExposedRefinements = valIdList;
                }
            }

            //Set sort order
            if (_sortFields.Count > 0)
            {
                int sortCount = 0;
                ERecSortKeyList sortKeys = new ERecSortKeyList();

                foreach (EndecaSortParam param in _sortFields)
                {
                    // Sort by location
                    if (param.Latitude > 0.0d || param.Longitude > 0.0d)
                    {
                        sortKeys.Add(sortCount, param.GetERecGeoSortKey());
                    }// Normal field sort
                    else
                    {
                        sortKeys.Add(sortCount, param.GetERecSortKey());
                    }

                    sortCount++; //What is up with Endeca's complete inability to create normal Add methods.
                }

                searchQuery.SetNavActiveSortKeys(sortKeys);
            }

            //Set the results per page
            searchQuery.NavNumERecs = _resultsPerPage;

            //Set the start record
            searchQuery.NavERecsOffset = _startRecord;

            //Setup range filters
            if (_rangeFilters.Count > 0)
            {
                searchQuery.NavRangeFilters = new RangeFilterList();

                int filterCount = 0;
                foreach (EndecaRangeFilter filter in _rangeFilters)
                {
                    searchQuery.NavRangeFilters.Add(filterCount, filter.GetRangeFilter());
                    filterCount++;
                }
            }

            //setup field selection
        }

        #endregion
    }
}
