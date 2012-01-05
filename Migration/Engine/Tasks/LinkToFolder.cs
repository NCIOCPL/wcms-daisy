using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using MonikerProviders;
using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Utilities;

namespace MigrationEngine.Tasks
{
    public class LinkToFolder : MigrationTask
    {
        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<FolderLinkDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            List<FolderLinkDescription> folderLinks = DataGetter.LoadData();

            int index = 1;
            int count = folderLinks.Count;

            string community = LookupCommunityName(Constants.Aliases.SITE);

            using (CMSController controller = new CMSController(community))
            {
                foreach (FolderLinkDescription item in folderLinks)
                {
                    logger.BeginTaskItem(Name, index++, count, item, item.Path);

                    try
                    {
                        PercussionGuid itemID = new PercussionGuid(MonikerStore.Get(item.ObjectMonikerName).ContentID);
                        controller.AddFolderChildren(item.Path, new PercussionGuid[] { itemID });
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
