using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BSWork.ComponentsConfiguration;

namespace BSWork
{
    namespace ComponentsSettingsForms
    {
        public partial class BSSaveDataComponentForm : Form
        {
            public BSSaveDataComponentForm(IConfigurable iComponent)
            {
                component = iComponent;
                InitializeComponent();
            }

            private void selectFileButton_Click(object sender, EventArgs e)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    filePathTextBox.Text = saveFileDialog.FileName;
            }


            private IConfigurable component;
            private string filePath;

            private void filePathTextBox_TextChanged(object sender, EventArgs e)
            {
                filePath = (sender as TextBox).Text;
            }

            private void button1_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void BSSaveDataComponentForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                component.SetValueForKey("fileToSavePath", filePath);
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}