using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object describing a Percussion content type.
    /// </summary>
    public class ContentTypeDescription : MigrationData
    {
        [XmlIgnore()]
        public string ContentType { get; set; }
        public string Community { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<ContentType value=""{0}""/><Community value=""{1}""/>";
                return string.Format(fmt, ContentType, Community);
            }
        }
    }
}
