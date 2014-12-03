using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class PageAssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("pageAssemblyInfoTypes")]
        public PageAssemblyInfoTypeElementCollection PageAssemblyInfoTypes
        {
            get { return (PageAssemblyInfoTypeElementCollection)base["pageAssemblyInfoTypes"]; }
        }
    }
}
