using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MigrationEngine.Configuration
{
    /// <summary>
    /// Contains a list of community key mappings.  Corresponds
    /// to the Community element in:
    /// 
    ///   <CommunityLookup>
    ///     <Community>
    ///       <add key="site" name="CancerGov_Members" />
    ///       <add key="siteAdmin" name="CancGovConfig_Members" />
    ///       <add key="ctbAdmin" name="ctbAdmin_Members" />
    ///     </Community>
    ///   </CommunityLookup>
    ///   
    /// </summary>
    public class CommunityListElement : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates the new object when the underlying ConfigurationElementCollection
        /// object adds a new element.
        /// </summary>
        /// <returns>
        /// A new ConfigurationElement.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommunityElement();
        }

        /// <summary>
        /// Returns the value to be used as the unique key for looking up an element.
        /// </summary>
        /// <param name="element">The CommunityListElement object to return the key for.</param>
        /// <returns>
        /// An Object that acts as the key for the specified CommunityListElement.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommunityElement)element).Key;
        }

        /// <summary>
        /// Looks up a CommunityElement object based on its unique key. 
        /// </summary>
        /// <value></value>
        public CommunityElement this[string key]
        {
            get
            {
                return (CommunityElement)BaseGet(key);
            }
        }
    }
}
