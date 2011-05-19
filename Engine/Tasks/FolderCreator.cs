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
    /// Migration task for creating folders and setting Navon fields.
    /// This class is not intended to be instantiated directly. It is
    /// created by the deserialization process in Migrator.Run().
    /// </summary>
    public class FolderCreator : FolderCreatorBase
    {

        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<FolderDescription> DataGetter;

        override public void Doit(IMigrationLog logger)
        {
            List<FolderDescription> folders = DataGetter.LoadData();

            int index = 1;
            int count = folders.Count;

            string community = LookupCommunityName("site");

            using (CMSController controller = new CMSController(community))
            {
                foreach (FolderDescription folder in folders)
                {
                    logger.BeginTaskItem(Name, index++, count, folder, folder.Path);

                    try
                    {

                        controller.GuaranteeFolder(folder.Path);

                        // Find the Navon
                        string message;
                        PercussionGuid navonID;

                        if (folder.Path != "/")
                            navonID = PercWrapper.GetNavon(controller, folder.Path, out message);
                        else
                            navonID = PercWrapper.GetNavTree(controller, out message);


                        if (navonID == PercWrapper.ContentItemNotFound ||
                            navonID == PercWrapper.TooManyContentItemsFound)
                        {
                            logger.LogTaskItemWarning(folder, message, folder.Fields);
                            continue;
                        }
                        else if (navonID == PercWrapper.CmsErrorOccured)
                        {
                            logger.LogError(Name, message, folder, folder.Fields);
                            continue;
                        }

                        PercWrapper.UpdateItemWrapper(controller, navonID, new FieldSet(folder.Fields), out message);

                        if (!string.IsNullOrEmpty(message))
                        {
                            logger.LogTaskItemWarning(folder, message, folder.Fields);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(folder, message, folder.Fields);
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