using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using NCI.Data;

namespace Munger
{
    static class DataAccess
    {
        static string _dbConnectionString;

        static DataAccess()
        {
            ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings["IntranetInt"];
            if (connection == null)
            {
                throw new DataAcessLayerException("Unable to load connection string for IntranetInt.");
            }

            _dbConnectionString = connection.ConnectionString;
        }

        public static string GetMigrationID(string prettyUrl)
        {
            string migrationID;

            using (DataTable data = LoadMigrationID(prettyUrl))
            {
                migrationID = data.Rows[0].Field<Guid>("mig_id").ToString();
            }

            return migrationID;
        }

        private static DataTable LoadMigrationID(string prettyUrl)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@url", SqlDbType.VarChar){Value=prettyUrl}
                                   };

            DataTable dt = SqlHelper.ExecuteDatatable(_dbConnectionString, CommandType.StoredProcedure, "mig_URLmigIDlookup", param);

            return dt;
        }

        private static DataSet GetSQLQuery(string SQL, string connectionString)
        {
            DataSet ds = new DataSet();

            using (SqlDataAdapter dbAdapter = new SqlDataAdapter(SQL, connectionString))
            {
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.Fill(ds);
            }

            return ds;
        }
    }
}