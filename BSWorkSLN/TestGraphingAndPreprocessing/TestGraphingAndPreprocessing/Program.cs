using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BSWork.DataObjects;
using DataPreprocessing;
using DataObjectsSupply;
using System.IO;
using BSWork.DataVisualization;

namespace TestGraphingAndPreprocessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            //Creating an array with values, corresponding to linear function with added white noise
            const int n = 360;
            double[] array = new double[n];
            for (int i = 0; i < n; i++)
            {
                double noise = 0;
                noise = (double)rnd.Next(Int32.MinValue, Int32.MaxValue) / (Int32.MaxValue / rnd.Next(1, 30));
                //if (i % 10 == 0) noise = 100; //Noise for tukey filter testing

                array[i] = (double)i + noise;
            }
            BSDataObject source = new BSDataObject(array, "SourceData");
            IVisualizer visualizer = new SimpleVisualizationHandler();
            visualizer.AddToGraph(source);
            visualizer.AddToGraph(new MedianPreprocessor().Process(source));
            using (var ms = new MemoryStream(n * (sizeof(double) + sizeof(char))))
            {
                StreamWriter sw = new StreamWriter(ms);
                for (int i = 0; i < n; i++)
                {
                    array[i] = 80 * Math.Sin((double)i * Math.PI / 180.0);
                    sw.WriteLine(array[i]);
                }
                sw.Flush();
                ms.Position = 0;
                BSDataObject target = DataObjectSupplier.GetDataObjectForStream(ms, "Sine from memory stream");
                visualizer.AddToGraph(target);
            }
            (visualizer as SimpleVisualizationHandler)._TestShow();
        }
    }
}
