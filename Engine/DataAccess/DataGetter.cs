using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.DataAccess
{
    public abstract class DataGetter<ReturnType> where ReturnType : MigrationData
    {
        public abstract List<ReturnType> LoadData();
    }
}