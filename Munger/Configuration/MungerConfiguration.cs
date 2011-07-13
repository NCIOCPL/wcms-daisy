using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Munger.Configuration
{
    public class MungerConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Exposes a list of host names which are valid for accessing the site
        /// which is being migrated.
        /// </summary>
        /// <value>The host list.</value>
        [ConfigurationProperty("HostList", IsRequired = true)]
        [ConfigurationCollection(typeof(HostElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HostListElement HostList
        {
            get { return (HostListElement)base["HostList"]; }
        }

        /// <summary>
        /// Exposes a list of paths which are to be subsituted for older,
        /// no-longer-used paths.  Exact matches only.
        /// </summary>
        /// <value>The rewrite list.</value>
        [ConfigurationProperty("Substitute", IsRequired = true)]
        [ConfigurationCollection(typeof(HostElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public RewritingListElement SubstitutionList
        {
            get { return (RewritingListElement)base["Substitute"]; }
        }

        /// <summary>
        /// Exposes a list of programmatic links which are to be subsituted for older,
        /// no-longer-used paths.  Exact matches only.
        /// </summary>
        /// <value>The rewrite list.</value>
        [ConfigurationProperty("Programmatic", IsRequired = true)]
        [ConfigurationCollection(typeof(HostElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public RewritingListElement ProgrammaticLinkList
        {
            get { return (RewritingListElement)base["Programmatic"]; }
        }

        /// <summary>
        /// Exposes the value in /MungerConfig/rootElementPath. This path is substituted
        /// for / when looking up migration IDs.
        /// </summary>
        [ConfigurationProperty("rootElementPath")]
        public SingleValueElement RootElementPath
        {
            get { return (SingleValueElement)base["rootElementPath"]; }
        }
    }
}
