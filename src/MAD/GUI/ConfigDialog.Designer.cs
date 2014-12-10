namespace GUI
{
    partial class ConfigDialog
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
            this.NotificSettingsLabel = new System.Windows.Forms.Label();
            this.SourceMail = new System.Windows.Forms.TextBox();
            this.SourceMailLabel = new System.Windows.Forms.Label();
            this.TargetMailLabel = new System.Windows.Forms.Label();
            this.TargetMail = new System.Windows.Forms.TextBox();
            this.SmtpPassLabel = new System.Windows.Forms.Label();
            this.SmtpPass = new System.Windows.Forms.TextBox();
            this.SmtpSrvLabel = new System.Windows.Forms.Label();
            this.SmtpSrv = new System.Windows.Forms.TextBox();
            this.SmtpPortLabel = new System.Windows.Forms.Label();
            this.SmtpPort = new System.Windows.Forms.TextBox();
            this.SnmpIntLabel = new System.Windows.Forms.Label();
            this.SnmpInt = new System.Windows.Forms.TextBox();
            this.ArpIntLabel = new System.Windows.Forms.Label();
            this.ArpInt = new System.Windows.Forms.TextBox();
            this.NetworkSettingsLabel = new System.Windows.Forms.Label();
            this.OthersLabel = new System.Windows.Forms.Label();
            this.PathToLogFileLabel = new System.Windows.Forms.Label();
            this.PathToLog = new System.Windows.Forms.TextBox();
            this.EnableNotification = new System.Windows.Forms.CheckBox();
            this.SafeSettings = new System.Windows.Forms.Button();
            this.UseDefaults = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NotificSettingsLabel
            // 
            this.NotificSettingsLabel.AutoSize = true;
            this.NotificSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotificSettingsLabel.Location = new System.Drawing.Point(12, 72);
            this.NotificSettingsLabel.Name = "NotificSettingsLabel";
            this.NotificSettingsLabel.Size = new System.Drawing.Size(273, 31);
            this.NotificSettingsLabel.TabIndex = 0;
            this.NotificSettingsLabel.Text = "-Notification Settings:";
            // 
            // SourceMail
            // 
            this.SourceMail.AllowDrop = true;
            this.SourceMail.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SourceMail.Location = new System.Drawing.Point(30, 141);
            this.SourceMail.Name = "SourceMail";
            this.SourceMail.Size = new System.Drawing.Size(189, 20);
            this.SourceMail.TabIndex = 1;
            this.SourceMail.Text = "maalda.group@outlook.com";
            this.SourceMail.UseWaitCursor = true;
            this.SourceMail.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SourceMailLabel
            // 
            this.SourceMailLabel.AutoSize = true;
            this.SourceMailLabel.Location = new System.Drawing.Point(27, 125);
            this.SourceMailLabel.Name = "SourceMailLabel";
            this.SourceMailLabel.Size = new System.Drawing.Size(73, 13);
            this.SourceMailLabel.TabIndex = 2;
            this.SourceMailLabel.Text = "E-Mail Source";
            // 
            // TargetMailLabel
            // 
            this.TargetMailLabel.AutoSize = true;
            this.TargetMailLabel.Location = new System.Drawing.Point(247, 125);
            this.TargetMailLabel.Name = "TargetMailLabel";
            this.TargetMailLabel.Size = new System.Drawing.Size(70, 13);
            this.TargetMailLabel.TabIndex = 4;
            this.TargetMailLabel.Text = "E-Mail Target";
            // 
            // TargetMail
            // 
            this.TargetMail.AllowDrop = true;
            this.TargetMail.ForeColor = System.Drawing.SystemColors.GrayText;
            this.TargetMail.Location = new System.Drawing.Point(250, 141);
            this.TargetMail.Name = "TargetMail";
            this.TargetMail.Size = new System.Drawing.Size(189, 20);
            this.TargetMail.TabIndex = 3;
            this.TargetMail.Text = "xample@idk.bayern";
            this.TargetMail.UseWaitCursor = true;
            this.TargetMail.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SmtpPassLabel
            // 
            this.SmtpPassLabel.AutoSize = true;
            this.SmtpPassLabel.Location = new System.Drawing.Point(471, 125);
            this.SmtpPassLabel.Name = "SmtpPassLabel";
            this.SmtpPassLabel.Size = new System.Drawing.Size(53, 13);
            this.SmtpPassLabel.TabIndex = 6;
            this.SmtpPassLabel.Text = "Password";
            // 
            // SmtpPass
            // 
            this.SmtpPass.AllowDrop = true;
            this.SmtpPass.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SmtpPass.Location = new System.Drawing.Point(474, 141);
            this.SmtpPass.Name = "SmtpPass";
            this.SmtpPass.Size = new System.Drawing.Size(189, 20);
            this.SmtpPass.TabIndex = 5;
            this.SmtpPass.UseSystemPasswordChar = true;
            this.SmtpPass.UseWaitCursor = true;
            // 
            // SmtpSrvLabel
            // 
            this.SmtpSrvLabel.AutoSize = true;
            this.SmtpSrvLabel.Location = new System.Drawing.Point(27, 193);
            this.SmtpSrvLabel.Name = "SmtpSrvLabel";
            this.SmtpSrvLabel.Size = new System.Drawing.Size(71, 13);
            this.SmtpSrvLabel.TabIndex = 8;
            this.SmtpSrvLabel.Text = "SMTP-Server";
            // 
            // SmtpSrv
            // 
            this.SmtpSrv.AllowDrop = true;
            this.SmtpSrv.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SmtpSrv.Location = new System.Drawing.Point(30, 209);
            this.SmtpSrv.Name = "SmtpSrv";
            this.SmtpSrv.Size = new System.Drawing.Size(189, 20);
            this.SmtpSrv.TabIndex = 7;
            this.SmtpSrv.Text = "smtp-mail.outlook.com";
            this.SmtpSrv.UseWaitCursor = true;
            this.SmtpSrv.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SmtpPortLabel
            // 
            this.SmtpPortLabel.AutoSize = true;
            this.SmtpPortLabel.Location = new System.Drawing.Point(247, 193);
            this.SmtpPortLabel.Name = "SmtpPortLabel";
            this.SmtpPortLabel.Size = new System.Drawing.Size(59, 13);
            this.SmtpPortLabel.TabIndex = 10;
            this.SmtpPortLabel.Text = "SMTP-Port";
            // 
            // SmtpPort
            // 
            this.SmtpPort.AllowDrop = true;
            this.SmtpPort.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SmtpPort.Location = new System.Drawing.Point(250, 209);
            this.SmtpPort.Name = "SmtpPort";
            this.SmtpPort.Size = new System.Drawing.Size(189, 20);
            this.SmtpPort.TabIndex = 9;
            this.SmtpPort.Text = "587";
            this.SmtpPort.UseWaitCursor = true;
            this.SmtpPort.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SnmpIntLabel
            // 
            this.SnmpIntLabel.AutoSize = true;
            this.SnmpIntLabel.Location = new System.Drawing.Point(247, 337);
            this.SnmpIntLabel.Name = "SnmpIntLabel";
            this.SnmpIntLabel.Size = new System.Drawing.Size(94, 13);
            this.SnmpIntLabel.TabIndex = 15;
            this.SnmpIntLabel.Text = "Interface für Snmp";
            // 
            // SnmpInt
            // 
            this.SnmpInt.AllowDrop = true;
            this.SnmpInt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SnmpInt.Location = new System.Drawing.Point(250, 353);
            this.SnmpInt.Name = "SnmpInt";
            this.SnmpInt.Size = new System.Drawing.Size(189, 20);
            this.SnmpInt.TabIndex = 14;
            this.SnmpInt.Text = "1";
            this.SnmpInt.UseWaitCursor = true;
            // 
            // ArpIntLabel
            // 
            this.ArpIntLabel.AutoSize = true;
            this.ArpIntLabel.Location = new System.Drawing.Point(27, 337);
            this.ArpIntLabel.Name = "ArpIntLabel";
            this.ArpIntLabel.Size = new System.Drawing.Size(97, 13);
            this.ArpIntLabel.TabIndex = 13;
            this.ArpIntLabel.Text = "Interface for Scans";
            // 
            // ArpInt
            // 
            this.ArpInt.AllowDrop = true;
            this.ArpInt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ArpInt.Location = new System.Drawing.Point(30, 353);
            this.ArpInt.Name = "ArpInt";
            this.ArpInt.Size = new System.Drawing.Size(189, 20);
            this.ArpInt.TabIndex = 12;
            this.ArpInt.Text = "1";
            this.ArpInt.UseWaitCursor = true;
            // 
            // NetworkSettingsLabel
            // 
            this.NetworkSettingsLabel.AutoSize = true;
            this.NetworkSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NetworkSettingsLabel.Location = new System.Drawing.Point(12, 284);
            this.NetworkSettingsLabel.Name = "NetworkSettingsLabel";
            this.NetworkSettingsLabel.Size = new System.Drawing.Size(230, 31);
            this.NetworkSettingsLabel.TabIndex = 11;
            this.NetworkSettingsLabel.Text = "-Network Settings";
            // 
            // OthersLabel
            // 
            this.OthersLabel.AutoSize = true;
            this.OthersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OthersLabel.Location = new System.Drawing.Point(12, 433);
            this.OthersLabel.Name = "OthersLabel";
            this.OthersLabel.Size = new System.Drawing.Size(105, 31);
            this.OthersLabel.TabIndex = 16;
            this.OthersLabel.Text = "-Others";
            // 
            // PathToLogFileLabel
            // 
            this.PathToLogFileLabel.AutoSize = true;
            this.PathToLogFileLabel.Location = new System.Drawing.Point(27, 491);
            this.PathToLogFileLabel.Name = "PathToLogFileLabel";
            this.PathToLogFileLabel.Size = new System.Drawing.Size(115, 13);
            this.PathToLogFileLabel.TabIndex = 18;
            this.PathToLogFileLabel.Text = "Path to Log file (relativ)";
            // 
            // PathToLog
            // 
            this.PathToLog.AllowDrop = true;
            this.PathToLog.ForeColor = System.Drawing.SystemColors.GrayText;
            this.PathToLog.Location = new System.Drawing.Point(30, 507);
            this.PathToLog.Name = "PathToLog";
            this.PathToLog.Size = new System.Drawing.Size(189, 20);
            this.PathToLog.TabIndex = 17;
            this.PathToLog.Text = ".";
            this.PathToLog.UseWaitCursor = true;
            this.PathToLog.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // EnableNotification
            // 
            this.EnableNotification.AutoSize = true;
            this.EnableNotification.Location = new System.Drawing.Point(274, 508);
            this.EnableNotification.Name = "EnableNotification";
            this.EnableNotification.Size = new System.Drawing.Size(113, 17);
            this.EnableNotification.TabIndex = 19;
            this.EnableNotification.Text = "Enable notification";
            this.EnableNotification.UseVisualStyleBackColor = true;
            // 
            // SafeSettings
            // 
            this.SafeSettings.Location = new System.Drawing.Point(662, 505);
            this.SafeSettings.Name = "SafeSettings";
            this.SafeSettings.Size = new System.Drawing.Size(75, 23);
            this.SafeSettings.TabIndex = 20;
            this.SafeSettings.Text = "Save";
            this.SafeSettings.UseVisualStyleBackColor = true;
            this.SafeSettings.Click += new System.EventHandler(this.SafeSettings_Click);
            // 
            // UseDefaults
            // 
            this.UseDefaults.Location = new System.Drawing.Point(743, 505);
            this.UseDefaults.Name = "UseDefaults";
            this.UseDefaults.Size = new System.Drawing.Size(75, 23);
            this.UseDefaults.TabIndex = 21;
            this.UseDefaults.Text = "Use Default";
            this.UseDefaults.UseVisualStyleBackColor = true;
            this.UseDefaults.Click += new System.EventHandler(this.UseDefaults_Click);
            // 
            // PathToLogFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.UseDefaults);
            this.Controls.Add(this.SafeSettings);
            this.Controls.Add(this.EnableNotification);
            this.Controls.Add(this.PathToLogFileLabel);
            this.Controls.Add(this.PathToLog);
            this.Controls.Add(this.OthersLabel);
            this.Controls.Add(this.SnmpIntLabel);
            this.Controls.Add(this.SnmpInt);
            this.Controls.Add(this.ArpIntLabel);
            this.Controls.Add(this.ArpInt);
            this.Controls.Add(this.NetworkSettingsLabel);
            this.Controls.Add(this.SmtpPortLabel);
            this.Controls.Add(this.SmtpPort);
            this.Controls.Add(this.SmtpSrvLabel);
            this.Controls.Add(this.SmtpSrv);
            this.Controls.Add(this.SmtpPassLabel);
            this.Controls.Add(this.SmtpPass);
            this.Controls.Add(this.TargetMailLabel);
            this.Controls.Add(this.TargetMail);
            this.Controls.Add(this.SourceMailLabel);
            this.Controls.Add(this.SourceMail);
            this.Controls.Add(this.NotificSettingsLabel);
            this.Name = "PathToLogFile";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NotificSettingsLabel;
        private System.Windows.Forms.TextBox SourceMail;
        private System.Windows.Forms.Label SourceMailLabel;
        private System.Windows.Forms.Label TargetMailLabel;
        private System.Windows.Forms.TextBox TargetMail;
        private System.Windows.Forms.Label SmtpPassLabel;
        private System.Windows.Forms.TextBox SmtpPass;
        private System.Windows.Forms.Label SmtpSrvLabel;
        private System.Windows.Forms.TextBox SmtpSrv;
        private System.Windows.Forms.Label SmtpPortLabel;
        private System.Windows.Forms.TextBox SmtpPort;
        private System.Windows.Forms.Label SnmpIntLabel;
        private System.Windows.Forms.TextBox SnmpInt;
        private System.Windows.Forms.Label ArpIntLabel;
        private System.Windows.Forms.TextBox ArpInt;
        private System.Windows.Forms.Label NetworkSettingsLabel;
        private System.Windows.Forms.Label OthersLabel;
        private System.Windows.Forms.Label PathToLogFileLabel;
        private System.Windows.Forms.TextBox PathToLog;
        private System.Windows.Forms.CheckBox EnableNotification;
        private System.Windows.Forms.Button SafeSettings;
        private System.Windows.Forms.Button UseDefaults;
    }
}