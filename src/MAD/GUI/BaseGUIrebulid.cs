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
    public partial class BaseGUIrebulid : Form
    {
        public BaseGUIrebulid()
        {
            InitializeComponent();
            labelConfigStatus.Text = GUILogic.configStatus;
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            labelLastReloadTime.Text = DateTime.Now.ToLongTimeString();
        }

    }
}
