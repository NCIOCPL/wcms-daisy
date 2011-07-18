using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls.Configuration
{
    public class AddThisServiceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("addThisConfigs")]
        public AddThisConfigElementCollection Configs
        {
            get { return (AddThisConfigElementCollection)base["addThisConfigs"]; }
        }

    }
}
