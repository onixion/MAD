using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAD.GUI
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
            this.labelVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.github.com/onixion/MAD");
        }
        

    }
}
