using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class WindowingInputProvider : IInputProvider
        {
            public WindowingInputProvider(uint iLag, uint iWindowSize)
            {
                lag = iLag; windowSize = iWindowSize;
            }

            public void GenerateInputAndOutput(double[] iTimeSeries, out double[][] oInput, out double[][] oOutput)
            {
                long length = iTimeSeries.LongLength;
                long samplesNum = length - lag - windowSize + 1;
                oInput = new double[samplesNum][];
                oOutput = new double[samplesNum][];
                for (int i = 0; i < samplesNum; ++i)
                {
                    oInput[i] = new double[windowSize];
                    oInput[i] = iTimeSeries.Skip(i).Take((int)windowSize).ToArray();
                    oOutput[i] = new double[1];
                    oOutput[i][0] = iTimeSeries[i + lag + windowSize - 1];
                }
            }

            private uint lag, windowSize;
        }
    }
}
