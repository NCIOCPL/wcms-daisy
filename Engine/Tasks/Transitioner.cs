using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Migration task for transitioning content items between workflow states.
    /// This class is not intended to be instantiated directly. It is created
    /// by the deserialization process in Migrator.Run().
    /// </summary>
    public class Transitioner : TransitionerBase
    {
        // Maximum number of items to put in a request so we don't kill the server.
        const int MAX_REQUEST_SIZE = 50;

        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<ContentTypeTransitionDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            List<ContentTypeTransitionDescription> transitionTypes = DataGetter.LoadData();

            int index = 1;
            int count = transitionTypes.Count;

            string communityName;

            foreach (ContentTypeTransitionDescription description in transitionTypes)
            {
                communityName = LookupCommunityName(description.Community);

                using (CMSController controller = new CMSController(communityName))
                {
                    logger.BeginTaskItem(Name, index++, count, description, null);

                    try
                    {
                        PercussionGuid communityID = controller.Community[communityName];
                        Dictionary<string, string> criteria = new Dictionary<string, string>();
                        criteria.Add("sys_communityid", communityID.ID.ToString());

                        PercussionGuid[] itemList = controller.SearchForContentItems(description.ContentType, criteria);

                        int itemCount = itemList.Length;

                        for (int i = 0; i < itemCount; i++)
                        {
                            try
                            {
                                string fmt = "Transitioning {0} item {1} of {2}";
                                string message = string.Format(description.ContentType, fmt, i + 1, itemCount);
                                logger.LogTaskItemInfo(message);

                                PercussionGuid[] itemAsArray = new PercussionGuid[] { itemList[i] };
                                controller.CheckInItems(itemAsArray);
                                controller.PerformWorkflowTransition(itemAsArray, description.TriggerName);
                            }
                            catch (Exception ex)
                            {
                                string fmt = "Error while transitioning items {0} of {1}. Error: {{{2}}}";
                                string message = string.Format(fmt, i + 1, itemCount, ex.ToString());

                                logger.LogTaskItemError(description, message, description.Fields);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(description, message, description.Fields);
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
