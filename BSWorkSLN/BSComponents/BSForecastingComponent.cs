using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using BSWork.TimeSeriesForecasting;

namespace BSWork.BSComponents
{
    public class BSForecastingComponent : BSBaseComponent
    {
        public BSForecastingComponent(Rectangle iOnScreenRectangle)
            : base(iOnScreenRectangle)
        {
        }

        public override bool IsReady()
        {
            return true;
        }

        public override string WhatsWrong()
        {
            return "Компонент 'Прогнозування' налаштований коректно.";
        }

        public override void ShowSettingsForm()
        {
            //TODO: implement settings form for this component! Also, consider that it is pretty
            //similar to BSPreprocessorComponent settings form, and, try to use composition(?)
        }

        public override void Start()
        {
            CheckIfInputsAreProvided();
            DataObjects.BSDataObject[] inputs = new DataObjects.BSDataObject[Inputs.Count];
            Inputs.TryPopRange(inputs);
            //TODO: improve to allow multiple DataObjects to be used as inputs
            if (inputs.Length != 2)
                throw new NoInputProvidenException();

            if (!inputs.Any(x => x is IForecastingModel))
                throw new IncorrectInputs();
            IForecastingModel model;
            DataObjects.BSDataObject inputData;

            //This part is very, very bad!
            int i = 0;
            model = inputs[i] as IForecastingModel;
            if (null == model)
            {
                i = 1;
                model = inputs[i] as IForecastingModel;
            }
            inputData = inputs[Math.Abs(i - 1)];
            //End of very, very bad part.
            double[][] modelInput, modelOutput;
            model.InputProvider.GenerateInputAndOutput(inputData.DataArray, out modelInput, out modelOutput);
            forecaster = new BasicForecaster(model);
            modelOutput = forecaster.CalculateOutput(modelInput);
            //Also not very good - 1) think about new object name - it can be specified by user or something.
            //2) Check that modelOutput is exactly 1 row.
            double[] output = new double[modelOutput.LongLength];
            for (long j = 0; j < output.LongLength; ++j)
                output[j] = modelOutput[j][0];
            Output = new DataObjects.BSDataObject(output, inputData.Offset + model.InputSize, inputData.ObjName);
        }

        public override string ComponentType
        {
            get { return "IO"; }
        }

        private IForecaster forecaster;
    }
}
