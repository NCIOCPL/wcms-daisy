using System;
using System.Configuration;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.Test.Configuration
{
    class Config
    {
        private static readonly string _httpSimulatorPath = "nci.test/httpSimulator";
        private static HttpSimulatorSection _httpSimulatorSection;

        public static HttpSimulatorSection HttpSimulator
        {
            get
            {
                if (_httpSimulatorSection == null)
                {
                    _httpSimulatorSection = (HttpSimulatorSection)ConfigurationManager.GetSection(_httpSimulatorPath);

                    // When the delivery sectionGroup isn't defined, _delivery comes back null.
                    if (_httpSimulatorSection == null)
                    {
                        throw new ConfigurationErrorsException(String.Format("Could not load http simulator configuration from {0}.  Please ensure the cms.test section group and httpSimulator element have been defined in the configuration file.", _httpSimulatorPath));
                    }
                }

                return _httpSimulatorSection;
            }
        }
    }
}
