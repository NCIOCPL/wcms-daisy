using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SerializeImageUploads
{
    public class InputInfo
    {
        /// <summary>
        /// Migration ID.
        /// </summary>
        [XmlAttribute()]
        public Guid mig_id { get; set; }

        [XmlAttribute()]
        public string community { get; set; }

        /// <summary>
        /// File description. (long_title)
        /// </summary>
        [XmlAttribute()]
        public string title { get; set; }

        /// <summary>
        /// Where should the file be stored in Percussion.
        /// </summary>
        [XmlAttribute()]
        public string folder { get; set; }

        /// <summary>
        /// What File is this for?  (May include path)
        /// </summary>
        [XmlAttribute()]
        public string file { get; set; }
    }
}