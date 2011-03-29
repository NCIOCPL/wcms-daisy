using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;
using MigrationEngine.Mappers;

namespace MigrationEngine.DataAccess
{
    public class DatabaseDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        [XmlAttribute("ProcName")]
        public String ProcName;// = "usp_StoredProc";

        public DataMapper<ReturnType> Mapper;

        public override List<ReturnType> LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
