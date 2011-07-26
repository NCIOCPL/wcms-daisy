using System;
using System.Web;

namespace NCI.Web.CDE.HttpHandlers
{
    public class FlashSetCookieHandler : IHttpHandler
    {
        /// <summary>
        /// This handler is used by the flash component to set the cookie values required 
        /// by Flash.
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// 
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
            foreach (string param in context.Request.QueryString.Keys)
            {
                HttpCookie cookie = new HttpCookie(param.Trim(), context.Request.QueryString[param]);
                context.Response.AppendCookie(cookie);
            }
        }

        #endregion
    }
}
