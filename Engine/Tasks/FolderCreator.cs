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
    public class FolderCreator : FolderCreatorBase
    {
        public DataGetter<FolderDescription> DataGetter;

        override public void Doit(IMigrationLog logger)
        {
            using (CMSController controller = new CMSController())
            {
                List<FolderDescription> folders = DataGetter.LoadData();

                int index = 1;
                int count = folders.Count;

                foreach (FolderDescription folder in folders)
                {
                    logger.BeginTaskItem(Name, index++, count, folder.MigrationdID, folder.Path);

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
                            logger.LogWarning(Name, message, Guid.Empty, folder.Fields);
                            continue;
                        }
                        else if (navonID == PercWrapper.CmsErrorOccured)
                        {
                            logger.LogError(Name, message, Guid.Empty, folder.Fields);
                            continue;
                        }

                        PercWrapper.UpdateItemWrapper(controller, navonID, new FieldSet(folder.Fields), out message);

                        if (!string.IsNullOrEmpty(message))
                        {
                            logger.LogTaskItemWarning(message, folder.Fields);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(message, folder.Fields);
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