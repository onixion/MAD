using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;

using MAD;
using MAD.MacFinders;
using MAD.JobSystemCore;
using MAD.Database;

namespace MAD.GUI
{
    public static class GUILogic
    {
        public static MainWindow _MainWindow = new MainWindow();
        public static ArpScanWindow _ArpScanWindow = new ArpScanWindow();

        public static void RunGUI(JobSystem jobSys, DB dataBase)
        {
            ArpScanWindow.InitGUI(jobSys, dataBase);
            MainWindow.InitGUI(jobSys);
            Application.Run(_ArpScanWindow);           
        }

        public static void ExitArp()
        {
            _MainWindow.ShowDialog();
            _ArpScanWindow.Close();           
        }

        public static void RunBehind()
        {
            _ArpScanWindow.Hide();
            _MainWindow.ShowDialog();
        }
    }
}
