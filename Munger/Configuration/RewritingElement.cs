using System.Configuration;

namespace Munger.Configuration
{
    /// <summary>
    /// Common element for path substitutions.
    /// </summary>
    public class RewritingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the old path.
        /// </summary>
        /// <value>The old path.</value>
        [ConfigurationProperty("old", IsRequired = true)]
        public string OldPath
        {
            get { return (string)this["old"]; }
            set { this["old"] = value; }
        }

        /// <summary>
        /// Gets or sets the new path.
        /// </summary>
        /// <value>The new path.</value>
        [ConfigurationProperty("new", IsRequired = true)]
        public string NewPath
        {
            get { return (string)this["new"]; }
            set { this["new"] = value; }
        }
    }
}
