using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This class is used to manage a special javascript function that adds a special browser specific
    /// class to the html element of a page that defines what browser is being used.  This is 
    /// so that browser css quirks can be easily overcome by just defining styles that use
    /// the browser class.
    /// </summary>
    public static class CssBrowserSelectorManager
    {
        private static readonly object _scriptAdded;

        /// <summary>
        /// Static constructor for the CssBrowserSelectorManager class.
        /// </summary>
        static CssBrowserSelectorManager()
        {
            _scriptAdded = new object();
        }

        /// <summary>
        /// Adds the javascript function to a pages head element.  It makes sure that the script is only added once to a page.
        /// </summary>
        /// <param name="thisPage">The page that the script should be added to.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when thisPage is null.</exception>
        /// <exception cref="System.NullReferenceException">Thrown when the Header property of thisPage is null.
        /// This is indicative that the runat=server property was not set for the head element of the page.
        /// </exception>
        public static void AddSelectorJS(Page thisPage)
        {
            if (thisPage == null)
                throw new ArgumentNullException("The page passed in is null");

            if (thisPage.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            //Make sure we have not added the script yet.
            if (!HttpContext.Current.Items.Contains(_scriptAdded))
            {
                //This Javascript is special, and we need to actually add it to the head element.
                HtmlGenericControl scriptControl = new HtmlGenericControl("script");
                thisPage.Header.Controls.Add(scriptControl);
                scriptControl.Attributes.Add("type", "text/javascript");
                scriptControl.Attributes.Add("src",
                    thisPage.ClientScript.GetWebResourceUrl(typeof(CssBrowserSelectorManager),
                        "NCI.Web.UI.WebControls.Resources.CssBrowserSelector.js"));
                HttpContext.Current.Items.Add(_scriptAdded, true);
            }
        }
    }
}
