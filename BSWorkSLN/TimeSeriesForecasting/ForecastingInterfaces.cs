using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        /// <summary>
        /// 
        /// </summary>
        public interface IForecastingModelBuilder
        {
            IForecastingModel TrainNewModel(double[][] iInput, double[][] iOutput);
            Dictionary<string, object> ModelParametersDict { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IForecastingModel
        {
            double[] CalculateOutput(double[] iInput);
            long InputSize { get; set; }
            IInputProvider InputProvider { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IForecaster
        {
            double[][] CalculateOutput(double[][] iInput);
        }

        public interface IInputProvider
        {
            void GenerateInputAndOutput(double[] iTimeSeries, out double[][] oInput, out double[][] oOutput);
        }
    }
}
