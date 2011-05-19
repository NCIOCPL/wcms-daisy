using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Utilities;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Migration task for adding content to the CMS.  This class is not intended to be
    /// instantiated directly. It is created by the deserialization
    /// process in Migrator.Run().
    /// </summary>
    public class GeneralContentCreator : ContentCreatorBase
    {
        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<FullItemDescription> DataGetter;

        override public void Doit(IMigrationLog logger)
        {
            List<FullItemDescription> contentItems = DataGetter.LoadData();

            int index = 1;
            int count = contentItems.Count;

            string community = string.Empty;
            if (count > 0)
            {
                community = LookupCommunityName(contentItems[0].Community);
            }

            using (CMSController controller = new CMSController(community))
            {
                foreach (FullItemDescription item in contentItems)
                {
                    logger.BeginTaskItem(Name, index++, count, item.MigrationID, item.Path);

                    try
                    {
                        //convert HTML to XML and do Link Munging on fields
                        Dictionary<string, string> rectifiedFields =
                            FieldHtmlRectifier.Doit(item.MigrationID, item.Fields, logger, controller);

                        string message;
                        long contentID = PercWrapper.CreateItemWrapper(controller, item.ContentType, rectifiedFields, item.Path, out message);
                        if (!string.IsNullOrEmpty(message))
                        {
                            logger.LogTaskItemWarning(item.MigrationID, message, item.Fields);
                        }

                        //For Testing 
                        //if (index > 1)
                        //    break;

                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(item.MigrationID, message, item.Fields);
                    }
                    finally
                    {
                        logger.EndTaskItem();
                    }
                }
            }
        }
    }
}
