using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web;
using NCI.Test.Web;
namespace NCI.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for PageAssemblyContextTest and is intended
    ///to contain all PageAssemblyContextTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageAssemblyContextTest : CDETest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
 

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
        ///A test for InitializePageAssemblyInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]

        //[DeploymentItem("XmlFiles\\cancertopics.xml")]
        //[DeploymentItem("XmlFiles\\PageTemplateConfiguration.xml")]
        public void InitializePageAssemblyInfo_Test()
        {
            PageAssemblyContext_Accessor target = new PageAssemblyContext_Accessor(); // TODO: Initialize to an appropriate value

            string xmlFilePath = TestContext.TestDeploymentDir + "\\PublishedContent\\PageInstructions\\cancertopics.xml";
            IPageAssemblyInstruction pageAssemblyInfo = null;

            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {


                using (XmlReader xmlReader = XmlReader.Create(xmlFilePath))
                {
                    xmlReader.MoveToContent();
                    string pageAssemblyInfoTypeName = xmlReader.LocalName;

                    //XmlSerializer serializer = _serializers[pageAssemblyInfoTypeName];
                    XmlSerializer serializer = new XmlSerializer(typeof(SinglePageAssemblyInstruction));

                    // Deserialize the XML into an object.
                    pageAssemblyInfo = (IPageAssemblyInstruction)serializer.Deserialize(xmlReader);

                }

                IPageAssemblyInstruction info = pageAssemblyInfo;
                DisplayVersions dispayVersion = DisplayVersions.Web;

                //Load the page template info for the current request
                PageTemplateInfo pageTemplateInfo = new PageTemplateInfo();
                pageTemplateInfo.PageTemplatePath = "FOO.ASPX";

                target.InitializePageAssemblyInfo(info, dispayVersion, pageTemplateInfo,"url");

                Assert.Inconclusive("A method that does not return a value cannot be verified.");
            }


        }
    }
}
