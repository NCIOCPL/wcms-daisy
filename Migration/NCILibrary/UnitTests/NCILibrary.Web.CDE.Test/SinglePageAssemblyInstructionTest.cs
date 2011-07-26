using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web;
using System;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE;
using NCI.Web;

namespace NCI.Web.CDE.Test
{


    /// <summary>
    ///This is a test class for SinglePageAssemblyInstructionTest and is intended
    ///to contain all SinglePageAssemblyInstructionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SinglePageAssemblyInstructionTest
    {        
        private IPageAssemblyInstruction InitializeTestPageAssemblyInfo()
        {
            string xmlFilePath = TestContext.TestDeploymentDir + "\\PublishedContent\\PageInstructions\\cancertopics.xml";

            IPageAssemblyInstruction pageAssemblyInfo = null;
            using (XmlReader xmlReader = XmlReader.Create(xmlFilePath))
            {
                xmlReader.MoveToContent();
                string pageAssemblyInfoTypeName = xmlReader.LocalName;

                //XmlSerializer serializer = _serializers[pageAssemblyInfoTypeName];
                XmlSerializer serializer = new XmlSerializer(typeof(SinglePageAssemblyInstruction));

                // Deserialize the XML into an object.
                pageAssemblyInfo = (IPageAssemblyInstruction)serializer.Deserialize(xmlReader);
                return pageAssemblyInfo;
            }
        }

        private Dictionary<string, XmlSerializer> _serializers = new Dictionary<string, XmlSerializer>();

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

        /// <summary>
        ///A test for UrlFilterDelegates
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void SinglePageAssemblyInstruction_XMLSerializer_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            Assert.IsNotNull(pageAssemblyInfo);
            Assert.IsNotNull(pageAssemblyInfo.PageTemplateName);
            Assert.IsNotNull(pageAssemblyInfo.SectionPath);
        }



        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetField_Test()
        {

            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            pageAssemblyInfo.AddFieldFilter("Foo12345", (name,data) =>
            {
                data.Value = "Foo12345";
            });

            Assert.AreEqual("Foo12345", pageAssemblyInfo.GetField("Foo12345"));


        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetField_MultipleFieldFilters_Test()
        {

            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            pageAssemblyInfo.AddFieldFilter("Foo12345", (name, data) =>
            {
                data.Value = "Dictionary of cancer terms";
            });

            //Add another one, but make sure we chain from the previous
            pageAssemblyInfo.AddFieldFilter("Foo12345", (name, data) =>
            {
                data.Value += "--Modified";
            });

            Assert.AreEqual("Dictionary of cancer terms--Modified", pageAssemblyInfo.GetField("Foo12345"));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "The fieldName parameter may not be null or empty.")]
        [DeploymentItem(@"XmlFiles")]
        public void AddField_NullFieldName_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            pageAssemblyInfo.AddFieldFilter(null, (name, data) =>
            {
                data.Value = "Dictionary of cancer terms";
            });

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "The fieldName parameter may not be null or empty.")]
        [DeploymentItem(@"XmlFiles")]
        public void AddField_EmptyFieldName_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();

            pageAssemblyInfo.AddFieldFilter(string.Empty, (name, data) =>
            {
                data.Value = "Dictionary of cancer terms";
            });

        }

        /* Test the SinglePageAssemblyInstruction's individual Field Filters. */
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetField_HTML_Title_Field_Test()
        {
            string HTML_Title = "Cancer Topics Home Page";
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            Assert.AreEqual(HTML_Title, pageAssemblyInfo.GetField("HTML_Title"));
        }


        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetField_HTML_MetaDescription_Test()
        {
            string MetaDescription = "Information from the National Cancer Institute about cancer treatment, prevention, screening, genetics, causes, and how to cope with cancer.";
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            Assert.AreEqual(MetaDescription, pageAssemblyInfo.GetField("HTML_MetaDescription"));
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetField_HTML_MetaKeywords_Test()
        {
            string MetaKeywords = "cancer,information";
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            Assert.AreEqual(MetaKeywords, pageAssemblyInfo.GetField("HTML_MetaKeywords"));
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetUrl_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            pageAssemblyInfo.AddUrlFilter("foo", (name,url) =>
            {
                url.Clear();
                url.UriStem = "/foo";
            });

            NciUrl expected = new NciUrl();
            expected.UriStem = "/foo";

            Assert.AreEqual(expected.ToString(), pageAssemblyInfo.GetUrl("foo").ToString());
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetUrl_MultiFilter_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            pageAssemblyInfo.AddUrlFilter("foo", (name, url ) =>  
            {
                url.Clear();
                url.UriStem = "/foo";
            });

            pageAssemblyInfo.AddUrlFilter("foo", (name, url) =>
            {
                url.UriStem += "/bar";
            });

            NciUrl expected = new NciUrl();
            expected.UriStem = "/foo/bar";

            Assert.AreEqual(expected.ToString(), pageAssemblyInfo.GetUrl("foo").ToString());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "The urlType parameter may not be null or empty.")]
        [DeploymentItem(@"XmlFiles")]
        public void AddUrl_NullUrlType_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            pageAssemblyInfo.AddUrlFilter(null, (name, url ) =>
            {
                url.Clear();
                url.UriStem = "/foo";
            });
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "The urlType parameter may not be null or empty.")]
        [DeploymentItem(@"XmlFiles")]
        public void AddUrl_EmptyUrlType_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            pageAssemblyInfo.AddUrlFilter(string.Empty, (name, url ) =>
            {
                url.Clear();
                url.UriStem = "/foo";
            });
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetUrl_PrettyURL_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();


            NciUrl PrettyUrl = new NciUrl();
            PrettyUrl = pageAssemblyInfo.GetUrl(PageAssemblyInstructionUrls.PrettyUrl);

            Assert.AreEqual<string>("/cancertopics", PrettyUrl.UriStem);
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetUrl_Cannonical_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();



            NciUrl CanonicalUrl = new NciUrl();
            CanonicalUrl = pageAssemblyInfo.GetUrl(PageAssemblyInstructionUrls.CanonicalUrl);

            Assert.AreEqual<string>("/cancertopics", CanonicalUrl.UriStem);
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetBlockedSlots_Test()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();
            string[] expectedBlockSlots ={"ContentHeaderSlot"};
            string[] actualblockedSlots;

            actualblockedSlots = pageAssemblyInfo.BlockedSlotNames;
            Assert.AreEqual(expectedBlockSlots[0],actualblockedSlots[0]);

        }

        /// <summary>
        /// Helps in testing the new property AlternateContentVersion.  
        /// </summary>
        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetAlternateContentVersion()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();
            string[] alternateContentVersion = null;
            alternateContentVersion = pageAssemblyInfo.AlternateContentVersionsKeys;
            Assert.IsNotNull(alternateContentVersion);
            Assert.IsTrue(alternateContentVersion.Length > 0);
        }

        [TestMethod()]
        [DeploymentItem(@"XmlFiles")]
        public void GetWebAnalytics()
        {
            IPageAssemblyInstruction pageAssemblyInfo = null;
            pageAssemblyInfo = InitializeTestPageAssemblyInfo();
            WebAnalyticsSettings webAnalyticsSettings = null;
            webAnalyticsSettings = pageAssemblyInfo.GetWebAnalytics();
            Assert.IsNotNull(webAnalyticsSettings);
            Assert.IsTrue(webAnalyticsSettings.Props.Count > 0);
            Assert.IsTrue(webAnalyticsSettings.Evars.Count > 0);
        }

        private void FilterCurrentUrl(NciUrl url)
        {
            url.SetUrl("/cancertopics");
        }
    }
}
