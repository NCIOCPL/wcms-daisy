using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for PageTemplateConfigurationTest and is intended
    ///to contain all PageTemplateConfigurationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageTemplateConfigurationTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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


        ///// <summary>
        /////A test for PageTemplates
        /////</summary>
        //[TestMethod()]
        //public void PageTemplatesTest()
        //{
        //    PageTemplateConfiguration target = new PageTemplateConfiguration(); // TODO: Initialize to an appropriate value
        //    PageTemplateCollection[] expected = null; // TODO: Initialize to an appropriate value
        //    PageTemplateCollection[] actual;
        //    target.PageTemplates = expected;
        //    actual = target.PageTemplates;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for PageTemplateConfiguration Constructor
        /////</summary>
        //[TestMethod()]
        //public void PageTemplateConfigurationConstructorTest()
        //{
        //    PageTemplateConfiguration target = new PageTemplateConfiguration();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void PageTemplateConfiguration_XMLSerializerTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PageTemplateConfiguration));

            PageTemplateConfiguration ptc = (PageTemplateConfiguration)serializer.Deserialize(XmlReader.Create(TestContext.TestDeploymentDir + "/PublishedContent/PageTemplateConfigurations/PageTemplateConfiguration.xml"));

            //TODO: Compare object against XML structure.
            Assert.Inconclusive("This needs to be completed");
        }
    }
}
