using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object representing a translation relationship
    /// between two content items.
    /// </summary>
    public class TranslationDescription : MigrationData
    {
        public String EnglishIdentifier { get; set; }
        public String SpanishIdentifier { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<EnglishIdentifier value=""{0}""/><SpanishIdentifier value=""{1}""/>";

                return string.Format(fmt,
                    EnglishIdentifier,
                    SpanishIdentifier);
            }
        }
    }
}
