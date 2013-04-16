using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
namespace BSWork
{
    namespace TimeSeriesForecasting
    {
        public class GMDHANNmodelBuilder : IForecastingModelBuilder
        {

            public GMDHANNmodelBuilder()
            {
                ModelParametersDict = new Dictionary<string, object>();
            }

            public IForecastingModel TrainNewModel(double[][] iinput, double[][] ioutput)
            {
                //activationnetwork thenet = new activationnetwork(new sigmoidfunction(), 5, 3, 1);
                //thenet.randomize();
                //isupervisedlearning learning = new aforge.neuro.learning.perceptronlearning(thenet);
                GMDHANNforecastingModel model = new GMDHANNforecastingModel();

                SetModelParameters(model);

                /*
                 * 1. Perform algorithm steps -> receive neural networks (by levels), their order, input correspondence.
                 * Also, perhaps, some kind of function, that converts N inputs (N = InputSize) into all possible combinations
                 * of M inputs for the nets. For instance: 4 inputs [x1,x2,x3,x4] ->  all possible combinations of 3 inputs of kind:
                 * [x1, x2, x1*x2], [x1,x3,x1*x3], [x1,x4,x1*x4], [x2,x3,x2*x3], [x2,x4,x2*x4], [x3,x4,x3*x4].
                 * Or, whats better, a function, that takes 
                 */

                throw new NotImplementedException();
            }

            private void SetModelParameters(GMDHANNforecastingModel iModel)
            {
                if (ModelParametersDict.ContainsKey("InputSize"))
                    iModel.InputSize = (long)ModelParametersDict["InputSize"];
            }
            
            public const string InputSizeKey = "InputSize";

            public Dictionary<string, object> ModelParametersDict { get; set; }

        }
    }
}
