using BSWork.TimeSeriesForecasting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BSWork.DataObjects;
using BSWork.DataVisualization;

namespace TestTimeSeriesForecasting
{
    
    
    /// <summary>
    ///This is a test class for WindowingInputProviderTest and is intended
    ///to contain all WindowingInputProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WindowingInputProviderTest
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
        /// A test for GenerateInputAndOutput
        ///</summary>
        //[TestMethod()]
        public void GenerateInputAndOutputTest()
        {
            uint iLag = 1;
            uint iWindowSize = 5;
            int pointsNum = 360;
            IInputProvider target = new WindowingInputProvider(iLag, iWindowSize);
            Random rnd = new Random();
            double[] iTimeSeries = new double[pointsNum];
            for (int i = 0; i < pointsNum; i++)
            {
                iTimeSeries[i] = 1 * Math.Sin(2 * i * Math.PI / 180.0) + (Convert.ToDouble(rnd.Next(-2, 2)) / 5) * ((i % 3 == 0) ? 1 : 0);
            }
            double[][] oInput = null;
            double[][] oOutput = null;
            target.GenerateInputAndOutput(iTimeSeries, out oInput, out oOutput);
            IForecastingModelBuilder modelBuilder = new ANNmodelBuilder();
            IForecastingModel model = modelBuilder.TrainNewModel(oInput, oOutput);

            double[] forecast = new double[oInput.LongLength], actual = new double[oInput.LongLength];
            for (int i = 0; i < oInput.LongLength; ++i)
            {
                forecast[i] = model.CalculateOutput(oInput[i])[0]; actual[i] = oOutput[i][0];
            }

            BSDataObject forecastDO = new BSDataObject(forecast, "Forecast"), actualDO = new BSDataObject(actual, "Actual");

            SimpleVisualizationHandler handler = new SimpleVisualizationHandler();


            handler.AddToGraph(actualDO);
            handler.AddToGraph(forecastDO);
            handler._TestShow();
        }
    }
}
