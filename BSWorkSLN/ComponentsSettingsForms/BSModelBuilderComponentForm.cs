using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BSWork.ComponentsConfiguration;

namespace BSWork.ComponentsSettingsForms
{
    public partial class BSModelBuilderComponentForm : BaseSettingsForm
    {
        public BSModelBuilderComponentForm(IConfigurable iConfiguredComponent):base()
        {
            if (iConfiguredComponent == null) throw new ArgumentNullException();
            _configuredComponent = iConfiguredComponent;
            InitializeComponent();
            modelsListComboBox.Items.AddRange((_configuredComponent.GetValueForKey("modelsList") as object[]));
            inputsCountTextBox.Text = _configuredComponent.GetValueForKey("defaultInputsCount") as string;
        }

        public override TableLayoutPanel MainTableLayout
        {
            get
            {
                return tableLayoutPanel;
            }
        }

        public override TableLayoutPanel SecondaryTableLayout
        {
            get
            {
                return secondaryTableLayout;
            }
        }

        public override TabControl MainTabControl
        {
            get
            {
                return mainTabControl;
            }
        }

        private void inputsCountTextBox_Validated(object sender, EventArgs e)
        {
            _configuredComponent.SetValueForKey("inputsCount", Convert.ToUInt32((sender as TextBox).Text));
        }

        public IConfigurable ConfiguredComponent
        {
            get { return _configuredComponent; }
            set { _configuredComponent = value; }
        }

        private IConfigurable _configuredComponent;

        private void acceptButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BSModelBuilderComponentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void modelsListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderCB = sender as ComboBox;
            _configuredComponent.SetValueForKey("modelType", senderCB.Items[senderCB.SelectedIndex]);
        }

    }
}
