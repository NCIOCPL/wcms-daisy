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
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(cte => cte.Type == contentType);

            if (el == null)
                throw new Exception("The type, " + contentType + " could not be found in the configuration.");

            return el.extractFieldnames.Cast<FieldNameElement>().Select(i => i.Value).ToArray();
        }

        /// <summary>
        /// Gets a list of slots to be extracted
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public SlotItemElement[] GetSlotsForType(string contentType)
        {
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(cte => cte.Type == contentType);

            if (el == null)
                throw new Exception("The type, " + contentType + " could not be found in the configuration.");

            return el.ExtractSlotItems.Cast<SlotItemElement>().ToArray();
        }

        /// <summary>
        /// Determines if a field should be extracted for a specific type.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool ExtractFieldForType(string contentType, string fieldName)
        {
            return GetFieldsForType(contentType).Contains(fieldName);
        }

        /// <summary>
        /// Determines if a content item should be extracted an update.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool ExtractAsUpdate(string contentType, long contentID)
        {
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(cte => cte.Type == contentType);

            if (contentType == null)
                return false;

            return el.HandleAsUpdate(contentID); 
        }

        /// <summary>
        /// Determines if a slot's contents should be extracted for a specific type.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="slotName"></param>
        /// <returns></returns>
        public SlotItemExtractionType ExtractSlotForType(string contentType, string slotName)
        {
            SlotItemElement sie = GetSlotsForType(contentType).FirstOrDefault(e => e.Value == slotName);
            if (sie == null)
                return SlotItemExtractionType.NoExtract; // If it does not exist, return nothing
            else if (sie.IncludeUnextractedDependent)
                return SlotItemExtractionType.Extract; //Extract no matter what
            else
                return SlotItemExtractionType.ExtractIfExists; //Only extract if mig ID exists
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ShouldExtractItem(string contentType, long id)
        {
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(cte => cte.Type == contentType);

            if (contentType == null)
                return false;

            return !el.IsExcludedContentItem(id); //Return the negation of Is Excluded
        }

        /// <summary>
        /// Determines if this content should only be stored if Dependent in slot relationship
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool OnlyStoreCTIfDependent(string contentType)
        {
            ContentTypeElement el = _config.ContentTypes.Cast<ContentTypeElement>().FirstOrDefault(cte => cte.Type == contentType);

            if (contentType == null)
                return false;

            return el.OnlyIncludeIfDependent; //Return the negation of Is Excluded
        }

        private ContentExtractorConfigurationSection _config;

        public ContentExtractorConfiguration()
        {
            _config = (ContentExtractorConfigurationSection)System.Configuration.ConfigurationManager.GetSection("ContentExtractor");
        }


    }
}
