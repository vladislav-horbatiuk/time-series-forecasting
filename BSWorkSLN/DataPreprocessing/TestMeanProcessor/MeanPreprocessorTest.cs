using DataPreprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BSWork.DataObjects;

namespace TestMeanProcessor
{
    
    
    /// <summary>
    ///This is a test class for MeanPreprocessorTest and is intended
    ///to contain all MeanPreprocessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MeanPreprocessorTest
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
        ///A test for Mean class Process, checks for ordinary correctness
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            MeanPreprocessor target = new MeanPreprocessor(5);
            double[] iArray = { 1, 2, 3, 10, 5, 7, 4, 2, 12, 3, 10 };
            double[] expected = { 4.2, 5.4, 5.8, 5.6, 6, 5.6, 6.2 };
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            target = new MeanPreprocessor(4);
            iArray = new double[] { 1, 2, 3, 4, 5 };
            expected = new double[] { 2.5, 3.5 };
            actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
        }

        /// <summary>
        ///A test for MeanPreprocessor Constructor
        ///</summary>
        [TestMethod()]
        public void MeanPreprocessorConstructorTest()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int iWindowSize = rnd.Next();
                MeanPreprocessor target = new MeanPreprocessor(iWindowSize);
                Assert.AreEqual<int>(iWindowSize, target.WindowSize);
            }
        }

        /// <summary>
        ///A test to check that MeanPreprocessor Constructor accepts only positive integers
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MeanPreprocessorConstructorWindowSizeOnlyPositiveTest()
        {
            int iWindowSize = -10;
            MeanPreprocessor target = new MeanPreprocessor(iWindowSize);
        }

        /// <summary>
        ///A test for Mean Process method, checks if it deals correctly with unordinary situations (window size >= input vector size)
        ///</summary>
        [TestMethod()]
        public void ProcessUnordinaryWindowSizesTest()
        {
            MeanPreprocessor target = new MeanPreprocessor(6);
            double[] iArray = { 1, 2, 3, 4, 5, 6 };
            double[] expected = { 3.5 };
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            target = new MeanPreprocessor(5);
            iArray = new double[] { 1, 2, 3, 4 };
            actual = target.Process(new BSDataObject(iArray));
            Assert.AreEqual<int>(0, actual.DataArray.Length);
        }

        private static void CheckTwoOrderedArrays(double[] iActual, double[] iExpected)
        {
            Assert.AreEqual<int>(iActual.Length, iExpected.Length);
            for (int i = 0; i < iExpected.Length; i++)
                Assert.AreEqual<double>(iExpected[i], iActual[i]);
        }
    }
}
