using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using NCI.Web.CDE.Configuration;
using System.Web;

namespace NCI.Web.CDE
{
    public interface IMultiPageAssemblyInstruction : IPageAssemblyInstruction
    {
        /// <summary>
        /// Determines whether the specified requested URL contains URL.
        /// </summary>
        /// <param name="requestedURL">The requested URL.</param>
        /// <returns>
        /// 	<c>true</c> if the specified requested URL contains URL; otherwise, <c>false</c>.
        /// </returns>
        Boolean ContainsURL(string requestedURL);


        /// <summary>
        /// Sets the index of the current page.
        /// </summary>
        /// <param name="index">The index.</param>
        void SetCurrentPageIndex(int index);

        /// <summary>
        /// Gets the page index of URL.
        /// </summary>
        /// <param name="url">The page URL to get the index for.</param>
        int GetPageIndexOfUrl(string url);

        /// <summary>
        /// Gets the page snippets.
        /// </summary>
        /// <returns>Collection of page snippets</returns>
        List<SnippetInfo> GetPageSnippets();

    }
}
