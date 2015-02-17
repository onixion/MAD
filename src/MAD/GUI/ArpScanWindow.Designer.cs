namespace MAD.GUI
{
    partial class ArpScanWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArpScanWindow));
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBoxTopPanel = new System.Windows.Forms.PictureBox();
            this.labelTopBoxTitle = new System.Windows.Forms.Label();
            this.labelLocalIPTitle = new System.Windows.Forms.Label();
            this.textBoxLocalIP = new System.Windows.Forms.TextBox();
            this.textBoxNetwork = new System.Windows.Forms.TextBox();
            this.labelNetworkTitle = new System.Windows.Forms.Label();
            this.labelSubnetTitle = new System.Windows.Forms.Label();
            this.comboBoxSubnetmask = new System.Windows.Forms.ComboBox();
            this.buttonStartScan = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxOneScan = new System.Windows.Forms.CheckBox();
            this.progressBarArpScan = new System.Windows.Forms.ProgressBar();
            this.labelNetDevTitle = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.pictureBoxTopPanel);
            this.panelTop.Controls.Add(this.labelTopBoxTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(251, 56);
            this.panelTop.TabIndex = 1;
            // 
            // pictureBoxTopPanel
            // 
            this.pictureBoxTopPanel.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTopPanel.Image")));
            this.pictureBoxTopPanel.Location = new System.Drawing.Point(11, 9);
            this.pictureBoxTopPanel.Name = "pictureBoxTopPanel";
            this.pictureBoxTopPanel.Size = new System.Drawing.Size(42, 40);
            this.pictureBoxTopPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTopPanel.TabIndex = 2;
            this.pictureBoxTopPanel.TabStop = false;
            // 
            // labelTopBoxTitle
            // 
            this.labelTopBoxTitle.AutoSize = true;
            this.labelTopBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.labelTopBoxTitle.ForeColor = System.Drawing.SystemColors.Control;
            this.labelTopBoxTitle.Location = new System.Drawing.Point(59, 9);
            this.labelTopBoxTitle.Name = "labelTopBoxTitle";
            this.labelTopBoxTitle.Size = new System.Drawing.Size(95, 39);
            this.labelTopBoxTitle.TabIndex = 1;
            this.labelTopBoxTitle.Text = "Scan";
            // 
            // labelLocalIPTitle
            // 
            this.labelLocalIPTitle.AutoSize = true;
            this.labelLocalIPTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelLocalIPTitle.Location = new System.Drawing.Point(11, 68);
            this.labelLocalIPTitle.Name = "labelLocalIPTitle";
            this.labelLocalIPTitle.Size = new System.Drawing.Size(119, 15);
            this.labelLocalIPTitle.TabIndex = 2;
            this.labelLocalIPTitle.Text = "Local IP-Address:";
            // 
            // textBoxLocalIP
            // 
            this.textBoxLocalIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLocalIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.textBoxLocalIP.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxLocalIP.Location = new System.Drawing.Point(134, 66);
            this.textBoxLocalIP.MaxLength = 15;
            this.textBoxLocalIP.Name = "textBoxLocalIP";
            this.textBoxLocalIP.Size = new System.Drawing.Size(104, 21);
            this.textBoxLocalIP.TabIndex = 8;
            this.textBoxLocalIP.Text = "192.168.1.1";
            this.toolTip.SetToolTip(this.textBoxLocalIP, "Please enter the IP-Address\r\nof the Networkdevice.");
            this.textBoxLocalIP.TextChanged += new System.EventHandler(this.textBoxLocalIP_TextChanged);
            this.textBoxLocalIP.GotFocus += new System.EventHandler(this.textBoxLocalIP_GotFocus);
            this.textBoxLocalIP.LostFocus += new System.EventHandler(this.textBoxLocalIP_LostFocus);
            // 
            // textBoxNetwork
            // 
            this.textBoxNetwork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNetwork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.textBoxNetwork.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxNetwork.Location = new System.Drawing.Point(134, 91);
            this.textBoxNetwork.MaxLength = 15;
            this.textBoxNetwork.Name = "textBoxNetwork";
            this.textBoxNetwork.Size = new System.Drawing.Size(104, 21);
            this.textBoxNetwork.TabIndex = 10;
            this.textBoxNetwork.Text = "192.168.1.0";
            this.toolTip.SetToolTip(this.textBoxNetwork, "Please enter the\r\nNetwork-Address.");
            this.textBoxNetwork.TextChanged += new System.EventHandler(this.textBoxNetwork_TextChanged);
            this.textBoxNetwork.GotFocus += new System.EventHandler(this.textBoxNetwork_GotFocus);
            this.textBoxNetwork.LostFocus += new System.EventHandler(this.textBoxNetwork_LostFocus);
            // 
            // labelNetworkTitle
            // 
            this.labelNetworkTitle.AutoSize = true;
            this.labelNetworkTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelNetworkTitle.Location = new System.Drawing.Point(11, 93);
            this.labelNetworkTitle.Name = "labelNetworkTitle";
            this.labelNetworkTitle.Size = new System.Drawing.Size(63, 15);
            this.labelNetworkTitle.TabIndex = 9;
            this.labelNetworkTitle.Text = "Network:";
            // 
            // labelSubnetTitle
            // 
            this.labelSubnetTitle.AutoSize = true;
            this.labelSubnetTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSubnetTitle.Location = new System.Drawing.Point(11, 118);
            this.labelSubnetTitle.Name = "labelSubnetTitle";
            this.labelSubnetTitle.Size = new System.Drawing.Size(56, 15);
            this.labelSubnetTitle.TabIndex = 11;
            this.labelSubnetTitle.Text = "Subnet:";
            // 
            // comboBoxSubnetmask
            // 
            this.comboBoxSubnetmask.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSubnetmask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxSubnetmask.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.comboBoxSubnetmask.FormattingEnabled = true;
            this.comboBoxSubnetmask.Items.AddRange(new object[] {
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
            this.comboBoxSubnetmask.Location = new System.Drawing.Point(134, 115);
            this.comboBoxSubnetmask.Name = "comboBoxSubnetmask";
            this.comboBoxSubnetmask.Size = new System.Drawing.Size(104, 23);
            this.comboBoxSubnetmask.TabIndex = 12;
            this.comboBoxSubnetmask.Text = "Please choose";
            this.toolTip.SetToolTip(this.comboBoxSubnetmask, "Please choose a Subnet");
            this.comboBoxSubnetmask.SelectedValueChanged += new System.EventHandler(this.comboBoxSubnetmask_SelectedValueChanged);
            // 
            // buttonStartScan
            // 
            this.buttonStartScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStartScan.Enabled = false;
            this.buttonStartScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonStartScan.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonStartScan.Location = new System.Drawing.Point(14, 237);
            this.buttonStartScan.Name = "buttonStartScan";
            this.buttonStartScan.Size = new System.Drawing.Size(224, 31);
            this.buttonStartScan.TabIndex = 13;
            this.buttonStartScan.Text = "Start Scan";
            this.buttonStartScan.UseVisualStyleBackColor = false;
            this.buttonStartScan.Click += new System.EventHandler(this.buttonStartScan_Click);
            // 
            // checkBoxOneScan
            // 
            this.checkBoxOneScan.AutoSize = true;
            this.checkBoxOneScan.ForeColor = System.Drawing.Color.DarkGray;
            this.checkBoxOneScan.Location = new System.Drawing.Point(14, 185);
            this.checkBoxOneScan.Name = "checkBoxOneScan";
            this.checkBoxOneScan.Size = new System.Drawing.Size(170, 17);
            this.checkBoxOneScan.TabIndex = 14;
            this.checkBoxOneScan.Text = "I want to make only one Scan.";
            this.checkBoxOneScan.UseVisualStyleBackColor = true;
            this.checkBoxOneScan.CheckedChanged += new System.EventHandler(this.checkBoxOneScan_CheckedChanged);
            // 
            // progressBarArpScan
            // 
            this.progressBarArpScan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarArpScan.Location = new System.Drawing.Point(14, 208);
            this.progressBarArpScan.Name = "progressBarArpScan";
            this.progressBarArpScan.Size = new System.Drawing.Size(224, 23);
            this.progressBarArpScan.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarArpScan.TabIndex = 15;
            this.progressBarArpScan.Visible = false;
            // 
            // labelNetDevTitle
            // 
            this.labelNetDevTitle.AutoSize = true;
            this.labelNetDevTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelNetDevTitle.Location = new System.Drawing.Point(11, 143);
            this.labelNetDevTitle.Name = "labelNetDevTitle";
            this.labelNetDevTitle.Size = new System.Drawing.Size(110, 15);
            this.labelNetDevTitle.TabIndex = 17;
            this.labelNetDevTitle.Text = "Network Device:";
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.Enabled = false;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
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
            this.comboBox1.Location = new System.Drawing.Point(134, 140);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(104, 23);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.Text = "Default";
            this.toolTip.SetToolTip(this.comboBox1, "Please choose a Subnet");
            // 
            // ArpScanWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 283);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelNetDevTitle);
            this.Controls.Add(this.progressBarArpScan);
            this.Controls.Add(this.checkBoxOneScan);
            this.Controls.Add(this.buttonStartScan);
            this.Controls.Add(this.comboBoxSubnetmask);
            this.Controls.Add(this.labelSubnetTitle);
            this.Controls.Add(this.textBoxNetwork);
            this.Controls.Add(this.labelNetworkTitle);
            this.Controls.Add(this.textBoxLocalIP);
            this.Controls.Add(this.labelLocalIPTitle);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ArpScanWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAD - Networkmonitoring | Scan for Devices";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pictureBoxTopPanel;
        private System.Windows.Forms.Label labelTopBoxTitle;
        private System.Windows.Forms.Label labelLocalIPTitle;
        public System.Windows.Forms.TextBox textBoxLocalIP;
        private System.Windows.Forms.TextBox textBoxNetwork;
        private System.Windows.Forms.Label labelNetworkTitle;
        private System.Windows.Forms.Label labelSubnetTitle;
        private System.Windows.Forms.ComboBox comboBoxSubnetmask;
        private System.Windows.Forms.Button buttonStartScan;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxOneScan;
        public System.Windows.Forms.ProgressBar progressBarArpScan;
        private System.Windows.Forms.Label labelNetDevTitle;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}