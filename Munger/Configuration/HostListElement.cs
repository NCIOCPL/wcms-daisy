using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Munger.Configuration
{
    /// <summary>
    /// Provides a list of host name aliases for a single web site.
    /// e.g. www.cancer.gov, cancer.gov, www.nci.nih.gov
    /// </summary>
    public class HostListElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HostElement)element).Name;
        }

        public HostElement this[int index]
        {
            get
            {
                return (HostElement)BaseGet(index);
            }
        }
    }
}
