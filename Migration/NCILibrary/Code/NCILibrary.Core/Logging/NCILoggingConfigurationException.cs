using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Util
{
    /// <summary>
    /// This Class has behaviour that raises Exceptions raised on performing logging operations due to Configuration issues.
    /// </summary>
    public class NCILoggingConfigurationException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NCILoggingConfigurationException() : base() { }
        /// <summary>
        /// Constructor that takes Log Message as Argument.
        /// </summary>
        /// <param name="msg">The message passed into the Custom Exception.</param>
        public NCILoggingConfigurationException(string msg) : base(msg) { }
        /// <summary>
        /// Constructor that takes Log Message and Exception objects as Arguments respectively.
        /// </summary>
        /// <param name="msg">The message passed into the Custom Exception.</param>
        /// <param name="innerException">The inner Exception for the Custom Exception.</param>
        public NCILoggingConfigurationException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
