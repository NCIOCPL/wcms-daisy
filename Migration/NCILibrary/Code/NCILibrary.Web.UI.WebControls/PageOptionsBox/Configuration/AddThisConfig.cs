using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.UI.WebControls.Configuration
{
    public static class AddThisConfig
    {
        private static readonly string addThisConfigPath = "nci/web/addThis";

        public static AddThisConfigElement GetByCultureLanguage(CultureInfo culture)
        {            
            AddThisConfigElement rtnElem = null;
            AddThisConfigElement defaultElem = null;

            AddThisServiceConfigSection section = (AddThisServiceConfigSection)ConfigurationManager.GetSection(addThisConfigPath);

            foreach (AddThisConfigElement elem in section.Configs)
            {
                if (elem.CultureInfo.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                {
                    rtnElem = elem;
                    break;
                }

                if (elem.CultureInfo.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                {
                    defaultElem = elem;
                }

            }

            if (rtnElem == null)
                rtnElem = defaultElem;

            return rtnElem;
        }
    }
}
