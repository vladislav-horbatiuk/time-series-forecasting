using DataPreprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using BSWork.DataObjects;

namespace TestFourierPreprocessor
{
    
    
    /// <summary>
    ///This is a test class for FourierPreprocessorTest and is intended
    ///to contain all FourierPreprocessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FourierPreprocessorTest
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
        ///A test for Process
        ///</summary>
        [TestMethod()]
        public void ProcessTest()
        {
            Random rnd = new Random();
            FourierPreprocessor target = new FourierPreprocessor();
            const int n = 360, amplitude = 80;
            int freq = freq = rnd.Next(1, 5);
            double[] array = new double[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = amplitude * Math.Sin((double)freq * i * Math.PI / 180.0);
            }
            BSDataObject iObject = new BSDataObject(array, "SourceData");
            BSDataObject expected = new BSDataObject((from i in Enumerable.Range(0, n) select (i == freq || i ==  n - freq) ? amplitude / 2.0 : 0).ToArray());
            BSDataObject actual = target.Process(iObject);
            BSDataObject inversed = new InverseFourierPreprocessor().Process(expected);
            CheckTwoOrderedArrays(expected.DataArray, actual.DataArray, delta: 0.000001);
        }

        private void CheckTwoOrderedArrays(double[] iExpected, double[] iActual, double delta = 0.0000000001)
        {
            Assert.AreEqual<int>(iExpected.Length, iActual.Length);
            for (int i = 0; i < iExpected.Length; i++)
                Assert.AreEqual(iExpected[i], iActual[i], delta);
        }
    }
}
