using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BSWork.ComponentsConfiguration;
using System.IO;

namespace BSWork
{
    namespace ComponentsSettingsForms
    {
        public partial class InputComponentForm : Form
        {
            private IConfigurable configuredComponent;
            private string filePath;

            public InputComponentForm(IConfigurable iComponent)
            {
                if (iComponent == null) throw new ArgumentNullException();
                configuredComponent = iComponent;
                InitializeComponent();
            }

            private void radioButtons_CheckedChanged(object sender, EventArgs e)
            {
                filePathTextBox.Visible = !filePathTextBox.Visible;
                selectFileButton.Visible = !selectFileButton.Visible;
                enterDataButton.Visible = !enterDataButton.Visible;
            }

            private void selectFileButton_Click(object sender, EventArgs e)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePathTextBox.Text = openFileDialog.FileName;
                }
            }

            private void filePathTextBox_TextChanged(object sender, EventArgs e)
            {
                filePath = (sender as TextBox).Text;
            }

            private void button1_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void InputComponentForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                configuredComponent.SetValueForKey("sourceFilePath", filePath);
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
