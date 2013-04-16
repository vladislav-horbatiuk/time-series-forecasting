using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BSWork.DataObjects;
using AForge.Math;


namespace DataPreprocessing
{

    public interface IPreprocessor
    {
        BSDataObject Process(BSDataObject iObj);
    }

    /// <summary>
    /// A class for preprocessing data using discrete derivative (der(x) = y(x+1) - y(x)).
    /// Parameters: None.
    /// </summary>
    public class DerivativePreprocessor : IPreprocessor
    {
        public BSDataObject Process(BSDataObject iObj)
        {
            double[] iArray = iObj.DataArray;
            int count = iArray.Count();
            double[] result = new double[count - 1];
            for (int i = 0; i < count - 1; i++)
                result[i] = iArray[i + 1] - iArray[i];
            return new BSDataObject(result, iObj.ObjName + "_Derivate");
        }
    }

    /// <summary>
    /// An abstract class for all window-based preprocessors (like median filter, mean filter etc.)
    /// Parameters: window size - any positive (strictly greater than 0) number smaller than input vector size
    /// </summary>
    public abstract class WindowBasedPreprocessor : IPreprocessor
    {
        /// <summary>
        /// Default constructor, sets window size to 5
        /// </summary>
        public WindowBasedPreprocessor()
        {
        }

        public WindowBasedPreprocessor(int iWindowSize)
        {
            if (iWindowSize <= 0)
                throw new ArgumentOutOfRangeException("iWindowSize", "Value should be any positive integer (strictly greater than zero!)");
            WindowSize = iWindowSize;
        }

        public virtual BSDataObject Process(BSDataObject iObj)
        {
            return ProcessUsingWindowingFunc(iObj);
        }

        protected BSDataObject ProcessUsingWindowingFunc(BSDataObject iObj)
        {
            double[] iArray = iObj.DataArray;
            int count = iArray.Count();
            if (count < WindowSize)
                return new BSDataObject(new double[0], iObj.ObjName + "_" + this.GetType().Name);
            double[] result = new double[count - WindowSize + 1];
            for (int i = 0; i < count - WindowSize + 1; i++)
            {
                result[i] = windowProcessingFunc(iArray.Skip(i).Take(WindowSize));
            }
            return new BSDataObject(result, (WindowSize - 1) / 2, iObj.ObjName + "_" + this.GetType().Name.Replace("Preprocessor", ""));
        }

        protected abstract double windowProcessingFunc(IEnumerable<double> iArray);

        private int windowSize = 5;

