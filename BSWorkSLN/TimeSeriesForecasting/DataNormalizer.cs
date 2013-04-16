using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class DataNormalizer
        {
            public DataNormalizer(double iA, double iB, double iMin, double iMax)
            {
                k = iB - iA; r = iB * iMin - iA * iMax; min = iMin; max = iMax;
            }

            public DataNormalizer(double[][] iData, double iA = 0.1, double iB = 0.9)
                : this(iA, iB, iData.Min(row => row.Min()), iData.Max(row => row.Max()))
            {
            }

            public DataNormalizer(double[] iData, double iA = 0.1, double iB = 0.9)
                : this(iA, iB, iData.Min(), iData.Max())
            {
            }

            public double[] Normalize(double[] iData)
            {
                double[] result = new double[iData.LongLength];
                for (long i = 0; i < result.LongLength; ++i)
                    result[i] = (k * iData[i] - r) / (max - min);
                return result;
            }

            public double[][] Normalize(double[][] iData)
            {
                double[][] result = new double[iData.LongLength][];
                for (long i = 0; i < result.LongLength; ++i)
                    result[i] = Normalize(iData[i]);
                return result;
            }

            public double[] DeNormalize(double[] iData)
            {
                double[] result = new double[iData.LongLength];
                for (long i = 0; i < result.LongLength; ++i)
                    result[i] = (iData[i] * (max - min) + r) / k;
                return result;
            }

            public double[][] DeNormalize(double[][] iData)
            {
                double[][] result = new double[iData.LongLength][];
                for (long i = 0; i < result.LongLength; ++i)
                    result[i] = DeNormalize(iData[i]);
                return result;
            }

            public double K
            {
                get { return k; }
            }

            public double R
            {
                get { return r; }
            }

            public double Min
            {
                get { return min; }
            }
            public double Max
            {
                get { return max; }
            }
            private double k, r, min, max;
        }
    }
}
