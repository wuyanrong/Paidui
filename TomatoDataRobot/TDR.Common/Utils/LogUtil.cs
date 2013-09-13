using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TDR.Common.Utils
{
    public class LogUtil
    {
        private static LogUtil _instance = new LogUtil();

        public static LogUtil Instance
        {
            get { return _instance; }
        }

        public void Error(string logName, Exception ex)
        {
            StringBuilder content = new StringBuilder();
            content.Append(Environment.NewLine + Environment.NewLine + "================================" + logName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")================================");
            content.Append(Environment.NewLine + "     Message:" + ex.Message);
            content.Append(Environment.NewLine + "     InnerException:" + ex.InnerException);
            content.Append(Environment.NewLine + "     StackTrace:" + ex.StackTrace);
            content.Append(Environment.NewLine + "======================================================================================");
            WriteLog("Log\\Error\\", logName, content.ToString());
        }

        public void Info(string logName, string msg)
        {
            StringBuilder content = new StringBuilder();
            string time = "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")";
            content.Append(Environment.NewLine + logName + time + "：" + msg); //test(2013-09-11)：this is a test msg
            WriteLog("Log\\Info\\", logName, content.ToString());
        }

        private void WriteLog(string path, string logName, string content)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + path;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string fullPath = filePath + fileName;
            if (!File.Exists(fullPath))
            {
                var file = File.Create(fullPath);
                file.Dispose();
            }
            File.AppendAllText(fullPath, content, Encoding.UTF8);
        }
    }
}