        public int WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }
    }

    /// <summary>
    /// A class for preprocessing data using median filter
    /// Parameters: window size - any positive (strictly greater than 0) number smaller than input vector size
    /// In case of window size > input data size will return empty array
    /// </summary>
    public class MedianPreprocessor : WindowBasedPreprocessor
    {
        public MedianPreprocessor(int iWindowSize)
            : base(iWindowSize)
        {
        }

        /// <summary>
        /// Default constructor, sets window size to 5
        /// </summary>
        public MedianPreprocessor()
            : base()
        {
        }

        protected override double windowProcessingFunc(IEnumerable<double> iArray)
        {
            return iArray.OrderBy(value => value).ElementAt((iArray.Count() - 1) / 2);
        }

    }

    /// <summary>
    /// A class for preprocessing data using mean filter
    /// Parameters: window size - any positive (strictly greater than 0) number smaller than input vector size
    /// In case of window size > input data size will return empty array
    /// </summary>
    public class MeanPreprocessor : WindowBasedPreprocessor
    {

        public MeanPreprocessor(int iWindowSize)
            : base(iWindowSize)
        {
        }

        /// <summary>
        /// Default constructor, sets window size to 5
        /// </summary>
        public MeanPreprocessor()
            : base()
        {
        }

        protected override double windowProcessingFunc(IEnumerable<double> iArray)
        {
            return iArray.Sum() / iArray.Count();
        }
    }

    /// <summary>
    /// A class for preprocessing data using weighted average filter
    /// Parameters: 
    /// 1) Window size - any positive (strictly greater than 0) number smaller than input vector size
    /// In case of window size > input data size will return empty array;
    /// 2) Weights array - array of usually positive doubles, array size should be exactly equal to window size;
    /// Default value is array of 1's.
    /// NOTE: Could actually work as a mean preprocessor, but mean preprocessor is a bit faster (since it doesn't multiply elements by their weights).
    /// </summary>
    public class WeightedAveragePreprocessor : WindowBasedPreprocessor
    {
        public WeightedAveragePreprocessor(int iWindowSize)
            : base(iWindowSize)
        {
            InitializeWeights(WindowSize);
            weightsSum = weights.Sum();
        }

        /// <summary>
        /// Default constructor, sets window size to 5, initializes weights array to 1's
        /// </summary>
        public WeightedAveragePreprocessor()
            : base()
        {
            InitializeWeights(WindowSize);
            weightsSum = weights.Sum();
        }

        public WeightedAveragePreprocessor(int iWindowSize, double[] iWeights)
            : base(iWindowSize)
        {
            if (iWeights.Count() != iWindowSize)
                throw new ArgumentException("Weights array size should be exactly equal to window size!");
            weights = new double[iWindowSize];
            iWeights.CopyTo(weights, 0);
            weightsSum = weights.Sum();
        }

        protected override double windowProcessingFunc(IEnumerable<double> iArray)
        {
            return iArray.Zip(weights, (value, weight) => value * weight).Sum() / WeightsSum;
        }

        private void InitializeWeights(int iWeightsCount)
        {
            weights = new double[iWeightsCount];
            for (int i = 0; i < iWeightsCount; i++)
                weights[i] = 1;
        }


        private double[] weights;

        private double weightsSum;

        public double WeightsSum
        {
            get { return weightsSum; }
        }
    }
    /// <summary>
    /// A class for preprocessing data using Tukey53H filter
    /// Parameters: k - treshold to determine, whether the specified value is spike or not.
    /// Smaller k -> more values are considered as spikes. Default value is 5.
    /// Usual range of k values is 3...9.
    /// </summary>
    public class Tukey53HPreprocessor : IPreprocessor
    {
        /// <summary>
        /// Default constructor, sets k to 5
        /// </summary>
        public Tukey53HPreprocessor()
        {
        }

        public Tukey53HPreprocessor(double iK)
        {
            if (iK <= 0)
                throw new ArgumentOutOfRangeException("iK", "Value should be any positive integer (strictly greater than zero!)");
            K = iK;
        }

        public BSDataObject Process(BSDataObject iObj)
        {
            double[] iArray = iObj.DataArray;
            int count = iArray.Count();
            double mean = iArray.Sum() / count;
            double std = Math.Sqrt(iArray.Sum(value => (value - mean) * (value - mean)) / count);
            BSDataObject seq1 = ProcessWithoutArraySizeShrinkage(iObj, new MedianPreprocessor(5));
            BSDataObject seq2 = ProcessWithoutArraySizeShrinkage(seq1, new MedianPreprocessor(3));
            BSDataObject seq3 = ProcessWithoutArraySizeShrinkage(seq2, new WeightedAveragePreprocessor(3, new double[] { 1, 2, 1 }));
            double[] result = new double[count];
            for (int i = 0; i < iArray.Count(); i++)
            {
                if (Math.Abs(iArray[i] - seq3.DataArray[i]) <= k * std)
                    result[i] = iArray[i];
                else
                    result[i] = (iArray[i-1] + iArray[i+1]) / 2;
            }
            return new BSDataObject(result, iObj.ObjName + "_Tukey");
        }

        private static BSDataObject ProcessWithoutArraySizeShrinkage(BSDataObject iObj, WindowBasedPreprocessor iProcessor)
        {
            double[] iArray = iObj.DataArray;
            double[] result = iProcessor.Process(iObj).DataArray;
            List<double> aList = new List<double>();
            FieldInfo[] info = aList.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance); //this stuff is here to make list from array without copying its elements
            info[0].SetValue(aList, result); //_items private field
            info[1].SetValue(aList, result.Count()); //_size private field
            aList.Capacity = result.Count() + iProcessor.WindowSize - 1;
            aList.InsertRange(0, iArray.Take((iProcessor.WindowSize - 1) / 2));
            aList.AddRange(iArray.Skip(iArray.Count() - (iProcessor.WindowSize - 1) / 2));
            result = (double[])info[0].GetValue(aList);
            return new BSDataObject(result, iObj.ObjName);
        }

        private double k = 1;

        public double K
        {
            get { return k; }
            set { k = value; }
        }


    }

    public abstract class AbstractFourierPreprocessor : IPreprocessor
    {
        protected BSDataObject ProcessUsingFourierDirection(BSDataObject iObject, FourierTransform.Direction iDir)
        {
            Complex[] complexDataArray = (from tmp in iObject.DataArray select new Complex(tmp, 0)).ToArray();
            FourierTransform.DFT(complexDataArray, iDir);
            double[] resultingArray = (from tmp in complexDataArray select Math.Sqrt(tmp.Re * tmp.Re + tmp.Im * tmp.Im)).ToArray();
            return new BSDataObject(resultingArray, iObject.ObjName + "_Fouirer");
        }

        public abstract BSDataObject Process(BSDataObject iObject);
    }

    /// <summary>
    /// Class for preprocessing data using DFT.
    /// </summary>
    public class FourierPreprocessor : AbstractFourierPreprocessor
    {
        /// <summary>
        /// Processes data using DFT.
        /// </summary>
        /// <param name="iObject">A BSDataObject to process.</param>
        /// <returns>New BSDataObject, where each value in the DataArray is specturm amlitude for the given frequency.</returns>
        public override BSDataObject Process(BSDataObject iObject)
        {
            return ProcessUsingFourierDirection(iObject, FourierTransform.Direction.Forward);
        }
    }

    /// <summary>
    /// Class for preprocessing data using inverse DFT.
    /// </summary>
    public class InverseFourierPreprocessor : AbstractFourierPreprocessor
    {
        /// <summary>
        /// Processes data using inverse DFT.
        /// </summary>
        /// <param name="iObject">A BSDataObject to process.</param>
        /// <returns>New BSDataObject, where each value in the DataArray is function value for the given time.</returns>
        public override BSDataObject Process(BSDataObject iObject)
        {
            return ProcessUsingFourierDirection(iObject, FourierTransform.Direction.Backward);
        }
    }
}
