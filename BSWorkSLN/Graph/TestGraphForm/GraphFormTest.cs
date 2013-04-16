using Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TestGraphForm
{
    
    
    /// <summary>
    ///This is a test class for GraphFormTest and is intended
    ///to contain all GraphFormTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphFormTest
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
        ///A test for AddGraph
        ///</summary>
        //[TestMethod()]
        public void AddGraphTest()
        {
            Random rnd = new Random();
            GraphForm target = new GraphForm();
            double[] iDataArray = new double[100];
            for (int i = 0; i < 100; i++)
                iDataArray[i] = (double)rnd.Next(-100, 100) / 10.0;
            ConsoleColor iColor = ConsoleColor.Green; 
            string iName = "Random #1"; 
            target.AddGraph(iDataArray, iColor,iName : iName, iOffset: -10);
            iDataArray = new double[50];
            for (int i = 0; i < 50; i++)
                iDataArray[i] = (double)rnd.Next(-50, 50) / 10.0;
            iColor = ConsoleColor.Red;
            iName = "Random #2";
            target.AddGraph(iDataArray, iColor, iName : iName, iOffset: 30.0, iInterval:0.5);
            while (target.ShowDialog() == DialogResult.None)
                ;
        }
    }
}
