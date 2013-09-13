using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 文件Trace
    /// </summary>
    /// <remarks>
    /// Default file name: DateTime.Now.ToShortDateString()
    /// Default Level: Error
    /// </remarks>
    public class FileTrace : ITrace
    {
        #region Variables

        private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        private static readonly object _lock = new object();
        private Queue<string> _messages = new Queue<string>();
        private bool _enabled = true;

        /// <summary>
        /// 
        /// </summary>
        public FileTrace()
        {
            Thread t = new Thread(new ThreadStart(TraceThread));
            t.Start();
        }

        #endregion

        #region Virtual

        protected virtual string FileName
        {
            get { return DateTime.Now.ToShortDateString(); }
        }

        protected virtual TraceLevelEnum TraceLevel
        {
            get { return TraceLevelEnum.Error; }
        }

        protected virtual bool Enabled
        {
            get { return _enabled; }
        }

        #endregion

        #region 异步写入

        // 将信息入列
        private void Enqueue(string message)
        {
            lock (_lock)
            {
                _messages.Enqueue(message);
            }
        }

        // 通过线程写
        private void TraceThread()
        {
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                if (_messages.Count > 0)
                {
                    lock (_lock)
                    {
                        while (_messages.Count > 0)
                        {
                            sb.Append(_messages.Dequeue());
                        }
                        Trace(sb.ToString(), FileName);
                        sb.Remove(0, sb.Length);
                    }
                }

                Thread.Sleep(1500);
            }
        }

        // 写入指定的文件
        private void Trace(string message, string fileName)
        {
            FileStream fs = null;

            try
            {
                byte[] fileContent = Encoding.UTF8.GetBytes(message);
                fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 2048, true);

                AutoResetEvent manualEvent = new AutoResetEvent(false);
                IAsyncResult asyncResult = fs.BeginWrite(fileContent, 0, fileContent.Length,
                                                        new AsyncCallback(EndWriteCallback),
                                                        new WriteState(fs, manualEvent));
            }
            catch
            {
                //throw ex; (Exception ex)
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        // 异步写
        private void EndWriteCallback(IAsyncResult asyncResult)
        {
            WriteState stateInfo = (WriteState)asyncResult.AsyncState;
            try
            {
                stateInfo.fStream.EndWrite(asyncResult);
            }
            catch
            {
                //throw ex;Exception ex
            }
            finally
            {
                stateInfo.autoEvent.Set();
            }
        }

        /// <summary>
        /// 异步请求区别类
        /// </summary>
        internal sealed class WriteState
        {
            /// <summary>
            /// 文件流
            /// </summary>
            public FileStream fStream;

            /// <summary>
            /// 事件
            /// </summary>
            public AutoResetEvent autoEvent;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="fStream">文件流</param>
            /// <param name="autoEvent">事件</param>
            public WriteState(FileStream fStream, AutoResetEvent autoEvent)
            {
                this.fStream = fStream;
                this.autoEvent = autoEvent;
            }
        }

        #endregion

        #region ITrace 成员

        public void TraceDebug(string message)
        {
            if (!_enabled)
            {
                return;
            }

            int level = (int)TraceLevel;

            if (level != 0 && level < 2)
            {
                message = "\r\n[Debug on " + Environment.MachineName + " at " + DateTime.Now.ToString(DATETIME_FORMAT) + "]: " + message + "\r\n";
                Enqueue(message);
            }
        }

        public void TraceInfo(string message)
        {
            if (!_enabled)
            {
                return;
            }

            int level = (int)TraceLevel;

            if (level != 0 && level < 3)
            {
                message = "\r\n[Info on " + Environment.MachineName + " at " + DateTime.Now.ToString(DATETIME_FORMAT) + "]: " + message + "\r\n";
                Enqueue(message);
            }
        }

        public void TraceWarning(string message)
        {
            if (!_enabled)
            {
                return;
            }

            int level = (int)TraceLevel;

            if (level != 0 && level < 5)
            {
                message = "\r\n[Warning on " + Environment.MachineName + " at " + DateTime.Now.ToString(DATETIME_FORMAT) + "]: " + message + "\r\n";
                Enqueue(message);
            }
        }

        public void TraceError(string message)
        {
            if (!_enabled)
            {
                return;
            }

            int level = (int)TraceLevel;

            if (level != 0 && level < 9)
            {
                message = "\r\n[Error on " + Environment.MachineName + " at " + DateTime.Now.ToString(DATETIME_FORMAT) + "]: " + message + "\r\n";
                Enqueue(message);
            }
        }

        #endregion
    }
}
