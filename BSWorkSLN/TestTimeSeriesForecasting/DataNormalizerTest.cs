using BSWork.TimeSeriesForecasting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestTimeSeriesForecasting
{   
    
    /// <summary>
    ///This is a test class for TestDataNormalizer and is intended
    ///to contain all TestDataNormalizer Unit Tests
    ///</summary>
    [TestClass()]
    public class TestDataNormalizer
    {
        const double epsilon = 0.00001;

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
        ///A test for Normalize
        ///</summary>
        [TestMethod()]
        public void GeneralCorrectnessTest()
        {
            double[][] iData = new double[][]
                {
                    new double[] { 0, 2, 5},
                    new double[] {10, 3, 3},
                    new double[] {6, 2, 1}
                };
            double a = 0, b = 1;
            double[][] expected = new double[][]
                {
                    new double[] {0.0, 0.2, 0.5},
                    new double[] {1.0, 0.3, 0.3},
                    new double[] {0.6, 0.2, 0.1}

                };
            double[][] actual;
            DataNormalizer target = new DataNormalizer(iData, a, b);
            actual = target.Normalize(iData);
            AssertMatrixAreEqual(expected, actual);
        }

        private static void AssertMatrixAreEqual(double[][] expected, double[][] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual(expected[i].Length, actual[i].Length);
                for (int j = 0; j < actual[i].Length; ++j)
                    Assert.AreEqual(expected[i][j], actual[i][j], epsilon);
            }
        }

        /// <summary>
        ///A general test for DeNormalize
        ///</summary>
        [TestMethod()]
        public void DeNormalizeGeneralCorrectnessTest()
        {
            int m = 10000, n = 100;
            double epsilon = 0.000000001;
            Random rnd = new Random();
            double[][] iData = new double[m][];
            for (int i = 0; i < m; ++i)
            {
                iData[i] = new double[n];
                for (int j = 0; j < n; ++j)
                    iData[i][j] = rnd.Next();
            }
            double iA = 0.1;
            double iB = 0.9;
            DataNormalizer target = new DataNormalizer(iData, iA, iB);
            double[][] normalized = target.Normalize(iData);
            Assert.AreEqual(iA, normalized.Min(row => row.Min()), epsilon);
            Assert.AreEqual(iB, normalized.Max(row => row.Max()), epsilon);
            double maxAllowedError = 0.1, expectedAvg = 0.5, actualAvg = normalized.Average(row => row.Average());
            Assert.AreEqual(expectedAvg, actualAvg, maxAllowedError);
            double[][] actual = target.DeNormalize(normalized);
            AssertMatrixAreEqual(iData, actual);
        }
    }
}
