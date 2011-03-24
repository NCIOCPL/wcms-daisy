using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.DataAccess
{
    public interface IDataGetter<ReturnType> where ReturnType : MigrationData
    {
        ReturnType LoadData();
    }
}