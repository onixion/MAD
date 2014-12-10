using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

using MAD;

namespace GUI
{
    public partial class ConfigDialog : Form
    {
        private StartUp _cont = new StartUp();

        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            this.SourceMail.ForeColor = System.Drawing.SystemColors.WindowText;
        }

        private void ColorChangeTextBox()
        {
            if(this.SourceMail.ForeColor != System.Drawing.SystemColors.WindowText)
                this.SourceMail.ForeColor = System.Drawing.SystemColors.WindowText;
        }

        private void SafeSettings_Click(object sender, EventArgs e)
        {
            SetConf();
            MadConf.SaveConf(Mad.CONFFILE);

            StartUp.Show();
            this.Dispose();
            this.Close();
        }

        private void UseDefaults_Click(object sender, EventArgs e)
        {
            MadConf.SetToDefault();

            StartUp.Show();
            this.Dispose();
            this.Close();
        }

        private void SetConf()
        {
            MadConf.SetToDefault();

            if (this.SourceMail.Text != null) 
                MadConf.conf.SMTP_USER = new MailAddress(this.SourceMail.Text);

            if (this.TargetMail.Text != null) 
                MadConf.conf.MAIL_DEFAULT = new MailAddress[1]{ new MailAddress(this.TargetMail.Text) };

            if (this.SmtpPass != null) 
                MadConf.conf.SMTP_PASS = this.SmtpPass.Text;

            if (this.SmtpSrv.Text != null)
                MadConf.conf.SMTP_SERVER = this.SmtpSrv.Text;

            if (this.SmtpPort.Text != null)
                MadConf.conf.SMTP_PORT = Convert.ToInt32(this.SmtpPort.Text);

            if (this.ArpInt.Text != null)
                MadConf.conf.arpInterface = Convert.ToUInt32(this.ArpInt.Text);

            if (this.SnmpInt.Text != null)
                MadConf.conf.snmpInterface = this.SnmpInt.Text;

            if (this.PathToLog.Text != null)
                MadConf.conf.LOG_FILE_DIRECTORY = this.PathToLog.Text + Path.DirectorySeparatorChar;

            MadConf.conf.NOTI_ENABLE = this.EnableNotification.Checked;
        }
    }
}
