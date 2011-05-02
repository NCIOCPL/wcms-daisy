using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NCI.Search
{
    public interface ISiteWideSearchResultCollection : ICollection
    {
        ArrayList WordForms
        {
            get;
            set;
        }
        string DidYouMean
        {
            get;
            set;
        }
        long TotalNumResults
        {
            get;
            set;
        }

        void Add(ISiteWideSearchResult value);

        void Clear();

        bool Contains(ISiteWideSearchResult value);

        int IndexOf(ISiteWideSearchResult value);

        void Insert(int index, ISiteWideSearchResult value);

        void Remove(ISiteWideSearchResult value);

        void RemoveAt(int index);

        ISiteWideSearchResult this[int index]
        {
            get;
            set;
        }
    }
}
