using System;
using System.Collections.Generic;

namespace NCI.DataManager
{
    /// <summary>
    /// This class represents a single reuslt obtained after performing the search.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The record number for use in nvelocity template.
        /// </summary>
        public int RecNumber { get; set; }

        /// <summary>
        /// Long descritpion of the content item
        /// </summary>
        public string ShortTitle { get; set; }

        /// <summary>
        /// Long descritpion of the content item
        /// </summary>
        public string LongTitle { get; set; }

        /// <summary>
        /// Short Description of the content item.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Long descritpion of the content item
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// The link to the complete content.
        /// </summary>
        public string HRef { get; set; }

        /// <summary>
        /// Updated date of the content
        /// </summary>
        public string UpdatedDate { get; set; }

        /// <summary>
        /// Question and Answer Url
        /// </summary>
        public string QandAUrl { get; set; }

        /// <summary>
        /// Imagee URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Video URL
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Audio URL
        /// </summary>
        public string AudioUrl { get; set; }

        /// <summary>
        /// Other language Url
        /// </summary>
        public string OtherlanguageUrl { get; set; }
        /// <summary>
        /// Language settings
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Posted date of the content.
        /// </summary>
        public string PostedDate { get; set; }

        /// <summary>
        /// Posted date of the content.
        /// </summary>
        public string DateDisplayMode { get; set; }

        public string ReviewedDate { get; set; }

        public SearchResult()
        {
            ShortDescription = string.Empty;
            LongTitle = string.Empty;
            LongDescription = string.Empty;
            UpdatedDate = string.Empty;
            PostedDate = string.Empty;
            HRef = string.Empty;
        }
    }
}
