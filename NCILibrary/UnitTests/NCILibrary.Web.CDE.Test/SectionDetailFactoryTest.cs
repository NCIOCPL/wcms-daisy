using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web.CDE.Test;
using NCI.Test.Web;
namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for SectionDetailFactoryTest and is intended
    ///to contain all SectionDetailFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SectionDetailFactoryTest : CDETest
    {


        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetSectionDetail
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetSectionDetail_Test()
        {
            //Initialize the Section Detail Factory
            //TODO: FIX THIS
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string path = "/cancertopics";

                SectionDetail expected = GetCancertopicsSectionDetailForComparison(); // TODO: Initialize to an appropriate value
                SectionDetail actual;
                
                actual = SectionDetailFactory.GetSectionDetail(path);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for GetSectionDetail
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetSectionDetail_RootPath_Test()
        {
            //Initialize the Section Detail Factory
            //TODO: FIX THIS
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string path = "/";

                SectionDetail expected = GetRootSectionDetailForComparison(); // TODO: Initialize to an appropriate value
                SectionDetail actual;

                actual = SectionDetailFactory.GetSectionDetail(path);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for GetSectionDetail
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        [ExpectedException(typeof(PageAssemblyException))]
        public void GetSectionDetail_InvalidPath_Test()
        {
            //Initialize the Section Detail Factory
            //TODO: FIX THIS
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string path = "/23423423";

                SectionDetailFactory.GetSectionDetail(path);
            }
        }
    }
}
