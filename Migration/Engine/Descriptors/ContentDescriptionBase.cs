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

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<MigrationID value=""{0}""/><ContentType value=""{1}""/>";

                return string.Format(fmt, MigrationID, ContentType);
            }
        }
    }
}