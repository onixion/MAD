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
            this.menuStrip.Hide();
            this.SubnetmaskDropdown.SelectedIndex = 23;
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
            if (SubnetmaskDropdown.SelectedItem != null && LocalIPAddresTextBox.Text != null && NetworkTextBox.Text != null)
            {
                ARPReader.subnetMask = Convert.ToUInt32(SubnetmaskDropdown.SelectedItem);
                ARPReader.srcAddress = IPAddress.Parse(LocalIPAddresTextBox.Text);
                ARPReader.netAddress = IPAddress.Parse(NetworkTextBox.Text);
            }
            else
            {
                MessageBox.Show("Please select a value for the missing fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (OneShotCheckBox.Checked)
            {
                ARPReader.SetWindow(this);
                ARPReader.FloodStart();
                BaseGUI.js.SyncNodes(ModelHost.hostList);
                ARPReader.ResetWindow();
                MessageBox.Show("Updated Nodes. You may want to update the node windows.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ARPReader.SteadyStart(BaseGUI.js);
                MessageBox.Show("Started scaning.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return; 
        }

        private void NetworkTextBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButton();
        }

        private void LocalIPAddresTextBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButton();
        }

        private void ToggleButton()
        {
            if (String.IsNullOrEmpty(NetworkTextBox.Text) || String.IsNullOrEmpty(LocalIPAddresTextBox.Text))
                ScanButton.Enabled = false;
            else
                ScanButton.Enabled = true; 
        }
    }
}