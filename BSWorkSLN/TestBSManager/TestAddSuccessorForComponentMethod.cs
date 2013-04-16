using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSWork;
using BSWork.BSComponents;
using Moq;

namespace TestBSManager
{
    /// <summary>
    /// Summary description for TestAddSuccessorForComponentMethod
    /// </summary>
    [TestClass]
    public class TestAddSuccessorForComponentMethod
    {
        public TestAddSuccessorForComponentMethod()
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
        //
        #endregion

        [TestMethod]
        public void AddsSuccessorSuccessfully()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            Mock<BSBaseComponent> successorMock = new Mock<BSBaseComponent>();
            BSManager.AddComponent(componentMock.Object);
            BSManager.AddComponent(successorMock.Object);
            BSManager.AddSuccessorForComponent(componentMock.Object, successorMock.Object);
            IList<BSBaseComponent> receivedSuccs = BSManager.SuccessorsForComponent(componentMock.Object);
            Assert.IsTrue(receivedSuccs.Contains(successorMock.Object));
            Assert.AreEqual(1, receivedSuccs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsArgumentException()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            Mock<BSBaseComponent> successorMock = new Mock<BSBaseComponent>();
            BSManager.AddComponent(componentMock.Object);
            BSManager.AddSuccessorForComponent(componentMock.Object, successorMock.Object);
        }
    }
}
