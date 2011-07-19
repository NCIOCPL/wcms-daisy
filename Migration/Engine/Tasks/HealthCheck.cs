using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using NCI.CMS.Percussion.Manager.CMS;
using NCI.CMS.Percussion.Manager.Configuration;
using NCI.Data;

using MigrationEngine.Descriptors;
using MigrationEngine.DataAccess;
using MigrationEngine.Utilities;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Migration task for purging content from the CMS.  This class is not intended to be
    /// instantiated directly. It is created by the deserialization
    /// process in Migrator.Run().
    /// </summary>
    public class HealthCheck : MigrationTask
    {
        [XmlAttribute("ProcName")]
        public String ProcName;

        [XmlAttribute("ConnectionName")]
        public String ConnectionName;

        /// <summary>
        /// This is the main method for implementing a migration task.
        /// An instance of IMigrationLog is provided for logging the
        /// progress through the list of task items.
        /// </summary>
        /// <param name="logger">IMigrationLog object for recording task progress.</param>
        public override void Doit(IMigrationLog logger)
        {
            bool percussionIsOK = TestPercussionConnections();
            bool databaseIsOK = TestDataBaseConnection();

            if (percussionIsOK && databaseIsOK)
                Console.WriteLine("Health check passed.");
            else
                Console.WriteLine("Health check failed.");

            Console.WriteLine("Press Enter");
            Console.Read();
        }

        /// <summary>
        /// Test whether it's possible to create a Percussion connection.
        /// </summary>
        /// <returns></returns>
        private bool TestPercussionConnections()
        {
            bool testOK = true;
            string[] communityList = {"site", "siteAdmin", "ctbAdmin"};
            string communityName=string.Empty;

            foreach (string community in communityList)
            {
                try
                {
                    communityName = LookupCommunityName(community);
                    using (CMSController controller = new CMSController(communityName))
                    {

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error connecting to Percussion with {0} community ({1}).", community, communityName);
                    testOK = false;
                }
            }

            return testOK;
        }

        /// <summary>
        /// Test whether it's possible to connect to the database.
        /// </summary>
        /// <returns></returns>
        private bool TestDataBaseConnection()
        {
            bool testOK = true;

            // Don't perform the test unless the connection string is present.
            if (!string.IsNullOrEmpty(ConnectionName) && !string.IsNullOrEmpty(ProcName))
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
                    DataTable table = SqlHelper.ExecuteDatatable(connectionString, System.Data.CommandType.StoredProcedure, ProcName);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error executing {0} with connection {1}.", ProcName, ConnectionName);
                    testOK = false;
                }
            }
            else
            {
                Console.WriteLine("Skipping Database test.");
            }

            return testOK;
        }
    }
}