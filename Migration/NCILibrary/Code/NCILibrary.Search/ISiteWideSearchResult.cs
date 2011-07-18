using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Search
{
    public interface ISiteWideSearchResult
    {
        string Title
        {
            get;
        }
        string Description
        {
            get;
        }
        string Url
        {
            get;
        }
        string WrappedUrl
        {
            get;
        }
    }
}
