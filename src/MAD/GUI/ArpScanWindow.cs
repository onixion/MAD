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
    public partial class ArpScanWindow : Form
    {
        public ArpScanWindow()
        {
            InitializeComponent();
            if ((textBoxLocalIP.ForeColor != Color.DarkGray) && (textBoxNetwork.ForeColor != Color.DarkGray) && (comboBoxSubnetmask.SelectedText != ""))
            {
                Application.Exit();
            }
        }

        #region textBoxLocalIP

        private void textBoxLocalIP_GotFocus(object sender, EventArgs e)
        {
            if (textBoxLocalIP.ForeColor == Color.DarkGray)
            {
                textBoxLocalIP.Text = "";
                textBoxLocalIP.ForeColor = Color.Black;
            }
        }

        private void textBoxLocalIP_LostFocus(object sender, EventArgs e)
        {
            if (textBoxLocalIP.Text == "")
            {
                textBoxLocalIP.ForeColor = Color.DarkGray;
                textBoxLocalIP.Text = "192.168.1.1";
            }
        }

        private void textBoxLocalIP_TextChanged(object sender, EventArgs e)
        {
            if (((textBoxLocalIP.Text != "") && (textBoxLocalIP.ForeColor != Color.DarkGray)) && ((textBoxNetwork.Text != "") && (textBoxNetwork.ForeColor != Color.DarkGray)) && (comboBoxSubnetmask.Text != "Please choose"))
            {
                buttonStartScan.Enabled = true;
            }
            else
                buttonStartScan.Enabled = false;
        }

        #endregion

        #region textBoxNetwork

        private void textBoxNetwork_GotFocus(object sender, EventArgs e)
        {
            if (textBoxNetwork.ForeColor == Color.DarkGray)
            {
                textBoxNetwork.Text = "";
                textBoxNetwork.ForeColor = Color.Black;
            }
        }

        private void textBoxNetwork_LostFocus(object sender, EventArgs e)
        {
            if(textBoxNetwork.Text == "")
            {
                textBoxNetwork.ForeColor = Color.DarkGray;
                textBoxNetwork.Text = "192.168.1.0";
            }
        }
        
        private void textBoxNetwork_TextChanged(object sender, EventArgs e)
        {
            if (((textBoxNetwork.Text != "") && (textBoxNetwork.ForeColor != Color.DarkGray)) && ((textBoxLocalIP.Text != "") && (textBoxLocalIP.ForeColor != Color.DarkGray)) && (comboBoxSubnetmask.Text != "Please choose"))
            {
                buttonStartScan.Enabled = true;
            }
            else
                buttonStartScan.Enabled = false;

        }

        #endregion

        private void checkBoxOneScan_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxOneScan.ForeColor == Color.DarkGray)
            {
                checkBoxOneScan.ForeColor = Color.Black;
            }

            else
                checkBoxOneScan.ForeColor = Color.DarkGray;
        }

        private void comboBoxSubnetmask_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((textBoxNetwork.ForeColor != Color.DarkGray) && (textBoxLocalIP.ForeColor != Color.DarkGray))
            {
                buttonStartScan.Enabled = true;
            }
            else
                buttonStartScan.Enabled = false;

        }
        
        private void buttonStartScan_Click(object sender, EventArgs e)
        {
            
        }



    }
}
