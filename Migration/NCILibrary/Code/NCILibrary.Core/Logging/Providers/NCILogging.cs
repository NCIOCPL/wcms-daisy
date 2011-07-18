using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using NCI.Util;

namespace NCI.Logging.Providers
{
    public static class NCILogging
    {
        //Initialization related variables and logic
        private static bool _isInitialized = false;
        private static Exception _initializationException;

        private static object _initializationLock;

        private static NCILoggingProvider _defaultProvider;
        private static NCILoggingProviderCollection _providerCollection;

        /// <summary>
        /// 
        /// </summary>
        static NCILogging()
        {
            _isInitialized = false;
            _initializationLock = new object();
            _initializationException = null;
        }

        /// <summary>
        /// Initializes the Logging Providers specified in the web.config
        /// </summary>
        private static void Initialize()
        {
            //Check if we have been initialized before.
            if (_isInitialized)
            {
                //If we did initialize and there was an exception,
                //rethrow the exception
                if (_initializationException != null)
                    throw _initializationException;
            }
            else
            {
                lock (_initializationLock)
                {
                    //We do the check again here if 2 threads were trying
                    //to initialize at the same time, so one finished, the
                    //lock was released, and then the next one trys again.
                    if (_isInitialized)
                    {
                        if (_initializationException != null)
                            throw _initializationException;
                    }

                    //Now actually do the initialization
                    try
                    {                            
                        //Get the feature's configuration info
                        NCILoggingProviderConfiguration qc =
                            (NCILoggingProviderConfiguration)ConfigurationManager.GetSection("NCILoggingProviders");

                        //Instantiate the providers
                        _providerCollection = new NCILoggingProviderCollection();
                        InstantiateProviders(qc.Providers, _providerCollection, typeof(NCILoggingProvider));
                        _providerCollection.SetReadOnly();

                        if (qc.DefaultProvider == null)
                        {
                            _initializationException = new ProviderException("NCI.Logging.Providers.NCILogging: You must specify a valid default provider.");
                        }
                        else
                        {
                            try
                            {
                                _defaultProvider = _providerCollection[qc.DefaultProvider];
                            }
                            catch { }
                        }
                        if (_defaultProvider == null)
                        {
                            _initializationException = new ConfigurationErrorsException(
                                "NCI.Logging.Providers.NCILogging: You must specify a default provider for the feature.",
                                qc.ElementInformation.Properties["defaultProvider"].Source,
                                qc.ElementInformation.Properties["defaultProvider"].LineNumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        _initializationException = ex;
                    }
                    //Set it as initialized
                    _isInitialized = true;                   
                }

                //Finally throw any exceptions that happened.
                if (_initializationException != null)
                    throw _initializationException;
            }
        }

        private static ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
        {
            Type settingsType = Type.GetType(providerSettings.Type);

            if (settingsType == null)
                throw new ConfigurationErrorsException(String.Format("Could not find type: {0}", providerSettings.Type));
            if (!providerType.IsAssignableFrom(settingsType))
                throw new ConfigurationErrorsException(String.Format("Provider '{0}' must subclass from '{1}'", providerSettings.Name, providerType));

            ProviderBase provider = Activator.CreateInstance(settingsType) as ProviderBase;

            provider.Initialize(providerSettings.Name, providerSettings.Parameters);

            return provider;
        }

        private static void InstantiateProviders(ProviderSettingsCollection configProviders, ProviderCollection providers, Type providerType)
        {
            if (!typeof(ProviderBase).IsAssignableFrom(providerType))
                throw new ConfigurationErrorsException(String.Format("type '{0}' must subclass from ProviderBase", providerType));

            foreach (ProviderSettings settings in configProviders)
                providers.Add(InstantiateProvider(settings, providerType));
        }

        //Public feature API

        /// <summary>
        /// 
        /// </summary>
        public static NCILoggingProvider Provider
        {
            get
            {
                Initialize();
                return _defaultProvider;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static NCILoggingProviderCollection Providers
        {
            get
            {
                Initialize();
                return _providerCollection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="message"></param>
        /// <param name="errorLevel"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
            Provider.WriteToLog(facility,message,errorLevel,ex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="errorLevel"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static void WriteToLog(string facility, NCI.Logging.NCIErrorLevel errorLevel, Exception ex)
        {
             Provider.WriteToLog(facility,errorLevel,ex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="errorLevel"></param>
        /// <returns></returns>
        public static void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel)
        {
             Provider.WriteToLog(facility, message, errorLevel);
        }
        
        
    }

   
}
