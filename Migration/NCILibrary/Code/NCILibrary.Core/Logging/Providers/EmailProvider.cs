using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net.Mail;
using NCI.Logging.Providers;
using NCI.Logging;
using NCI.Util;

namespace NCI.Logging.Providers
{
 
    /// <summary>
    /// This class sends out emails for any error messages that the Client needs.
    /// </summary>
    public class EmailProvider : NCI.Logging.Providers.NCILoggingProvider
    {
        /// <summary>
        /// The sender email address
        /// </summary>
        private string _emailaddressfrom;
        /// <summary>
        /// The Destination email address
        /// </summary>
        private string _emailaddressesto;

        private const string _loggingFormat = "Facility: {0} \n\nError Level: {1} \n\nMessage:\n{2}\n\nException:\n{3}";

        /// <summary>
        /// 
        /// </summary>
        public EmailProvider()
        {
          
        }

        /// <summary>
        /// Initializes the private variables in this class from the configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {

            if ((config == null) || (config.Count == 0))
                throw new NCILoggingConfigurationException("NCI.Logging.Providers.EmailProvider: Error reading configuration section.");

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "EmailProvider");
            }

            //Let ProviderBase perform the basic initialization
            base.Initialize(name, config);

            //Perform feature-specific provider initialization here
            //A great deal more error checking and handling should exist here

            _emailaddressesto = config["emailaddressesto"];
            if (String.IsNullOrEmpty(_emailaddressesto))
                _emailaddressesto = "";

            _emailaddressfrom = config["emailaddressfrom"];
            if (String.IsNullOrEmpty(_emailaddressfrom))
                _emailaddressfrom = "";
        }

        /// <summary>
        /// Sends the Error message out as an Email.
        /// </summary>
        /// <param name="facility">specifies the facility to match against facilities specified in configuration file.</param>
        /// <param name="message">error message passed in by the client</param>
        /// <param name="errorLevel">specifies the error level such as debug,warning</param>
        /// <param name="ex">exception variable required to be logged</param>
        public override void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
            SendEmail(
                facility, 
                String.Format(
                        _loggingFormat,
                        facility,
                        errorLevel.ToString(),
                        message,
                        ex.ToString()));
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facility">specifies the facility to match against facilities specified in configuration file.</param>
        /// <param name="errorLevel">specifies the error level such as debug,warning</param>
        /// <param name="ex">exception variable required to be logged</param>
        public override void WriteToLog(string facility, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
            SendEmail(
                facility,
                String.Format(
                        _loggingFormat,
                        facility,
                        errorLevel.ToString(),
                        "",
                        ex.ToString()));
        }

        /// <summary>
        /// Sends the Error message out as an Email.
        /// </summary>
        /// <param name="facility">specifies the facility to match against facilities specified in configuration file.</param>
        /// <param name="message">error message passed in by the client</param>
        /// <param name="errorLevel">specifies the error level such as debug,warning</param>        
        public override void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel)
        {
            SendEmail(
                facility,
                String.Format(
                    _loggingFormat,
                    facility,
                    errorLevel.ToString(),
                    message,
                    ""));

        }

        /// <summary>
        /// Sends out the Email for the Error Message that the client needs to send.
        /// </summary>
        /// <param name="Subject">Subject of the Email.</param>
        /// <param name="Body">Body of the Email.</param>
        private void SendEmail(string subject, string body)
        {
            try
            {
                using (MailMessage message = new MailMessage(_emailaddressfrom, _emailaddressesto, subject, body))
                {
                    SmtpClient smtpClient = new SmtpClient();                    
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new NCILoggingException("NCI.Logging.Providers.EmailProvider: Could not send email.", ex);
            }

        }

    }
}
