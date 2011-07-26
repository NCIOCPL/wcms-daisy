using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.UI.WebControls.Configuration
{
    [ConfigurationCollection(typeof(AddThisConfigElement),
        AddItemName = "addThisConfig",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AddThisConfigElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultLang", IsRequired = true)]
        public string DefaultLanguage
        {
            get
            {
                return (string)base["defaultLang"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AddThisConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AddThisConfigElement)element).Language;
        }
    }
}
