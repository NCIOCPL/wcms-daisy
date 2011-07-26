using System;
using System.Web;
using NCI.Logging;
using System.Globalization;

namespace NCI.Web.CDE
{
    public class FileInstructionLoader : IHttpModule
    {
        private static readonly object REQUEST_URL_KEY = new object();

        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
            context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            //Check if the PageAssemblyInstruction is not null then it was processed as pretty url.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));

            if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                return;

            //Check if the url has been rewritten yet.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            RewriteUrl(context, url);
        }

        void OnLogRequest(object sender, EventArgs e)
        {
            //New comments for prototype.  So if there are multiple PrettyUrl modules, then this would
            //not be good to put here -- it should be in its own module.


            //IIS 7's logging is slightly different from IIS 6.  IIS 6 would log the prettyurl because
            //logging is not part of the asp.net pipeline and therefore the url of the GET header is what 
            //is logged.  
            //IIS 7's integrated mode logs the Request object's url.  Maybe the AbsoluteUrl, who knows,
            //but, since we do a RewritePath, the Request object's url is now the ugly url and that is
            //what is logged.  So what we can do at this stage is do a RewritePath back to the
            //orgininal requested url if the requested url was a pretty url.  Then the pretty url will
            //get logged and everyone is happy.  This should not affect anything since the page handler
            //has already run and rendered the content to the Response object.
            //SCR 30301.

            HttpContext context = ((HttpApplication)sender).Context;

            string url = string.Empty;

            if (context.Items.Contains(REQUEST_URL_KEY))
            {
                url = (string)context.Items[REQUEST_URL_KEY];

                if (context.Items.Contains(REQUEST_URL_KEY) && !string.IsNullOrEmpty((string)context.Items[REQUEST_URL_KEY]))
                    url += (string)context.Items[REQUEST_URL_KEY];
                else
                    url += "?";
            }

            if (!string.IsNullOrEmpty(url))
            {
                //context.RewritePath(url, false);
            }
        }

        #endregion

        /// <summary>
        /// Loads the appropriate xml file and loads the IFileInstruction
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="url">Requested URL</param>
        void RewriteUrl(HttpContext context, string url)
        {

            if (url.EndsWith("/")) //The key will never be just / because of the statement above
            {
                //strip the trailing /
                url = url.Substring(0, url.LastIndexOf('/'));
            }

            //Store the url so it can be rewritten for logging.
            context.Items[REQUEST_URL_KEY] = url;

            IFileInstruction fileInstruction = null;

            try
            {
                //Load the XML file to create a file instruction object of type IFileInstruction.If the XML file fails to create IFileInstruction
                //Object log error
                fileInstruction = FileInstructionFactory.GetFileInstruction(url);
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:FileInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to Create IFileInstruction with the XML file Provided.", NCIErrorLevel.Error, ex);
                RaiseErrorPage();
                return;
            }
            //Exiting because there is no page assembly instruction for the requested URL.
            if (fileInstruction == null)
            {
                return;
            }

            // Do not reset the client path because it'll break form action url's.
            try
            {
                context.RewritePath(fileInstruction.FilePath, false);
            }

            catch (HttpException ex)
            {
                Logger.LogError("CDE:FileInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to rewrite URL.", NCIErrorLevel.Error, ex);
                RaiseErrorPage();
            }
        }

        private static void RaiseErrorPage()
        {
            HttpContext.Current.Response.Write("There was an error processing the Request");
            HttpContext.Current.Response.End();
            return;
        }

    }
}
