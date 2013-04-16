using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using AForge.Neuro;

using BSWork.TimeSeriesForecasting;
using BSWork.ComponentsConfiguration;
using BSWork.ComponentsSettingsForms;

using BSComponents.Properties;

namespace BSWork.BSComponents
{
    public class BSModelBuilderComponent : BSBaseComponent, IConfigurable
    {
        private struct ModelStruct
        {
            public Type modelBuilderType;
            public Action<BaseSettingsForm> settingsFormConfigurationAction;
        }

        public BSModelBuilderComponent(Rectangle iOnScreenRectangle)
            : base(iOnScreenRectangle)
        {
            _modelsList = new Dictionary<string, ModelStruct>()
            {
                {strings.ANNString, 
                    new ModelStruct {modelBuilderType = typeof(ANNmodelBuilder), 
                    settingsFormConfigurationAction = new Action<BaseSettingsForm>(this._ConfigureSettingsFormForANN)}},
                /*{"Комбінація МГУА і ШНМ",
                    new ModelStruct {modelBuilderType = typeof(GMDHANNmodelBuilder),
                    settingsFormConfigurationAction = new Action<BaseSettingsForm>(this._ConfigureSettingsFormForGMDHANN)}},*/
            };
        }

        public override bool IsReady()
        {
            return null != _modelBuilder;
        }

        public override string WhatsWrong()
        {
            if (null == _modelBuilder)
                return strings.modelBuilderIsNullString;
            else
                return strings.correctString;
        }

        public override void ShowSettingsForm()
        {
            if (null == aForm)
                aForm = new BSModelBuilderComponentForm(this);
            aForm.ShowDialog();
        }

        public override void Start()
        {
            CheckIfInputsAreProvided();
            DataObjects.BSDataObject tmp;
            Inputs.TryPop(out tmp);
            double[][] modelTrainInput, modelTrainOutput;
            inputProvider = new WindowingInputProvider(kDefaultLag, _windowSize);
            inputProvider.GenerateInputAndOutput(tmp.DataArray, out modelTrainInput, out modelTrainOutput);
            IForecastingModel model = _modelBuilder.TrainNewModel(modelTrainInput, modelTrainOutput);
            model.InputProvider = inputProvider;
            Output = (DataObjects.BSDataObject)model;
        }

        public override string ComponentType
        {
            get { return "IO"; }
        }

        public object GetValueForKey(string iKey)
        {
            switch (iKey)
            {
                case "modelsList":
                    return _modelsList.Keys.ToArray();
                case "defaultInputsCount":
                    return Convert.ToString(kDefaultWindowSize);
                default:
                    throw new ArgumentException(string.Format("Unknown key: '{0}'", iKey));
            }
        }

        public void SetValueForKey(string iKey, object iValue)
        {
            switch (iKey)
            {
                case "modelType":
                    ModelStruct requestedModelStruct = _modelsList[iValue as string];
                    if (_modelBuilder != null && _modelBuilder.GetType() == requestedModelStruct.modelBuilderType)
                        return;
                    _modelBuilder = (IForecastingModelBuilder)Activator.CreateInstance(requestedModelStruct.modelBuilderType);
                    _RollbackSettingsFormToDefault();
                    requestedModelStruct.settingsFormConfigurationAction(aForm);
                    break;
                case "inputsCount":
                    _windowSize = (uint)iValue;
                    break;
                default:
                    throw new ArgumentException(string.Format("Unknown key: '{0}'", iKey));
            }
        }

