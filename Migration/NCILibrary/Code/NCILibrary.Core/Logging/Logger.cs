using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Logging
{
    
    
    /// <summary>
    /// Specifies the level of Error Messages that should be logged.
    /// </summary>
    [Flags]
    public enum NCIErrorLevel : uint
    {
        /// <summary>
        /// No Logging neccessary.
        /// </summary>
        Clear = 0x00,
        /// <summary>
        /// Messages related to debugging.
        /// </summary>
        Debug = 0x01,
        /// <summary>
        /// Messages related to specific information.
        /// </summary>
        Info = Debug<<1,
        /// <summary>
        /// Warning Messages only.
        /// </summary>
        Warning = Info << 1,
        /// <summary>
        /// All Error Messages.
        /// </summary>
        Error = Warning << 1,
        /// <summary>
        /// Complete Error Messages, such as failed database connectivitiy.
        /// </summary>
        Critical = Error << 1,
        /// <summary>
        /// All messages for any level
        /// </summary>
        All = Debug | Info | Warning | Error | Critical

    }

    /// <summary>
    /// Class contains Behaviour and properties related to Logging purposes within NCI.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs Error to the provider by sending facility, message and NCIErrorLevel in the message.
        /// </summary>
        /// <param name="message">Description of the Error Message.</param>
        /// <param name="facility">Description of the facility of the Error Message.</param>
        /// <param name="level">Specifies the level of Error Messages.</param>
        public static void LogError(string facility, string message, NCIErrorLevel level)
        {
            LoggingHelper helper = LoggingHelper.Instance;
            helper.LogError(facility, message, level);
        }
        /// <summary>
        /// Logs Error to the provider by sending facility, message, NCIErrorLevel and Exception in the message.
        /// </summary>
        /// <param name="message">Description of the Error Message.</param>
        /// <param name="facility">Description of the facility of the Error Message.</param>
        /// <param name="level">Specifies the level of Error Messages.</param>
        /// <param name="ex">Actual Exception object.</param>
        public static void LogError(string facility, string message, NCIErrorLevel level, Exception ex)
        {
            LoggingHelper helper = LoggingHelper.Instance;
            helper.LogError(facility, message, level, ex);
        }
        /// <summary>
        /// Logs Error to the provider by sending facility,NCIErrorLevel and Exception in the message.
        /// </summary>
        /// <param name="facility">Description of the facility of the Error Message.</param>
        /// <param name="level">Specifies the level of Error Messages.</param>
        /// <param name="ex">Actual Exception object.</param>
        public static void LogError(string facility, NCIErrorLevel level, Exception ex)
        {
            LoggingHelper helper = LoggingHelper.Instance;
            helper.LogError(facility, level, ex);
        }
    }

}
