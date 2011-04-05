using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace NCI.Logging.Configuration
{
    public class LoggingSinksCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggingSinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }

        public LoggingSinkElement this[int index]
        {
            get { return (LoggingSinkElement)BaseGet(index); }

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
            return elementName == "loggingSink";
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }
    }
}
