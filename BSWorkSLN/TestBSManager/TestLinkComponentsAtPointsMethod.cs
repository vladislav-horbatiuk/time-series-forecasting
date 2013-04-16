using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BSWork;
using System.Drawing;
using BSWork.BSComponents;

namespace TestBSManager
{
    /// <summary>
    /// Summary description for TestLinkComponentsAtPoints
    /// </summary>
    [TestClass]
    public class TestLinkComponentsAtPointsMethod
    {
        Random rnd = new Random();
        const int width = 100, height = 100, distanceBetweenRects = 40;
        string inputComponentName = "input", preprocessorComponentName = "preprocessor";
        public TestLinkComponentsAtPointsMethod()
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
        public void LinksComponentsSuccessfullyIfEverythingIsGood()
        {
            BSManager.AddComponentWithNameAndRectangle(inputComponentName, new Rectangle(0, 0, width, height));
            BSBaseComponent input = BSManager.Components[0];
            BSManager.AddComponentWithNameAndRectangle(preprocessorComponentName, new Rectangle(width + distanceBetweenRects, 0, width, height));
            BSBaseComponent preprocessor = BSManager.Components[1];

            BSManager.LinkComponentsAtPoints(new Point(rnd.Next(width), rnd.Next(height)),
                new Point(rnd.Next(width + distanceBetweenRects, 2 * width + distanceBetweenRects), rnd.Next(height)));
            IList<BSBaseComponent> actualSuccs = BSManager.SuccessorsForComponent(input), actualPreds = BSManager.PredecessorsForComponent(preprocessor);
            Assert.AreEqual(1, actualSuccs.Count); Assert.AreEqual(1, actualPreds.Count);
            Assert.AreSame(preprocessor, actualSuccs[0]);
            Assert.AreSame(input, actualPreds[0]);
        }

        [ExpectedException(typeof(ComponentsCantBeLinkedException))]
        [TestMethod]
        public void ThrowsComponentsCantBeLinkedExceptionIfComponentsAreOfIncorrectType()
        {
            BSManager.AddComponentWithNameAndRectangle(inputComponentName, new Rectangle(0, 0, width, height));
            BSManager.AddComponentWithNameAndRectangle(preprocessorComponentName, new Rectangle(width + distanceBetweenRects, 0, width, height));
            BSManager.LinkComponentsAtPoints(new Point(rnd.Next(width + distanceBetweenRects, 2 * width + distanceBetweenRects), rnd.Next(height)),
                new Point(rnd.Next(width), rnd.Next(height)));
        }

        [ExpectedException(typeof(ComponentsCantBeLinkedException))]
        [TestMethod]
        public void ThrowsComponentsCantBeLinkedExceptionIfComponentsHaveBeenLinkedBefore()
        {
            BSManager.AddComponentWithNameAndRectangle(inputComponentName, new Rectangle(0, 0, width, height));
            BSManager.AddComponentWithNameAndRectangle(preprocessorComponentName, new Rectangle(width + distanceBetweenRects, 0, width, height));

            BSManager.LinkComponentsAtPoints(new Point(rnd.Next(width), rnd.Next(height)),
                new Point(rnd.Next(width + distanceBetweenRects, 2 * width + distanceBetweenRects), rnd.Next(height)));
            BSManager.LinkComponentsAtPoints(new Point(rnd.Next(width), rnd.Next(height)),
                new Point(rnd.Next(width + distanceBetweenRects, 2 * width + distanceBetweenRects), rnd.Next(height)));
        }
    }
}
