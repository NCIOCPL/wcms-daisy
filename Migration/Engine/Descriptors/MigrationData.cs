using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Base class 
    /// </summary>
    public abstract class MigrationData : IMigrationData
    {
        /// <summary>
        /// Storage for migration object data fields to be stored in Percussion.
        /// </summary>
        /// <value>The fields.</value>
        /// <remarks>This property is not serialized.</remarks>
        [XmlIgnore()]
        public Dictionary<string, string> Fields { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MigrationData()
        {
            Fields = new Dictionary<string, string>();
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToXmlString()
        {
            string xml;
            string fieldString = string.Empty;

            foreach (string key in Fields.Keys)
            {
                fieldString += string.Format("\t\t<{0}><![CDATA[{1}]]></{0}>\n", key, Fields[key]);
            }
            xml = string.Format("<{0}>\n\t<properties>\n{1}\t</properties>\n\t<fields>\n{2}\t</fields>\n</{0}>\n", this.GetType().Name, PropertyString, fieldString);

            return xml;
        }

        protected abstract string PropertyString { get; }
    }
}
