using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web.CDE;

namespace NCILibrary.Web.CDE.Test
{
    /// <summary>
    /// Summary description for Module_PageOptionsBox_Test
    /// </summary>
    [TestClass]
    public class Module_PageOptionsBox_Test
    {
        public Module_PageOptionsBox_Test()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void LoadModule_PageOptionsBoxTest()
        {
            // In this case the snippet info data is not HTML(which is often the case)
            // but xml data which contains rendering properties for each page option item 
            string xmlData =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<cde:Module_PageOptionsBox xsi:schemaLocation=""http://www.example.org/CDESchema CDESchema.xsd"" xmlns:cde=""http://www.example.org/CDESchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <Title>Hello</Title>
    <PageOptions>
           <PageOption key="""">
               <cssClass>Class Name</cssClass>
               <LinkText>text</LinkText>
               <WebAnalyticsFunction>functionname</WebAnalyticsFunction>    
               <OptionType>type</OptionType>
          </PageOption>
           <PageOption key="""">
               <cssClass>Class Name</cssClass>
               <LinkText>text</LinkText>
               <WebAnalyticsFunction>functionname</WebAnalyticsFunction>    
               <OptionType>type</OptionType>
          </PageOption>
    </PageOptions>
</cde:Module_PageOptionsBox>";

            XmlTextReader reader = new XmlTextReader(xmlData, XmlNodeType.Element, null);
            XmlSerializer serializer = new XmlSerializer(typeof(Module_PageOptionsBox), "cde");
            Module_PageOptionsBox obj = (Module_PageOptionsBox)serializer.Deserialize(reader);
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Title);
            Assert.IsTrue(obj.PageOptions.Count > 0);
        }
    }
}
