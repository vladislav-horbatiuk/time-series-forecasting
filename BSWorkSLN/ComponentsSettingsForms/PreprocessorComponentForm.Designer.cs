namespace BSWork
{
    namespace ComponentsSettingsForms
    {
        partial class PreprocessorComponentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreprocessorComponentForm));
            this.preprocessorsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.inputsCountTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // preprocessorsComboBox
            // 
            resources.ApplyResources(this.preprocessorsComboBox, "preprocessorsComboBox");
            this.preprocessorsComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.preprocessorsComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.preprocessorsComboBox.FormattingEnabled = true;
            this.preprocessorsComboBox.Name = "preprocessorsComboBox";
            this.preprocessorsComboBox.SelectedIndexChanged += new System.EventHandler(this.preprocessorsComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // inputsCountTextBox
            // 
            resources.ApplyResources(this.inputsCountTextBox, "inputsCountTextBox");
            this.inputsCountTextBox.Name = "inputsCountTextBox";
            this.inputsCountTextBox.Validated += new System.EventHandler(this.inputsCountTextBox_Validated);
            // 
            // PreprocessorComponentForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputsCountTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.preprocessorsComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PreprocessorComponentForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreprocessorComponentForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.ComboBox preprocessorsComboBox;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Button button1;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.TextBox inputsCountTextBox;

        }
    }
}