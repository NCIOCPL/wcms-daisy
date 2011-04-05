using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    public static class CssManager
    {
        public static void AddResource(Page p, Type type, string resourceName)
        {
            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");            

            if (!HttpContext.Current.Items.Contains("LoadedCss_" + resourceName))
            {
                HtmlLink link = new HtmlLink();
                p.Header.Controls.Add(link);

                link.Href = p.ClientScript.GetWebResourceUrl(
                    type,
                    resourceName);

                link.Attributes.Add("rel", "stylesheet");

                //Mark that the stylesheet has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedCss_" + resourceName, true);
            }
        }

        public static void AddStyleSheet(Page p, string styleSheet)
        {
            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            //Make sure that ~/path works.
            styleSheet = VirtualPathUtility.ToAbsolute(styleSheet);

            if (!HttpContext.Current.Items.Contains("LoadedCss_" + styleSheet))
            {
                HtmlLink link = new HtmlLink();
                p.Header.Controls.Add(link);

                link.Href = styleSheet;

                link.Attributes.Add("rel", "stylesheet");

                //Mark that the stylesheet has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedCss_" + styleSheet, true);
            }
        }

        public static void AddStyle(Page p, string styleKey, string style)
        {
            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            if (!HttpContext.Current.Items.Contains("LoadedCss_" + styleKey))
            {
                HtmlGenericControl link = new HtmlGenericControl("style");
                p.Header.Controls.Add(link);

                link.InnerHtml = style;

                link.Attributes.Add("type", "text/css");

                //Mark that the stylesheet has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedCss_" + styleKey, true);
            }
        }

    }
}
