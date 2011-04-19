using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    public class ContentTypeDescription : MigrationData
    {
        [XmlIgnore()]
        public string ContentType { get; set; }

        public override string ToString()
        {
            string fmt = @"ContentType: {{{0}}};";

            return string.Format(fmt, ContentType);
        }
    }
}
