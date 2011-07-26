using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using NCI.DataManager;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents all the fields that are required to be processed by the 
    /// nVelocity to render the results.
    /// </summary>
    public class DynamicSearch
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ResultCount { get; set; }
        public int StartCount { get; set; }
        public int EndCount { get; set; }
        public ICollection<SearchResult> Results { get; set; }
        public string KeyWord { get; set; }

        public DynamicSearch()
        {
            StartCount = 0;
            EndCount = 0;
            ResultCount = 0;
            StartDate = "";
            EndDate = "";
            Results = null;
        }
    }
}
