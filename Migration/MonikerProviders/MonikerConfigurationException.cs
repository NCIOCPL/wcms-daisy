using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonikerProviders
{
    [global::System.Serializable]
    public class MonikerConfigurationException : Exception
    {
        public MonikerConfigurationException() { }
        public MonikerConfigurationException(string message) : base(message) { }
        public MonikerConfigurationException(string message, Exception inner) : base(message, inner) { }
        protected MonikerConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
