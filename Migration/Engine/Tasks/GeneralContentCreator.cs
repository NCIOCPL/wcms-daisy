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

            // Get the list of communities.
            string[] communities =
                (from comm in contentItems select comm.Community).Distinct().ToArray();

            // Sort the content items according to their communities.
            Dictionary<string, List<FullItemDescription>> itemsByCommunity = new Dictionary<string, List<FullItemDescription>>();
            foreach (string comm in communities)
            {
                itemsByCommunity.Add(comm, new List<FullItemDescription>());
                itemsByCommunity[comm].AddRange(
                        from item in contentItems
                        where item.Community== comm
                        select item
                    );
            }

            int index = 1;
            int count = contentItems.Count;

            // Loop through the sets of communities.
            foreach (KeyValuePair<string, List<FullItemDescription>> itemGroup in itemsByCommunity)
            {
                // Log in to the community for each group of content items.
                string community = string.Empty;
                if (count > 0)
                {
                    community = LookupCommunityName(itemGroup.Key);
                }

                using (CMSController controller = new CMSController(community))
                {
                    foreach (FullItemDescription item in itemGroup.Value)
                    {
                        logger.BeginTaskItem(Name, index++, count, item, item.Path);

                        try
                        {
                            Dictionary<string, string> fields = PreProcessFields(item, controller, logger);

                            string message;
                            long contentID = PercWrapper.CreateItemWrapper(controller, item.ContentType, fields, item.Path, out message);
                            if (!string.IsNullOrEmpty(message))
                            {
                                logger.LogTaskItemWarning(item, message, item.Fields);
                            }

                            MonikerStore.Add(item.UniqueIdentifier, contentID, item.ContentType);
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
}
