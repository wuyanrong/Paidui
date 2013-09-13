using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace TDR.WindowsService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            Thread.Sleep(1000*10);
            ServicesToRun = new ServiceBase[] 
			{ 
				new RobotWinService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
