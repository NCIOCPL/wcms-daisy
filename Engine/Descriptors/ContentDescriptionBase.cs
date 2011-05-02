using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    abstract public class ContentDescriptionBase : MigrationData
    {
        public String ContentType { get; set; }
        public Guid MigrationID { get; set; }

        public override string ToString()
        {
            string fmt = @"MigrationID: {{{0}}}; ContentType: {{{1}}};";

            return string.Format(fmt, MigrationID, ContentType);
        }
    }
}