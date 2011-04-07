using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

using NCI.Data;

using MigrationEngine.Descriptors;
using MigrationEngine.Mappers;

namespace MigrationEngine.DataAccess
{
    public class DatabaseDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        [XmlAttribute("ProcName")]
        public String ProcName;

        [XmlAttribute("ConnectionName")]
        public String ConnectionName;

        public DataMapper<ReturnType> Mapper;

        public override List<ReturnType> LoadData()
        {
            List<ReturnType> returnData = new List<ReturnType>();

            if (string.IsNullOrEmpty(ConnectionName))
                throw new ConfigurationException(string.Format("Connection string name not specified for {0}.", this.GetType().Name));

            if (string.IsNullOrEmpty(ProcName))
                throw new ConfigurationException(string.Format("Stored procedure name not specified for {0}.", this.GetType().Name));

            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;

            DataTable table = SqlHelper.ExecuteDatatable(connectionString, System.Data.CommandType.StoredProcedure, ProcName);

            foreach (DataRow row in table.Rows)
            {
                ReturnType data = Mapper.MapItem(row);
                returnData.Add(data);
            }

            return returnData;
        }
    }
}
