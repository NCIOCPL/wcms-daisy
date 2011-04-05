using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.CDE.Configuration;
using System.Web;
using System.IO;
using NCI.Logging;
namespace NCI.Web.CDE
{
    public static class SectionDetailFactory
    {
        // XmlSerializer to deserialization SectionDetails
        private static XmlSerializer _serializer = new XmlSerializer(typeof(SectionDetail));

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        /// <summary>
        /// Gets a single SectionDetail without any parents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static SectionDetail GetSectionDetail(string path)
        {
            SectionDetail sectionDetail = null;
            string sectionDetailXmlFileName = null;

            try
            {
                sectionDetailXmlFileName = String.Format(ContentDeliveryEngineConfig.PathInformation.SectionPathFormat.Path, (path == "/" ? String.Empty : path));
                
                sectionDetailXmlFileName = Server.MapPath(sectionDetailXmlFileName);
                using (FileStream xmlFile = File.Open(sectionDetailXmlFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {

                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {
                        sectionDetail = (SectionDetail)_serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("Unable to load section detail from file \"{0}.\"  The file may not exist or the XML in the file may not be serializable into a valid SectionDetail object.", sectionDetailXmlFileName);
                Logger.LogError("CDE:SectionDetailFactory.cs:GetSectionDetail", message, NCIErrorLevel.Error, ex);
                return null;
            }
            return sectionDetail;
        }

    }
}
