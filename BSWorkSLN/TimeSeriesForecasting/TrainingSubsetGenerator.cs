using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public static class TrainingSubsetGenerator
        {
            public static void GenerateRandomly(double[][] iInput, double[][] iOutput, out double[][] oTrainInputSet, out double[][] oTrainOutputSet, double iMultiplier = 0.7)
            {
                int trainSize = (int)Math.Round(iInput.Length * iMultiplier);
                Random rnd = new Random();
                IEnumerable<int> shuffledIndices = Enumerable.Range(0, iInput.Length).Shuffle(rnd).Take(trainSize);
                oTrainInputSet = new double[trainSize][];
                oTrainOutputSet = new double[trainSize][];
                for (int i = 0; i < trainSize; ++i)
                {
                    oTrainInputSet[i] = iInput[shuffledIndices.ElementAt(i)];
                    oTrainOutputSet[i] = iOutput[shuffledIndices.ElementAt(i)];
                }
            }

            public static T[] Shuffle<T>(this IEnumerable<T> source, Random rng)
            {
                T[] elements = source.ToArray();
                for (int i = elements.Length - 1; i >= 0; i--)
                {
                    // Swap element "i" with a random earlier element it (or itself)
                    // ... except we don't really need to swap it fully, as we can
                    // return it immediately, and afterwards it's irrelevant.
                    int swapIndex = rng.Next(i + 1);
                    T tmp = elements[swapIndex];
                    elements[swapIndex] = elements[i];
                    elements[i] = tmp;
                }
                return elements;
            }
        }
    }
}
