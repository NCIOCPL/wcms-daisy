using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MonikerProviders.Configuration
{
    public class MonikerProviderConfiguration:ConfigurationSection
    {
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base["providers"];
            }
        }
    }
}
