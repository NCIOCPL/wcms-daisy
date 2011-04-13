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
    public class GeneralContentCreator : ContentCreatorBase
    {
        public DataGetter<FullItemDescription> DataGetter;

        override public void Doit(IMigrationLog logger)
        {
            using (CMSController controller = new CMSController())
            {

                List<FullItemDescription> contentItems = DataGetter.LoadData();

                // TODO: Actual task code goes here.
                //Console.WriteLine("Creating {0} content items.", contentItems.Count);
                //contentItems.ForEach(item => Console.WriteLine("Migration ID: {0}", item.MigrationID));


                int index = 1;
                int count = contentItems.Count;

                foreach (FullItemDescription item in contentItems)
                {
                    logger.BeginTaskItem(Name, index++, count, item.MigrationID, item.Path);

                    try
                    {
                        //Console.WriteLine("Migration ID: {0}", item.MigrationID); 

                        //convert HTML to XML
                        Dictionary<string, string> rectifiedFields = 
                            FieldHtmlRectifier.Doit(item.MigrationID, item.Fields, logger);



                        //string message;
                        //long contentID = PercWrapper.CreateItemWrapper(controller, item.ContentType, item.Fields, item.Path, out message);
                        //if (!string.IsNullOrEmpty(message))
                        //{
                        //    logger.LogTaskItemWarning(message, item.Fields);
                        //}
                        if (index > 1)
                            break;

                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        logger.LogTaskItemError(message, item.Fields);
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
