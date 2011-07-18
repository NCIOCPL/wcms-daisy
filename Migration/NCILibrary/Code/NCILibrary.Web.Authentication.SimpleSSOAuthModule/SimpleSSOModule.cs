using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace NCI.Web.Authentication.SimpleSSOAuthModule
{
    public class SimpleSSOModule : IHttpModule
    {

        //These are the extensions that site minder does not care about
        //and just ignores.
        private readonly string[] _ignoreExtensions = { 
            ".class", ".gif" , ".jpg", ".jpeg",
            ".png", ".fcc", ".scc", ".sfcc", ".ccc", ".ntc" };
        
        /// <summary>
        /// "Authenticate" the current request using SiteMinder Http Headers.
        /// <remarks>
        /// Really all this does is take in the HTTP_SM_User Http Header and
        /// create a generic principal.
        /// </remarks>
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Event args.</param>
        private void AuthenticateRequest(object sender, EventArgs e)
        {
            // Get the application and request objects.
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = context.Request;
            
            try
            {
                //Make sure that the current request does not have an extension that
                //SiteMinder will just passthrough.
                if (DoesRequestExtRequireAuth(request))
                {
                    // Check for SSO username and domain override in the config file.
                    string userName = WebConfigurationManager.AppSettings["Override_SSO_Username"];
                    if (String.IsNullOrEmpty(userName))
                        userName = request.ServerVariables["HTTP_SM_USER"];

                    //We do not need the auth domain for this really... well I do not think so unless,
                    //NIH is going to start allowing other domains to connect

                    if (!String.IsNullOrEmpty(userName))
                    {
                        // Create a new identity object.
                        IIdentity identity = new GenericIdentity(userName, "SimpleSSOModule");
                        // Create a new principal object and assign it to the current context.
                        context.User = new GenericPrincipal(identity, new string[] { });
                    }
                    else
                    {
                        throw new Exception("SM_USER header is blank!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to authenticate request.", ex);
            }
        }

        /// <summary>
        /// Determines if the extension of the current request requires
        /// authentication.
        /// </summary>
        /// <returns></returns>
        private bool DoesRequestExtRequireAuth(HttpRequest request)
        {
            bool rtnVal = true;

            string ext = Path.GetExtension(request.Path);

            if (!string.IsNullOrEmpty(ext))
            {
                foreach (string passExt in _ignoreExtensions)
                {
                    if (string.Compare(ext, passExt, true) == 0)
                    {
                        rtnVal = false;
                        break;
                    }
                }
            }

            return rtnVal;
        }

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements 
        /// <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        public void Dispose()
        {
            //We have nothing to dispose
        }

        /// <summary>Initializes a module and prepares it to handle requests.</summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, 
        /// properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(AuthenticateRequest);
        }

        #endregion

    }
}
