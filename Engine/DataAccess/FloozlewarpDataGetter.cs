using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.DataAccess
{
    public class FloozlewarpDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        public override ReturnType LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
