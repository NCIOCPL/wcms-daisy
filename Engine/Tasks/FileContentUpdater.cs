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

            string community = LookupCommunityName("site");

            using (CMSController controller = new CMSController(community))
            {
                foreach (UpdateFileItem item in contentItems)
                {
                    logger.BeginTaskItem(Name, index++, count, item, "");

                    try
                    {
                        NciFileInfo fileInfo = NciFileInfo.DownloadImage(hostSetting, item.OriginalUrl);
                        NciFile nciFile = new NciFile(fileInfo);

                        long rawID =
                            controller.CreateItem(NciFile.ContentType, nciFile.FieldSet, null, fileInfo.Path, null);
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
