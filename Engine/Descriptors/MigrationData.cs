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
        public override abstract string ToString();
    }
}
