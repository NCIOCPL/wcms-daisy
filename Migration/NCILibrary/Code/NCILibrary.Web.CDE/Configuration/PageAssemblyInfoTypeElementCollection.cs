using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    [ConfigurationCollection(typeof(PageAssemblyInfoTypeElement),
        AddItemName = "pageAssemblyInfoType",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class PageAssemblyInfoTypeElementCollection : ConfigurationElementCollection
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
            return new PageAssemblyInfoTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageAssemblyInfoTypeElement)element).Name;
        }
    }
}
