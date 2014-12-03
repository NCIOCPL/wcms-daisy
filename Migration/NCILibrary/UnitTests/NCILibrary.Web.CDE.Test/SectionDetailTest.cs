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
    ///This is a test class for SectionDetailTest and is intended
    ///to contain all SectionDetailTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SectionDetailTest : CDETest
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
        ///A test for GetSnippetsNotAssociatedWithSlots
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetSnippetsNotAssociatedWithSlots_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                string path = "/cancertopics";

                SectionDetail target = new SectionDetail(); // TODO: Initialize to an appropriate value
                List<SnippetInfo> expected = null ; // TODO: Initialize to an appropriate value
                expected = new List<SnippetInfo>() { new SnippetInfo {ContentID="",Data="",SlotName="FooterSlot",SnippetTemplatePath=""},
                    new SnippetInfo {ContentID="",Data="",SlotName="LeftNavSlot",SnippetTemplatePath=""} };
                List<SnippetInfo> actual;

                List<string> snippets = new List<string>() { "BannerSlot" };
                IEnumerable<string> templateSlotExclusionList = snippets;

                SectionDetail sectionDetail = SectionDetailFactory.GetSectionDetail(path);
                actual = sectionDetail.GetSnippetsNotAssociatedWithSlots(templateSlotExclusionList);
                Assert.AreEqual(expected.ToArray()[0].SlotName, actual.ToArray()[0].SlotName);
                Assert.AreEqual(expected.ToArray()[1].SlotName, actual.ToArray()[1].SlotName);                
            }
        }
    }
}
