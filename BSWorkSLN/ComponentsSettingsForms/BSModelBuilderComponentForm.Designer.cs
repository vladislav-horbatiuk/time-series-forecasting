namespace BSWork.ComponentsSettingsForms
{
    partial class BSModelBuilderComponentForm : BaseSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BSModelBuilderComponentForm));
            this.acceptButton = new System.Windows.Forms.Button();
            this.inputsCountTextBox = new System.Windows.Forms.TextBox();
            this.inputsCountLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.modelsListComboBox = new System.Windows.Forms.ComboBox();
            this.modelTypeLabel = new System.Windows.Forms.Label();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.primaryTabPage = new System.Windows.Forms.TabPage();
            this.secondaryTabPage = new System.Windows.Forms.TabPage();
            this.secondaryTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.primaryTabPage.SuspendLayout();
            this.secondaryTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            resources.ApplyResources(this.acceptButton, "acceptButton");
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // inputsCountTextBox
            // 
            resources.ApplyResources(this.inputsCountTextBox, "inputsCountTextBox");
            this.inputsCountTextBox.Name = "inputsCountTextBox";
            this.inputsCountTextBox.Validated += new System.EventHandler(this.inputsCountTextBox_Validated);
            // 
            // inputsCountLabel
            // 
            resources.ApplyResources(this.inputsCountLabel, "inputsCountLabel");
            this.inputsCountLabel.Name = "inputsCountLabel";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.modelsListComboBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.modelTypeLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.inputsCountTextBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.inputsCountLabel, 0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // modelsListComboBox
            // 
            resources.ApplyResources(this.modelsListComboBox, "modelsListComboBox");
            this.modelsListComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.modelsListComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.modelsListComboBox.FormattingEnabled = true;
            this.modelsListComboBox.Name = "modelsListComboBox";
            this.modelsListComboBox.SelectedIndexChanged += new System.EventHandler(this.modelsListComboBox_SelectedIndexChanged);
            // 
            // modelTypeLabel
            // 
            resources.ApplyResources(this.modelTypeLabel, "modelTypeLabel");
            this.modelTypeLabel.Name = "modelTypeLabel";
            // 
            // mainTabControl
            // 
            resources.ApplyResources(this.mainTabControl, "mainTabControl");
            this.mainTabControl.Controls.Add(this.primaryTabPage);
            this.mainTabControl.Controls.Add(this.secondaryTabPage);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            // 
            // primaryTabPage
            // 
            resources.ApplyResources(this.primaryTabPage, "primaryTabPage");
            this.primaryTabPage.Controls.Add(this.tableLayoutPanel);
            this.primaryTabPage.Name = "primaryTabPage";
            this.primaryTabPage.UseVisualStyleBackColor = true;
            // 
            // secondaryTabPage
            // 
            resources.ApplyResources(this.secondaryTabPage, "secondaryTabPage");
            this.secondaryTabPage.Controls.Add(this.secondaryTableLayout);
            this.secondaryTabPage.Name = "secondaryTabPage";
            this.secondaryTabPage.UseVisualStyleBackColor = true;
            // 
            // secondaryTableLayout
            // 
            resources.ApplyResources(this.secondaryTableLayout, "secondaryTableLayout");
            this.secondaryTableLayout.Name = "secondaryTableLayout";
            // 
            // BSModelBuilderComponentForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BSModelBuilderComponentForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BSModelBuilderComponentForm_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.primaryTabPage.ResumeLayout(false);
            this.primaryTabPage.PerformLayout();
            this.secondaryTabPage.ResumeLayout(false);
            this.secondaryTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox inputsCountTextBox;
        private System.Windows.Forms.Label inputsCountLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox modelsListComboBox;
        private System.Windows.Forms.Label modelTypeLabel;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage primaryTabPage;
        private System.Windows.Forms.TabPage secondaryTabPage;
        private System.Windows.Forms.TableLayoutPanel secondaryTableLayout;
    }
}