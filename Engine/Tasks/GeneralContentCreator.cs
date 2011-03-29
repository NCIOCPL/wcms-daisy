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
        public DataGetter<FullContentItemDescription> DataGetter = new XmlDataGetter<FullContentItemDescription>();

        override public void Doit()
        {
        }
    }
}
