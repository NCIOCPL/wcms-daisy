using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using NCI.Logging;
using NCI.Util;

namespace NCI.Logging.Providers
{
    /// <summary>
    /// This class writes to the EventLog any error messages that the Client needs.
    /// </summary>
    public class EventLogProvider : NCI.Logging.Providers.NCILoggingProvider
    {
        /// <summary>
        /// The log source
        /// </summary>
        private string logsource;

        private const string _loggingFormat = "Facility: {0} \n\nError Level: {1} \n\nMessage:\n{2}\n\nException:\n{3}";

        public EventLogProvider()
        {
           // NCI.Logging.Providers.NCILoggingProvider eventlogprovider = NCI.Logging.Providers.NCILogging.Providers["EventLogProvider"];
        }

        /// <summary>
        /// Initializes the private variables in this class from the configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {

            try
            {
                if ((config == null) || (config.Count == 0))
                    throw new NCILoggingConfigurationException("NCI.Logging.Providers.EventLogProvider: Error reading configuration section.");

                if (string.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                    config.Add("description", "EmailProvider");
                }

                //Let ProviderBase perform the basic initialization
                base.Initialize(name, config);

                //Perform feature-specific provider initialization here

                logsource = config["logsource"];
                if (String.IsNullOrEmpty(logsource))
                    logsource = "";
            }
            catch (Exception ex)
            {
                throw new NCILoggingConfigurationException("NCI.Logging.Providers.EventLogProvider: Error initializing provider variables from Configuration file.", ex);
            }
            
            if (!EventLog.SourceExists(logsource))
            {
                throw new NCILoggingException("NCI.Logging.Providers.EventLogProvider: The Event Log " + logsource + " source does not exist");
            }
        }
               
        public override void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
            try
            {
                EventLog.WriteEntry(
                    logsource, 
                    String.Format(
                        _loggingFormat, 
                        facility, 
                        errorLevel.ToString(), 
                        message, 
                        ex.ToString()), 
                    GetEventLogETFromNCIErrorLevel(errorLevel));
            }
            catch (Exception newex)
            {
                throw new NCILoggingException("NCI.Logging.Providers.EventLogProvider: Could not write error to Event Log ", newex);
            }
        }
     
        public override void WriteToLog(string facility, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
            try
            {
                EventLog.WriteEntry(
                    logsource,
                    String.Format(
                        _loggingFormat,
                        facility,
                        errorLevel.ToString(),
                        "",
                        ex.ToString()),
                    GetEventLogETFromNCIErrorLevel(errorLevel));
            }
            catch (Exception newex)
            {
                throw new NCILoggingException("NCI.Logging.Providers.EventLogProvider: Could not write error to Event Log ", newex);
            }
        }

        public override void WriteToLog(string facility, string message, NCIErrorLevel errorLevel)
        {
            try
            {
                EventLog.WriteEntry(
                    logsource,
                    String.Format(
                        _loggingFormat,
                        facility,
                        errorLevel.ToString(),
                        message,
                        ""),
                    GetEventLogETFromNCIErrorLevel(errorLevel));
            }
            catch (Exception newex)
            {
                throw new NCILoggingException("NCI.Logging.Providers.EventLogProvider: Could not write error to Event Log ", newex);
            }
        }

        private EventLogEntryType GetEventLogETFromNCIErrorLevel(NCIErrorLevel errorLevel)
        {
            if (errorLevel >= NCIErrorLevel.Error)
                return EventLogEntryType.Error;
            else if (errorLevel > NCIErrorLevel.Info)
                return EventLogEntryType.Warning;
            else
                return EventLogEntryType.Information;
        }
    }
}
