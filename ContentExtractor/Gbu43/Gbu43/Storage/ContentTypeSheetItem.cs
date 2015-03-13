using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbu43.Storage
{
    public class ContentTypeSheetItem: Dictionary<string, string>
    {
        private List<RelationshipSheetItem> _slotItems = new List<RelationshipSheetItem>();

        /// <summary>
        /// Gets and internally sets the content id for this sheet item
        /// </summary>
        public long ContentID { get; internal set; }

        /// <summary>
        /// Gets and internally sets the percussion folder path for this sheet item relative to the site root.  (e.g. /foo and not //Sites/CancerGov/foo)
        /// </summary>
        public string Folder { get; internal set; }

        /// <summary>
        /// Gets and internally sets the site, site_config, ctb_config Community name for this sheet item.  NOTE: This should NOT be the actual community name
        /// </summary>
        public string Community { get; internal set; }

        /// <summary>
        /// Gets the Migration ID for this item.  This is based on the contentid
        /// </summary>
        public string MigID
        {
            get
            {
                if (HandleAsUpdate)
                    return ContentID.ToString();
                else
                    return "Extracted_" + ContentID;
            }
        }

        /// <summary>
        /// Gets and internally sets the content type name for this item
        /// </summary>
        public string ContentType { get; internal set; }

        /// <summary>
        /// Gets an array of RelationshipSheetItems where this sheet item is the Owner in an Active Assembly Relationship (i.e. slot)
        /// </summary>
        public RelationshipSheetItem[] SlotItems { get { return _slotItems.ToArray(); } }
        
        /// <summary>
        /// Processing flag for saving
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Gets and internally sets whether or not this item should be stored ONLY if it is a dependent in an AA Relationship.  
        /// This is based on the extraction configuration for the content type
        /// </summary>
        public bool OnlyStoreIfDependent { get; internal set; }

        /// <summary>
        /// Gets and internally sets whether or not this item should be stored ONLY if it is a dependent in an AA Relationship.  
        /// This is based on the extraction configuration for the content type
        /// </summary>
        private bool HandleAsUpdate { get; set; }

        public ContentTypeSheetItem(long contentID, bool onlyStoreIfDependent, bool handleAsUpdate)
        {
            ContentID = contentID;
            OnlyStoreIfDependent = onlyStoreIfDependent;
            HandleAsUpdate = handleAsUpdate;
        }

        public void AddSlotItem(RelationshipSheetItem item)
        {
            _slotItems.Add(item);
        }

        public void RemoveSlotItem(RelationshipSheetItem item)
        {
            _slotItems.Remove(item);
        }
    }
}
