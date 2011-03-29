using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Mappers
{
    public abstract class DataMapper<ReturnType>
    {
        public abstract ReturnType MapItem(object dataItem);
    }
}
