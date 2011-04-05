using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;


namespace NCI.Web.CDE.HttpHandlers
{ 
    /// <summary>
    /// HttpHandler to handle forms for subscribing users to the NIH list serv.
    /// </summary>
    public class ListservSubscriptionHandler : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary> 
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return false; }
        }  

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            //throw new NotImplementedException();
            
            string toEmail = "listserv@list.nih.gov";

            string fromEmail = context.Request.Params["__SubscribeEmail"];
            string listserv = context.Request.Params["__ListServ"];
            string redirectPage = context.Request.Params["__RedirectUrl"];
            string emailValidator = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            string bodyFormatter = "quiet subscribe {0} no name";
            string errorMessage = "";
            bool hasError = false;

            //Determine if email is valid        
            if (fromEmail == null || (fromEmail = fromEmail.Trim()) == string.Empty)
            {
                hasError = true;
                errorMessage += "<br />You must provide an email address.";
            }
            else if (!Regex.IsMatch(fromEmail, emailValidator))
            {
                hasError = true;
                errorMessage += "<br />The email address you provided is invalid.";
            }

            //Make sure list is not empty
            if (listserv == null || (listserv = listserv.Trim()) == string.Empty)
            {
                hasError = true;
                errorMessage += "<br />The listserv to subscribe to was not specified.";
            }

            //Make sure Redirect Url is not empty
            if (redirectPage == null || (redirectPage = redirectPage.Trim()) == string.Empty)
            {
                hasError = true;
                errorMessage += "<br />The redirect page was not specified.";
            }

            //If there is no error, send email, else, print error message
            if (!hasError)
            {
                try
                {
                    using (MailMessage mess = new MailMessage(fromEmail, toEmail, string.Empty, string.Format(bodyFormatter, listserv)))
                    {
                        SmtpClient client = new SmtpClient();
                        client.Send(mess);
                    }
                    context.Response.Redirect(redirectPage, true);                    
                    //Page.Response.Redirect(redirectPage, true);
                }
                catch (System.Threading.ThreadAbortException)
                {
                    throw; //This is the Respose.Redirect.  Let it bubble!
                }
                catch (Exception ex)
                {
                   context.Response.Write("Errors occurred while attempting to subscribe:<br /> " + ex.ToString());
                }
            }
            else
            {
                context.Response.Write(errorMessage);
            }

        }

        #endregion
    }
}
