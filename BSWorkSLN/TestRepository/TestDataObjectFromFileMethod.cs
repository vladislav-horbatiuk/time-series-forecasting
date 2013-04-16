using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSWork;
using BSWork.DataObjects;
using System.Security.AccessControl;

namespace TestRepository
{
    /// <summary>
    /// Summary description for TestDataObjectFromFileMethod
    /// </summary>
    [TestClass]
    public class TestDataObjectFromFileMethod
    {

        public TestDataObjectFromFileMethod()
        {
            //
            // TODO: Add constructor logic here
            //
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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        
        /// <summary>
        /// Basic test for Repository.DataObjectFromFile method.
        /// Checks whether it creates correct DataObject for a given file.
        /// </summary>
        [TestMethod]
        public void General()
        {
            string fileName = Path.GetTempFileName();
            Random rnd = new Random();
            const int numbersAmount = 100;
            double[] expected = new double[numbersAmount];
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                for (int i = 0; i < expected.Count(); ++i)
                {
                    expected[i] = rnd.Next(int.MinValue, int.MaxValue);
                    sw.WriteLine(expected[i]);
                }
            }
            BSDataObject result = BSRepository.DataObjectFromFile(fileName);
            File.Delete(fileName);
            CollectionAssert.AreEqual(expected, result.DataArray);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ThrowsArgumentNullExceptionForNullArgument()
        {
            BSDataObject doesntMatter = BSRepository.DataObjectFromFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void ThrowsFileNotFoundException()
        {
            string fileName = Path.GetTempFileName();
            File.Delete(fileName);
            BSDataObject doesntMatter = BSRepository.DataObjectFromFile(fileName);
        }

    }
}
