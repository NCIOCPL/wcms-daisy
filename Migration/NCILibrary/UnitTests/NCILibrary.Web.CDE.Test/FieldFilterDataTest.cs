using NCI.Web.CDE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace NCILibrary.Web.CDE.Test
{
    
    
    /// <summary>
    ///This is a test class for FieldFilterDataTest and is intended
    ///to contain all FieldFilterDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FieldFilterDataTest
    {
        private Dictionary<string, FieldFilterDelegate> _FieldFilterDelegates = new Dictionary<string, FieldFilterDelegate>();

        private Dictionary<string, FieldFilterDelegate> FieldFilterDelegates
        {
            get
            {
                return _FieldFilterDelegates;
            }
        }

        public void AddFieldFilter(string fieldName, FieldFilterDelegate filter)
        {

            if (FieldFilterDelegates.ContainsKey(fieldName.ToLower()) == false)
            {
                FieldFilterDelegates.Add(fieldName.ToLower(), filter);
            }
            else
            {
                FieldFilterDelegate existingFieldFilterLinkDelegate = FieldFilterDelegates[fieldName.ToLower()];
                existingFieldFilterLinkDelegate += filter;

            }
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
        ///A test for Value
        ///</summary>
        public void ValueTest()
        {
            FieldFilterData target = new FieldFilterData(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Value = expected;
            actual = target.Value;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FieldFilterData Constructor
        ///</summary>
        public void FieldFilterDataConstructorTest()
        {
            AddFieldFilter("ShotTitle", (name,data) =>
            {
                data.Value = "Dictionary of cancer terms";
            });

            AddFieldFilter("ShotTitle", (name, data) =>
            {
                data.Value = "Dictionary of cancer terms--Modified";
            });

            AddFieldFilter("HTMLTitle", (name, data) =>
            {
                data.Value = GetField("ShotTitle") + "-National cancer Institute";
            });


            string FieldFilterData = string.Empty;
            FieldFilterData = GetField("HTMLTitle");

            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        public string GetField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("The field name may not be null or empty.");

            string rtnValue = string.Empty;

            FieldFilterDelegate del = FieldFilterDelegates[fieldName.ToLower()];
            if (del != null)
            {
                //Initialize the field data to empty field data
                FieldFilterData data = new FieldFilterData();

                //Call delegate, all delegates will modify the FieldData string of the
                //FieldFilterData object we are passing in.
                del(fieldName,data);

                //set the return value to the processed value of the FieldFilterData
                rtnValue = data.Value;
            }

            return rtnValue;
        }
    }
}
