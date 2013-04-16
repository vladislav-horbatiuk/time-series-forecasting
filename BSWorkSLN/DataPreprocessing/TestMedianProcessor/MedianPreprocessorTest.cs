using DataPreprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BSWork.DataObjects;

namespace TestMedianProcessor
{
    
    
    /// <summary>
    ///This is a test class for MedianPreprocessorTest and is intended
    ///to contain all MedianPreprocessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MedianPreprocessorTest
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
        ///A test for median class Process, checks for ordinary correctness
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            MedianPreprocessor target = new MedianPreprocessor(5); 
            double[] iArray = { 1, 2, 3, 10, 5, 7, 4, 2, 12, 3, 10 };
            double[] expected = { 3, 5, 5, 5, 5, 4, 4 };
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            target = new MedianPreprocessor(3);
            iArray = new double[] { 1, 2, 3, 4, 5 };
            expected = new double[] { 2, 3, 4 };
            actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
        }

        private static void CheckTwoOrderedArrays(double[] iActual, double[] iExpected)
        {
            Assert.AreEqual<int>(iActual.Length, iExpected.Length);
            for (int i = 0; i < iExpected.Length; i++)
                Assert.AreEqual<double>(iExpected[i], iActual[i]);
        }

        /// <summary>
        ///A test for MedianPreprocessor Constructor
        ///</summary>
        [TestMethod()]
        public void MedianPreprocessorConstructorTest()
        {
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int iWindowSize = rnd.Next(); 
                MedianPreprocessor target = new MedianPreprocessor(iWindowSize);
                Assert.AreEqual<int>(iWindowSize, target.WindowSize);
            }
        }

        /// <summary>
        ///A test to check that MedianPreprocessor Constructor accepts only positive integers
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MedianPreprocessorConstructorWindowSizeOnlyPositiveTest()
        {
            int iWindowSize = -10;
            MedianPreprocessor target = new MedianPreprocessor(iWindowSize);
        }

        /// <summary>
        ///A test for Process method, checking if it deals correctly with even window sizes
        ///</summary>
        [TestMethod()]
        public void ProcessMedianForEvenWindowSizeTest()
        {
            MedianPreprocessor target = new MedianPreprocessor(4); 
            double[] iArray = {4, 1, 2, 5, 6, 7, 10, 3, 5, 2}; 
            double[] expected = {2, 2, 5, 6, 6, 5, 3};
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
        }

        /// <summary>
        ///A test for median Process method, checks if it deals correctly with unordinary situations (window size >= input vector size)
        ///</summary>
        [TestMethod()]
        public void ProcessUnordinaryWindowSizesTest()
        {
            MedianPreprocessor target = new MedianPreprocessor(6);
            double[] iArray = {1, 2, 3, 4, 5, 6}; 
            double[] expected = {3};
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            target = new MedianPreprocessor(5);
            iArray = new double[] { 1, 2, 3, 4};
            actual = target.Process(new BSDataObject(iArray));
            Assert.AreEqual<int>(0, actual.DataArray.Length);
        }
    }
}
