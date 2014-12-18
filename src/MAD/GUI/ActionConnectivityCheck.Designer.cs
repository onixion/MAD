namespace MAD.GUI
{
    partial class ActionConnectivityCheck
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.intensityComboBox = new System.Windows.Forms.ComboBox();
            this.intensityLabel = new System.Windows.Forms.Label();
            this.targetIPTextBox = new System.Windows.Forms.TextBox();
            this.targetIPLabel = new System.Windows.Forms.Label();
            this.targetIPPanel = new System.Windows.Forms.Panel();
            this.checkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).BeginInit();
            this.targetIPPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(484, 64);
            // 
            // title
            // 
            this.title.Size = new System.Drawing.Size(393, 51);
            this.title.Text = "Check Connectivity";
            // 
            // intensityComboBox
            // 
            this.intensityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.intensityComboBox.FormattingEnabled = true;
            this.intensityComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.intensityComboBox.Location = new System.Drawing.Point(12, 122);
            this.intensityComboBox.Name = "intensityComboBox";
            this.intensityComboBox.Size = new System.Drawing.Size(121, 21);
            this.intensityComboBox.TabIndex = 4;
            this.intensityComboBox.SelectedValueChanged += new System.EventHandler(this.intensityComboBox_SelectedValueChanged);
            // 
            // intensityLabel
            // 
            this.intensityLabel.AutoSize = true;
            this.intensityLabel.ForeColor = System.Drawing.Color.Cyan;
            this.intensityLabel.Location = new System.Drawing.Point(9, 106);
            this.intensityLabel.Name = "intensityLabel";
            this.intensityLabel.Size = new System.Drawing.Size(112, 13);
            this.intensityLabel.TabIndex = 5;
            this.intensityLabel.Text = "Intensity of the check:";
            // 
            // targetIPTextBox
            // 
            this.targetIPTextBox.Location = new System.Drawing.Point(44, 26);
            this.targetIPTextBox.Name = "targetIPTextBox";
            this.targetIPTextBox.Size = new System.Drawing.Size(100, 20);
            this.targetIPTextBox.TabIndex = 7;
            this.targetIPTextBox.Text = "192.168.1.1";
            this.targetIPTextBox.TextChanged += new System.EventHandler(this.targetIPTextBox_TextChanged);
            // 
            // targetIPLabel
            // 
            this.targetIPLabel.AutoSize = true;
            this.targetIPLabel.ForeColor = System.Drawing.Color.Cyan;
            this.targetIPLabel.Location = new System.Drawing.Point(41, 9);
            this.targetIPLabel.Name = "targetIPLabel";
            this.targetIPLabel.Size = new System.Drawing.Size(54, 13);
            this.targetIPLabel.TabIndex = 6;
            this.targetIPLabel.Text = "Target IP:";
            // 
            // targetIPPanel
            // 
            this.targetIPPanel.Controls.Add(this.targetIPTextBox);
            this.targetIPPanel.Controls.Add(this.targetIPLabel);
            this.targetIPPanel.Location = new System.Drawing.Point(171, 97);
            this.targetIPPanel.Name = "targetIPPanel";
            this.targetIPPanel.Size = new System.Drawing.Size(284, 55);
            this.targetIPPanel.TabIndex = 8;
            this.targetIPPanel.Visible = false;
            // 
            // checkButton
            // 
            this.checkButton.BackColor = System.Drawing.Color.Cyan;
            this.checkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkButton.Location = new System.Drawing.Point(111, 216);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 9;
            this.checkButton.Text = "Check";
            this.checkButton.UseVisualStyleBackColor = false;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Cyan;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(292, 216);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ActionConnectivityCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 251);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.targetIPPanel);
            this.Controls.Add(this.intensityLabel);
            this.Controls.Add(this.intensityComboBox);
            this.Name = "ActionConnectivityCheck";
            this.Text = "Connectivity";
            this.Controls.SetChildIndex(this.titlePanel, 0);
            this.Controls.SetChildIndex(this.intensityComboBox, 0);
            this.Controls.SetChildIndex(this.intensityLabel, 0);
            this.Controls.SetChildIndex(this.targetIPPanel, 0);
            this.Controls.SetChildIndex(this.checkButton, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).EndInit();
            this.targetIPPanel.ResumeLayout(false);
            this.targetIPPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void DrawIPBox()
        {

        }

        #endregion

        private System.Windows.Forms.ComboBox intensityComboBox;
        private System.Windows.Forms.Label intensityLabel;
        private System.Windows.Forms.TextBox targetIPTextBox;
        private System.Windows.Forms.Label targetIPLabel;
        private System.Windows.Forms.Panel targetIPPanel;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button cancelButton;
    }
}