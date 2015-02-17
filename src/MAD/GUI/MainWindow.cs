using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using MAD;
using MAD.JobSystemCore;
using MAD.CLICore;
using MAD.Logging;
using MAD.Database;
using MAD.MacFinders;
using MAD.Helper;

namespace MAD.GUI
{
    public partial class MainWindow : Form
    {
        #region Decleration

        public static string configStatus;
        protected static JobSystem _js;
        protected static DB _db;
        protected static DHCPReader _dhcp;
        protected static MAD.CLICore.CLI cli;
        public static Thread _CLITHREAD;
        private static int _countNode;
        private static int _countJob;
        static ArpScanWindow _Scan = new ArpScanWindow();
        Info _Info = new Info();
               
        private List<JobNodeInfo> _nodes = new List<JobNodeInfo>();
        List<JobInfo> _jobInfo = new List<JobInfo>();

        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            labelConfigStatus.Text = configStatus;
        }

        #region Events

        private void buttonReload_Click(object sender, EventArgs e)
        {
            _nodes = _js.GetNodesAndJobs();
            this.listBoxNodes.Items.Clear();
            this.listBoxInfoJobs.Items.Clear();
            _countNode = 0;
            foreach(JobNodeInfo node in _nodes)
            {
                _countNode++;
                this.listBoxNodes.Items.Add(_countNode.ToString() + ".) " + node.mac);
            }
            labelLastReloadTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void MainWindow_FormClosing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listBoxNodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBoxInfoJobs.Items.Clear();

            labelInfoJobsGUID.Text = "";
            labelInfoJobsID.Text = "";
            labelInfoJobsOutstate.Text = "";
            labelInfoJobsType.Text = "";

            labelInfoMAC.Text = _nodes[listBoxNodes.SelectedIndex].mac;
            labelInfoID.Text = _nodes[listBoxNodes.SelectedIndex].id.ToString();
            labelInfoGUID.Text = _nodes[listBoxNodes.SelectedIndex].guid.ToString();
            labelInfoIP.Text = _nodes[listBoxNodes.SelectedIndex].ip.ToString();
            labelInfoName.Text = _nodes[listBoxNodes.SelectedIndex].name;
            labelInfoState.Text = _js.NodeState(_nodes[listBoxNodes.SelectedIndex].state);
            textBoxInfoJobsMemo1.Text = _nodes[listBoxNodes.SelectedIndex].memo1;
            textBoxInfoJobsMemo2.Text = _nodes[listBoxNodes.SelectedIndex].memo2;



            if (_js.NodeState(_nodes[listBoxNodes.SelectedIndex].state) == "Active")
            {
                this.pictureBoxLightGreenOn.Visible = true;
                this.pictureBoxLightGreenOff.Visible = false;
                this.pictureBoxLightRedOn.Visible = false;
                this.pictureBoxLightRedoff.Visible = true;
            }

            else
            {
                this.pictureBoxLightGreenOn.Visible = false;
                this.pictureBoxLightGreenOff.Visible = true;
                this.pictureBoxLightRedOn.Visible = true;
                this.pictureBoxLightRedoff.Visible = false;
            }

            _countJob = 0;
            foreach (JobInfo jf in _nodes[listBoxNodes.SelectedIndex].jobs)
            {
                _countJob++;
                this.listBoxInfoJobs.Items.Add(_countJob.ToString() + ".) " + jf.name);
            }
            
        }

        private void buttonCLI_Click(object sender, EventArgs e)
        {
            Logger.Log("Programm Start. CLI Start.", Logger.MessageType.INFORM);
            _CLITHREAD.Start();
            this.buttonCLI.Enabled = false;

        }

        private void listBoxInfoJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelInfoJobsGUID.Text = _nodes[listBoxNodes.SelectedIndex].jobs[listBoxInfoJobs.SelectedIndex].guid.ToString();
            labelInfoJobsID.Text = _nodes[listBoxNodes.SelectedIndex].jobs[listBoxInfoJobs.SelectedIndex].id.ToString();
            labelInfoJobsOutstate.Text = _nodes[listBoxNodes.SelectedIndex].jobs[listBoxInfoJobs.SelectedIndex].outstate;
            labelInfoJobsType.Text = _nodes[listBoxNodes.SelectedIndex].jobs[listBoxInfoJobs.SelectedIndex].type;
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            _Info.ShowDialog(this);
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            Scan();
        }

        #endregion Events

        #region Logic

        public static void InitGUI(JobSystem jobSys, DB database, DHCPReader dhcp)
        {
            _js = jobSys;
            _db = database;
            _dhcp = dhcp;

            cli = new CLI(_js, _dhcp, _db);
            _CLITHREAD = new Thread(new ThreadStart(cli.Start));
        }

        private static void Scan()
        {
            _Scan.ShowDialog();
        }
        #endregion 

        

        

        

    }
}
