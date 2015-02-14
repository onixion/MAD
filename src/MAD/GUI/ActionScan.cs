using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MAD.MacFinders;

namespace MAD.GUI
{
    public partial class ActionScan : Form
    {
        protected static Database.DB db;
        protected static JobSystemCore.JobSystem js;
        public ActionScan()
        {
            InitializeComponent();
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ScanButton_Click(object sender, EventArgs e)
        {
            InvokeArpReader();
            this.Close();
        }
        private void InvokeArpReader()
        {
            ARPReader.srcAddress = IPAddress.Parse(LocalIPAddresTextBox.Text);
            ARPReader.netAddress = IPAddress.Parse(NetworkTextBox.Text);
            ARPReader.subnetMask = Convert.ToUInt32(SubnetmaskDropdown.SelectedIndex);
            if (OneShotCheckBox.Checked)
            {
                ARPReader.FloodStart();
                js.SyncNodes(ModelHost.hostList);
            }
            else
                ARPReader.SteadyStart(js);
            return;
        }

        private void listBoxNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
                    }

        private void LocalIPAddresTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}