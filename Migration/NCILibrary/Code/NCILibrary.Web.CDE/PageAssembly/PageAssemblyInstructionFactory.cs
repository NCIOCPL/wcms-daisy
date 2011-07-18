using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.CDE.Configuration;
using System.Web;
using System.IO;
using System.Xml.Schema;
using NCI.Logging;
namespace NCI.Web.CDE
{
    public class PageAssemblyInstructionFactory
    {
        /// <summary>
        /// The singleton instance of the factory.
        /// </summary>
        private static PageAssemblyInstructionFactory _instance;

        /// <summary>
        /// An object instance to use for locking access to _instance.
        /// </summary>
        private static object _syncObject = new object();

        /// <summary>
        /// Holds a mapping of all available XmlSerializers by name.
        /// </summary>
        private Dictionary<string, XmlSerializer> _serializers = new Dictionary<string, XmlSerializer>();


        /// <summary>
        /// Creates an IPageAssemblyInstruction derived instance from the XML at the specified path.        /// 
        /// 
        /// </summary>
        public static IPageAssemblyInstruction GetPageAssemblyInfo(string requestedPath)
        {
            bool pageAssemblyInstructionXmlValid = true;
            string xmlFilePath = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, requestedPath));
            // Input validation.
            if (xmlFilePath == null)
            {
                throw new ArgumentNullException("pathToXml", "The pathToXml parameter cannot be null.");
            }

            if (!File.Exists(xmlFilePath))
                return null;

            if (ContentDeliveryEngineConfig.PageAssembly.PageAssemblyInfoTypes.EnableValidation == true)
            {
                pageAssemblyInstructionXmlValid = PageAssemblyInstructionFactory.ValidateXml(xmlFilePath, 
                    HttpContext.Current.Server.MapPath(ContentDeliveryEngineConfig.PageAssembly.PageAssemblyInfoTypes.XsdPath));
            }

            if (pageAssemblyInstructionXmlValid == false)
            {
                return null;
            }


            // Load the XML file into an XmlReader and create a IPageAssemblyInstruction derived instance from the XML.
            IPageAssemblyInstruction pageAssemblyInfo = null;

            try
            {
                using (FileStream xmlFile = File.Open(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {

                        // Read just enough to get the page type of assembly info object we want to create 
                        // e.g. a SinglePageAssemblyInfo, BookletAssemblyInfo, etc.
                        xmlReader.MoveToContent();

                        // Get the name of hte serializer type we need (the document element name is the 
                        // name of the type).
                        string pageAssemblyInfoTypeName = xmlReader.LocalName;

                        // Make sure the serializer we need is in the cache (if it isn't, we've got invalid XML).
                        if (Instance._serializers.ContainsKey(pageAssemblyInfoTypeName) == false)
                        {
                            string message = String.Format("Unable to find XmlSerializer for type \"{0}.\"  Either \"{0}\" is an unsupported type and the XML file at \"{1}\" is invalid or the page assembly configuration needs a new entry to support \"{0}\"", pageAssemblyInfoTypeName, xmlFilePath);
                            throw new PageAssemblyException(message);
                        }

                        // Get the serializer from the cache.
                        XmlSerializer serializer = Instance._serializers[pageAssemblyInfoTypeName];


                        // Deserialize the XML into an object.
                        pageAssemblyInfo = (IPageAssemblyInstruction)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new PageAssemblyException(String.Format("Unable to create IPageAssemblyInfo for file \"{0}.\"", xmlFilePath), ex);
            }

            return pageAssemblyInfo;
        }

        public static bool IsPageInstructionResourcePresent(string requestedPath)
        {
            string xmlFilePath = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, requestedPath));
            // Input validation.
            if (xmlFilePath == null)
                return false;

            if (!File.Exists(xmlFilePath))
                return false;
            return true;
        }

