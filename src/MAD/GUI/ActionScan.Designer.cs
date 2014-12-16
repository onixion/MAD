namespace MAD.GUI
{
    partial class ActionScan
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
            this.LocalIPAddressLabel = new System.Windows.Forms.Label();
            this.LocalIPAddresTextBox = new System.Windows.Forms.TextBox();
            this.NetworkTextBox = new System.Windows.Forms.TextBox();
            this.NetworkLabel = new System.Windows.Forms.Label();
            this.SubnetmaskDropdown = new System.Windows.Forms.ComboBox();
            this.SubnetmaskLabel = new System.Windows.Forms.Label();
            this.OneShotCheckBox = new System.Windows.Forms.CheckBox();
            this.ScanButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(400, 64);
            // 
            // title
            // 
            this.title.Size = new System.Drawing.Size(303, 54);
            this.title.Text = "Scan for hosts";
            // 
            // LocalIPAddressLabel
            // 
            this.LocalIPAddressLabel.AutoSize = true;
            this.LocalIPAddressLabel.Location = new System.Drawing.Point(12, 97);
            this.LocalIPAddressLabel.Name = "LocalIPAddressLabel";
            this.LocalIPAddressLabel.Size = new System.Drawing.Size(80, 13);
            this.LocalIPAddressLabel.TabIndex = 4;
            this.LocalIPAddressLabel.Text = "local IPAddress";
            // 
            // LocalIPAddresTextBox
            // 
            this.LocalIPAddresTextBox.Location = new System.Drawing.Point(12, 113);
            this.LocalIPAddresTextBox.Name = "LocalIPAddresTextBox";
            this.LocalIPAddresTextBox.Size = new System.Drawing.Size(100, 20);
            this.LocalIPAddresTextBox.TabIndex = 5;
            this.LocalIPAddresTextBox.Text = "192.168.0.1";
            // 
            // NetworkTextBox
            // 
            this.NetworkTextBox.Location = new System.Drawing.Point(144, 113);
            this.NetworkTextBox.Name = "NetworkTextBox";
            this.NetworkTextBox.Size = new System.Drawing.Size(100, 20);
            this.NetworkTextBox.TabIndex = 7;
            this.NetworkTextBox.Text = "192.168.0.0";
            // 
            // NetworkLabel
            // 
            this.NetworkLabel.AutoSize = true;
            this.NetworkLabel.Location = new System.Drawing.Point(144, 97);
            this.NetworkLabel.Name = "NetworkLabel";
            this.NetworkLabel.Size = new System.Drawing.Size(47, 13);
            this.NetworkLabel.TabIndex = 6;
            this.NetworkLabel.Text = "Network";
            // 
            // SubnetmaskDropdown
            // 
            this.SubnetmaskDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SubnetmaskDropdown.FormattingEnabled = true;
            this.SubnetmaskDropdown.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32"});
            this.SubnetmaskDropdown.Location = new System.Drawing.Point(12, 174);
            this.SubnetmaskDropdown.Name = "SubnetmaskDropdown";
            this.SubnetmaskDropdown.Size = new System.Drawing.Size(121, 21);
            this.SubnetmaskDropdown.TabIndex = 8;
            // 
            // SubnetmaskLabel
            // 
            this.SubnetmaskLabel.AutoSize = true;
            this.SubnetmaskLabel.Location = new System.Drawing.Point(9, 158);
            this.SubnetmaskLabel.Name = "SubnetmaskLabel";
            this.SubnetmaskLabel.Size = new System.Drawing.Size(66, 13);
            this.SubnetmaskLabel.TabIndex = 9;
            this.SubnetmaskLabel.Text = "Subnetmask";
            // 
            // OneShotCheckBox
            // 
            this.OneShotCheckBox.AutoSize = true;
            this.OneShotCheckBox.Checked = true;
            this.OneShotCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OneShotCheckBox.Location = new System.Drawing.Point(152, 176);
            this.OneShotCheckBox.Name = "OneShotCheckBox";
            this.OneShotCheckBox.Size = new System.Drawing.Size(92, 17);
            this.OneShotCheckBox.TabIndex = 10;
            this.OneShotCheckBox.Text = "only one scan";
            this.OneShotCheckBox.UseVisualStyleBackColor = true;
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(77, 246);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanButton.TabIndex = 11;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(228, 246);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 210);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(373, 23);
            this.progressBar1.TabIndex = 13;
            // 
            // ActionScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 281);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ScanButton);
            this.Controls.Add(this.OneShotCheckBox);
            this.Controls.Add(this.SubnetmaskLabel);
            this.Controls.Add(this.SubnetmaskDropdown);
            this.Controls.Add(this.NetworkTextBox);
            this.Controls.Add(this.NetworkLabel);
            this.Controls.Add(this.LocalIPAddresTextBox);
            this.Controls.Add(this.LocalIPAddressLabel);
            this.Name = "ActionScan";
            this.Text = "Scan";
            this.Controls.SetChildIndex(this.titlePanel, 0);
            this.Controls.SetChildIndex(this.LocalIPAddressLabel, 0);
            this.Controls.SetChildIndex(this.LocalIPAddresTextBox, 0);
            this.Controls.SetChildIndex(this.NetworkLabel, 0);
            this.Controls.SetChildIndex(this.NetworkTextBox, 0);
            this.Controls.SetChildIndex(this.SubnetmaskDropdown, 0);
            this.Controls.SetChildIndex(this.SubnetmaskLabel, 0);
            this.Controls.SetChildIndex(this.OneShotCheckBox, 0);
            this.Controls.SetChildIndex(this.ScanButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.progressBar1, 0);
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LocalIPAddressLabel;
        private System.Windows.Forms.TextBox LocalIPAddresTextBox;
        private System.Windows.Forms.TextBox NetworkTextBox;
        private System.Windows.Forms.Label NetworkLabel;
        private System.Windows.Forms.ComboBox SubnetmaskDropdown;
        private System.Windows.Forms.Label SubnetmaskLabel;
        private System.Windows.Forms.CheckBox OneShotCheckBox;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}