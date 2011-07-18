using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public static class ContentDeliveryEngineConfig
    {
        private static string _deliveryEnginePath = "nci/web/cde";
        private static ContentDeliveryEngineSection _deliveryEngine;

        public static PathInformationElement PathInformation
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PathInformation;
            }
        }

        public static PageAssemblyElement PageAssembly
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PageAssembly;
            }
        }

        public static FileInstructionElement FileInstruction
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.FileInstruction;
            }
        }

        public static PageTitleInformationElement PageTitle
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PageTitle;
            }
        }

        public static CanonicalHostNameInformationElement CanonicalHostName
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.CanonicalHostName;
            }
        }

        public static DefaultHomePageElement DefaultHomePage
        {
            get
            {
                return ContentDeliveryEngine.DefaultHomePage;
            }
        }

        //Question: Should this still be public???
        public static ContentDeliveryEngineSection ContentDeliveryEngine
        {
            get
            {
                if (_deliveryEngine == null)
                {
                    _deliveryEngine = (ContentDeliveryEngineSection)ConfigurationManager.GetSection(_deliveryEnginePath);

                    // When the delivery sectionGroup isn't defined, _delivery comes back null.
                    if (_deliveryEngine == null)
                    {
                        throw new ConfigurationErrorsException(String.Format(
                            "Could not load delivery engine configuration from {0}.  Please ensure the nci/web/cde section group and cde element have been defined in the configuration file.", _deliveryEnginePath));
                    }
                }

                return _deliveryEngine;
            }
        }
    }
}
