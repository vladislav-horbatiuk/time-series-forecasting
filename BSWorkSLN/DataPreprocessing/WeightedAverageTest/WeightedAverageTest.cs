using DataPreprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using BSWork.DataObjects;

namespace WeightedAverageTest
{
    
    
    /// <summary>
    ///This is a test class for WeightedAverageTest and is intended
    ///to contain all WeightedAverageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WeightedAverageTest
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
        ///A test for WeightedAverage Constructor, tests that it throws exception if window size != weights array size
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WeightedAverageConstructorThrowsExceptionTest()
        {
            int iWindowSize = 3;
            double[] iWeights = {1, 2, 3, 4, 5, 6};
            WeightedAveragePreprocessor target = new WeightedAveragePreprocessor(iWindowSize, iWeights);
            FieldInfo info = target.GetType().GetField("weights", BindingFlags.Instance | BindingFlags.NonPublic);
            CheckTwoOrderedArrays((double[]) info.GetValue(target), iWeights);
        }


        /// <summary>
        ///A general test for WeightedAverage Constructor, checks that it works correctly in general
        ///</summary>
        [TestMethod()]
        public void WeightedAverageConstructorGeneralTest()
        {
            int iWindowSize = 6;
            double[] iWeights = { 1, 2, 3, 4, 5, 6 };
            WeightedAveragePreprocessor target = new WeightedAveragePreprocessor(iWindowSize, iWeights);
            FieldInfo info = target.GetType().GetField("weights", BindingFlags.Instance | BindingFlags.NonPublic);
            CheckTwoOrderedArrays((double[])info.GetValue(target), iWeights);
            target = new WeightedAveragePreprocessor(10);
            info = target.GetType().GetField("weights", BindingFlags.Instance | BindingFlags.NonPublic);
            CheckTwoOrderedArrays((double[])info.GetValue(target), new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        }


        /// <summary>
        ///A test for WeightedAverage Constructor, checks that it calculates WeightSum correctly when different weights arrays are passed to constructor
        ///</summary>
        [TestMethod()]
        public void WeightedAverageConstructorCalculatesWeightsSumCorrectlyTest()
        {
            int iWindowSize = 6;
            double[] iWeights = { 1, 2, 3, 4, 5, 6 };
            WeightedAveragePreprocessor target = new WeightedAveragePreprocessor(iWindowSize, iWeights);
            Assert.AreEqual(1 + 2 + 3 + 4 + 5 + 6, target.WeightsSum);
            target = new WeightedAveragePreprocessor(10);
            Assert.AreEqual(10, target.WeightsSum);
            target = new WeightedAveragePreprocessor();
            Assert.AreEqual(5, target.WeightsSum);
        }


        /// <summary>
        ///A test for Weighted average Process method, checks general correctness of the preprocessor
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            WeightedAveragePreprocessor target = new WeightedAveragePreprocessor(4, new double[] {1, 2, 3, 4});
            double[] iArray = {1, 3, 8, 5, 2, 2, 9, 4, 6};
            double[] expected = {5.1, 4.2, 3.2, 5.1, 4.9, 5.6};
            BSDataObject actual = target.Process(new BSDataObject(iArray));
            CheckTwoOrderedArrays(actual.DataArray, expected);
            target = new WeightedAveragePreprocessor();
            iArray = new double[] {1, 2, 3, 4, 5, 6, 7};
            expected = new double[] { 3, 4, 5 };
            actual = target.Process(new BSDataObject(iArray));
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
