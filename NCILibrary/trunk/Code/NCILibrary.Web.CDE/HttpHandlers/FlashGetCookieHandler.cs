using System;
using System.Web;

namespace NCI.Web.CDE.HttpHandlers
{
    public class FlashGetCookieHandler : IHttpHandler
    {
        /// <summary>
        /// This handler is used by the flash component.
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string requestedKeys = context.Request.QueryString["keys"];
            context.Response.AddHeader("Cache-Control", "no-cache");
            context.Response.ContentType = "application/x-www-form-urlencoded";
            if ((requestedKeys != null) && (requestedKeys.Trim() != ""))
            {
                string[] keys = requestedKeys.Split('|');
                bool first = true;

                foreach (string key in keys)
                {
                    context.Response.Write("&");
                    context.Response.Write(key.Trim());
                    context.Response.Write("=");
                    System.Web.HttpCookie cookie = context.Request.Cookies[key.Trim()];
                    if ((cookie != null) && (cookie.Value.Trim() != ""))
                    {
                        context.Response.Write(cookie.Value);
                    }
                    else
                    {
                        string err = "";
                        foreach (string cookkey in context.Request.Cookies.Keys)
                        {
                            err += " " + cookkey;
                        }
                    }
                }
            }
            context.Response.Write("&done=true");
        }

        #endregion
    }
}
