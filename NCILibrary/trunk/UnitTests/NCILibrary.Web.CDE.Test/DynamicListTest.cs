using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;

namespace NCILibrary.Web.CDE.Test
{
    /// <summary>
    /// Summary description for DynamicList
    /// </summary>
    [TestClass]
    public class DynamicListTest
    {
        public DynamicListTest()
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
        public void LoadDynamicList()
        {
            // In this case the snippet info data is not HTML(which is often the case)
            // but xml data which contains rendering properties for each page option item 
            string xmlData =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<cde:Module_DynamicList xsi:schemaLocation=""http://www.example.org/CDESchema CDESchema.xsd"" xmlns:cde=""http://www.example.org/CDESchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
   <SearchTitle>search_title field</SearchTitle>
    <RecordsPerPage>1</RecordsPerPage>
    <MaxResults>100</MaxResults>
    <SearchFilter>search_filter field</SearchFilter>
    <ExcludeSearchFilter>exclude_search_filter field</ExcludeSearchFilter>
    <ResultsSortOrder>results_sort_order field</ResultsSortOrder>
    <Language>language field</Language>
    <SearchType>search_type field</SearchType>
    <SearchParameters>
        <Keyword>keyword field</Keyword>
        <StartDate>01/01/1919</StartDate>
        <EndDate>01/02/1919</EndDate>
    </SearchParameters>
    <ResultsTemplate>
  <![CDATA[ The template which will be used to render the results on the CDE. See RQ002027 ]]>
    </ResultsTemplate>
</cde:Module_DynamicList>";

            DynamicList obj = ModuleObjectFactory<DynamicList>.GetModuleObject(xmlData);

            Assert.IsNotNull(obj);
        }
    }
}
