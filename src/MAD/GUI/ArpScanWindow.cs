﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

using MAD;
using MAD.MacFinders;
using MAD.JobSystemCore;
using MAD.Database;


namespace MAD.GUI
{
    public partial class ArpScanWindow : Form
    {
        #region Decleration

        protected static JobSystem js;
        protected static DB db;
        private bool validEntering = false;


        public static void InitGui(JobSystem jobSys, DB dataBase)
        {
            db = dataBase;
            js = jobSys;
        }

        #endregion

        public ArpScanWindow()
        {
            InitializeComponent();
        }

        #region Events

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
            if (checkBoxOneScan.Checked == true)
            {
                progressBarArpScan.Visible = true;
            }

            InvokeArpReader();

            if (validEntering == true)
            {
                if (checkBoxOneScan.Checked)
                {
                    MainWindow MainWindow = new MainWindow();
                    MainWindow.Show();
                    this.Close();
                }

                else
                {
                    MainWindow MainWindow = new MainWindow();
                    MainWindow.Show();
                    this.Hide();
                }
            }
           
        }

        #endregion

        #region Logic

        private void InvokeArpReader()
        {

            try
            {
                ARPReader.srcAddress = IPAddress.Parse(textBoxLocalIP.Text);
                ARPReader.netAddress = IPAddress.Parse(textBoxNetwork.Text);
                ARPReader.subnetMask = Convert.ToUInt32(comboBoxSubnetmask.SelectedItem);
                if (checkBoxOneScan.Checked)
                {
                    ARPReader.SetWindow(this);
                    ARPReader.FloodStart();
                    js.SyncNodes(ModelHost.hostList);
                    ARPReader.ResetWindow();

                }
                else
                    ARPReader.SteadyStart(js);

                validEntering = true;
            }

            catch
            {
                MessageBox.Show("Please check the fields again, your enterings seem to be incorrect", "Wrong input!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                validEntering = false;
            }

            
            return;
        }

        #endregion


    }
}
