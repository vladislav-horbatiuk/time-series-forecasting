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
    /// Summary description for TestAddPredecessorsForComponentMethod
    /// </summary>
    [TestClass]
    public class TestAddPredecessorsForComponentMethod
    {
        public TestAddPredecessorsForComponentMethod()
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
        public void AddsPredecessorsSuccessfully()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            Mock<BSBaseComponent> predecessorMock = new Mock<BSBaseComponent>();
            BSManager.AddComponent(componentMock.Object);
            BSManager.AddComponent(predecessorMock.Object);
            BSManager.AddPredecessorForComponent(componentMock.Object, predecessorMock.Object);
            IList<BSBaseComponent> receivedPredecessors = BSManager.PredecessorsForComponent(componentMock.Object);
            Assert.IsTrue(receivedPredecessors.Contains(predecessorMock.Object));
            Assert.AreEqual(1, receivedPredecessors.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsArgumentException()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            Mock<BSBaseComponent> successorMock = new Mock<BSBaseComponent>();
            BSManager.AddComponent(componentMock.Object);
            BSManager.AddPredecessorForComponent(componentMock.Object, successorMock.Object);
        }

    }
}
