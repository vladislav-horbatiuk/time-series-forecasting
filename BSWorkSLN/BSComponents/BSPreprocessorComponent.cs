using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

using BSWork.ComponentsSettingsForms;
using BSWork.ComponentsConfiguration;
using DataPreprocessing;
using BSWork.DataObjects;

using BSComponents.Properties;

namespace BSWork
{
    namespace BSComponents
    {
        public class BSPreprocessorComponent : BSBaseComponent, IConfigurable
        {
            public BSPreprocessorComponent(Rectangle iOnScreenRectangle)
                : base(iOnScreenRectangle)
            {
            }

            public override string ComponentType
            {
                get
                {
                    return "IO";
                }
            }

            public override void Start()
            {
                CheckIfInputsAreProvided();
                BSDataObject tmp;
                Inputs.TryPop(out tmp);
                Output = preprocessor.Process(tmp);
            }

            public override void ShowSettingsForm()
            {
                if (null == aForm)
                aForm = new PreprocessorComponentForm(this);
                aForm.ShowDialog();
            }

            public void SetValueForKey(string iKey, object iValue)
            {
                switch (iKey)
                {
                    case "preprocessorType":
                        preprocessor = (IPreprocessor) Activator.CreateInstance(preprocessorsList[(iValue as string)]);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            public object GetValueForKey(string iKey)
            {
                switch (iKey)
                {
                    case "preprocessorsList":
                        return preprocessorsList.Keys.ToArray();
                    case "defaultInputsCount":
                        return Convert.ToString(kDefaultWindowSize);
                    default:
                        throw new ArgumentException();
                }
            }

            public override bool IsReady()
            {
                return preprocessor != null;
            }

            public override string WhatsWrong()
            {
                if (preprocessor == null)
                    return "Не заданий тип обробки для компонента 'обробка'.";
                else
                    return "Компонент 'Обробка' налаштований коректно.";
            }

            private readonly Dictionary<string, Type> preprocessorsList = new Dictionary<string, Type>()
            {
            {strings.derivativeString, typeof(DerivativePreprocessor)},
            {strings.medianFilter, typeof(MedianPreprocessor)},
            {strings.movingAverageFilter, typeof(MeanPreprocessor)}, 
            {strings.weightedMovingAverageFilter, typeof(WeightedAveragePreprocessor)},
            {strings.tukeyFilter, typeof(Tukey53HPreprocessor)}, 
            {strings.fourierTransform, typeof(FourierPreprocessor)},
            {strings.inverseFourierTransform, typeof(InverseFourierPreprocessor)},
            };

            private IPreprocessor preprocessor;
            private PreprocessorComponentForm aForm = null;
            private uint kDefaultWindowSize = 5;
        }
    }
}
