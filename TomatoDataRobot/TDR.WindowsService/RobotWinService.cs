using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using TDR.Common.Utils;
using TDR.Services;
using System.Configuration;

namespace TDR.WindowsService
{
    public partial class RobotWinService : ServiceBase
    {
        public RobotWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        protected override void OnStop()
        {
        }

        private void Run()
        {
            while (true)
            {
                ConfigurationManager.RefreshSection("appSettings");
                int runTime = ConfigUtil.RunTime;
                int currentHours = DateTime.Now.Hour;
                if (runTime == currentHours)
                {
                    RobotService.Instance.DoWork();
                }
                Thread.Sleep(1000 * 60);
            }
        }
    }
}
