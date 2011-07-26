using System;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Web.CDE.Configuration;
using NCI.Logging;
using System.Reflection;

namespace NCI.Web.CDE
{
    public class PageAssemblyInstructionLoader : IHttpModule
    {
        private const string PRINT_URL_ENDING = "/print";
        private const string VIEWALL_URL_ENDING = "/allpages";
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
        /// <summary>
        /// Instantiated  events OnBeginRequest and OnLogRequest
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            // Handle LogRequest event and provide custom logging implementation
            context.LogRequest += new EventHandler(OnLogRequest);
        }
        /// <summary>
        /// OnBeginRequest process the requested URL and rewrite the URL to load the correct template.
        /// </summary>        
        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));

            if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                return;

            //Check if the url has been rewritten yet.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return; 
          
            //if (url == "/")
            //{
            //    //This may fix a bug with .net, and just use the virdir instead.
            //    //The issue is that instead of the clientdir being /, the clientfile
            //    //is / and the clientdir is null.  Internal things then start throwing
            //    //exceptions.
            //    return;
            //}

            RewriteUrl(context, url);
        }
        /// <summary>
        /// Identifies the display type print or web.loads the appropriate xml file and loads the IPageAssemblyInstruction
        /// Loads the page template info and provides the complete intructions/assemblyinfo to assemble a page for the requested URL.
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="url">Requested URL</param>
        void RewriteUrl(HttpContext context, string url)
        {
            DisplayVersions displayVersion = DisplayVersions.Web;

            if (url.EndsWith("/")) //The key will never be just / because of the statement above
            {
                //strip the trailing /
                url = url.Substring(0, url.LastIndexOf('/'));
                if (string.IsNullOrEmpty(url))
                {
                    url = ContentDeliveryEngineConfig.DefaultHomePage.Homepage;
                }
            }

            //Store the url so it can be rewritten for logging.
            context.Items[REQUEST_URL_KEY] = url;

            bool isPrint = false;            

            if (url.EndsWith(PRINT_URL_ENDING))
            {
                //Do not take a substring if someone is trying to print the homepage 
                //of the site
                if (url != PRINT_URL_ENDING)
                {
                    //Since the pretty url map knows nothing about urls
                    //that end with print we need to remove /print from
                    //the key
                    url = url.Substring(0, url.Length - PRINT_URL_ENDING.Length);
                }
                else
                {
                    //We know the key for the homepage
                    //This code does not seem to actually work
                    //At the very least it should rewrite the url to /?print=1 or whatever
                    url = "/";
                }

                //Set the URL to Default home page if the requested url is "/".
                if (url == "/")
                    url = ContentDeliveryEngineConfig.DefaultHomePage.Homepage;

                isPrint = true;
                displayVersion=DisplayVersions.Print;
            }

            //Now check to see if it is the view all. (For MultiPageAssemblyInstructions)
            if (url.EndsWith(VIEWALL_URL_ENDING))
            {
                //Do not take a substring if someone is trying to print the homepage 
                //of the site
                if (url != VIEWALL_URL_ENDING)
                {
                    //Since the pretty url map knows nothing about urls
                    //that end with print we need to remove /print from
                    //the key
                    url = url.Substring(0, url.Length - VIEWALL_URL_ENDING.Length);
                }
                else
                {
                    //We know the key for the homepage
                    //This code does not seem to actually work
                    //At the very least it should rewrite the url to /?print=1 or whatever
                    url = "/";
                }

                if (isPrint)
                    displayVersion = DisplayVersions.PrintAll;
                else
                    displayVersion = DisplayVersions.ViewAll;
            }

            //Handle for Dictionary Print pages AND foia summaries pages.
            if (context.Request.Url.Query.Contains("print") && context.Request.Url.Query.Contains("allpages"))
            {
                displayVersion = DisplayVersions.PrintAll;
            }
            else if (context.Request.Url.Query.Contains("print"))
            {
                displayVersion = DisplayVersions.Print;
            }


            // Set Display version before loading the assembly instructions so it can be accessed in the constructor
            PageAssemblyContext.CurrentDisplayVersion = displayVersion;

            //Now lookup the url..

            IPageAssemblyInstruction assemblyInfo = null;

            try
            {
                //Load the XML file to create a assemblyinfo object of type ipageassemblyinstruction.If the XML file fails to create IPageAssemblyInstruction
                //Object log error
               assemblyInfo = PageAssemblyInstructionFactory.GetPageAssemblyInfo(url);
                //set Field filters,url filters...and other necessary things which depend on assemblyinfo.
                //Handle multipage pages               
               if (assemblyInfo == null)
               {
                   //1. Remove last part of path, e.g. /cancertopics/wyntk/bladder/page10 becomes /cancertopics/wyntk/bladder
                   string truncUrl = url.Substring(0, url.LastIndexOf('/'));
                   if (truncUrl != string.Empty)
                   {
                       assemblyInfo = PageAssemblyInstructionFactory.GetPageAssemblyInfo(truncUrl);
                       //check if is IMAPI
                       if (assemblyInfo is IMultiPageAssemblyInstruction)
                       {
                           //check if the page requested exists and return null if page does not exists
                           int index = ((IMultiPageAssemblyInstruction)assemblyInfo).GetPageIndexOfUrl(url);

                           if (index >= 0)
                           {
                               //This url is a page, so set the current index so we can get the page template later.
                               ((IMultiPageAssemblyInstruction)assemblyInfo).SetCurrentPageIndex(index);
                               assemblyInfo.Initialize();
                           }

                           else
                           {
                               assemblyInfo = null;
                               return;
                           }
                       }
                       else
                           assemblyInfo = null;
                   }
               }

               else
               {
                   assemblyInfo.Initialize();
               }

            }
            catch(Exception ex)
            {
                Logger.LogError("CDE:PageAssemblyInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to Create IPageAssemblyInstruction with the XML file Provided.", NCIErrorLevel.Error, ex);
                RaiseErrorPage();
                return;
            }
            //Exiting because there is no page assembly instruction for the requested URL.
            if (assemblyInfo == null)
            {
                return;
            }

            //Load the page template info for the current request
            PageTemplateInfo pageTemplateInfo=null;
            try 
            {
                    pageTemplateInfo = PageTemplateResolver.GetPageTemplateInfo(assemblyInfo.PageTemplateName, displayVersion);
            }
            catch(Exception ex)
            {
                Logger.LogError("CDE:PageAssemblyInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nCannot Load the pageTemplateInfo problem with the PageTemplateConfiguration XML file ", NCIErrorLevel.Error, ex);
                RaiseErrorPage();
                return;
            }

            if (pageTemplateInfo == null)
            {
                Logger.LogError("CDE:PageAssemblyInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nPage Template Not Found", NCIErrorLevel.Error);
                RaiseErrorPage();
                return;
            }

            //set the page assembly context with the assemblyInfo, dispayVersion and pageTemplateInfo
            PageAssemblyContext.Current.InitializePageAssemblyInfo(assemblyInfo, displayVersion, pageTemplateInfo, url);

            if (!string.IsNullOrEmpty(assemblyInfo.Language))
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(assemblyInfo.Language);

            string rewriteUrl = PageAssemblyContext.Current.PageTemplateInfo.PageTemplatePath;

            // Append original parameters in the request URL
            if (!String.IsNullOrEmpty(context.Request.Url.Query))
            {
                //The query should contain a ?
                rewriteUrl += context.Request.Url.Query;
            }

            //If we are showing the print version then append the print query
            //variable
            if (isPrint)
            {
                if (rewriteUrl.Contains("?"))
                    rewriteUrl += "&print=1";
                else
                    rewriteUrl += "?print=1";
            }

            // Do not reset the client path because it'll break form action url's.
            try
            {
                context.RewritePath(rewriteUrl,  false); 
            }

            catch (HttpException ex)
            {
                Logger.LogError("CDE:PageAssemblyInstructionLoader.cs:RewriteUrl", "Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to rewrite URL.", NCIErrorLevel.Error, ex);
                RaiseErrorPage();
            }

        }

        private static void RaiseErrorPage()
        {
            HttpContext.Current.Response.Write("There was an error processing the Request");
            HttpContext.Current.Response.End();
            return;
        }

        #endregion
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
    }
}
