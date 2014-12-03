using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using NCI.Util;

namespace NCI.Web.UI.WebControls.JSLibraries
{
    /// <summary>
    /// Manages the HtmlBuilder javascript library on a page.
    /// This is used to easily create elements and add them to the DOM.
    /// It can be found at http://www.ivy.fr/js/xml/index.html.
    /// </summary>
    public static class HtmlBuilderManager
    {
        private static object _htmlBuilderIsLoaded = new object();

        /// <summary>
        /// Loads the library on a page.  If the library has already been loaded
        /// then we will not load it again, if a different version of the library
        /// has been loaded than the one we are loading we must throw an exception.
        /// </summary>
        /// <param name="p">The page to load the library</param>
        public static void Load(Page p)
        {
            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            bool isLoaded = Strings.ToBoolean(HttpContext.Current.Items[_htmlBuilderIsLoaded]);

            if (!isLoaded)
            {
                //Since nothing else is loaded, load the library
                //This Javascript is special, and we need to actually add it to the head element.
                HtmlGenericControl scriptControl = new HtmlGenericControl("script");
                p.Header.Controls.Add(scriptControl);
                scriptControl.Attributes.Add("type", "text/javascript");
                scriptControl.Attributes.Add("src",
                    p.ClientScript.GetWebResourceUrl(typeof(HtmlBuilderManager),
                        "NCI.Web.UI.WebControls.JSLibraries.Resources.HtmlBuilder.js"));
                HttpContext.Current.Items.Add(_htmlBuilderIsLoaded, true);

            }
        }

    }
}
