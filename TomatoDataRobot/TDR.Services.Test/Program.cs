using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TDR.Common.Utils;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Configuration;

namespace TDR.Services.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("正在抓取数据....");
                RobotService.Instance.DoWork();
                Console.Write("数据抓取结束");
            }
            catch (Exception ex)
            {
                Console.Write("程序出错了！消息：" + ex.Message);
                LogUtil.Instance.Error("test", ex);
            }
        }
    }
}
