using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NCI.Logging.Configuration
{
    public class NCILoggingSection : ConfigurationSection
    {
        [ConfigurationProperty("lastResortLogFile")]
        public string LastResortLogFile
        {
            get { return (string)base["lastResortLogFile"]; }
            set { base["lastResortLogFile"] = value; }
        }

        [ConfigurationProperty("logAllLoggingErrors")]
        public bool LogAllLoggingErrors
        {
            get { return (bool)base["logAllLoggingErrors"]; }
            set { base["logAllLoggingErrors"] = value; }
        }

        [ConfigurationProperty("loggingSinks", IsDefaultCollection = false)]
        public LoggingSinksCollection LoggingSinks
        {
            get { return (LoggingSinksCollection)base["loggingSinks"]; }
        }
    }
}
