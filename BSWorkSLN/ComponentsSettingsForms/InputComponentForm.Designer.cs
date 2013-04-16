namespace BSWork
{
    namespace ComponentsSettingsForms
    {
        partial class InputComponentForm
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputComponentForm));
            this.fileRadioButton = new System.Windows.Forms.RadioButton();
            this.userInputRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.enterDataButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileRadioButton
            // 
            resources.ApplyResources(this.fileRadioButton, "fileRadioButton");
            this.fileRadioButton.Checked = true;
            this.fileRadioButton.Name = "fileRadioButton";
            this.fileRadioButton.TabStop = true;
            this.fileRadioButton.UseVisualStyleBackColor = true;
            this.fileRadioButton.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // userInputRadioButton
            // 
            resources.ApplyResources(this.userInputRadioButton, "userInputRadioButton");
            this.userInputRadioButton.Name = "userInputRadioButton";
            this.userInputRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // filePathTextBox
            // 
            resources.ApplyResources(this.filePathTextBox, "filePathTextBox");
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.TextChanged += new System.EventHandler(this.filePathTextBox_TextChanged);
            // 
            // selectFileButton
            // 
            resources.ApplyResources(this.selectFileButton, "selectFileButton");
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // enterDataButton
            // 
            resources.ApplyResources(this.enterDataButton, "enterDataButton");
            this.enterDataButton.Name = "enterDataButton";
            this.enterDataButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputComponentForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.enterDataButton);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userInputRadioButton);
            this.Controls.Add(this.fileRadioButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InputComponentForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputComponentForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.RadioButton fileRadioButton;
            private System.Windows.Forms.RadioButton userInputRadioButton;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.TextBox filePathTextBox;
            private System.Windows.Forms.Button selectFileButton;
            private System.Windows.Forms.Button enterDataButton;
            private System.Windows.Forms.OpenFileDialog openFileDialog;
            private System.Windows.Forms.Button button1;
        }
    }
}