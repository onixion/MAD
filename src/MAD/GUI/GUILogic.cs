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
        public static bool SteadyActivated;
        public static MainWindow _MainWindow = new MainWindow();
        public static ArpScanWindow _ArpScanWindow = new ArpScanWindow();

        public static void RunGUI(JobSystem jobSys, DB dataBase, DHCPReader dhcp)
        {
            ArpScanWindow.InitGUI(jobSys, dataBase, _MainWindow);
            MainWindow.InitGUI(jobSys, dataBase, dhcp);
            Application.Run(_MainWindow);           
        }
    }
}
