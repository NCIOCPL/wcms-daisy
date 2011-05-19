using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MigrationEngine.Configuration
{
    /// <summary>
    /// Configuration element to specify a community in a community list.
    /// This corresponds to the innermost node in the list
    ///   <CommunityLookup>
    ///     <Community>
    ///       <add key="site" name="CancerGov_Members" />
    ///       <add key="siteAdmin" name="CancGovConfig_Members" />
    ///       <add key="ctbAdmin" name="ctbAdmin_Members" />
    ///     </Community>
    ///   </CommunityLookup>
    /// </summary>
    public class CommunityElement : ConfigurationElement
    {
        /// <summary>
        /// The key for looking up a community.
        /// </summary>
        /// <value>The key.</value>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Specifies a community name
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
