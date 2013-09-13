using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Trace工具类
    /// </summary>
    public static class TraceUtil
    {
        private static Dictionary<string, ITrace> _traces = new Dictionary<string, ITrace>();

        public static void RegisterTraces(string key, ITrace trace)
        {
            _traces[key] = trace;
        }

        public static ITrace GetTrace(string key)
        {
            return _traces[key];
        }

        public static void Remove(string key)
        {
            _traces.Remove(key);
        }

        public static void Clear()
        {
            _traces.Clear();
        }

        public static void TraceDebug(string message)
        {
            foreach (KeyValuePair<string, ITrace> item in _traces)
            {
                item.Value.TraceDebug(message);
            }
        }

        public static void TraceInfo(string message)
        {
            foreach (KeyValuePair<string, ITrace> item in _traces)
            {
                item.Value.TraceDebug(message);
            }
        }

        public static void TraceWarning(string message)
        {
            foreach (KeyValuePair<string, ITrace> item in _traces)
            {
                item.Value.TraceDebug(message);
            }
        }

        public static void TraceError(string message)
        {
            foreach (KeyValuePair<string, ITrace> item in _traces)
            {
                item.Value.TraceDebug(message);
            }
        }
    }
}
