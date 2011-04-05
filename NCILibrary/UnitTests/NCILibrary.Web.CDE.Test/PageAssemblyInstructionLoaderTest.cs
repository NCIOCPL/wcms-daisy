using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using NCI.Web.CDE.Test;
using NCI.Test.Web;

namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for PageAssemblyInstructionLoaderTest and is intended
    ///to contain all PageAssemblyInstructionLoaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageAssemblyInstructionLoaderTest : CDETest
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
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_Cancertopics_Web_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                PageAssemblyInstructionLoader_Accessor target = new PageAssemblyInstructionLoader_Accessor();
                HttpContext context = HttpContext.Current;
                string url = "/cancertopics"; 

                target.RewriteUrl(context, url);

                PageTemplateInfo pti = GetWebPageTemplateInfo();
                string path = VirtualPathUtility.ToAbsolute(pti.PageTemplatePath);

                Assert.IsNotNull(PageAssemblyContext.Current, "PageAssemblyContext.Current is null");
                Assert.IsNotNull(PageAssemblyContext.Current.PageAssemblyInstruction, "PageAssemblyContext.Current.PageAssemblyInstruction is null");
                Assert.AreEqual(pti, PageAssemblyContext.Current.PageTemplateInfo);
                Assert.AreEqual(DisplayVersions.Web, PageAssemblyContext.Current.DisplayVersion);
                Assert.AreEqual(GetCancerTopicsSinglePageAssemblyInstuction(), PageAssemblyContext.Current.PageAssemblyInstruction);
                Assert.AreEqual(path, HttpContext.Current.Request.Path); //Check if the rewrite is pointing to the correct aspx page.
                //TODO: Check for where the variable used for the rewrite log is stored.
            }
        }

        /// <summary>
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_NonExistantUrl_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                PageAssemblyInstructionLoader_Accessor target = new PageAssemblyInstructionLoader_Accessor();
                HttpContext context = HttpContext.Current;
                string url = "/fooboo";

                target.RewriteUrl(context, url);

            }
        }

        /// <summary>
        ///A test for RewriteUrl with xml file not well formed.
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_NoTWellFormedDataInXMLFile_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                PageAssemblyInstructionLoader_Accessor target = new PageAssemblyInstructionLoader_Accessor();
                HttpContext context = HttpContext.Current;
                string url = "/cancertopics_not_wellformed";
                
                target.RewriteUrl(context, url);

                Assert.IsNull(PageAssemblyContext.Current.PageAssemblyInstruction, "PageAssemblyContext.Current.PageAssemblyInstruction is null");

            }

        }


        /// <summary>
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrlMultiPage_Cancertopics_Web_Test()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                PageAssemblyInstructionLoader_Accessor target = new PageAssemblyInstructionLoader_Accessor();
                HttpContext context = HttpContext.Current;
                string url = "/multicancertopics/page1";
                target.RewriteUrl(context, url);
                Assert.IsNotNull(PageAssemblyContext.Current, "PageAssemblyContext.Current is null");
                Assert.IsNotNull(PageAssemblyContext.Current.PageAssemblyInstruction, "PageAssemblyContext.Current.PageAssemblyInstruction is null");
                Assert.AreEqual(DisplayVersions.Web, PageAssemblyContext.Current.DisplayVersion);

            }
        }

        /// <summary>
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_DisplayVersion_Print_Test()
        {
            //For url/print
            Assert.Inconclusive("Not Implemented");
        }

        /// <summary>
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_DisplayVersion_ViewAll_Test()
        {
            //For url/allpages
            Assert.Inconclusive("Not Implemented");
        }

        /// <summary>
        ///A test for RewriteUrl
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void RewriteUrl_DisplayVersion_PrintAll_Test()
        {
            //For url/allpages/print
            Assert.Inconclusive("Not Implemented");
        }


    }
}
