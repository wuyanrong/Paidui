using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using System.Configuration;
using System.Threading;
using log4net;
using Newtonsoft.Json;
//[assembly: Evt.Framework.PerformanceMonitor.PerformanceMonitorAttribute(
//                       AttributeTargetTypes = "PDW.RMS.Services.OrderDishService")]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository("log4net-tracemanager-repository")]

namespace Evt.Framework.PerformanceMonitor
{
    [Serializable]
    public sealed class PerformanceMonitorAttribute : OnMethodBoundaryAspect
    {
        private static ILog _Log = null;
        private static int _MethodExcuteMaxDuration;
        private static bool _Enable;
        private static bool _EnableAgumentsLog;
        private static int _LogWriteInterval;
        private static Thread _LogWriteThread;
        private static Dictionary<int, string> _LogDictionary = new Dictionary<int, string>();
        private static object _SyncObject = new object();

        public static int ExcuteID;


        static PerformanceMonitorAttribute()
        {
            try
            {
                string methodExcuteMaxDuration = ConfigurationManager.AppSettings["MethodExcuteMaxDuration"];
                _MethodExcuteMaxDuration = string.IsNullOrEmpty(methodExcuteMaxDuration) ? 100 : Convert.ToInt32(methodExcuteMaxDuration);
            }
            catch
            {
                _MethodExcuteMaxDuration = 100;
            }

            try
            {
                int enable = Convert.ToInt32(ConfigurationManager.AppSettings["PerformanceMonitorEnable"]);

                _Enable = enable == 0 ? false : true;
            }
            catch
            {
                _Enable = false;
            }

            try
            {
                string performanceMonitorLogWriteInterval = ConfigurationManager.AppSettings["PerformanceMonitorLogWriteInterval"];
                _LogWriteInterval = string.IsNullOrEmpty(performanceMonitorLogWriteInterval) ? 3000 : Convert.ToInt32(performanceMonitorLogWriteInterval);
            }
            catch
            {
                _LogWriteInterval = 30000;
            }

            try
            {
                int enable = Convert.ToInt32(ConfigurationManager.AppSettings["ArgumentsLogEnable"]);

                _EnableAgumentsLog = enable == 0 ? false : true;
            }
            catch
            {
                _EnableAgumentsLog = false;
            }

            try
            {
                _Log = LogManager.GetLogger("PerformanceMonitor");
                StartWriteLog();
            }
            catch
            {
            }
        }

        private static void StartWriteLog()
        {
            _LogWriteThread = new Thread(new ThreadStart(WriteLog));
            _LogWriteThread.IsBackground = true;
            _LogWriteThread.Start();
        }
        private static void WriteLog()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(_LogWriteInterval);

                    StringBuilder sb = new StringBuilder();
                    lock (_SyncObject)
                    {
                        foreach (var log in _LogDictionary)
                        {
                            sb.AppendLine(log.Value);
                        }
                        _LogDictionary.Clear();
                    }
                    if (sb.Length > 0)
                        _Log.Info(sb.ToString());
                }
            }
            catch
            {
            }
        }
        

        public override void OnEntry(MethodExecutionArgs args)
        {
            try
            {
                if (!_Enable) return;

                ExcuteArgs excuteArgs = new ExcuteArgs();
                excuteArgs.ExcuteStartTime = DateTime.Now;
                excuteArgs.ExcuteID = Interlocked.Increment(ref ExcuteID);

                args.MethodExecutionTag = excuteArgs;

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Format("{0}.{1} Excute Start.", args.Method.DeclaringType, args.Method.Name));

                if (_EnableAgumentsLog)
                {
                    string argumentsStr = GetArgumentString(args);

                    if (!string.IsNullOrEmpty(argumentsStr))
                    {
                        sb.AppendLine("Arguments:");
                        sb.AppendLine(argumentsStr);
                    }
                }

                lock (_SyncObject)
                {
                    _LogDictionary.Add(excuteArgs.ExcuteID, sb.ToString());
                }
            }
            catch
            {
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            try
            {
                if (!_Enable) return;

                ExcuteArgs excuteArgs = args.MethodExecutionTag as ExcuteArgs;

                TimeSpan excuteDuration = DateTime.Now - excuteArgs.ExcuteStartTime;

                if (excuteDuration.TotalMilliseconds > _MethodExcuteMaxDuration)
                {
                    lock (_SyncObject)
                    {
                        _LogDictionary.Add(Interlocked.Increment(ref ExcuteID), string.Format("{0}.{1} Excute End. #Duration : {2}", args.Method.DeclaringType, args.Method.Name, excuteDuration.TotalMilliseconds));
                    }
                }
                else
                {
                    lock (_SyncObject)
                    {
                        _LogDictionary.Remove(excuteArgs.ExcuteID);
                    }
                }
            }
            catch
            {
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            try
            {
                _Log.Error(GetArgumentString(args), args.Exception);
            }
            catch
            {
            }
        }

        public string GetArgumentString(MethodExecutionArgs args)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var arg in args.Arguments)
            {
                try
                {
                    sb.AppendLine(JsonConvert.SerializeObject(arg));
                }
                catch
                {
                    continue;
                }
            }

            return sb.ToString();
        }
    }

    public class ExcuteArgs
    {
        public DateTime ExcuteStartTime
        {
            get;
            set;
        }

        public int ExcuteID
        {
            get;
            set;
        }
    }
}
