using System;
using System.Configuration.Provider;

namespace NCI.Logging.Providers
{
    public class NCILoggingProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is NCILoggingProvider))
                throw new ArgumentException("The provider parameter must be of type LoggingProvider.");

            base.Add(provider);
        }

        public bool Contains(string name)
        {
            foreach (ProviderBase provider in this)
            {
                if (provider.Name == name)
                    return true;
            }

            return false;
        }

        new public NCILoggingProvider this[string name]
        {
            get { return (NCILoggingProvider)base[name]; }
        }

        public void CopyTo(NCILoggingProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
