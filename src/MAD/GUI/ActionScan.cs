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
            ARPReader.subnetMask = Convert.ToUInt32(SubnetmaskDropdown.SelectedIndex);
            if (OneShotCheckBox.Checked)
            {
                ARPReader.FloodStart();
                BaseGUI.js.SyncNodes(ModelHost.hostList);
            }
            else
                ARPReader.SteadyStart(BaseGUI.js);

            return; 
        }
    }
}
