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
            using (CMSController controller = new CMSController())
            {

                List<UpdateContentItem> contentItems = DataGetter.LoadData();

                int index = 1;
                int count = contentItems.Count;

                foreach (UpdateContentItem item in contentItems)
                {
                    
                    logger.BeginTaskItem(Name, index++, count, item.MigrationID, "");

                    try
                    {
                        //For Testing 
                        //if (index > 2)
                        //    break;                       // Get Percussion ID

                        string message ="";
                        PercussionGuid precID = PercWrapper.GetPercussionIDFromMigID(controller, item.MigrationID, item.ContentType);
                        if (precID == PercWrapper.ContentItemNotFound)
                        {
                            logger.LogTaskItemWarning(Guid.Empty, "Content Item Not Found", item.Fields);
                            continue;
                        }
                        else if (precID == PercWrapper.TooManyContentItemsFound)
                        {
                            logger.LogTaskItemWarning(Guid.Empty, "Too Many Content Items Found", item.Fields);
                            continue;

                        }
                        else if (precID == PercWrapper.CmsErrorOccured)
                        {
                            logger.LogError(Name, message, Guid.Empty, item.Fields);
                            continue;
                        }
                        
                        //convert HTML to XML and do Link Munging on fields 
                        Dictionary<string, string> rectifiedFields =
                            FieldHtmlRectifier.Doit(item.MigrationID, item.Fields, logger, controller);

                        PercWrapper.UpdateItemWrapper(controller, precID, new FieldSet(rectifiedFields), out message);
                        if (!string.IsNullOrEmpty(message))
                        {
                            logger.LogTaskItemWarning(item.MigrationID, message, item.Fields);
                        }



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
