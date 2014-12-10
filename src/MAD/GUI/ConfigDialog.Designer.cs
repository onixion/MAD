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
            this.SMTP_USER = new System.Windows.Forms.TextBox();
            this.SourceMailLabel = new System.Windows.Forms.Label();
            this.TargetMailLabel = new System.Windows.Forms.Label();
            this.MAIL_DEFAULT = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.SMTP_PASS = new System.Windows.Forms.TextBox();
            this.SmtpSrvLabel = new System.Windows.Forms.Label();
            this.SNMP_SERV = new System.Windows.Forms.TextBox();
            this.SmtpPortLabel = new System.Windows.Forms.Label();
            this.SMTP_PORT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.ArpIntLabel = new System.Windows.Forms.Label();
            this.ArpInt = new System.Windows.Forms.TextBox();
            this.NetworkSettingsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NotificSettingsLabel
            // 
            this.NotificSettingsLabel.AutoSize = true;
            this.NotificSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotificSettingsLabel.Location = new System.Drawing.Point(12, 25);
            this.NotificSettingsLabel.Name = "NotificSettingsLabel";
            this.NotificSettingsLabel.Size = new System.Drawing.Size(273, 31);
            this.NotificSettingsLabel.TabIndex = 0;
            this.NotificSettingsLabel.Text = "-Notification Settings:";
            // 
            // SMTP_USER
            // 
            this.SMTP_USER.AllowDrop = true;
            this.SMTP_USER.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SMTP_USER.Location = new System.Drawing.Point(30, 94);
            this.SMTP_USER.Name = "SMTP_USER";
            this.SMTP_USER.Size = new System.Drawing.Size(189, 20);
            this.SMTP_USER.TabIndex = 1;
            this.SMTP_USER.Text = "maalda.group@outlook.com";
            this.SMTP_USER.UseWaitCursor = true;
            this.SMTP_USER.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SourceMailLabel
            // 
            this.SourceMailLabel.AutoSize = true;
            this.SourceMailLabel.Location = new System.Drawing.Point(27, 78);
            this.SourceMailLabel.Name = "SourceMailLabel";
            this.SourceMailLabel.Size = new System.Drawing.Size(73, 13);
            this.SourceMailLabel.TabIndex = 2;
            this.SourceMailLabel.Text = "E-Mail Source";
            // 
            // TargetMailLabel
            // 
            this.TargetMailLabel.AutoSize = true;
            this.TargetMailLabel.Location = new System.Drawing.Point(247, 78);
            this.TargetMailLabel.Name = "TargetMailLabel";
            this.TargetMailLabel.Size = new System.Drawing.Size(70, 13);
            this.TargetMailLabel.TabIndex = 4;
            this.TargetMailLabel.Text = "E-Mail Target";
            // 
            // MAIL_DEFAULT
            // 
            this.MAIL_DEFAULT.AllowDrop = true;
            this.MAIL_DEFAULT.ForeColor = System.Drawing.SystemColors.GrayText;
            this.MAIL_DEFAULT.Location = new System.Drawing.Point(250, 94);
            this.MAIL_DEFAULT.Name = "MAIL_DEFAULT";
            this.MAIL_DEFAULT.Size = new System.Drawing.Size(189, 20);
            this.MAIL_DEFAULT.TabIndex = 3;
            this.MAIL_DEFAULT.Text = "xample@idk.bayern";
            this.MAIL_DEFAULT.UseWaitCursor = true;
            this.MAIL_DEFAULT.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(471, 78);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswordLabel.TabIndex = 6;
            this.PasswordLabel.Text = "Password";
            // 
            // SMTP_PASS
            // 
            this.SMTP_PASS.AllowDrop = true;
            this.SMTP_PASS.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SMTP_PASS.Location = new System.Drawing.Point(474, 94);
            this.SMTP_PASS.Name = "SMTP_PASS";
            this.SMTP_PASS.Size = new System.Drawing.Size(189, 20);
            this.SMTP_PASS.TabIndex = 5;
            this.SMTP_PASS.UseSystemPasswordChar = true;
            this.SMTP_PASS.UseWaitCursor = true;
            // 
            // SmtpSrvLabel
            // 
            this.SmtpSrvLabel.AutoSize = true;
            this.SmtpSrvLabel.Location = new System.Drawing.Point(27, 146);
            this.SmtpSrvLabel.Name = "SmtpSrvLabel";
            this.SmtpSrvLabel.Size = new System.Drawing.Size(71, 13);
            this.SmtpSrvLabel.TabIndex = 8;
            this.SmtpSrvLabel.Text = "SMTP-Server";
            // 
            // SNMP_SERV
            // 
            this.SNMP_SERV.AllowDrop = true;
            this.SNMP_SERV.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SNMP_SERV.Location = new System.Drawing.Point(30, 162);
            this.SNMP_SERV.Name = "SNMP_SERV";
            this.SNMP_SERV.Size = new System.Drawing.Size(189, 20);
            this.SNMP_SERV.TabIndex = 7;
            this.SNMP_SERV.Text = "smtp-mail.outlook.com";
            this.SNMP_SERV.UseWaitCursor = true;
            this.SNMP_SERV.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
            // 
            // SmtpPortLabel
            // 
            this.SmtpPortLabel.AutoSize = true;
            this.SmtpPortLabel.Location = new System.Drawing.Point(247, 146);
            this.SmtpPortLabel.Name = "SmtpPortLabel";
            this.SmtpPortLabel.Size = new System.Drawing.Size(71, 13);
            this.SmtpPortLabel.TabIndex = 10;
            this.SmtpPortLabel.Text = "SMTP-Server";
            // 
            // SMTP_PORT
            // 
            this.SMTP_PORT.AllowDrop = true;
            this.SMTP_PORT.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SMTP_PORT.Location = new System.Drawing.Point(250, 162);
            this.SMTP_PORT.Name = "SMTP_PORT";
            this.SMTP_PORT.Size = new System.Drawing.Size(189, 20);
            this.SMTP_PORT.TabIndex = 9;
            this.SMTP_PORT.Text = "587";
            this.SMTP_PORT.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "E-Mail Target";
            // 
            // textBox2
            // 
            this.textBox2.AllowDrop = true;
            this.textBox2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.textBox2.Location = new System.Drawing.Point(250, 306);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(189, 20);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = "xample@idk.bayern";
            this.textBox2.UseWaitCursor = true;
            // 
            // ArpIntLabel
            // 
            this.ArpIntLabel.AutoSize = true;
            this.ArpIntLabel.Location = new System.Drawing.Point(27, 290);
            this.ArpIntLabel.Name = "ArpIntLabel";
            this.ArpIntLabel.Size = new System.Drawing.Size(97, 13);
            this.ArpIntLabel.TabIndex = 13;
            this.ArpIntLabel.Text = "Interface for Scans";
            // 
            // ArpInt
            // 
            this.ArpInt.AllowDrop = true;
            this.ArpInt.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ArpInt.Location = new System.Drawing.Point(30, 306);
            this.ArpInt.Name = "ArpInt";
            this.ArpInt.Size = new System.Drawing.Size(189, 20);
            this.ArpInt.TabIndex = 12;
            this.ArpInt.UseWaitCursor = true;
            // 
            // NetworkSettingsLabel
            // 
            this.NetworkSettingsLabel.AutoSize = true;
            this.NetworkSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NetworkSettingsLabel.Location = new System.Drawing.Point(12, 237);
            this.NetworkSettingsLabel.Name = "NetworkSettingsLabel";
            this.NetworkSettingsLabel.Size = new System.Drawing.Size(230, 31);
            this.NetworkSettingsLabel.TabIndex = 11;
            this.NetworkSettingsLabel.Text = "-Network Settings";
            // 
            // ConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.ArpIntLabel);
            this.Controls.Add(this.ArpInt);
            this.Controls.Add(this.NetworkSettingsLabel);
            this.Controls.Add(this.SmtpPortLabel);
            this.Controls.Add(this.SMTP_PORT);
            this.Controls.Add(this.SmtpSrvLabel);
            this.Controls.Add(this.SNMP_SERV);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.SMTP_PASS);
            this.Controls.Add(this.TargetMailLabel);
            this.Controls.Add(this.MAIL_DEFAULT);
            this.Controls.Add(this.SourceMailLabel);
            this.Controls.Add(this.SMTP_USER);
            this.Controls.Add(this.NotificSettingsLabel);
            this.Name = "ConfigDialog";
            this.Text = "Set Configuration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NotificSettingsLabel;
        private System.Windows.Forms.TextBox SMTP_USER;
        private System.Windows.Forms.Label SourceMailLabel;
        private System.Windows.Forms.Label TargetMailLabel;
        private System.Windows.Forms.TextBox MAIL_DEFAULT;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox SMTP_PASS;
        private System.Windows.Forms.Label SmtpSrvLabel;
        private System.Windows.Forms.TextBox SNMP_SERV;
        private System.Windows.Forms.Label SmtpPortLabel;
        private System.Windows.Forms.TextBox SMTP_PORT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label ArpIntLabel;
        private System.Windows.Forms.TextBox ArpInt;
        private System.Windows.Forms.Label NetworkSettingsLabel;
    }
}