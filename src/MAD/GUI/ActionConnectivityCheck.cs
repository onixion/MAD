using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

using MAD.CLICore;

namespace MAD.GUI
{
    public partial class ActionConnectivityCheck : BaseGUI
    {
        public ActionConnectivityCheck()
        {
            InitializeComponent();
        }

        private void intensityComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(this.intensityComboBox.SelectedItem) == 5)
                targetIPPanel.Visible = true;
            else
                targetIPPanel.Visible = false;
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            InitializeCheck();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeCheck()
        {
            string _buffer = ""; 
            switch(Convert.ToUInt16(this.intensityComboBox.SelectedItem))
            {
                case 1: 
                    _buffer += ConnectivityTestCommand.Intensity1Check();
                    break;
                case 2: 
                    _buffer += ConnectivityTestCommand.Intensity2Check();
                    break;
                case 3: 
                    _buffer += ConnectivityTestCommand.Intensity3Check();
                    break;
                case 4:
                    _buffer += ConnectivityTestCommand.Intensity4Check();
                    break;
                case 5:
                    _buffer += ConnectivityTestCommand.Intensity5Check(targetIPTextBox.Text.ToString());
                    break;
            }
            MessageBox.Show(_buffer, "Check Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return; 
        }
    }
}
