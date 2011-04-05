using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This class contains the fields which can be used to get the pretty URL 
    ///and canonical URL for the Page requested.
    /// </summary>
    public class PageAssemblyInstructionUrls
    {
        /// <summary>
        /// Pretty Url is the primary url which user type in to get to a page.
        /// </summary>
        public static readonly string PrettyUrl = "PrettyUrl";
        /// <summary>
        /// Canonical URL is normalized URL. Two syntactically different URL are semantically the same 
        ///URLs. Any secondary URL's Canonical URL is a pretty URL. This property returns the 
        ///Pretty URL as the normalized URL.
        /// </summary>
        public static readonly string CanonicalUrl = "CanonicalUrl ";

    }
}
