using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MAD;
using MAD.JobSystemCore;

namespace MAD.GUI
{
    public partial class MainWindow : Form
    {
        public static string configStatus;
        protected static JobSystem _js;
        private List<JobNodeInfo> _nodes = new List<JobNodeInfo>();
        List<JobInfo> _jobInfo = new List<JobInfo>();
        
        public static void InitGUI(JobSystem jobSys)
        {
            _js = jobSys;
        }

        public MainWindow()
        {            
            InitializeComponent();
            labelConfigStatus.Text = configStatus;
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            _nodes = _js.GetNodesAndJobs();
            this.listBoxNodes.Items.Clear();
            foreach(JobNodeInfo node in _nodes)
            {
                this.listBoxNodes.Items.Add(node.mac);
            }
            labelLastReloadTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void MainWindow_FormClosing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listBoxNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelInfoMAC.Text = _nodes[listBoxNodes.SelectedIndex].mac;
            labelInfoID.Text = _nodes[listBoxNodes.SelectedIndex].id.ToString();
            labelInfoGUID.Text = _nodes[listBoxNodes.SelectedIndex].guid.ToString();
            labelInfoIP.Text = _nodes[listBoxNodes.SelectedIndex].ip.ToString();
            labelInfoName.Text = _nodes[listBoxNodes.SelectedIndex].name;
            labelInfoState.Text = _js.NodeState(_nodes[listBoxNodes.SelectedIndex].state);

            foreach (JobNodeInfo node in _nodes)
            {
                foreach (JobInfo jf in _jobInfo)
                {
                    this.listBoxInfoJobs.Items.Add(jf.type);
                }
            }
        }

    }
}
