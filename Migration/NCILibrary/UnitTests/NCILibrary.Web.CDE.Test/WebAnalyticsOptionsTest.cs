using NCI.Web.CDE.WebAnalytics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace NCILibrary.Web.CDE.Test
{


    /// <summary>
    ///This is a test class for WebAnalyticsOptionsTest and is intended
    ///to contain all WebAnalyticsOptionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebAnalyticsOptionsTest
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


        /// <summary>
        ///A test for IsEnabled
        ///</summary>
        [TestMethod()]
        public void IsEnabledTest()
        {
            bool actual;
            actual = WebAnalyticsOptions.IsEnabled;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EnableNonJavaScriptTagging
        ///</summary>
        [TestMethod()]
        public void EnableNonJavaScriptTaggingTest()
        {
            bool actual;
            actual = WebAnalyticsOptions.EnableNonJavaScriptTagging;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            WebAnalyticsOptions.Initialize();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetSuitesForChannel
        ///</summary>
        [TestMethod()]
        public void GetSuitesForChannelTest()
        {
            string channelName = string.Empty; // TODO: Initialize to an appropriate value
            string language = string.Empty; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = WebAnalyticsOptions.GetSuitesForChannel(channelName, language);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetChannelForUrlPath
        ///</summary>
        [TestMethod()]
        public void GetChannelForUrlPathTest()
        {
            string urlFolderPath = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            Assert.AreEqual("NCI Home", WebAnalyticsOptions.GetChannelForUrlPath("/test"));
            Assert.AreEqual("Cancer Statistics", WebAnalyticsOptions.GetChannelForUrlPath("/statistics/test"));
            Assert.AreEqual("Center for Cancer Training (CCT)", WebAnalyticsOptions.GetChannelForUrlPath("/researchandfunding/cancertraining/test"));
            Assert.AreEqual("Research & Funding", WebAnalyticsOptions.GetChannelForUrlPath("/researchandfunding/test"));
            Assert.AreEqual("NCI Cancer Bulletin", WebAnalyticsOptions.GetChannelForUrlPath("/ncicancerbulletin/test"));
            Assert.AreEqual("NCI Home", WebAnalyticsOptions.GetChannelForUrlPath("/homepage/test"));
            Assert.AreEqual("Lo que usted necesita saber (WYNTK)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/tipos/necesita-saber/test"));
            Assert.AreEqual("Tipos de cancer (Cancer Types)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/tipos/test"));
            Assert.AreEqual("Apoyo y recursos (Support and Resources-Other)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/recursos/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/wilms/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/retinoblastoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/osteosarcoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/neuroblastoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/ewing/test"));
            Assert.AreEqual("PDQ Adult Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/canceres-infantiles-poco-comunes/patient"));
            Assert.AreEqual("PDQ Complementary and Alternative Medicine (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/mca/test"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/grados-comprobacion-cuidados-apoyo/test"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/cuidados-medicos-apoyo/test"));
            Assert.AreEqual("Noticias (News)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/noticias/test"));
            Assert.AreEqual("OLACPD (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto/olacpd/test"));
            Assert.AreEqual("Decreto de Recuperacion y Reinversion en NCI (ARRA)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto/decretoderecuperacion/test"));
            Assert.AreEqual("Nuestro Instituto (About NCI)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto/test"));
            Assert.AreEqual("Indice de hojas informativas (Fact Sheets)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/hojas-informativas/test"));
            Assert.AreEqual("Indice de hojas informativas (Fact Sheets)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/hojasinformativas/test"));
            Assert.AreEqual("Entendiendo al Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/entendiendo/test"));
            Assert.AreEqual("El cancer (Cancer)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/test"));
            Assert.AreEqual("Pagina principal (Home Page)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/test"));
            Assert.AreEqual("NCI Drug Dictionary", WebAnalyticsOptions.GetChannelForUrlPath("/drugdictionary/test"));
            Assert.AreEqual("Dictionary of Cancer Terms", WebAnalyticsOptions.GetChannelForUrlPath("/dictionary/test"));
            Assert.AreEqual("Diccionario de cancer (Dictionary of Cancer Terms)", WebAnalyticsOptions.GetChannelForUrlPath("/diccionario/test"));
            Assert.AreEqual("Clinical Trials Results", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/results/test"));
            Assert.AreEqual("Featured Clinical Trials", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/featured/trials/test"));
            Assert.AreEqual("Educational Materials About Clinical Trials", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/education/test"));
            Assert.AreEqual("Clinical Trials Reporting Program (CTRP)", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/conducting/ncictrp/test"));
            Assert.AreEqual("Clinical Trials (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/test"));
            Assert.AreEqual("What You Need To Know", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/wyntk/test"));
            Assert.AreEqual("Understanding Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/understandingcancer/test"));
            Assert.AreEqual("Cancer Types", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/types/test"));
            Assert.AreEqual("Screening and Testing to Detect Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/screening/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/wilms/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/retinoblastoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/osteosarcoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/neuroblastoma/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/lchistio/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/lateeffects/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/extracranial-germ-cell/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/ewings/test"));
            Assert.AreEqual("PDQ Adult Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/test"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/childbrain/patient"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/supportivecare/test"));
            Assert.AreEqual("PDQ Prevention (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/prevention/test"));
            Assert.AreEqual("PDQ Complementary and Alternative Medicine (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/cam/test"));
            Assert.AreEqual("PDQ (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/test"));
            Assert.AreEqual("NCI Fact Sheets", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/factsheet/test"));
            Assert.AreEqual("Cancer Drug Information Summaries", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/druginfo/test"));
            Assert.AreEqual("Lifelines", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/disparities/lifelines/test"));
            Assert.AreEqual("Coping with Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/coping/test"));
            Assert.AreEqual("Health Professional Training Tools", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/cancerlibrary/health-professional-training-tools/test"));
            Assert.AreEqual("Complementary and Alternative Medicine", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/cam/test"));
            Assert.AreEqual("Cancer Topics (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/test"));
            Assert.AreEqual("El Boletin del Instituto Nacional del Cancer (Cancer Bulletin)", WebAnalyticsOptions.GetChannelForUrlPath("/boletin/test"));
            Assert.AreEqual("Science Serving People", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/servingpeople/test"));
            Assert.AreEqual("American Recovery and Reinvestment Act at NCI (ARRA)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/recovery/test"));
            Assert.AreEqual("OLACPD (English)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/organization/olacpd/test"));
            Assert.AreEqual("Director", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/director/test"));
            Assert.AreEqual("About NCI (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/test"));

            Assert.AreEqual("NCI Home", WebAnalyticsOptions.GetChannelForUrlPath("/"));
            Assert.AreEqual("Newscenter",WebAnalyticsOptions.GetChannelForUrlPath("/newscenter"));
            Assert.AreEqual("Cancer Statistics", WebAnalyticsOptions.GetChannelForUrlPath("/statistics"));
            Assert.AreEqual("Center for Cancer Training (CCT)", WebAnalyticsOptions.GetChannelForUrlPath("/researchandfunding/cancertraining"));
            Assert.AreEqual("Research & Funding", WebAnalyticsOptions.GetChannelForUrlPath("/researchandfunding"));
            Assert.AreEqual("NCI Cancer Bulletin", WebAnalyticsOptions.GetChannelForUrlPath("/ncicancerbulletin"));
            Assert.AreEqual("NCI Home", WebAnalyticsOptions.GetChannelForUrlPath("/homepage"));
            Assert.AreEqual("Lo que usted necesita saber (WYNTK)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/tipos/necesita-saber"));
            Assert.AreEqual("Tipos de cancer (Cancer Types)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/tipos"));
            Assert.AreEqual("Apoyo y recursos (Support and Resources-Other)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/recursos"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/wilms"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/retinoblastoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/osteosarcoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/neuroblastoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/ewing"));
            Assert.AreEqual("PDQ Adult Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento"));
