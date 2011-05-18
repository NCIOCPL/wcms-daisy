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

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<ContentType value=""{0}""/>";
                return string.Format(fmt, ContentType);
            }
        }
    }
}
