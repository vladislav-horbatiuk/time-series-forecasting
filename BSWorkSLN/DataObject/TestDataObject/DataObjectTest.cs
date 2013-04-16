﻿using BSWork.DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDataObject
{
    
    
    /// <summary>
    ///This is a test class for DataObjectTest and is intended
    ///to contain all DataObjectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataObjectTest
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
        ///A test for DataObject Constructor, tests its general correctness.
        ///</summary>
        [TestMethod()]
        public void DataObjectConstructorTest()
        {
            double[] iDataArray = {1, 2, 3, 4, 5};
            string iName = "someStr";
            BSDataObject target = new BSDataObject(iDataArray, iName);
            Assert.AreEqual<string>(iName, target.ObjName);
            CollectionAssert.AreEqual(iDataArray, target.DataArray);
            iDataArray[2] = 2.2;
            CollectionAssert.AreEqual(iDataArray, target.DataArray);
        }
    }
}
