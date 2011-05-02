using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    [ConfigurationCollection(typeof(FileInstructionTypeElement),
        AddItemName = "fileInstructionType",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class FileInstructionTypeElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("enableValidation", IsRequired = false, DefaultValue = false)]
        public bool EnableValidation
        {
            get
            {
                return (bool)base["enableValidation"];
            }
        }

        [ConfigurationProperty("xsdPath", IsRequired = false)]
        public string XsdPath
        {
            get
            {
                return (string)base["xsdPath"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileInstructionTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileInstructionTypeElement)element).Name;
        }
    }
}
