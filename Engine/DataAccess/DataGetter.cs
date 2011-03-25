using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.DataAccess
{
    //[XmlInclude(typeof(DatabaseDataGetter<FolderDescription>)),
    //XmlInclude(typeof(FloozlewarpDataGetter<FolderDescription>))]
    public abstract class DataGetter<ReturnType>
        : IDataGetter<ReturnType> where ReturnType : MigrationData
    {
        public abstract ReturnType LoadData();
    }
}