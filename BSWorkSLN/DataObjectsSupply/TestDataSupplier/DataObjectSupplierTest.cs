using DataObjectsSupply;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using BSWork.DataObjects;

namespace TestDataSupplier
{


    /// <summary>
    ///This is a test class for IBSDataObjectSupplierTest and is intended
    ///to contain all IBSDataObjectSupplierTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BSDataObjectSupplierTest
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
        ///A test for GetBSDataObjectForStream, tests its general correctness.
        ///</summary>
        [TestMethod()]
        public void GetBSDataObjectForStreamTest()
        {
            const int numberOfPoints = 360;
            double[] expectedArray = new double[numberOfPoints];
            string fileName = Path.GetTempFileName();
            Random rnd = new Random();
            //Create a file with random values in it
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var streamWriter = new StreamWriter(fileStream);
                for (int i = 0; i < numberOfPoints; i++)
                {
                    expectedArray[i] = rnd.Next();
                    streamWriter.WriteLine(expectedArray[i]);
                }
                streamWriter.Flush();
            }
            //Get BSDataObject from this file
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                BSDataObject target = DataObjectSupplier.GetDataObjectForStream(fileStream);
                CollectionAssert.AreEqual(expectedArray, target.DataArray);
            }
            File.Delete(fileName);

            //The same for memory stream
            using (var memoryStream = new MemoryStream(numberOfPoints * (sizeof(double) + sizeof(char))))
            {
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                for (int i = 0; i < numberOfPoints; i++)
                {
                    expectedArray[i] = Math.Sin((double)i * Math.PI / 180.0);
                    streamWriter.WriteLine(expectedArray[i]);
                }
                streamWriter.Flush();
                memoryStream.Position = 0;
                BSDataObject target = DataObjectSupplier.GetDataObjectForStream(memoryStream);
                CheckTwoOrderedArrays(expectedArray, target.DataArray);
            }
        }

        private void CheckTwoOrderedArrays(double[] iExpected, double[] iActual, double delta = 0.0000000001)
        {
            Assert.AreEqual<int>(iExpected.Length, iActual.Length);
            for (int i = 0; i < iExpected.Length; i++)
                Assert.AreEqual(iExpected[i], iActual[i], delta);
        }
    }
}
