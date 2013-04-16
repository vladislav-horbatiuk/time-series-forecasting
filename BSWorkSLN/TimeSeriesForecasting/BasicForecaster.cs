using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class BasicForecaster : IForecaster
        {
            public BasicForecaster(IForecastingModel iModel)
            {
                model = iModel;
            }

            public double[][] CalculateOutput(double[][] iInput)
            {
                if (null == iInput)
                    throw new ArgumentNullException();
                long length = iInput.LongLength;
                double[][] result = new double[length][];
                for (int i = 0; i < length; ++i)
                {
                    result[i] = model.CalculateOutput(iInput[i]);
                }
                return result;
            }

            private IForecastingModel model;
        }
    }
}
