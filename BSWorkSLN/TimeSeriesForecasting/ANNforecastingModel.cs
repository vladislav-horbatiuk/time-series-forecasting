using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;

namespace BSWork
{
namespace TimeSeriesForecasting
{
    public class ANNforecastingModel : DataObjects.BSDataObject, IForecastingModel
    {
        public ANNforecastingModel(Network iNet, DataNormalizer iNormalizer)
            : base(null)
        {
            InputSize = iNet.InputsCount;
            forecastingNet = iNet;
            normalizer = iNormalizer;
        }

        public double[] CalculateOutput(double[] iInput)
        {
            return normalizer.DeNormalize(forecastingNet.Compute(normalizer.Normalize(iInput)));
        }

        public long InputSize { get; set; }
        public IInputProvider InputProvider { get; set; }
        private Network forecastingNet;
        private DataNormalizer normalizer;
    }
}
}
