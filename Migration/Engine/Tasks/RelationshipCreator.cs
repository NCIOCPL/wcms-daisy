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
    /// Migration task for creating relationships between content items.
    /// This class is not intended to be instantiated directly. It is
    /// created by the deserialization process in Migrator.Run().
    /// </summary>
    public class RelationshipCreator : RelationshipCreatorBase
    {
        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<RelationshipDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            List<RelationshipDescription> relationships = DataGetter.LoadData();

            int index = 1;
            int count = relationships.Count;

            string community = LookupCommunityName(Constants.Aliases.SITE);

            using (CMSController controller = new CMSController())
            {
                foreach (RelationshipDescription relation in relationships)
                {
                    logger.BeginTaskItem(Name, index++, count, relation, null);

                    try
                    {
                        bool error = false;
                        string message = string.Empty; ;

                        PercussionGuid ownerItem = new PercussionGuid(LookupMoniker(relation.OwnerUniqueIdentifier, controller).ContentID);
                        PercussionGuid dependentItem = new PercussionGuid(LookupMoniker(relation.DependentUniqueIdentifier, controller).ContentID);

                        if (ownerItem == PercWrapper.ContentItemNotFound)
                        {
                            message = string.Format("No content item found for relationship owner with migid = {{{0}}}.", relation.OwnerUniqueIdentifier);
                            error = true;
                        }
                        else if (ownerItem == PercWrapper.TooManyContentItemsFound)
                        {
                            message = string.Format("Multiple content items found for relationship owner with migid = {{{0}}}.", relation.OwnerUniqueIdentifier);
                            error = true;
                        }

                        if (dependentItem == PercWrapper.ContentItemNotFound)
                        {
                            message = string.Format("No content item found for relationship dependent with migid = {{{0}}}.", relation.DependentUniqueIdentifier);
                            logger.LogTaskItemError(relation, message, relation.Fields);
                            error = true;
                        }
                        else if (dependentItem == PercWrapper.TooManyContentItemsFound)
                        {
                            message = string.Format("Multiple content items found for relationship dependent with migid = {{{0}}}.", relation.DependentUniqueIdentifier);
                            logger.LogTaskItemError(relation, message, relation.Fields);
                            error = true;
                        }

                        // Don't bother trying to create a relationship if there's already an error.
                        if (error)
                        {
                            logger.LogTaskItemError(relation, message, null);
                            continue;
                        }


                        // Create Relationship.
                        PercWrapper.CreateSingleRelationshipWrapper(controller, ownerItem, dependentItem, relation.SlotName, relation.TemplateName);

                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(relation, message, null);
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
