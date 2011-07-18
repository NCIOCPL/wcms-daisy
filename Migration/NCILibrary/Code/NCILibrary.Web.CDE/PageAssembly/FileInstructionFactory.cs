using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NCI.Web.CDE.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    public class FileInstructionFactory
    {
        /// <summary>
        /// The singleton instance of the factory.
        /// </summary>
        private static FileInstructionFactory _instance;

        /// <summary>
        /// An object instance to use for locking access to _instance.
        /// </summary>
        private static object _syncObject = new object();

        /// <summary>
        /// Holds a mapping of all available XmlSerializers by name.
        /// </summary>
        private Dictionary<string, XmlSerializer> _serializers = new Dictionary<string, XmlSerializer>();

        /// <summary>
        /// Creates an IFileInstruction derived instance from the XML at the specified path.        /// 
        /// 
        /// </summary>
        public static IFileInstruction GetFileInstruction(string requestedPath)
        {
            bool isFileInstructionXmlValid = true;

            string xmlFilePath = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.FilePathFormat.Path, requestedPath));
            // Input validation.
            if (xmlFilePath == null)
            {
                throw new ArgumentNullException("pathToXml", "The pathToXml parameter cannot be null.");
            }

            if (!File.Exists(xmlFilePath))
                return null;

            if (ContentDeliveryEngineConfig.FileInstruction.FileInstructionTypes.EnableValidation == true)
            {
                isFileInstructionXmlValid = FileInstructionFactory.ValidateXml(xmlFilePath,
                    HttpContext.Current.Server.MapPath(ContentDeliveryEngineConfig.FileInstruction.FileInstructionTypes.XsdPath));
            }

            if (isFileInstructionXmlValid == false)
            {
                return null;
            }

            // Load the XML file into an XmlReader and create a IFileInstruction derived instance from the XML.
            IFileInstruction fileInstruction = null;

            try
            {
                using (FileStream xmlFile = File.Open(xmlFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {

                        // Read just enough to get the file instruction type we want to create 
                        // e.g. a GenericFileInstruction, etc.
                        xmlReader.MoveToContent();

                        // Get the name of the serializer type we need (the document element name is the 
                        // name of the type).
                        string fileInstructionTypeName = xmlReader.LocalName;

                        // Make sure the serializer we need is in the cache (if it isn't, we've got invalid XML).
                        if (Instance._serializers.ContainsKey(fileInstructionTypeName) == false)
                        {
                            string message = String.Format("Unable to find XmlSerializer for type \"{0}.\"  Either \"{0}\" is an unsupported type and the XML file at \"{1}\" is invalid or the file instruction configuration needs a new entry to support \"{0}\"", fileInstructionTypeName, xmlFilePath);
                            throw new FileInstructionException(message);
                        }

                        // Get the serializer from the cache.
                        XmlSerializer serializer = Instance._serializers[fileInstructionTypeName];


                        // Deserialize the XML into an object.
                        fileInstruction = (IFileInstruction)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileInstructionException(String.Format("Unable to create IFileInstruction for file \"{0}.\"", xmlFilePath), ex);
            }

            return fileInstruction;
        }

        /// <summary>
        /// Validates the file instruction XML content against the xsd.
        /// </summary>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="XsdPath">The XSD path.</param>
        /// <returns></returns>
        public static bool ValidateXml(string xmlPath, string XsdPath)
        {
            bool isFileInstructionXmlValid = false;
            string errorMessage = string.Empty;

            XmlReader xsd = null;
            try
            {
                using (FileStream xmlFile = File.Open(xmlPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    errorMessage = string.Empty;
                    xsd = new XmlTextReader(XsdPath);

                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add("http://www.example.org/CDESchema", xsd);

                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ValidationType = ValidationType.Schema;
                    xmlReaderSettings.Schemas.Add(schema);
                    xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler((sender, args) => {
                        errorMessage = errorMessage + args.Message + "\r\n";
                    });

                    XmlTextReader xmlTextReader = new XmlTextReader(xmlFile);
                    XmlReader xmlReader = XmlReader.Create(xmlTextReader, xmlReaderSettings);

                    while (xmlReader.Read()) ;
                    xmlReader.Close();
                }
                //Only set this to true if we made it past the validation.
                isFileInstructionXmlValid = true;
            }
            catch (Exception ex)
            {
                isFileInstructionXmlValid = false;
                errorMessage = ex.ToString();
            }
            finally
            {
                if (xsd != null)
                    ((IDisposable)xsd).Dispose();
            }

            if (errorMessage != string.Empty)
            {
                isFileInstructionXmlValid = false;
                Exception ex = new Exception(errorMessage);
                // Write exception massage to log
                throw new PageAssemblyException(String.Format("XML File, {0}, failed the validation against the schema(xsd file).", xmlPath), ex);
            }

            return isFileInstructionXmlValid;
        }

        /// <summary>
        /// Provides access to the singleton instance, lazy initializing the markup extensions 
        /// when first accessed.
        /// </summary>
        private static FileInstructionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new FileInstructionFactory();
                            _instance.Initialize();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Loads XmlSerializers for all configured FileInstruction types.
        /// </summary>
        private void Initialize()
        {
            // Load the file instruction collection can be loaded from configuration.  Note: we are 
            // not checking that this loads since even if there is no configuration section at all 
            // this works fine - we just get an empty collection of file instruction types.
            FileInstructionTypeElementCollection fileInstructionTypes = ContentDeliveryEngineConfig.FileInstruction.FileInstructionTypes;

            // Make sure at least one file instruction type was configured.
            if (fileInstructionTypes.Count == 0)
            {
                throw new FileInstructionException("No FileInstructionTypes were found in the configuration.");
            }

            // Add an XmlSerializer to the serializers cache for each configured file instruction.
            foreach (FileInstructionTypeElement fileInstructionType in fileInstructionTypes)
            {
                // Note: fileInstructionType.Name and fileInstructionType.Type are required 
                // properties so we can safely assume they will not be null.  If they are left out 
                // in the config, .NET will throw an exception before this code is ever reached.  

                // Get the .NET type of the FileInstruction we want to create an XmlSerializer for.
                Type type = type = Type.GetType(fileInstructionType.Type);
                if (type == null)
                {
                    throw new FileInstructionException(String.Format("Unable to load type \"{0}.\"", fileInstructionType.Type));
                }

                // Create an XmlSerializer that can be used to deserialize objects of type 
                // fileInstructionType.Type.
                XmlSerializer xmlSerializer = null;
                try
                {
                    xmlSerializer = new XmlSerializer(type);
                }
                catch (Exception ex)
                {
                    throw new FileInstructionException(String.Format("Unable to create XmlSerializer for \"{0}.\"", fileInstructionType.Name), ex);
                }

                // Add the XmlSerializer to the serializers cache.
                try
                {
                    _serializers.Add(fileInstructionType.Name, xmlSerializer);
                }
                catch (Exception ex)
                {
                    throw new FileInstructionException(String.Format("Unable to cache XmlSerializer for \"{0}.\"  This may be due to a duplicate configuration.", fileInstructionType.Name), ex);
                }
            }
        }

    }
}
