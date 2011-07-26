using NCI.Web.CDE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web.CDE.Test;
using NCI.Test.Web;
namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for FileInstructionFactoryTest and is intended
    ///to contain all FileInstructionFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileInstructionFactoryTest : CDETest
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
        ///A test for GetFileInstruction
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetFileInstructionTest()
        {
            using (HttpSimulator httpSimulator = GetStandardSimulatedRequest())
            {
                IFileInstruction actual = FileInstructionFactory.GetFileInstruction("/cancertopics/test.pdf");
                GenericFileInstruction expected = GetTestFileInstuction();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
