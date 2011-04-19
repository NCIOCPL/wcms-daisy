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
    public class PurgeContent : MigrationTask
    {
        // Maximum number of items to put in a request so we don't kill the server.
        const int MAX_REQUEST_SIZE = 100;

        public DataGetter<ContentTypeDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            using (CMSController controller = new CMSController())
            {
                List<ContentTypeDescription> typeList = DataGetter.LoadData();

                int index = 1;
                int count = typeList.Count;

                foreach (ContentTypeDescription contentType in typeList)
                {
                    logger.BeginTaskItem(Name, index++, count, contentType, null);

                    try
                    {
                        PercussionGuid[] itemList = controller.SearchForContentItems(contentType.ContentType, null);


                        int itemCount = itemList.Length;
                        int loopCount = (itemCount / MAX_REQUEST_SIZE);
                        int remainder = itemCount % MAX_REQUEST_SIZE;

                        int subsetSize = 0;
                        int first;

                        for (int i = 0; i <= loopCount; i++)
                        {
                            first = i * MAX_REQUEST_SIZE;

                            try
                            {
                                if (i < loopCount)
                                    subsetSize = MAX_REQUEST_SIZE; // Get an entire batch.
                                else
                                    subsetSize = remainder; // Get the remainder.

                                PercussionGuid[] listSubset = new PercussionGuid[subsetSize];
                                Array.ConstrainedCopy(itemList, (i * MAX_REQUEST_SIZE), listSubset, 0, subsetSize);

                                string fmt = "Deleting items {0} to {1}";
                                string message = string.Format(fmt, first, first + subsetSize - 1);
                                logger.LogTaskItemInfo(message);

                                if (subsetSize > 0)
                                {
                                    controller.CheckInItems(listSubset);
                                    controller.DeleteItemList(listSubset);
                                }
                            }
                            catch (Exception ex)
                            {
                                string fmt = "Error while deleting items {0} to {1}. Error: {{{2}}}";
                                string message = string.Format(fmt, first, first + subsetSize - 1, ex.ToString());

                                logger.LogTaskItemError(contentType, message, contentType.Fields);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(contentType, message, contentType.Fields);
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