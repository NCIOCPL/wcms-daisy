using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using MigrationEngine.Descriptors;

namespace MigrationEngine.Mappers
{
    public class XmlTranslationDescriptionMapper
        : XmlDataMapper<TranslationDescription>
    {
        public override TranslationDescription MapItem(object dataItem)
        {
            if (!(dataItem is XmlNode))
                throw new ArgumentException(string.Format("Parameter dataItem is of type {0}, expected XmlNode."),
                    dataItem.GetType().Name);

            XmlNode item = (XmlNode)dataItem;

            TranslationDescription description = new TranslationDescription();

            try
            {
                description.EnglishIdentifier = GetNamedFieldValue(item, "english_id");
                description.SpanishIdentifier = GetNamedFieldValue(item, "spanish_id");
            }
            finally
            {
                CheckForRecordedErrors();
            }

            return description;
        }
    }
}
