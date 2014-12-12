using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAD
{
    public partial class StartUp : Form
    {
        public StartUp()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           // Environment.Exit(0);
            Application.Exit();
        }

        private void jobsToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}
