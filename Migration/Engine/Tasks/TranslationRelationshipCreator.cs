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
    /// Migration task for creating translation relationships between content items.
    /// This class is not intended to be instantiated directly. It is
    /// created by the deserialization process in Migrator.Run().
    /// </summary>
    public class TranslationRelationshipCreator : RelationshipCreatorBase
    {
        /// <summary>
        /// Contains the task-specific data access object.  This property is not
        /// intended to be instantiated directly. It is created by the deserialization
        /// process in Migrator.Run().
        /// </summary>
        public DataGetter<TranslationDescription> DataGetter;


        public override void Doit(IMigrationLog logger)
        {
            List<TranslationDescription> tranlationList = DataGetter.LoadData();

            int index = 1;
            int count = tranlationList.Count;

            using (CMSController controller = new CMSController())
            {
                foreach (TranslationDescription translation in tranlationList)
                {
                    logger.BeginTaskItem(Name, index++, count, translation, null);

                    try
                    {
                        PercussionGuid englishItem = new PercussionGuid(LookupMoniker(translation.EnglishIdentifier, controller).ContentID);
                        PercussionGuid spanishItem = new PercussionGuid(LookupMoniker(translation.SpanishIdentifier, controller).ContentID);

                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(translation, message, null);
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
