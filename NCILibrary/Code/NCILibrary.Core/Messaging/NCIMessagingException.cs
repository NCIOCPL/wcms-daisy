using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Messaging
{
    /// <summary>
    /// Represents error that occurs during messaging execution
    /// </summary>
    public class NCIMessagingException : Exception
    {
        public NCIMessagingException() : base() { }
        public NCIMessagingException(string msg) : base(msg) { }
        public NCIMessagingException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
