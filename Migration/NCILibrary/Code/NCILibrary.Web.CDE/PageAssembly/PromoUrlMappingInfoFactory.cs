using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.IO;

using NCI.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public static class PromoUrlMappingInfoFactory
    {
        // XmlSerializer to deserialization PromoUrlMapping
        private static XmlSerializer _serializer = new XmlSerializer(typeof(PromoUrlMapping));

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        /// <summary>
        /// Gets a single PromoUrlMapping without any parents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static PromoUrlMapping GetPromoUrlMapping(string path, ref FileInfo fileInfo )
        {
            PromoUrlMapping promoUrlMapping = null;
            string xmlFileName = null;

            try
            {
                xmlFileName = String.Format(ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath.Path, (path == "/" ? String.Empty : path));
                xmlFileName = Server.MapPath(xmlFileName);
                FileInfo promoUrlFileInfo = new FileInfo(xmlFileName);
                if (!promoUrlFileInfo.Exists)
                {
                    Logger.LogError("CDE:promoUrlMappingFactory.cs:GetPromoUrlMapping", "PromoUrl Mapping file does not exists", NCIErrorLevel.Warning);
                    return null;
                }
                else
                    fileInfo = promoUrlFileInfo;

                using (FileStream xmlFile = File.Open(xmlFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.IgnoreWhitespace = true;
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile, xmlReaderSettings))
                    {
                        promoUrlMapping = (PromoUrlMapping)_serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("Unable to load section Promo Url mapping from file \"{0}.\"  The file may not exist or the XML in the file may not be serializable into a valid promoUrlMapping object.", xmlFileName);
                Logger.LogError("CDE:promoUrlMappingFactory.cs:GetPromoUrlMapping", message, NCIErrorLevel.Error, ex);
                return null;
            }
            return promoUrlMapping;
        }

    }
}
