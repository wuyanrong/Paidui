//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/13       创建

//第三方组件版本：log4net v1.2.11.0

using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository("log4net-tracemanager-repository")]
namespace Evt.Framework.Common
{
    /// <summary>
    /// 日志工具类。
    /// </summary>
    public class LogUtil
    {
        /// <summary>
        /// ILog
        /// </summary>
        private static ILog _log = LogManager.GetLogger(typeof(LogUtil));

        /// <summary>
        /// 记录调试级别的日志。
        /// </summary>
        /// <param name="message">日志信息。</param>
        public static void Debug(string message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(FormatMessage(message, "Debug", String.Empty));
            }
        }

        /// <summary>
        /// 记录消息级别的日志。
        /// </summary>
        /// <param name="message">日志信息。</param>
        public static void Info(string message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(FormatMessage(message, "Info", String.Empty));
            }
        }

        /// <summary>
        /// 记录警告级别的日志。
        /// </summary>
        /// <param name="message">日志信息。</param>
        public static void Warn(string message)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(FormatMessage(message, "Warn", String.Empty));
            }
        }

        /// <summary>
        /// 记录错误级别的日志。
        /// </summary>
        /// <param name="message">日志信息。</param>
        public static void Error(string message)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(FormatMessage(message, "Error", String.Empty));
            }
        }

        /// <summary>
        /// 记录错误级别的日志。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="ex">异常对象。</param>
        public static void Error(string message, Exception ex)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(FormatMessage(message, "Error", ex.StackTrace));
            }
        }

        /// <summary>
        /// 记录手机端应用错误级别的日志。
        /// </summary>
        /// <param name="app">应用标识</param>
        /// <param name="mobiletype">设备大类名</param>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志信息</param>
        public static void MobileError(string app, string mobiletype, string level, string message)
        {
            if (_log.IsErrorEnabled)
            {
                string msg = "{0}|{1}|{2}|{3}|{4}";
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                msg = String.Format(msg, app, mobiletype, level, datetime, message);
                _log.Error(msg);
            }
        }

        /// <summary>
        /// 格式化日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="level">日志级别。</param>
        /// <param name="stackTrace">堆栈信息。</param>
        /// <returns>格式化后的日志信息。</returns>
        private static string FormatMessage(string message, string level, string stackTrace)
        {
            if (ConfigUtil.TraceSimpleContent)
            {
                return FormatSimpleMessage(message, level);
            }
            else
            {
                return FormatDetailMessage("", message, level, stackTrace);
            }
        }

        /// <summary>
        /// 格式化精简日志信息<br />
        /// 信息格式为：系统名称|日志级别|发生时间|日志信息
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="level">日志级别。</param>
        /// <returns>格式化后的精简日志信息。</returns>
        private static string FormatSimpleMessage(string message, string level)
        {
            string msg = "{0}|{1}|{2}|{3}";
            //string systemName = "CRM";
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            return String.Format(msg, ConfigUtil.SystemName, level, datetime, message);
        }

        /// <summary>
        /// 格式化详细日志信息<br />
        /// 信息格式为：系统名称|日志级别|发生时间|日志信息|所在机器|客户端IP|Session信息|浏览器信息|请求信息|堆栈信息
        /// </summary>
        /// <param name="sessioninfo">
        /// 当前会话信息。
        /// 例如：CRM，sessionInfo = String.Format("SESSION[LoginName:{0};UserId:{1};RoleCode:{2}]", SessionUtil.Current.LoginName, SessionUtil.Current.UserId, SessionUtil.Current.Role);
        /// </param>
        /// <param name="message">日志信息。</param>
        /// <param name="level">日志级别。</param>
        /// <param name="stackTrace">堆栈信息。</param>
        /// <returns>格式化后的详细日志信息。</returns>
        private static string FormatDetailMessage(string sessioninfo, string message, string level, string stackTrace)
        {
            string msg = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}";
            //string systemName = "CRM";
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string machine = Environment.MachineName;
            string clientIP = String.Empty;
            string sessionInfo = String.Empty;
            string browserInfo = String.Empty;
            string requestInfo = String.Empty;

            
            if (HttpContext.Current != null)
            {
                try
                {
                    clientIP = HttpContext.Current.Request.UserHostAddress;

                    sessionInfo = sessioninfo;
                    //LoginInfo currentUser = SessionUtil.Current;
                    //if (currentUser != null)
                    //{
                    //    sessionInfo = String.Format("SESSION[LoginName:{0};UserId:{1};RoleCode:{2}]", currentUser.LoginName, currentUser.UserId, currentUser.Role);
                    //}

                    NameValueCollection headers = HttpContext.Current.Request.Headers;
                    if (headers != null && headers.Count > 0)
                    {
                        browserInfo = "BROWSER[";
                        foreach (string header in headers)
                        {
                            foreach (string headervalue in headers.GetValues(header))
                            {
                                browserInfo += String.Format("{0}:{1};", header, headervalue);
                            }
                        }
                        browserInfo += "]";
                    }

                    string httpMethod = HttpContext.Current.Request.HttpMethod.ToUpper();
                    if (httpMethod == "GET")
                    {
                        requestInfo = GetGetRequestInfo();
                    }
                    else if (httpMethod == "POST")
                    {
                        requestInfo = GetPostRequestInfo();
                    }
                }
                catch (Exception ex)
                {
                }
            }            

            return String.Format(msg, ConfigUtil.SystemName, level, datetime, message, machine, clientIP, sessionInfo, browserInfo, requestInfo, stackTrace);
        }

        /// <summary>
        /// GetGetRequestInfo
        /// </summary>
        /// <returns>string</returns>
        private static string GetGetRequestInfo()
        {
            NameValueCollection querystring = HttpContext.Current.Request.QueryString;

            string requestInfo = "GET[";
            try
            {
                foreach (string key in querystring.AllKeys)
                {
                    foreach (string value in querystring.GetValues(key))
                    {
                        requestInfo += String.Format("{0}:{1};", key, value);
                    }
                }
            }
            catch
            {

            }

            requestInfo += "]";

            return requestInfo;

        }

        /// <summary>
        /// GetPostRequestInfo
        /// </summary>
        /// <returns>string</returns>
        private static string GetPostRequestInfo()
        {
            string requestInfo = "POST[";
            try
            {
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();

                //读取请求输入流的内容
                System.IO.Stream requestStream = HttpContext.Current.Request.InputStream;
                while ((count = requestStream.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }

                requestInfo += builder.ToString();
            }
            catch
            {

            }
            requestInfo += "]";

            return requestInfo;
        }
    }
}
