using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Test.Web;
using NCI.Web.CDE.Test;
namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for PageTemplateResolverTest and is intended
    ///to contain all PageTemplateResolverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageTemplateResolverTest : CDETest
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
        ///A test for LoadPageTemplateConfiguration
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void PageTemplateConfigurationTest()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {

                PageTemplateConfiguration expected = GetPageTemplateConfigurationForCompare();

                PageTemplateConfiguration actual = PageTemplateResolver_Accessor.PageTemplateConfiguration;

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for GetTemplateInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetTemplateInfoTest_DefaultCancerGovTemplate_Web()
        {

            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string templateName = "DefaultCancerGovTemplate"; 
                DisplayVersions version = DisplayVersions.Web;

                PageTemplateInfo expected = GetWebPageTemplateInfo();

                PageTemplateInfo actual;
                actual = PageTemplateResolver.GetPageTemplateInfo(templateName, version);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for GetTemplateInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetTemplateInfoTest_DefaultCancerGovTemplate_Print()
        {

            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string templateName = "DefaultCancerGovTemplate";
                DisplayVersions version = DisplayVersions.Print;

                PageTemplateInfo expected = GetPrintPageTemplateInfo();

                PageTemplateInfo actual;
                actual = PageTemplateResolver.GetPageTemplateInfo(templateName, version);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for GetTemplateInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetTemplateInfoTest_PrintAll_DefaultToPrint()
        {
            Assert.Inconclusive("This test is not implemented");
        }

        /// <summary>
        ///A test for GetTemplateInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetTemplateInfoTest_PrintAll_DefaultToViewAll()
        {
            Assert.Inconclusive("This test is not implemented");
        }

        /// <summary>
        ///A test for GetTemplateInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetTemplateInfoTest_DefaultToWeb()
        {
            Assert.Inconclusive("This test is not implemented");
        }


        /// <summary>
        ///A test for PageTemplateConfigurationPath
        ///</summary>
        [TestMethod()]
        public void PageTemplateConfigurationPathTest()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {

                string actual;
                actual = PageTemplateResolver_Accessor.PageTemplateConfigurationPath;
                Assert.Inconclusive("Verify the correctness of this test method.");
            }
        }



    }
}
