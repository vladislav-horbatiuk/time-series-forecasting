﻿using System;
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
    /// Summary description for TestSuccessorsForComponentMethod
    /// </summary>
    [TestClass]
    public class TestPredecessorsForComponentMethod
    {
        public TestPredecessorsForComponentMethod()
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
        public void ReturnsPredecessorsCorrectly()
        {

            (new TestAddPredecessorsForComponentMethod()).AddsPredecessorsSuccessfully();
        }

        [TestMethod]
        public void ReturnsEmptyList()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            BSManager.AddComponent(componentMock.Object);
            IList<BSBaseComponent> receivedPredecessors = BSManager.PredecessorsForComponent(componentMock.Object);
            Assert.AreEqual(0, receivedPredecessors.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ThrowsKeyNotFoundException()
        {
            Mock<BSBaseComponent> componentMock = new Mock<BSBaseComponent>();
            BSManager.PredecessorsForComponent(componentMock.Object);
        }
    }
}
