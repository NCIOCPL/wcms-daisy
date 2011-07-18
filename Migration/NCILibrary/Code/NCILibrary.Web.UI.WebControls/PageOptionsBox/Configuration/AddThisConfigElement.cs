using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.UI.WebControls.Configuration
{
    public class AddThisConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("userName")]
        public string UserName
        {
            get
            {
                return (string)base["userName"];
            }
        }

        [ConfigurationProperty("lang", IsKey=true, IsRequired=true)]
        public string Language
        {
            get
            {
                return (string)base["lang"];
            }
        }

        public CultureInfo CultureInfo
        {
            get
            {
                CultureInfo culture = CultureInfo.CurrentUICulture;

                try
                {
                    culture = new CultureInfo(Language);
                }
                catch { }

                return culture;
            }
        }

        [ConfigurationProperty("compactServices", IsRequired = true)]
        public string CompactServices
        {
            get
            {
                return (string)base["compactServices"];
            }
        }

        [ConfigurationProperty("expandedServices", IsRequired = true)]
        public string ExpandedServices
        {
            get
            {
                return (string)base["expandedServices"];
            }
        }

    }
}
