using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Test.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.CDE.Configuration;
using System.Web;
using System.IO;
using System.Xml.Schema;
using NCI.Logging;
namespace NCI.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for PageAssemblyInstructionFactoryTest and is intended
    ///to contain all PageAssemblyInstructionFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageAssemblyInstructionFactoryTest : CDETest
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
        ///A test for Instance
        ///</summary>
        public void InstanceTest()
        {
            PageAssemblyInstructionFactory actual;
            actual = PageAssemblyInstructionFactory_Accessor.Instance;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [DeploymentItem("NCILibrary.Web.ContentDeliveryEngine.dll")]
        public void InitializeTest()
        {
            PageAssemblyInstructionFactory_Accessor target = new PageAssemblyInstructionFactory_Accessor(); // TODO: Initialize to an appropriate value
            target.Initialize();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetPageAssemblyInfo.Should load the IPageAssemblyInstruction with data
        ///after this method is invoked
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetPageAssemblyInfo_SinglePageAssemblyInstruction_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                IPageAssemblyInstruction actual = PageAssemblyInstructionFactory.GetPageAssemblyInfo("/cancertopics");
                SinglePageAssemblyInstruction expected = GetCancerTopicsSinglePageAssemblyInstuction();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetPageAssemblyInfo_SinglePageAssemblyInstruction_XMLFileNotExixts_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                IPageAssemblyInstruction actual = PageAssemblyInstructionFactory.GetPageAssemblyInfo("/cancertopics2");
                SinglePageAssemblyInstruction expected = GetCancerTopicsSinglePageAssemblyInstuction();
                Assert.AreNotEqual(expected, actual);
            }
        }
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void ValidateXml_Valid_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {

                string xmlPath = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/cancertopics"));
                string XsdPath = HttpContext.Current.Server.MapPath("~/schema/CDESchema.xsd");
                bool expected = true;
                bool actual;
                actual = PageAssemblyInstructionFactory.ValidateXml(xmlPath, XsdPath);
                Assert.AreEqual(expected, actual);
            }
        }
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void ValidateXml_NotValid_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {

                string xmlPath = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/SinglePageAssemblyInstructionNotValid"));
                string XsdPath = HttpContext.Current.Server.MapPath("~/schema/CDESchema.xsd");
                bool expected = false;
                bool actual;
                actual = PageAssemblyInstructionFactory.ValidateXml(xmlPath, XsdPath);
                Assert.AreEqual(expected, actual);
            }
        }

 

     }
}
