using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDR.Common;
using TDR.Common.Utils;
using System.Data.OleDb;

namespace TDR.Common.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            string ss = HttpUtil.Get("http://www.baidu.com");
            Dictionary<string, OleDbType> dic = new Dictionary<string, OleDbType>  
            {
                {"a",OleDbType.VarChar},
                {"b",OleDbType.VarChar},
                {"c",OleDbType.VarChar}
            };
            
            // ExcelUtil.CreateWorkBook("dddd", "ssss", dic);
            Dictionary<string, string> dic2 = new Dictionary<string, string>()
            {
                {"a","1"},
                {"b","2"},
                {"c","3"}
            };
            ExcelUtil.AddWorkSheetRows("dddd", "ssss", dic2);
        }
    }
}
