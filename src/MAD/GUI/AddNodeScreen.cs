using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MAD.JobSystemCore;

namespace MAD.GUI
{
    public partial class AddNodeScreen : BaseGUI
    {
        JobSystem _js;
        public AddNodeScreen(JobSystem _js)
        {
            this._js = _js;
            InitializeComponent();
            this.menuStrip.Hide();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ExecuteAddingNode();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExecuteAddingNode()
        {
            if (String.IsNullOrEmpty(nodeNameTextBox.Text) || String.IsNullOrEmpty(ipAddrTextBox.Text) || String.IsNullOrEmpty(macAddrTextBox.Text))
            {
                MessageBox.Show("Please insert values in all fields!", "Error: Missing values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string _name = nodeNameTextBox.Text;
            string _macAddr = macAddrTextBox.Text;
            string _ipAddr = ipAddrTextBox.Text;

            JobNode _tmp = new JobNode(_name, _macAddr, System.Net.IPAddress.Parse(_ipAddr), new List<Job> { });
            _js.AddNode(_tmp);
        }

        private void nodeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButton();
        }

        private void ipAddrTextBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButton();
        }

        private void macAddrTextBox_TextChanged(object sender, EventArgs e)
        {
            ToggleButton();
        }

        private void ToggleButton()
        {
            if (String.IsNullOrEmpty(nodeNameTextBox.Text) || String.IsNullOrEmpty(ipAddrTextBox.Text) || String.IsNullOrEmpty(macAddrTextBox.Text))
                this.addButton.Enabled = false;
            else
                this.addButton.Enabled = true;

            return;
        }
    }
}
