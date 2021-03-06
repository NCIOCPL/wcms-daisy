﻿using System;
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
    /// Migration task for updating CMS content.  This class is not intended to be
    /// instantiated directly. It is created by the deserialization
    /// process in Migrator.Run().
    /// </summary>
    public class ContentUpdater : ContentUpdaterBase
    {

        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<UpdateContentItem> DataGetter;

        public override void Doit(IMigrationLog logger)
        {

            List<UpdateContentItem> contentItems = DataGetter.LoadData();

            int index = 1;
            int count = contentItems.Count;

            string community = LookupCommunityName(Constants.Aliases.SITE);

            using (CMSController controller = new CMSController(community))
            {
                foreach (UpdateContentItem item in contentItems)
                {

                    logger.BeginTaskItem(Name, index++, count, item, "");

                    try
                    {
                        string message = "";
                        PercussionGuid percID = new PercussionGuid(LookupMoniker(item.UniqueIdentifier, controller).ContentID);
                        if (percID == PercWrapper.ContentItemNotFound)
                        {
                            logger.LogTaskItemWarning(item, "Content Item Not Found", item.Fields);
                            continue;
                        }
                        else if (percID == PercWrapper.TooManyContentItemsFound)
                        {
                            logger.LogTaskItemWarning(item, "Too Many Content Items Found", item.Fields);
                            continue;

                        }
                        else if (percID == PercWrapper.CmsErrorOccured)
                        {
                            logger.LogError(Name, message, item, item.Fields);
                            continue;
                        }

                        //convert HTML to XML and do Link Munging on fields 
                        Dictionary<string, string> fields = PreProcessFields(item, controller, logger);

                        PercWrapper.UpdateItemWrapper(controller, percID, new FieldSet(fields), out message);
                        if (!string.IsNullOrEmpty(message))
                        {
                            logger.LogTaskItemWarning(item, message, item.Fields);
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
