using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace NodeLiveService
{
    public partial class NLiveService : ServiceBase
    {
        private Timer timer1 = null;
        private static string apppath = ConfigurationManager.AppSettings["apppath"].ToString();
        private static Int32 port = Convert.ToInt32(ConfigurationManager.AppSettings["port"].ToString());
        private static Int32 interval = Convert.ToInt32(ConfigurationManager.AppSettings["interval"].ToString());

        public NLiveService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = interval;
            this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            UtilityLibrary.WriteErrorLog("Service Started");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            GetServerRun();
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            UtilityLibrary.WriteErrorLog("Service Stopped");
        }

        public void GetServerRun()
        {
            try
            {
                bool isPortRunning = false;
                string exCommand = "npm start";

                isPortRunning = UtilityLibrary.PortInUse(port);
                UtilityLibrary.WriteErrorLog("Port: " + port.ToString() + " Status: " + (isPortRunning ? "Running" : "Not Running"));

                if (!isPortRunning)
                {
                    ProcessStartInfo proInf = new ProcessStartInfo();
                    proInf.FileName = "cmd.exe";
                    proInf.WorkingDirectory = @"" + apppath + "";
                    proInf.UseShellExecute = true;
                    proInf.Arguments = "/c " + exCommand;

                    Process proStart = new Process();
                    proStart.StartInfo = proInf;
                    proStart.Start();
                    UtilityLibrary.WriteErrorLog("Server listening on Port: " + port.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                UtilityLibrary.WriteErrorLog("Error : " + ex.ToString());
            }
        }
    }
}
