using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

using NCI.Data;

using MigrationEngine.BusinessObjects;
using MigrationEngine.Mappers;

namespace MigrationEngine.DataAccess
{
    public class DatabaseDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        [XmlAttribute("ProcName")]
        public String ProcName;

        [XmlAttribute("ConnectionString")]
        public String ConnectionString;

        public DataMapper<ReturnType> Mapper;

        public override List<ReturnType> LoadData()
        {
            List<ReturnType> returnData = new List<ReturnType>();

            DataTable table = SqlHelper.ExecuteDatatable(ConnectionString, System.Data.CommandType.StoredProcedure, ProcName);

            foreach (DataRow row in table.Rows)
            {
                ReturnType data = Mapper.MapItem(row);
                returnData.Add(data);
            }

            return returnData;
        }
    }
}