//            Assert.AreEqual("PDQ Pediatric Treatment (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/tratamiento/canceres-infantiles-poco-comunes/patient"));
            Assert.AreEqual("PDQ Complementary and Alternative Medicine (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/mca"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/grados-comprobacion-cuidados-apoyo"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/pdq/cuidados-medicos-apoyo"));
            Assert.AreEqual("Noticias (News)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/noticias"));
            Assert.AreEqual("OLACPD (Spanish)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto/olacpd"));
            Assert.AreEqual("Decreto de Recuperacion y Reinversion en NCI (ARRA)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto/decretoderecuperacion"));
            Assert.AreEqual("Nuestro Instituto (About NCI)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/instituto"));
            Assert.AreEqual("Indice de hojas informativas (Fact Sheets)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/hojas-informativas"));
            Assert.AreEqual("Indice de hojas informativas (Fact Sheets)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/hojasinformativas"));
            Assert.AreEqual("Entendiendo al Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer/entendiendo"));
            Assert.AreEqual("El cancer (Cancer)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol/cancer"));
            Assert.AreEqual("Pagina principal (Home Page)", WebAnalyticsOptions.GetChannelForUrlPath("/espanol"));
            Assert.AreEqual("NCI Drug Dictionary", WebAnalyticsOptions.GetChannelForUrlPath("/drugdictionary"));
            Assert.AreEqual("Dictionary of Cancer Terms", WebAnalyticsOptions.GetChannelForUrlPath("/dictionary"));
            Assert.AreEqual("Diccionario de cancer (Dictionary of Cancer Terms)", WebAnalyticsOptions.GetChannelForUrlPath("/diccionario"));
            Assert.AreEqual("Clinical Trials Results", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/results"));
            Assert.AreEqual("Featured Clinical Trials", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/featured/trials"));
            Assert.AreEqual("Educational Materials About Clinical Trials", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/education"));
            Assert.AreEqual("Clinical Trials Reporting Program (CTRP)", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials/conducting/ncictrp"));
            Assert.AreEqual("Clinical Trials (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/clinicaltrials"));
            Assert.AreEqual("What You Need To Know", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/wyntk"));
            Assert.AreEqual("Understanding Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/understandingcancer"));
            Assert.AreEqual("Cancer Types", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/types"));
            Assert.AreEqual("Screening and Testing to Detect Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/screening"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/wilms"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/retinoblastoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/osteosarcoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/neuroblastoma"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/lchistio"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/lateeffects"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/extracranial-germ-cell"));
            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/ewings"));
            Assert.AreEqual("PDQ Adult Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment"));
//            Assert.AreEqual("PDQ Pediatric Treatment (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/treatment/childbrain/patient"));
            Assert.AreEqual("PDQ Supportive and Palliative Care (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/supportivecare"));
            Assert.AreEqual("PDQ Prevention (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/prevention"));
            Assert.AreEqual("PDQ Complementary and Alternative Medicine (English)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq/cam"));
            Assert.AreEqual("PDQ (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/pdq"));
            Assert.AreEqual("NCI Fact Sheets", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/factsheet"));
            Assert.AreEqual("Cancer Drug Information Summaries", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/druginfo"));
            Assert.AreEqual("Lifelines", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/disparities/lifelines"));
            Assert.AreEqual("Coping with Cancer", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/coping"));
            Assert.AreEqual("Health Professional Training Tools", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/cancerlibrary/health-professional-training-tools"));
            Assert.AreEqual("Complementary and Alternative Medicine", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics/cam"));
            Assert.AreEqual("Cancer Topics (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/cancertopics"));
            Assert.AreEqual("El Boletin del Instituto Nacional del Cancer (Cancer Bulletin)", WebAnalyticsOptions.GetChannelForUrlPath("/boletin"));
            Assert.AreEqual("Science Serving People", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/servingpeople"));
            Assert.AreEqual("American Recovery and Reinvestment Act at NCI (ARRA)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/recovery"));
            Assert.AreEqual("OLACPD (English)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/organization/olacpd"));
            Assert.AreEqual("Director", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci/director"));
            Assert.AreEqual("About NCI (Other)", WebAnalyticsOptions.GetChannelForUrlPath("/aboutnci"));

       }
    }
}