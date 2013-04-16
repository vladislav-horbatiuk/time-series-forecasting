using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

using BSWork.ComponentsConfiguration;

namespace BSWork
{
    namespace ComponentsSettingsForms
    {
        public partial class PreprocessorComponentForm : Form
        {
            private IConfigurable _configuredComponent;

            public IConfigurable ConfiguredComponent
            {
                get { return _configuredComponent; }
                set { _configuredComponent = value; }
            }

            public PreprocessorComponentForm()
            {
                _configuredComponent = null;
                InitializeComponent();
            }

            public PreprocessorComponentForm(IConfigurable iComponent)
            {
                if (iComponent == null) throw new ArgumentNullException();
                _configuredComponent = iComponent;
                InitializeComponent();
                preprocessorsComboBox.Items.AddRange((_configuredComponent.GetValueForKey("preprocessorsList") as object[]));
                inputsCountTextBox.Text = _configuredComponent.GetValueForKey("defaultInputsCount") as string;
            }

            private void preprocessorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                ComboBox senderCB = sender as ComboBox;
                _configuredComponent.SetValueForKey("preprocessorType", senderCB.Items[senderCB.SelectedIndex]);
            }

            private void PreprocessorComponentForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                this.Hide();
                e.Cancel = true;
            }

            private void button1_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void inputsCountTextBox_Validated(object sender, EventArgs e)
            {
                //TODO: consider creating some base class for components settings forms
                //TODO: delete inputsCountTextBox and corresponding label - we should add them only if needed!
                _configuredComponent.SetValueForKey("inputsCount", Convert.ToUInt32((sender as TextBox).Text));
            }
        }
    }
}
