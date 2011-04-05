using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Search.Endeca
{
    public class EndecaDimensionSearchParameter
    {
        private string _searchTerm = "";
        private EndecaMatchModes _matchMode = EndecaMatchModes.MatchAll;

        #region Properties

        public string SearchTerm
        {
            get { return _searchTerm; }
        }

        public EndecaMatchModes MatchMode
        {
            get { return _matchMode; }
            set { _matchMode = value; }
        }
        #endregion

        #region Constructors
        public EndecaDimensionSearchParameter(string searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public EndecaDimensionSearchParameter(string searchTerm, EndecaMatchModes matchMode)
        {
            _searchTerm = searchTerm;
            _matchMode = matchMode;
        }
        #endregion


    }
}
