using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSWork.DataObjects;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class GMDHANNforecastingModel : BSDataObject, IForecastingModel
        {
            public GMDHANNforecastingModel()
                : base(null)
            {
            }

            public double[] CalculateOutput(double[] iInput)
            {
                throw new NotImplementedException();
            }

            public long InputSize
            {
                get
                {
                    return inputSize;
                }
                set
                {
                    inputSize = value;
                }
            }

            public IInputProvider InputProvider { get; set; }
            private long inputSize;
        }
    }
}