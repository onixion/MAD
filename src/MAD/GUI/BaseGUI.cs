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
using MAD.Database;


namespace MAD.GUI
{
    public partial class BaseGUI : Form 
    {
        #region fields
        protected static DB db;
        protected static JobSystem js;

        //protected static Font DEFAULT_FONT = new Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        #endregion

        public BaseGUI()
        {
            InitializeComponent();
        }

        public static void InitGui(JobSystem jobSys, DB dataBase)
        {
            db = dataBase;
            js = jobSys;
           
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionScan _tmp = new ActionScan();
            _tmp.ShowDialog();
        }

        private void connectivityCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionConnectivityCheck _tmp = new ActionConnectivityCheck();
            _tmp.ShowDialog();
        }

        private void BaseGUI_Load(object sender, EventArgs e)
        {

        }

        private void openWorkingDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = @path;
            process.StartInfo.Arguments = @" ";
            process.Start();
        }


    }
}
