using System;
using System.Configuration;

namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    [ConfigurationCollection(typeof(ReportingSuiteElement),
         AddItemName = "suite",
         CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ReportingSuitesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReportingSuiteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReportingSuiteElement)element).Name;
        }
    }
}
