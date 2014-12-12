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
    public partial class Form_Main : Form
    {
        
            
        public Form_Main()
        {
            InitializeComponent();
           
            this.Activated += new EventHandler(Form_Main_Activated);
        }

        void Form_Main_Activated(object sender, EventArgs e)
        {
            Form Start = new Form_Start();
            Start.Show();
            this.Hide();
        }        

        
    }
}