        /// <summary>
        /// Validates the single page assembly instruction XML content against the xsd.
        /// </summary>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="XsdPath">The XSD path.</param>
        /// <returns></returns>
        public static bool ValidateXml(string xmlPath, string XsdPath)
        {
            XmlReader xsd = null;
            string errorMessage = string.Empty;
            bool pageAssemblyInstructionXmlValid = false;

            try
            {
                using (FileStream xmlFile = File.Open(xmlPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {

                    pageAssemblyInstructionXmlValid = true;
                    xsd = new XmlTextReader(XsdPath);

                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add("http://www.example.org/CDESchema", xsd);

                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ValidationType = ValidationType.Schema;
                    xmlReaderSettings.Schemas.Add(schema);
                    
                    xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler((sender, e) =>
                    {
                        errorMessage = errorMessage + e.Message + "\r\n";
                    });

                    XmlTextReader xmlTextReader = new XmlTextReader(xmlFile);
                    XmlReader xmlReader = XmlReader.Create(xmlTextReader, xmlReaderSettings);

                    while (xmlReader.Read()) ;
                    xmlReader.Close();
                }
            }
            catch (Exception ex)
            {
                pageAssemblyInstructionXmlValid = false;
                errorMessage = ex.ToString();
            }
            finally
            {
                if (xsd != null)
                    ((IDisposable)xsd).Dispose();
            }

            if (errorMessage != string.Empty)
            {
                pageAssemblyInstructionXmlValid = false;
                Exception ex = new Exception(errorMessage);
                // Write exception massage to log
                throw new PageAssemblyException(String.Format("XML File, {0}, failed the validation against the schema(xsd file).", xmlPath), ex);
            }


            return pageAssemblyInstructionXmlValid;
        }

        /// <summary>
        /// Provides access to the singleton instance, lazy initializing the markup extensions 
        /// when first accessed.
        /// </summary>
        private static PageAssemblyInstructionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new PageAssemblyInstructionFactory();
                            _instance.Initialize();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Loads XmlSerializers for all configured PageAssemblyInfo types.
        /// </summary>
        private void Initialize()
        {
            // Load the page assembly collection can be loaded from configuration.  Note: we are 
            // not checking that this loads since even if there is no configuration section at all 
            // this works fine - we just get an empty collection of page assembly info types.
            PageAssemblyInfoTypeElementCollection pageAssemblyInfoTypes = pageAssemblyInfoTypes = ContentDeliveryEngineConfig.PageAssembly.PageAssemblyInfoTypes;

            // Make sure at least one page assembly info type was configured.
            if (pageAssemblyInfoTypes.Count == 0)
            {
                throw new PageAssemblyException("No PageAssemblyInfoTypes were found in the configuration.");
            }

            // Add an XmlSerializer to the serializers cache for each configured page.
            foreach (PageAssemblyInfoTypeElement pageAssemblyInfoType in pageAssemblyInfoTypes)
            {
                // Note: pageAssemblyInfoType.Name and pageAssemblyInfoType.Type are required 
                // properties so we can safely assume they will not be null.  If they are left out 
                // in the config, .NET will throw an exception before this code is ever reached.  

                // Get the .NET type of the PageAssemblyInfo we want to create an XmlSerializer for.
                Type type = type = Type.GetType(pageAssemblyInfoType.Type);
                if (type == null)
                {
                    throw new PageAssemblyException(String.Format("Unable to load type \"{0}.\"", pageAssemblyInfoType.Type));
                }

                // Create an XmlSerializer that can be used to deserialize objects of type 
                // pageAssemblyInfoType.Type.
                XmlSerializer xmlSerializer = null;
                try
                {
                    xmlSerializer = new XmlSerializer(type);
                }
                catch (Exception ex)
                {
                    throw new PageAssemblyException(String.Format("Unable to create XmlSerializer for \"{0}.\"", pageAssemblyInfoType.Name), ex);
                }

                // Add the XmlSerializer to the serializers cache.
                try
                {
                    _serializers.Add(pageAssemblyInfoType.Name, xmlSerializer);
                }
                catch (Exception ex)
                {
                    throw new PageAssemblyException(String.Format("Unable to cache XmlSerializer for \"{0}.\"  This may be due to a duplicate configuration.", pageAssemblyInfoType.Name), ex);
                }
            }
        }

    }
}
