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
    public partial class ActionScan : BaseGUI
    {
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
            if (SubnetmaskDropdown.SelectedItem != null)
                ARPReader.subnetMask = Convert.ToUInt32(SubnetmaskDropdown.SelectedItem);
            else
            {
                MessageBox.Show("Please select a value for the subnetmask", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            if (OneShotCheckBox.Checked)
            {
                ARPReader.FloodStart();
                BaseGUI.js.SyncNodes(ModelHost.hostList);
            }
            else
                ARPReader.SteadyStart(BaseGUI.js);

            MessageBox.Show("Updated Nodes. You may want to update the node windows.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return; 
        }
    }
}