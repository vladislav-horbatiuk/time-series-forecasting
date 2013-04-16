using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BSWork;
using BSWork.BSComponents;

namespace TestBSManager
{
    /// <summary>
    /// Summary description for TestAddComponentMethod
    /// </summary>
    [TestClass]
    public class TestAddComponentMethod
    {
        public TestAddComponentMethod()
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
        [TestCleanup()]
        public void MyTestCleanup() { BSManager.ClearComponentsList(); }
        
        #endregion

        [TestMethod]
        public void AddsNewComponentSuccessfully()
        {
            foreach (int i in Enumerable.Range(1, new Random().Next(10)))
            {
                Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
                BSManager.AddComponent(componentMock.Object);
                Assert.AreEqual(i, BSManager.Components.Count);
                Assert.IsTrue(BSManager.Components.Contains(componentMock.Object));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsArgumentNullException()
        {
            BSManager.AddComponent(null);
        }
    }
}
