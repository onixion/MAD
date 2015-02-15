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
    public partial class MainWindow : Form
    {
        public static string configStatus;

        public MainWindow()
        {            
            InitializeComponent();
            labelConfigStatus.Text = configStatus;
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            labelLastReloadTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void MainWindow_FormClosing(object sender, EventArgs e)
        {
            FormCollection openForms = Application.OpenForms;

            foreach (Form form in openForms)
            {
                Environment.Exit(0);
            }

            Application.Exit();

        }

    }
}
