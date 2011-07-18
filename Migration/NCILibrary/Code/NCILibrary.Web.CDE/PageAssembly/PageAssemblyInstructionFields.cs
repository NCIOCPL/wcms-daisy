using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This class contains the fields which can be used to set the tile,metadescription and mata keywords for a page.
    /// </summary>
    public class PageAssemblyInstructionFields
    {
        /// <summary>
        /// Html tile is the title for the page being rendered
        /// </summary>
        public static readonly string HTML_Title = "HTML_Title";
        /// <summary>
        /// The text that should be used for the meta name="description" tag
        /// </summary>
        public static readonly string HTML_MetaDescription="HTML_MetaDescription";
        /// <summary>
        /// The text that should be used for the meta name="keywords" tag.
        /// </summary>
        public static readonly string HTML_MetaKeywords="HTML_MetaKeywords";
    }
}
