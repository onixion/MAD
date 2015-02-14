using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MAD_GUI
{
    public partial class Form_Start : Form
    {
        public Form_Start()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form ShowDevices = new Form_ShowDevices();
            ShowDevices.Show();
            this.Close();
        }

        private void Form_Start_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form SearchNodes = new Form_SearchNodes();
            SearchNodes.Show();
            this.Close();
        }

        private void Form_Start_Load_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Form_Start_Load_2(object sender, EventArgs e)
        {

        }
    }
}
