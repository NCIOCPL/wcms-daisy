using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Utilities;

using FileManipulation;
using MigrationEngine.Configuration;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Migration task for updating CMS content.  This class is not intended to be
    /// instantiated directly. It is created by the deserialization
    /// process in Migrator.Run().
    /// </summary>
    public class FileContentUpdater : ContentUpdaterBase
    {

        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<UpdateFileItem> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            MigrationEngineSection config = (MigrationEngineSection)ConfigurationManager.GetSection("MigrationEngine");
            string hostSetting = config.SiteHostName.Value;

            List<UpdateFileItem> contentItems = DataGetter.LoadData();

            int index = 1;
            int count = contentItems.Count;

            string community = LookupCommunityName(Constants.Aliases.SITE);

            using (CMSController controller = new CMSController(community))
            {
                foreach (UpdateFileItem item in contentItems)
                {
                    logger.BeginTaskItem(Name, index++, count, item, "");

                    try
                    {
                        // Download the file for transfer.
                        NciFileInfo fileInfo = NciFileInfo.DownloadImage(hostSetting, item.OriginalUrl);
                        NciFile nciFile = new NciFile(fileInfo);

                        // Look up the content item.
                        //PercussionGuid percID = PercWrapper.GetPercussionIDFromMigID(controller, item.UniqueIdentifier, item.ContentType);
                        PercussionGuid percID = new PercussionGuid(LookupMoniker(item.UniqueIdentifier, controller).ContentID);
                        string message = "";
                        if (percID == PercWrapper.ContentItemNotFound)
                        {
                            logger.LogTaskItemWarning(item, "Content item not found", item.Fields);
                            continue;
                        }
                        else if (percID == PercWrapper.TooManyContentItemsFound)
                        {
                            logger.LogTaskItemWarning(item, "Too many matching content items found", item.Fields);
                            continue;
                        }
                        else
                        {
                            // Must the right number of items.
                            // Update the copy in the CMS.
                            PercWrapper.UpdateItemWrapper(controller, percID, nciFile.FieldSet, out message);
                            if (!string.IsNullOrEmpty(message))
                            {
                                logger.LogTaskItemWarning(item, message, item.Fields);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(item, message, item.Fields);
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
