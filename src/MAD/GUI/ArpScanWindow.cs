﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

using SharpPcap;

using MAD;
using MAD.MacFinders;
using MAD.JobSystemCore;
using MAD.Database;


namespace MAD.GUI
{
    public partial class ArpScanWindow : Form
    {
        #region Decleration

        protected static JobSystem _js;
        protected static DB _db;
        protected static MainWindow _mw;
        private bool validEntering = false;


        public static void InitGUI(JobSystem jobSys, DB dataBase, MainWindow mainWindow)
        {
            _db = dataBase;
            _js = jobSys;
            _mw = mainWindow;
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
            if (0 < comboBoxSubnetmask.SelectedIndex && comboBoxSubnetmask.SelectedIndex < 33)
            {
                if (checkBoxOneScan.Checked == true)
                {
                    progressBarArpScan.Visible = true;
                }

                InvokeArpReader();

                if (validEntering == true)
                {
                    this.textBoxLocalIP.Text = "";
                    this.textBoxNetwork.Text = "";
                    this.comboBoxSubnetmask.SelectedIndex = -1;
                    this.comboBoxSubnetmask.Text = "Please Choose";
                    this.comboBoxNWD.SelectedIndex = -1;
                    this.comboBoxNWD.Text = "Default";
                    this.checkBoxOneScan.Checked = false;
                    this.progressBarArpScan.Value = 0;
                    this.buttonStartScan.Enabled = false;
                    this.Close();
                }
            }
            else
                MessageBox.Show("Please check the fields again, your enterings seem to be incorrect", "Ups...Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void ArpScanWindow_Load(object sender, EventArgs e)
        {
            ComboBoxNWDloadContent();
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
                if (this.comboBoxNWD.Text != "Default")
                {
                    ARPReader.networkInterface = Convert.ToUInt16(this.comboBoxNWD.SelectedItem.ToString()) - (uint)1;
                }

                if (checkBoxOneScan.Checked)
                {
                    ARPReader.SetWindow(this);
                    ARPReader.FloodStart();
                    _js.SyncNodes(ModelHost.hostList);
                    ARPReader.ResetWindow();
                }

                else
                {
                    ARPReader.SteadyStart(_js);
                    GUILogic.SteadyActivated = true;
                    _mw.buttonScan.Enabled = false;
                }
                validEntering = true;
            }

            catch
            {
                MessageBox.Show("Please check the fields again, your enterings seem to be incorrect", "Ups...Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                validEntering = false;
            }

            
            return;
        }

        private void ComboBoxNWDloadContent()
        {
            CaptureDeviceList _list = CaptureDeviceList.Instance;
            List<object> devList = new List<object>();

            for(int i = 0; i <= _list.Count; i++)
            {
                int devNr = i+1;
                devList.Add(devNr);
            }

            this.comboBoxNWD.Items.Clear();
            this.comboBoxNWD.Items.Add("Default");

            object[] devs = devList.ToArray();
            this.comboBoxNWD.Items.AddRange(devs);
        }

        #endregion

    }
}
