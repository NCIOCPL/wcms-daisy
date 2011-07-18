using System;
using System.Configuration;

namespace NCI.Logging.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class NCILoggingProviderConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base["providers"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("defaultProvider", DefaultValue = "NCIEmailProvider")]
        [StringValidator(MinLength = 1)]
        public string DefaultProvider
        {
            get
            {
                return (string)base["defaultProvider"];
            }
            set
            {
                base["defaultProvider"] = value;
            }
        }
    }

   

   


}
