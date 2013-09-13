using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Trace接口
    /// </summary>
    public interface ITrace
    {
        void TraceDebug(string message);
        void TraceInfo(string message);
        void TraceWarning(string message);
        void TraceError(string message);
    }
}
