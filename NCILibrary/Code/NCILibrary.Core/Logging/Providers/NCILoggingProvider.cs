using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Logging.Providers
{
    public abstract class NCILoggingProvider : ProviderBase
    {
        //Methods
        /// <summary>
        /// Writes out the ErrorMessage to the implemented Provider, using the facility, message, errorLevel and Exception variables passed in as parameters
        /// </summary>
        /// <param name="facility">facility of the errormessage</param>
        /// <param name="message">friendly message logged using the provider</param>
        /// <param name="errorLevel">Clear, Debug, Info, Error, Critical, Warning or All NCIErrorLevel</param>
        /// <param name="ex">Exception object to be used by the provider</param>
        public abstract void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel, Exception ex);
        /// <summary>
        /// Writes out the ErrorMessage to the implemented Provider, using the facility, errorLevel and Exception variables passed in as parameters
        /// </summary>
        /// <param name="facility">facility of the errormessage</param>
        /// <param name="errorLevel">Clear, Debug, Info, Error, Critical, Warning or All NCIErrorLevel</param>
        /// <param name="ex">Exception object to be used by the provider</param>
        public abstract void WriteToLog(string facility, NCI.Logging.NCIErrorLevel errorLevel, Exception ex);
        /// <summary>
        /// Writes out the ErrorMessage to the implemented Provider, using the facility, message and errorLevel variables passed in as parameters
        /// </summary>
        /// <param name="facility">facility of the errormessage</param>
        /// <param name="message">friendly message logged using the provider</param>
        /// <param name="errorLevel">Clear, Debug, Info, Error, Critical, Warning or All NCIErrorLevel</param>
        public abstract void WriteToLog(string facility, string message, NCI.Logging.NCIErrorLevel errorLevel);  
      
        
                
    }


}
