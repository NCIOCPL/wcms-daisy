using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;

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
                    logger.IncrementTaskProgress(Name, index++, count, folder.MigrationdID, folder.Path);

                    // Do some stuff.
                }
            }
        }
    }
}