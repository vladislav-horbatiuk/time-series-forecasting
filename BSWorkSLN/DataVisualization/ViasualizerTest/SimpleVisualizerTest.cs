using BSWork.DataVisualization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BSWork.DataObjects;
using System.Threading;

namespace ViasualizerTest
{
    
    
    /// <summary>
    ///This is a test class for SimpleVisualizerTest and is intended
    ///to contain all SimpleVisualizerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SimpleVisualizerTest
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
        ///A test for Visualize
        ///</summary>
        //[TestMethod()]
        public void VisualizeTest()
        {
            SimpleVisualizationHandler target = new SimpleVisualizationHandler();
            double[] iArray = {1.2, 3.4, 5.6, 2.2, 7.7};
            BSDataObject obj = new BSDataObject(iArray, "asdasd");
            target.AddToGraph(obj);
            iArray = new double[] { 2.6, 7.8, 12.2, 10, 2.2, 3.3, 4.4, 5.5, 3.3, -5, 30};
            target.AddToGraph(new BSDataObject(iArray, "anotherGraph"));
            target._TestShow();
        }

    }
}
