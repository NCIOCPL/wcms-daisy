using System;
using System.Collections.Generic;
using System.Text;

using NCI.Util;

namespace NCI.Search
{
    public class GenericSiteWideSearchResult : ISiteWideSearchResult
    {
        private string _title;
        private string _description;
        private string _url;

        public string Title
        {
            get
            {
                if (!String.IsNullOrEmpty(_title))
                    return _title;
                else
                    return "Untitled";
            }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string WrappedUrl
        {
            get
            {
                if (_url != null)
                    return Strings.Wrap(_url, 85, "<br />");
                else
                    return "";
            }
        }

        public GenericSiteWideSearchResult(string title, string description, string url)
        {
            this._title = title;
            this._description = description;
            this._url = url;
        }
    }
}