        private void _RollbackSettingsFormToDefault()
        {
            var formType = aForm.GetType();
            Queue<Control> controlsToDelete = new Queue<Control>(aForm.Controls.Count);
            //Find all controls, that were added programmatically - those should be deleted
            foreach (Control control in aForm.MainTableLayout.Controls)
            {
                var element = formType.GetField(control.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (element == null)
                    controlsToDelete.Enqueue(control);
            }
            foreach (Control controlToDelete in controlsToDelete)
                aForm.MainTableLayout.Controls.Remove(controlToDelete);
            controlsToDelete.Clear();
            foreach (Control control in aForm.SecondaryTableLayout.Controls)
            {
                var element = formType.GetField(control.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (element == null)
                    controlsToDelete.Enqueue(control);
            }
            foreach (Control controlToDelete in controlsToDelete)
                aForm.SecondaryTableLayout.Controls.Remove(controlToDelete);

            aForm.MainTableLayout.RowStyles.Clear();
            aForm.MainTableLayout.ColumnStyles.Clear();
            aForm.SecondaryTableLayout.RowStyles.Clear();
            aForm.SecondaryTableLayout.ColumnStyles.Clear();
        }

        private void _ConfigureSettingsFormForGMDHANN(BaseSettingsForm oSettingsForm)
        {
            throw new NotImplementedException();
        }

        private void _ConfigureSettingsFormForANN(BaseSettingsForm oSettingsForm)
        {
            oSettingsForm.MainTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            oSettingsForm.MainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            oSettingsForm.MainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            {
                oSettingsForm.MainTableLayout.Controls.Add(UIExtensions.LabelWithTextAndName(strings.hiddenCountsLabelText, "hiddenCountsLabel"),
                    0, oSettingsForm.MainTableLayout.GetRowHeights().Length);
            }
            {
                TextBox hiddenCountsTextBox = new TextBox();
                hiddenCountsTextBox.Name = "hiddenCountsTextBox";
                hiddenCountsTextBox.Text = string.Join(" ", ANNmodelBuilder.kDefaultNeuronsCount.Take(ANNmodelBuilder.kDefaultNeuronsCount.Count() - 1));
                hiddenCountsTextBox.Validating += new System.ComponentModel.CancelEventHandler(_settingsFormHiddenCountsTextBox_Validating);
                hiddenCountsTextBox.Validated += new EventHandler(_settingsFormHiddenCountsTextBox_Validated);
                oSettingsForm.MainTableLayout.Controls.Add(hiddenCountsTextBox, 1, oSettingsForm.MainTableLayout.GetRowHeights().Length - 1);
            }
            {
                oSettingsForm.MainTableLayout.Controls.Add(UIExtensions.LabelWithTextAndName(strings.activationFuncLabelText, "activationFuncLabel"),
                0, oSettingsForm.MainTableLayout.GetRowHeights().Length);
            }
            {
                ComboBox activationFuncComboBox = new ComboBox();
                activationFuncComboBox.Items.AddRange(ANNmodelBuilder.ActivationFunctionsDict.Keys.ToArray());
                activationFuncComboBox.Text = ANNmodelBuilder.kDefaultActivationFunction;
                activationFuncComboBox.AdjustWidth();
                activationFuncComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                activationFuncComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                activationFuncComboBox.SelectedIndexChanged += new EventHandler(_settingsFormActivationFuncComboBox_SelectedIndexChanged);
                activationFuncComboBox.Name = "activationFuncComboBox";
                oSettingsForm.MainTableLayout.Controls.Add(activationFuncComboBox, 1, oSettingsForm.MainTableLayout.GetRowHeights().Length - 1);
            }

            oSettingsForm.SecondaryTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            oSettingsForm.SecondaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            oSettingsForm.SecondaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            {
                oSettingsForm.SecondaryTableLayout.Controls.Add(UIExtensions.LabelWithTextAndName(strings.maxIterationsCountLabelText, "maxIterationsCountLabel"),
                    0, oSettingsForm.SecondaryTableLayout.GetRowHeights().Length);
            }
            {
                TextBox maxIterationsCountTextBox = new TextBox();
                maxIterationsCountTextBox.Name = "hiddenCountsTextBox";
                maxIterationsCountTextBox.Text = ANNmodelBuilder.kMaxInterationsNum.ToString();
                maxIterationsCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(UIExtensions.uintTextBox_Validating);
                maxIterationsCountTextBox.Validated += new EventHandler(_settingsFormHiddenCountsTextBox_Validated);
                oSettingsForm.SecondaryTableLayout.Controls.Add(maxIterationsCountTextBox, 1, oSettingsForm.SecondaryTableLayout.GetRowHeights().Length - 1);
            }
            {
                oSettingsForm.SecondaryTableLayout.Controls.Add(UIExtensions.LabelWithTextAndName(strings.stopErrorLabelText,
                    "stopErrorLabel"), 0, oSettingsForm.SecondaryTableLayout.GetRowHeights().Length);
            }
            {
                TextBox stopErrorTextBox = new TextBox();
                stopErrorTextBox.Name = "stopErrorTextBox";
                stopErrorTextBox.Text = ANNmodelBuilder.kStopError.ToString();
                stopErrorTextBox.Validating += new System.ComponentModel.CancelEventHandler(UIExtensions.doubleTextBox_Validating);
                stopErrorTextBox.Validated += new EventHandler(_settingsFormStopErrorTextBox_Validated);
                oSettingsForm.SecondaryTableLayout.Controls.Add(stopErrorTextBox, 1, oSettingsForm.SecondaryTableLayout.GetRowHeights().Length - 1);
            }
            oSettingsForm.MainTabControl.Size = new Size(Math.Max(oSettingsForm.MainTableLayout.Size.Width, oSettingsForm.SecondaryTableLayout.Size.Width) + 10,
                Math.Max(oSettingsForm.MainTableLayout.Size.Height, oSettingsForm.SecondaryTableLayout.Size.Height) + 50);
            //TODO: VERY MESSY! CONSIDER CREATING CUSTOM CONTROLS, THAT FIT THE NEEDS!!
        }

        private void _settingsFormStopErrorTextBox_Validated(object iSender, EventArgs e)
        {
            _modelBuilder.ModelParametersDict[ANNmodelBuilder.StopErrorKey] = Convert.ToDouble((iSender as TextBox).Text);
        }

        private void _settingsFormMaxIterationsNumberTextBox_Validated(object iSender, EventArgs e)
        {
            _modelBuilder.ModelParametersDict[ANNmodelBuilder.MaxIterationsNumberKey] = Convert.ToInt32((iSender as TextBox).Text);
        }

        private void _settingsFormActivationFuncComboBox_SelectedIndexChanged(object iSender, EventArgs e)
        {
            ComboBox sender = iSender as ComboBox;
            _modelBuilder.ModelParametersDict[ANNmodelBuilder.ActivationFunctionKey] = sender.Items[sender.SelectedIndex];
        }

        private void _settingsFormHiddenCountsTextBox_Validated(object iSender, EventArgs e)
        {
            _modelBuilder.ModelParametersDict[ANNmodelBuilder.NeuronsInLayersKey] = Array.ConvertAll(((iSender as TextBox).Text+" 1").Split(' '),
                delegate(string iStr) { return Convert.ToInt32(iStr); });
        }

        private void _settingsFormHiddenCountsTextBox_Validating(object iSender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox sender = iSender as TextBox;
            string text = sender.Text;
            if (!text.Split(' ').All(subStr => subStr.All(ch => char.IsDigit(ch))))
            {
                e.Cancel = true;
                sender.Select(0, text.Length);
            }
        }

        private IForecastingModelBuilder _modelBuilder;
        private const uint kDefaultLag = 1, kDefaultWindowSize = 5;
        private uint _windowSize = kDefaultWindowSize;
   
        //TODO: user should have a possibility to configure this
        private IInputProvider inputProvider;
        private readonly Dictionary<string, ModelStruct> _modelsList;

        private BaseSettingsForm aForm = null;
    }
}
