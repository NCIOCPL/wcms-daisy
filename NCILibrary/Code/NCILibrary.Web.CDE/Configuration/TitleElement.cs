using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class TitleElement : ConfigurationElement
    {
        [ConfigurationProperty("title")]
        public string Title
        {
            get { return (string)base["title"]; }
        }
    }
}
