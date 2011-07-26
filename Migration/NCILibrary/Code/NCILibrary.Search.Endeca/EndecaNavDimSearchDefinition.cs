using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    public class EndecaNavDimSearchDefinition : IEndecaSearchDefinition
    {
        private EndecaNormalSearchDefinition _normalDefinition = null;
        private EndecaDimensionSearchDefinition _dimensionDefinition = null;

        public EndecaNormalSearchDefinition NormalDefinition
        {
            get { return _normalDefinition; }
            set { _normalDefinition = value; }
        }

        public EndecaDimensionSearchDefinition DimensionDefinition
        {
            get { return _dimensionDefinition; }
            set { _dimensionDefinition = value; }
        }

        public EndecaNavDimSearchDefinition(EndecaDimensionSearchParameter dimParameter, EndecaNavigationSearchParameter navParameter)
        {
            _normalDefinition = new EndecaNormalSearchDefinition();
            _normalDefinition.SearchParameters.Add(navParameter);
            _dimensionDefinition = new EndecaDimensionSearchDefinition(dimParameter);
        }

        #region IEndecaSearchDefinition Members

        public void SetupQuery(ENEQuery searchQuery)
        {

            _normalDefinition.SetupQuery(searchQuery);
            _dimensionDefinition.SetupQuery(searchQuery);

            ////set up navigation search
            ////Add in the record search parameters.
            //if (_normalDefinition.SearchParameters.Count > 0)
            //{
            //    searchQuery.NavERecSearches = new ERecSearchList();

            //    int paramCount = 0;
            //    foreach (EndecaNavigationSearchParameter param in _normalDefinition.SearchParameters)
            //    {
            //        searchQuery.NavERecSearches.Add(paramCount, param.GetERecSearch()); // <-- JUST IMPLEMENT ADD(object)!!!
            //        paramCount++; // So yea I could use a for loop but maybe someday endeca will fix the stuipd add.
            //    }


            //    //Set did you mean.  This can only happen if there is at least one ERecSearch
            //    searchQuery.NavERecSearchDidYouMean = _normalDefinition.EnableDidYouMean;

            //}

            ////now add the dimension filters.   
            //searchQuery.NavDescriptors = new DimValIdList();
            //if (_normalDefinition.DimensionFilters.Count > 0)
            //{
            //    foreach (long dimID in _normalDefinition.DimensionFilters)
            //    {
            //        searchQuery.NavDescriptors.AddDimValueId(dimID);
            //    }
            //}
            //else
            //{
            //    //Search the root
            //    searchQuery.NavDescriptors.AddDimValueId(0);
            //}

            ////Set refinement options
            //if (_normalDefinition.EnableRefinements)
            //{
            //    if (_normalDefinition.RefinementDimIDList.Count == 0)
            //    {
            //        searchQuery.NavAllRefinements = true;//This is the most expensive query!
            //    }
            //    else
            //    {
            //        DimValIdList valIdList = new DimValIdList();
            //        foreach (long id in _normalDefinition.RefinementDimIDList)
            //            valIdList.AddDimValueId(id);
            //        searchQuery.NavExposedRefinements = valIdList;
            //    }
            //}

            ////Set sort order
            //if (_normalDefinition.SortFields.Count > 0)
            //{
            //    int sortCount = 0;
            //    ERecSortKeyList sortKeys = new ERecSortKeyList();

            //    foreach (EndecaSortParam param in _normalDefinition.SortFields)
            //    {
            //        sortKeys.Add(sortCount, param.GetERecSortKey());

            //        sortCount++; //What is up with Endeca's complete inability to create normal Add methods.
            //    }

            //    searchQuery.SetNavActiveSortKeys(sortKeys);
            //}

            ////Set the results per page
            //searchQuery.NavNumERecs = _normalDefinition.ResultsPerPage;

            ////Set the start record
            //searchQuery.NavERecsOffset = _normalDefinition.StartRecord;

            ////Setup range filters
            //if (_normalDefinition.RangeFilters.Count > 0)
            //{
            //    searchQuery.NavRangeFilters = new RangeFilterList();

            //    int filterCount = 0;
            //    foreach (EndecaRangeFilter filter in _normalDefinition.RangeFilters)
            //    {
            //        searchQuery.NavRangeFilters.Add(filterCount, filter.GetRangeFilter());
            //        filterCount++;
            //    }
            //}

            ////setup dimension search
            
            //if (_dimensionDefinition.DimensionFilters.Count > 0)
            //{
            //    searchQuery.DimSearchDimensions = new DimValIdList();
            //    foreach (long dimID in _dimensionDefinition.DimensionFilters)
            //    {
            //        searchQuery.DimSearchDimensions.AddDimValueId(dimID);
            //    }
            //}

            ////set up search terms
            //searchQuery.DimSearchTerms = _dimensionDefinition.SearchParameter.SearchTerm;
            //searchQuery.DimSearchOpts = "mode " + _dimensionDefinition.SearchParameter.MatchMode.ToString();

        }

        #endregion
    }
}
