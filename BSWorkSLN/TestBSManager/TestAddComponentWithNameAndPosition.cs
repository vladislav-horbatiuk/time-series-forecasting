using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BSWork;
using BSWork.BSComponents;
using System.Drawing;

namespace TestBSManager
{
    /// <summary>
    /// Summary description for TestAddComponentWithNameAndPosition
    /// </summary>
    [TestClass]
    public class TestAddComponentWithNameAndPosition
    {
        public TestAddComponentWithNameAndPosition()
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
        public void AddsNewComponentCorrectly()
        {
            Random rnd = new Random();
            string randomComponentName = BSManager.ComponentsNamesDict.Keys.ElementAt(rnd.Next(BSManager.ComponentsNamesDict.Keys.Count));
            IComponentsFactory correspondingComponentFactory = BSManager.ComponentsNamesDict[randomComponentName];
            Rectangle componentRectangle = new Rectangle(rnd.Next(), rnd.Next(), rnd.Next(), rnd.Next());
            BSBaseComponent expectedComp = correspondingComponentFactory.NewComponent(componentRectangle);
            BSManager.AddComponentWithNameAndRectangle(randomComponentName, componentRectangle);
            IList<BSBaseComponent> actualCompsList = BSManager.Components;
            Assert.AreEqual(1, actualCompsList.Count);
            BSBaseComponent actualComp = actualCompsList.First();
            Assert.AreEqual(expectedComp.GetType(), actualComp.GetType());
            Assert.AreEqual(componentRectangle, actualComp.OnScreenRectangle);
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        [TestMethod]
        public void ThrowsKeyNotFoundException()
        {
            Random rnd = new Random();
            string notPresentKey;
            while (BSManager.ComponentsNamesDict.ContainsKey(notPresentKey = Convert.ToString(rnd.Next()))) ;
            BSManager.AddComponentWithNameAndRectangle(notPresentKey, new Rectangle());
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ThrowsArgumentNullException()
        {
            BSManager.AddComponentWithNameAndRectangle(null, new Rectangle());
        }

    }
}
