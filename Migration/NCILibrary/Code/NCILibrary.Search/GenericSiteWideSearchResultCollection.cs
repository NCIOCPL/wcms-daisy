using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace NCI.Search
{
    public class GenericSiteWideSearchResultCollection : ISiteWideSearchResultCollection
    {
        private ArrayList _wordForms;
        private List<GenericSiteWideSearchResult> _searchResults;
        private long _totalNumResults;
        private string _dym;

        #region ISiteWideSearchResultCollection Members

        public ArrayList WordForms
        {
            get { return _wordForms; }
            set { _wordForms = value; }
        }

        public string DidYouMean
        {
            get { return _dym; }
            set { _dym = value; }
        }

        public long TotalNumResults
        {
            get { return _totalNumResults; }
            set { _totalNumResults = value; }
        }

        public GenericSiteWideSearchResultCollection()
        {
            _searchResults = new List<GenericSiteWideSearchResult>();
        }

        public GenericSiteWideSearchResultCollection(string dym, ArrayList wForms)
        {
            _searchResults = new List<GenericSiteWideSearchResult>();
            this._dym = dym;
            this._wordForms = wForms;
        }

        #endregion


        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            foreach (GenericSiteWideSearchResult r in _searchResults)
            {
                array.SetValue(r, index);
                index++;
            }
        }

        public int Count
        {
            get { return _searchResults.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _searchResults.GetEnumerator();
        }

        #endregion

        public void Add(ISiteWideSearchResult value)
        {
            _searchResults.Add((GenericSiteWideSearchResult) value);
        }

        public void Clear()
        {
            _searchResults.Clear();
        }

        public bool Contains(ISiteWideSearchResult value)
        {
            return _searchResults.Contains((GenericSiteWideSearchResult)value);
        }

        public int IndexOf(ISiteWideSearchResult value)
        {
            return _searchResults.IndexOf((GenericSiteWideSearchResult)value);
        }

        public void Insert(int index, ISiteWideSearchResult value)
        {
            _searchResults.Insert(index, (GenericSiteWideSearchResult)value);
        }

        public void Remove(ISiteWideSearchResult value)
        {
            _searchResults.Remove((GenericSiteWideSearchResult)value);
        }

        public void RemoveAt(int index)
        {
            _searchResults.RemoveAt(index);
        }

        public ISiteWideSearchResult this[int index]
        {
            get
            {
                return _searchResults[index];
            }
            set
            {
                _searchResults[index] = (GenericSiteWideSearchResult)value;
            }
        }

    }
}
