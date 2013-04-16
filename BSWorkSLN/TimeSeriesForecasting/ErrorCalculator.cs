using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public static class ErrorCalculator
        {
            public static double CalculateMSE(IForecastingModel iModel, double[][] iInput, double[][] iOutput)
            {
                if (null == iModel || null == iInput || null == iOutput)
                    throw new ArgumentNullException();
                long length = iInput.LongLength;
                if (length != iOutput.LongLength)
                    throw new ArgumentException();

                double error = 0;
                for (int i = 0;i < length;++i)
                {
                    double[] row = iInput[i], expectedOutput = iOutput[i];
                    if (row.LongLength != iModel.InputSize)
                        throw new ArgumentException();
                    double[] modelOutput = iModel.CalculateOutput(row);
                    error += CalculateMSE(modelOutput, expectedOutput);
                }
                return error / length;
            }

            public static double CalculateMSE(double[] iActual, double[] iExpected)
            {
                if (null == iActual || null == iExpected)
                    throw new ArgumentNullException();
                long length = iActual.LongLength;
                if (length != iExpected.LongLength)
                    throw new ArgumentException();
                return iActual.Zip(iExpected, (actual, expected) => (actual - expected) * (actual - expected)).Sum() / length;
            }
        }
    }
}
