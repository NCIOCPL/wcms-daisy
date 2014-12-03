using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace NCI.Logging.Configuration
{
    public class FacilityMatchStringsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FacilityMatchStringElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }

        public FacilityMatchStringElement this[int index]
        {
            get { return (FacilityMatchStringElement)BaseGet(index); }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName == "facilityMatchString";
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }
    }
}
