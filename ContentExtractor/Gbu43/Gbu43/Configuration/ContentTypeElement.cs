using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gbu43.Configuration
{
    public class ContentTypeElement : ConfigurationElement
    {
        /// <summary>
        /// The Name of the Content Type
        /// </summary>
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        /// <summary>
        /// The sheet name within the spreadsheet to add this item
        /// </summary>
        [ConfigurationProperty("tabName")]
        public string TabName
        {
            get { return (string)base["tabName"]; }
            set { base["tabName"] = value; }
        }

        /// <summary>
        /// A comma separated list of Percussion IDs to exclude from extraction
        /// </summary>
        [ConfigurationProperty("excludeIDs")]
        public string ExcludeIDs
        {
            get { return (string)base["excludeIDs"]; }
            set { base["excludeIDs"] = value; }
        }

        /// <summary>
        /// A comma separated list of Percussion IDs to treat as updates rather than new content creation (e.g. MigID is just the contentID)
        /// </summary>
        [ConfigurationProperty("handleAsUpdateIDs")]
        public string HandleAsUpdateIDs
        {
            get { return (string)base["handleAsUpdateIDs"]; }
            set { base["handleAsUpdateIDs"] = value; }
        }

        /// <summary>
        /// Should this content type only be extracted if it is a dependent of a slot relationship
        /// </summary>
        [ConfigurationProperty("onlyIncludeIfDependent", DefaultValue=false)]
        public bool OnlyIncludeIfDependent
        {
            get { return (bool)base["onlyIncludeIfDependent"]; }
            set { base["onlyIncludeIfDependent"] = value; }
        }


        private long[] _excludedIDs = null;

        private void InitExcludedIDs()
        {
            if (_excludedIDs == null)
            {
                List<long> ids = new List<long>();

                if (!string.IsNullOrEmpty(ExcludeIDs))
                {
                    foreach (string part in ExcludeIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        try
                        {
                            long l = long.Parse(part.Trim());
                            ids.Add(l);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Excluded ID, " + part + ", Is not valid");
                        }
                    }
                }

                _excludedIDs = ids.ToArray();
            }
        }

        public bool IsExcludedContentItem(long id)
        {
            InitExcludedIDs();

            return _excludedIDs.Contains(id);
        }

        private long[] _handleAsUpdateIDs = null;

        private void InitHandleAsUpdateIDs()
        {
            if (_handleAsUpdateIDs == null)
            {
                List<long> ids = new List<long>();

                if (!string.IsNullOrEmpty(HandleAsUpdateIDs))
                {
                    foreach (string part in HandleAsUpdateIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        try
                        {
                            long l = long.Parse(part.Trim());
                            ids.Add(l);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Handle As Update ID, " + part + ", Is not valid");
                        }
                    }
                }

                _handleAsUpdateIDs = ids.ToArray();
            }
        }

        public bool HandleAsUpdate(long id)
        {
            InitHandleAsUpdateIDs();

            return _handleAsUpdateIDs.Contains(id);
        }

        [ConfigurationProperty("extractFieldnames", IsDefaultCollection = false)]
        public FieldNameCollection extractFieldnames
        {
            get { return (FieldNameCollection)base["extractFieldnames"]; }
        }

        [ConfigurationProperty("extractSlotItems", IsDefaultCollection = false)]
        public SlotItemCollection ExtractSlotItems
        {
            get { return (SlotItemCollection)base["extractSlotItems"]; }
        }

    }
}
