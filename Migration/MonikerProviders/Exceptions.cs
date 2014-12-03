using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonikerProviders
{
    /// <summary>
    /// Base class for all exceptions related to monikers.
    /// </summary>
    [global::System.Serializable]
    abstract public class MonikerException : Exception
    {
        public MonikerException() { }
        public MonikerException(string message) : base(message) { }
        public MonikerException(string message, Exception inner) : base(message, inner) { }
        protected MonikerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown for errors loading moniker configuration data.
    /// </summary>
    [global::System.Serializable]
    public class MonikerConfigurationException : MonikerException
    {
        public MonikerConfigurationException() { }
        public MonikerConfigurationException(string message) : base(message) { }
        public MonikerConfigurationException(string message, Exception inner) : base(message, inner) { }
        protected MonikerConfigurationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when a moniker is not available in moniker store.
    /// </summary>
    [global::System.Serializable]
    public class MonikerNotFoundException : MonikerException
    {
        public MonikerNotFoundException() { }
        public MonikerNotFoundException(string message) : base(message) { }
        public MonikerNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected MonikerNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when a duplicate moniker is added to a store.
    /// </summary>
    [global::System.Serializable]
    public class DuplicateMonikerException : MonikerException
    {
        public DuplicateMonikerException() { }
        public DuplicateMonikerException(string message) : base(message) { }
        public DuplicateMonikerException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateMonikerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
