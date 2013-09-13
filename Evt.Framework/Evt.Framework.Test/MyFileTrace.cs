using System;
using System.Collections.Generic;
using System.Text;

using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace Evt.Framework.Test
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class MyFileTrace : FileTrace
    {
        protected override string FileName
        {
            get
            {
                return @"c:\trace.txt";
            }
        }

        protected override TraceLevelEnum TraceLevel
        {
            get
            {
                return TraceLevelEnum.Debug;
            }
        }
    }
}
