using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NCI.Logging.Providers;
using NCI.Logging;
using NCI.Logging.Configuration;
using NCI.Util;

namespace NCI.Logging
{
    
    public class LoggingHelper
    {

        //Lets make this thing a singleton
        private static readonly LoggingHelper _helperInstance = new LoggingHelper();

        private NCI.Logging.SinkCollection  _loggingSinks;        
        private string _lastResortLogFile = string.Empty;
        private bool _logAllErrors = false;

        /// <summary>
        /// Gets an instance of the LoggingHelper class.
        /// </summary>
        public static LoggingHelper Instance
        {
            get { return _helperInstance; }
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit.  Basically,
        // if we just init _helperInstance, we are not guaranteed
        // that it will init before everything else.  If we put in
        // the static constructor we are sure.
        static LoggingHelper()
        {
        }

        private LoggingHelper()
        {
            Initialize();
        }

          /// <summary>
          /// Initilizes the configuration Section.
          /// </summary>
        private void Initialize()
        {
            bool hasError = false;

            //Go to config and make sinks
            try
            {
                NCILoggingSection logsection = (NCILoggingSection)ConfigurationManager.GetSection("nci/logging");
                _loggingSinks = new SinkCollection();

                _lastResortLogFile = Strings.Clean(logsection.LastResortLogFile);
                _logAllErrors = logsection.LogAllLoggingErrors;

                foreach (LoggingSinkElement sinkConfig in logsection.LoggingSinks)
                {

                    string[] facilitymatchstrings = new string[sinkConfig.FacilityMatchStrings.Count];
                    for (int i = 0; i < sinkConfig.FacilityMatchStrings.Count; i++)
                    {
                        facilitymatchstrings[i] = sinkConfig.FacilityMatchStrings[i].Value;
                    }

                    //Get error levels out of the sinkConfig, GetMatchstrings out of it
                    Sink s = new Sink(sinkConfig.Name, GetLevelsFromString(sinkConfig.ErrorLevels), sinkConfig.ProviderName, facilitymatchstrings);
                    _loggingSinks.Add(s);
                }
            }
            catch (Exception ex)
            {
                LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: Invalid Configuration Section in nci/logging", ex));
                hasError = true;
            }



            if (!hasError)
            {
                //When NCILogging.Providers is called for the first time, NCILogging calls Initialize,
                //which in turn loads up all of the providers and calls thier initialize.  If we look
                //as some trivial property of the providers before we call the logging, then we can
                //catch and log any exceptions that might happen while initializing the providers.
                try
                {
                    //I know this is wierd, but this will force NCILogging to initalize
                    if (NCILogging.Providers == null)
                    {
                    }
                }
                catch (Exception ex)
                {
                    LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: Invalid Configuration Section in nci.logging", ex));
                }
            }
        }

        /// <summary>
        /// This logs any errors to a file specified in the logging config.
        /// </summary>
        /// <param name="ex">The exception to be logged</param>
        private void LogToLastResort(Exception ex)
        {
            try
            {
                StreamWriter error = null;

                //If logging all errors open log file in append mode.
                if (_logAllErrors)
                    error = new StreamWriter(_lastResortLogFile, true);
                else
                    error = new StreamWriter(_lastResortLogFile, false);

                error.WriteLine("------------------------------------------------------");
                error.WriteLine("Time: " + DateTime.Now.ToString());
                error.WriteLine(ex.ToString());
                error.WriteLine();
                error.Close();
            }
            catch (Exception newex)
            {
                //Do nothing. BTW newex is there for debugging purposes.
            }
        }


        #region Configuration Helpers

        /// <summary>
        /// Parses the string passed in and return the ErrorLevel
        /// </summary>
        /// <param name="s">String containting the error level.</param>
        /// <returns>The Error Level of the string passed in.</returns>
        private NCIErrorLevel GetLevelsFromString(string s)
        {
            NCIErrorLevel errorLevels = NCIErrorLevel.Clear;

            //Call enum parser
            try
            {
                NCIErrorLevel[] levels = ConvertEnum<NCIErrorLevel>.ConvertToArray(s);
                //uint[] levels = Strings.ToUIntArray(s, true);

                //foreach (uint level in levels)
                //{
                //    errorLevels |= (NCIErrorLevel)level;
                //}

                foreach (NCIErrorLevel level in levels)
                {
                    errorLevels |= level;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("NCI.Logging.LoggingHelper: Invalid Error Level in Logging Sinks", ex);
            }
            return errorLevels;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs the Error Mesaage.
        /// </summary>

        /// <param name="errorLevel">The error level of the message.</param>
        /// <param name="message">Description of the Error Message.</param>
        /// <param name="facility">Description of the facility of the Error Message.</param>

        public void LogError(string facility, string message, NCIErrorLevel errorLevel)
        {
            string[] providerNames = { };

            try
            {
                providerNames = _loggingSinks.GetProviderNamesByErrorAndFacility(facility, errorLevel);
            }
            catch (Exception ex)
            {
                if (_logAllErrors)
                    LogToLastResort(ex);

                return;
            }

            //There are no providers so return
            if (providerNames.Length <= 0)
            {
                if (_logAllErrors)
                    LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: No providers have been loaded, so cannot log to any provider."));

                return;
            }

            try
            {
                Array.ForEach<string>(providerNames, delegate(string providerName)
                {
                    if (providerName != null)
                    {
                        //DO THIS RIGHT, PUT IN ERROR LOGGING
                        if (NCILogging.Providers.Contains(providerName))
                        {
                            NCILogging.Providers[providerName].WriteToLog(facility, message, errorLevel);
                        }
                        else
                        {
                            if (_logAllErrors)
                                LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: Could not find provider, " + providerName + ", in providers collection when logging."));
                        }
                    }
                    else //Use default provider
                    {
                        NCILogging.WriteToLog(facility, message, errorLevel);
                    }
                });
            }
            catch (Exception ex)
            {
                if (_logAllErrors)
                    LogToLastResort(ex);
            }
        }

        /// <summary>
        /// Logs the Error Mesaage.
        /// </summary>
        /// <param name="message">Description of the Error Message.</param>
        /// <param name="facility">Description of the facility of the Error Message.</param>
        /// <param name="errorLevel">Specifies the level of Error Messages.</param>
        /// <param name="ex">Actual Exception object.</param>
        public void LogError(string facility, string message, NCIErrorLevel errorLevel, Exception ex)
        {

            string[] providerNames;
            try
            {
                providerNames = _loggingSinks.GetProviderNamesByErrorAndFacility(facility, errorLevel);
            }
            catch (Exception ex1)
            {
                if (_logAllErrors)
                    LogToLastResort(ex1);

                return;
            }

            //There are no providers so return
            if (providerNames.Length <= 0)
            {
                if (_logAllErrors)
                    LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: No providers have been loaded, so cannot log to any provider."));

                return;
            }

            try
            {
                Array.ForEach<string>(providerNames, delegate(string providerName)
                {
                    if (providerName != null)
                    {
                        //DO THIS RIGHT, PUT IN ERROR LOGGING
                        if (NCILogging.Providers.Contains(providerName))
                        {
                            try
                            {
                                NCILogging.Providers[providerName].WriteToLog(facility, message, errorLevel, ex);
                            }
                            catch (Exception logProvEx)
                            {
                                //There is an error logging to a provider.
                                LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: There was an error logging to a provider.", logProvEx));
                            }
                        }
                        else
                        {
                            if (_logAllErrors)
                                LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: Could not find provider, " + providerName + ", in providers collection when logging."));
                        }
                    }
                    else //Use default provider
                    {
                        NCILogging.WriteToLog(facility, message, errorLevel, ex);
                    }
                });

            }
            catch (Exception ex1)
            {
                if (_logAllErrors)
                    LogToLastResort(ex1);
            }
        }

        /// <summary>
        /// Logs the Error Mesaage.
        /// </summary>
        /// <param name="facility">Description of the facility of the Error Message.</param>
        /// <param name="errorLevel">Specifies the level of Error Messages.</param>
        /// <param name="ex">Actual Exception object.</param>
        public void LogError(string facility, NCIErrorLevel errorLevel, Exception ex)
        {
            string[] providerNames;

            try
            {
                providerNames = _loggingSinks.GetProviderNamesByErrorAndFacility(facility, errorLevel);
            }
            catch (Exception ex1)
            {
                if (_logAllErrors)
                    LogToLastResort(ex1);

                return;
            }

            //There are no providers so return
            if (providerNames.Length <= 0)
            {
                if (_logAllErrors)
                    LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: No providers have been loaded, so cannot log to any provider."));

                return;
            }

            try
            {
                Array.ForEach<string>(providerNames, delegate(string providerName)
                {
                    if (providerName != null)
                    {
                        //DO THIS RIGHT, PUT IN ERROR LOGGING
                        if (NCILogging.Providers.Contains(providerName))
                        {
                            NCILogging.Providers[providerName].WriteToLog(facility, errorLevel, ex);
                        }
                        else
                        {
                            if (_logAllErrors)
                                LogToLastResort(new NCILoggingException("NCI.Logging.LoggingHelper: Could not find provider, " + providerName + ", in providers collection when logging."));
                        }
                    }
                    else //Use default provider
                    {
                        NCILogging.WriteToLog(facility, errorLevel, ex);
                    }
                });
            }
            catch (Exception ex1)
            {
                if (_logAllErrors)
                    LogToLastResort(ex1);
            }
        }

        #endregion

    }
}
