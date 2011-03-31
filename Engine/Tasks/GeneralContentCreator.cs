using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;
using MigrationEngine.DataAccess;

namespace MigrationEngine.Tasks
{
    public class GeneralContentCreator : ContentCreatorBase
    {
        public DataGetter<FullContentItemDescription> DataGetter;

        override public void Doit()
        {
            List<FullContentItemDescription> contentItems = DataGetter.LoadData();

            // TODO: Actual task code goes here.
            Console.WriteLine("Creating {0} content items.", contentItems.Count);
            contentItems.ForEach(item => Console.WriteLine("Migration ID: {0}", item.MigrationID));
        }
    }
}
