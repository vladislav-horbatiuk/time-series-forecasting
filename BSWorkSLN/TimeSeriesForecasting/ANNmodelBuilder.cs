using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using AForge.Neuro;
using AForge.Neuro.Learning;

using TimeSeriesForecasting.Properties;

namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class ANNmodelBuilder : IForecastingModelBuilder
        {
            public ANNmodelBuilder()
            {
                ModelParametersDict = new Dictionary<string, object>()
                {
                    {NeuronsInLayersKey, kDefaultNeuronsCount},
                    {ActivationFunctionKey, kDefaultActivationFunction},
                    {MaxIterationsNumberKey, kMaxInterationsNum},
                    {StopErrorKey, kStopError},
                };
            }

            public IForecastingModel TrainNewModel(double[][] iInput, double[][] iOutput)
            {
                int inputSize = iInput[0].Length, samplesNum = iOutput.Length;
                if (samplesNum != iInput.Length)
                    throw new ArgumentException();

                for (int i = 0; i < samplesNum;++i)
                    if (iInput[i].Length != inputSize || iOutput[i].Length != 1) //iInput isn't a square matrix or iOutput isn't a vector
                        throw new ArgumentException();

                int[] neuronsCount = (int[]) ModelParametersDict[NeuronsInLayersKey];
                string activationFunction = (string) ModelParametersDict[ActivationFunctionKey];
                long maxIterNum = (long) ModelParametersDict[MaxIterationsNumberKey];
                double stopError = (double)ModelParametersDict[StopErrorKey];

                ActivationNetwork netToTrain = new ActivationNetwork(ActivationFunctionsDict[activationFunction], inputSize, neuronsCount);
                DataNormalizer normalizer = new DataNormalizer(iInput.Concat(iOutput).ToArray());
                IForecastingModel aModel = new ANNforecastingModel(netToTrain, normalizer);
                ISupervisedLearning teacher = new ResilientBackpropagationLearning(netToTrain);

                double[][] trainInputSet, trainOutputSet;
                TrainingSubsetGenerator.GenerateRandomly(iInput, iOutput, out trainInputSet, out trainOutputSet, iMultiplier: TrainSubsetMultiplier);

                trainInputSet = normalizer.Normalize(trainInputSet); trainOutputSet = normalizer.Normalize(trainOutputSet);

                long epochsCount = 0;
                double nextError = ErrorCalculator.CalculateMSE(aModel, iInput, iOutput), prevError;
                do
                {
                    prevError = nextError;
                    teacher.RunEpoch(trainInputSet, trainOutputSet);
                    nextError = ErrorCalculator.CalculateMSE(aModel, iInput, iOutput);
                }
                while (epochsCount++ <= maxIterNum && Math.Abs(prevError - nextError) >= stopError);
                return aModel;
            }

            public const string NeuronsInLayersKey = "NeuronsCount";
            public const string ActivationFunctionKey = "ActivationFunction";
            public const string MaxIterationsNumberKey = "MaxIterNum";
            public const string StopErrorKey = "StopError";

            public static readonly int[] kDefaultNeuronsCount = {8, 1};
            // Network training stops, when |next error - previous error| < stopError.
            public const double kStopError = 0.000001;
            public const long kMaxInterationsNum = 10000;
            public double TrainSubsetMultiplier = 0.7;

            public static readonly Dictionary<string, IActivationFunction> ActivationFunctionsDict = new Dictionary<string,IActivationFunction>
            {
                {strings.sigmoid, new SigmoidFunction()},
                {strings.threshold, new ThresholdFunction()},
                {strings.bipolarSigmoid, new BipolarSigmoidFunction()},
            };

            public static string kDefaultActivationFunction = strings.bipolarSigmoid;

            public Dictionary<string, object> ModelParametersDict { get; set; }
        }
    }
}