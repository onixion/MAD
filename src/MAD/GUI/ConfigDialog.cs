using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class ConfigDialog : Form
    {
        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            this.SMTP_USER.ForeColor = System.Drawing.SystemColors.WindowText;
        }

        private void ColorChangeTextBox()
        {
            if(this.SMTP_USER.ForeColor != System.Drawing.SystemColors.WindowText)
                this.SMTP_USER.ForeColor = System.Drawing.SystemColors.WindowText;
        }
    }
}
