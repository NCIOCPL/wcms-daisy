using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    abstract public class ContentDescriptionBase : MigrationData
    {
        const string LOCALE_FIELD = "sys_lang";

        public String ContentType { get; set; }
        public String UniqueIdentifier { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<UniqueIdentifier value=""{0}""/><ContentType value=""{1}""/>";

                return string.Format(fmt, UniqueIdentifier, ContentType);
            }
        }

        public string Locale
        {
            get
            {
                if (this.Fields.ContainsKey(LOCALE_FIELD))
                    return this.Fields[LOCALE_FIELD];

                throw new DataFieldException("Locale not set.");
            }
        }
    }
}