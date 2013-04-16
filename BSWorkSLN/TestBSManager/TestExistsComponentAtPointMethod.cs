using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using BSWork;

namespace TestBSManager
{
    /// <summary>
    /// Summary description for TestExistsComponentAtPointMethod
    /// </summary>
    [TestClass]
    public class TestExistsComponentAtPointMethod
    {
        private static Random rnd = new Random();
        private static int width = 100, height = 100;
        public TestExistsComponentAtPointMethod()
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
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            Rectangle componentRectangle = new Rectangle(0, 0, width, height);
            string randomComponentName = BSManager.ComponentsNamesDict.Keys.ElementAt(rnd.Next(BSManager.ComponentsNamesDict.Keys.Count));
            BSManager.AddComponentWithNameAndRectangle(randomComponentName, componentRectangle);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            BSManager.ClearComponentsList();
        }
        //
        #endregion

        [TestMethod]
        public void WorksCorrectlyIfComponentExists()
        {
            const int numOfTries = 50;

            for (int i = 0; i < numOfTries; ++i)
            {
                Point pointInsideRect = new Point(rnd.Next(width), rnd.Next(height));
                Assert.IsTrue(BSManager.ExistsComponentAtPoint(pointInsideRect));
            }
        }

        [TestMethod]
        public void WorksCorrectlyIfNoComponentMatches()
        {
            const int numOfTries = 50;
            int j;
            for (int i = 0; i < numOfTries; ++i)
            {
                j = rnd.Next(2);
                Point pointOutsideRect = new Point(rnd.Next(width) + width * j, rnd.Next(height) + height * (1 - j));
                Assert.IsFalse(BSManager.ExistsComponentAtPoint(pointOutsideRect));
            }
        }
    }
}
