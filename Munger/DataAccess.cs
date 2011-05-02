using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Munger
{
    static class DataAccess
    {
        static string dbConnectionString =
           ConfigurationManager.ConnectionStrings["MigrationConnectionString"].ConnectionString;
        static string cdrDbConnectionString =
            ConfigurationManager.ConnectionStrings["CdrDbConnectionString"].ConnectionString;

        public static LinkLegacyDetails GetUrlDetails(string prettyUrl)
        {
            LinkLegacyDetails details = null;

            using (DataSet data = LoadUrlDetails(prettyUrl))
            {

                if (data.Tables[0].Rows.Count > 0)
                {
                    string viewid = data.Tables[0].Rows[0].Field<Guid>("viewid").ToString();
                    string contentType = data.Tables[0].Rows[0].Field<string>("contentType");
                    details = new LinkLegacyDetails(viewid, contentType);
                }
            }

            return details;
        }

        private static DataSet LoadUrlDetails(string prettyUrl)
        {
            string sql = string.Format("select viewid, [content type] contentType from dbo.mig_urlviewidlookup('{0}')", prettyUrl);

            DataSet ds = GetSQLQuery(sql, dbConnectionString);
            if (ds != null)
                ds.Tables[0].TableName = "UrlDetails";

            return ds;
        }

        public static List<string> GetProtocolPrettyUrlIDs()
        {
            List<string> theList = new List<string>();

            using (DataSet data = LoadProtocolPrettyUrlIDs())
            {
                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in data.Tables[0].Rows)
                    {
                        string idstring = row.Field<string>("urlID").Trim().ToLower();
                        theList.Add(idstring);
                    }
                }
                else
                    throw new DataAcessLayerException("Unable to retrieve Protocol pretty URLs.");
            }

            return theList;
        }

        private static DataSet LoadProtocolPrettyUrlIDs()
        {
            string sql =
@"select PrimaryPrettyUrlID urlID from protocoldetail
  union
select IDstring urlID from ProtocolSecondaryUrl";
            DataSet ds = GetSQLQuery(sql, cdrDbConnectionString);

            return ds;
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