using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.UI.Configuration
{
    public class Settings
    {
        public static bool IsLive
        {
            get
            {
                string value = System.Configuration.ConfigurationManager.AppSettings["isLive"];
                return string.IsNullOrEmpty(value) ? false : bool.Parse(value);
            }
        }
    }
}
