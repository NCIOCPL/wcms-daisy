using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbu43.Configuration
{
    public class ContentExtractorConfiguration
    {
        /// <summary>
        /// Gets a list of content types to be extracted
        /// </summary>
        public string[] ExtractContentTypes
        {
            get
            {
                return _config.ContentTypes.Cast<ContentTypeElement>().Select(i => i.Type).ToArray();
            }
        }

        /// <summary>
        /// Checks to see if a content type is one of the types we should extract
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool IsTypeToExtract(string contentType)
        {
            return ExtractContentTypes.Contains(contentType);
        }

        /// <summary>
        /// Gets the Sheetname for a given type, or returns the default.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string GetSheetNameForType(string contentType)
        {
            ContentTypeElement elem = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(item => item.Type == contentType);
            if (elem != null && !string.IsNullOrEmpty(elem.TabName))
            {
                return _config.SheetConfiguration.ContentTabPrefix + elem.TabName;
            }
            else
            {
                //Just return the prefix if tab name is not set.  If the prefix is null or empty, then force it to be a value.
                return String.IsNullOrEmpty(_config.SheetConfiguration.ContentTabPrefix) ? "2 CT " : _config.SheetConfiguration.ContentTabPrefix; 
            }
        }

        /// <summary>
        /// Gets a list of fields to be extracted
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string[] GetFieldsForType(string contentType)
        {
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault();

            if (el == null)
                throw new Exception("The type, " + contentType + " could not be found in the configuration.");

            return el.extractFieldnames.Cast<FieldNameElement>().Select(i => i.Value).ToArray();
        }

        private ContentExtractorConfigurationSection _config;

        public ContentExtractorConfiguration()
        {
            _config = (ContentExtractorConfigurationSection)System.Configuration.ConfigurationManager.GetSection("ContentExtractor");
        }


    }
}
