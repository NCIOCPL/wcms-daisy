using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonikerProviders.Configuration;

namespace MonikerProviders
{
    public class MonikerStore : IMonikerStore
    {
        MonikerProvider provider;

        public MonikerStore(string storeName)
        {
            MonikerProviderConfiguration config = (MonikerProviderConfiguration)ConfigurationManager.GetSection("MonikerProviders");
            Type providerType = typeof(MonikerProvider);

            // Only use the first Provider.
            foreach (ProviderSettings settings in config.Providers)
            {
                if (!storeName.Equals(settings.Name, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                Type settingsType = Type.GetType(settings.Type);

                if (settingsType == null)
                    throw new ConfigurationErrorsException(String.Format("Could not find type: {0}", settings.Type));
                if (!providerType.IsAssignableFrom(settingsType))
                    throw new ConfigurationErrorsException(String.Format("Provider '{0}' must subclass from '{1}'", settings.Name, providerType));

                provider = Activator.CreateInstance(settingsType) as MonikerProvider;

                if (provider != null)
                {
                    provider.Initialize(settings.Name, settings.Parameters);
                    break;
                }
            }

            if (provider == null)
                throw new ConfigurationErrorsException(string.Format("No MonikerProvider found for.", storeName));
        }

        #region IMonikerStore Members

        public void Add(Moniker moniker)
        {
            provider.Add(moniker);
        }

        public void Add(string name, long contentID, string contentType)
        {
            provider.Add(new Moniker(name, contentID, contentType));
        }

        public bool Contains(string name)
        {
            return provider.Contains(name);
        }

        public void Delete(string name)
        {
            provider.Delete(name);
        }

        public Moniker Get(string name)
        {
            return provider.Get(name);
        }

        public void Clear()
        {
            provider.Clear();
        }

        #endregion
    }
}
