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
    public class RelationshipCreator : RelationshipCreatorBase
    {
        public DataGetter<RelationshipDescription> DataGetter;

        public override void Doit(IMigrationLog logger)
        {
            using (CMSController controller = new CMSController())
            {
                List<RelationshipDescription> relationships = DataGetter.LoadData();

                int index = 1;
                int count = relationships.Count;

                foreach (RelationshipDescription relation in relationships)
                {
                    logger.BeginTaskItem(Name, index++, count, relation.OwnerMigrationID, null);

                    try
                    {
                        bool error = false;
                        string message = string.Empty; ;

                        PercussionGuid ownerItem =
                            PercWrapper.GetPercussionIDFromMigID(controller, relation.OwnerMigrationID, relation.OwnerContentType);
                        PercussionGuid dependentItem =
                            PercWrapper.GetPercussionIDFromMigID(controller, relation.DependentMigrationID, relation.DependentContentType);

                        if (ownerItem == PercWrapper.ContentItemNotFound)
                        {
                            message = string.Format("No content item found for relationship owner with migid = {{{0}}}.", relation.OwnerMigrationID);
                            error = true;
                        }
                        else if (ownerItem == PercWrapper.TooManyContentItemsFound)
                        {
                            message = string.Format("Multiple content items found for relationship owner with migid = {{{0}}}.", relation.OwnerMigrationID);
                            error = true;
                        }

                        if (dependentItem == PercWrapper.ContentItemNotFound)
                        {
                            message = string.Format("No content item found for relationship dependent with migid = {{{0}}}.", relation.DependentMigrationID);
                            error = true;
                        }
                        else if (dependentItem == PercWrapper.TooManyContentItemsFound)
                        {
                            message = string.Format("Multiple content items found for dependent owner with migid = {{{0}}}.", relation.DependentMigrationID);
                            error = true;
                        }

                        if (error)
                        {
                            logger.LogTaskItemError(relation.OwnerMigrationID, message, relation.Fields);
                        }


                        // Create Relationship.

                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(relation.OwnerMigrationID, message, null);
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
