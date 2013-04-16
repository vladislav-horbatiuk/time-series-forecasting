using DataPreprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BSWork.DataObjects;

namespace TestTukey53HPreprocessor
{
    
    
    /// <summary>
    ///This is a test class for Tukey53HPreprocessorTest and is intended
    ///to contain all Tukey53HPreprocessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Tukey53HPreprocessorTest
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
        ///A general test for Tukey53H Process method; checks its general "correct" behavior
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            //Test case #1
            Tukey53HPreprocessor target = new Tukey53HPreprocessor();
            double[] iArray = {1, 2, 3, 4, 5, 6, 7, 8};
            double[] expected = {1, 2, 3, 4, 5, 6, 7, 8};
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            //Test case #2
            target = new Tukey53HPreprocessor(1.5);
            iArray = new double[] { 1, 2, 3, 10, 7, 1, 12, 4, 3, 2, 1 };
            expected = new double[] { 1, 2, 3, 5, 7, 1, 2.5, 4, 3, 2, 1 };
            actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            //Test case #3
            target = new Tukey53HPreprocessor(100);
            iArray = new double[] { 1, 100, 2, 3, 100, 200, 5, 6, 200, 7, 8 };
            expected = iArray;
            actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
        }


        /// <summary>
        ///A regression test cases for Tukey53H Process method
        ///</summary>
        [TestMethod()]
        public void RegressionTests()
        {
            //Test case #1
            Tukey53HPreprocessor target = new Tukey53HPreprocessor(0.2);
            double[] iArray = {538608907.0, 1614349569.0, 46527135.0, 271407561.0, 1224014298.0, 109214588.0, 1401750087.0, 1772709690.0, 1987789339.0,
                                  774552815.0, 1226448635.0, 1418861964.0, 2008047439.0, 1719056947.0, 2091971495.0, 2013473641.0, 1857880694.0,
                                  986804018.0, 623007992.0, 1047211489.0};
            double[] expected = {538608907.0, 292568021.0, 942878565.0, 271407561.0, 190311074.5, 1312882192.5, 1401750087.0, 1694769713.0,
                                    1273631252.5, 1607118987.0, 1096707389.5, 1418861964.0, 1568959455.5, 2050009467.0, 2091971495.0, 2013473641.0,
                                    1500138829.5, 1240444343.0, 1017007753.5, 1047211489.0};
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
        }


        private static void CheckTwoOrderedArrays(double[] iActual, double[] iExpected)
        {
            Assert.AreEqual<int>(iActual.Length, iExpected.Length);
            for (int i = 0; i < iExpected.Length; i++)
                Assert.AreEqual<double>(iExpected[i], iActual[i]);
        }
    }
}
