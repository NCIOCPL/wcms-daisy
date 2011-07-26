using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    /// <summary>
    /// Once this class is completed it will be an abstraction of the fields used for an endeca dimension search. 
    /// </summary>
    public class EndecaDimensionSearchDefinition : IEndecaSearchDefinition
    {
        private List<long> _dimensionFilters;
        private List<EndecaRangeFilter> _rangeFilters;

        private EndecaDimensionSearchParameter _searchParameter;

        public EndecaDimensionSearchParameter SearchParameter
        {
            get { return _searchParameter; }
        }

        public List<long> DimensionFilters
        {
            get { return _dimensionFilters; }
            set { _dimensionFilters = value; }
        }

        public EndecaDimensionSearchDefinition(EndecaDimensionSearchParameter searchParameter)
        {
            _searchParameter = searchParameter;
            _dimensionFilters = new List<long>();
            _rangeFilters = new List<EndecaRangeFilter>();
        }


        #region IEndecaSearchDefinition Members

        public void SetupQuery(ENEQuery searchQuery)
        {
            //set up dimensions to search
            searchQuery.DimSearchDimensions = new DimValIdList();
            if (_dimensionFilters.Count > 0)
            {
                foreach (long dimID in _dimensionFilters)
                {
                    searchQuery.DimSearchDimensions.AddDimValueId(dimID);
                }
            }

            //set up search terms
            searchQuery.DimSearchTerms = _searchParameter.SearchTerm;
            searchQuery.DimSearchOpts = "mode " + _searchParameter.MatchMode.ToString();
        }

        #endregion
    }
}
