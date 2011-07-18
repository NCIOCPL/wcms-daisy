using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class PageTitleInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("appendPageTitle")]
        public TitleElement AppendPageTitle
        {
            get { return (TitleElement)base["appendPageTitle"]; }
        }
    }
}
