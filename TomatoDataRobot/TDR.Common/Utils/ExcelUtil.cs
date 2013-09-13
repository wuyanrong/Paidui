using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Web;
using System.Data;
using System.IO;

namespace TDR.Common.Utils
{
    public class ExcelUtil
    {
        /// <summary>
        /// Excel连接字符串
        /// </summary>
        private static string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0";

        /// <summary>
        /// 创建工作薄
        /// </summary>
        /// <param name="workBookName">工作薄名称</param>
        /// <param name="workSheetName">默认的工作表名</param>
        /// <param name="cloums">默认工作表名的数据列</param>
        public static void CreateWorkBook(string workBookName, string workSheetName, Dictionary<string, OleDbType> cloums)
        {
            string columsTotal = GetColumsNameAndType(cloums);
            string sql = string.Format("CREATE TABLE [{0}] {1}", workSheetName, columsTotal);
            ExecuteNonQuery(workBookName, sql);
        }

        /// <summary>
        /// 判断工作薄是否存在
        /// </summary>
        /// <param name="workBookName"></param>
        /// <returns></returns>
        public static bool CheckWorkBookExist(string workBookName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Excel\";
            path = path + workBookName + ".xls";
            return File.Exists(path);
        }

        /// <summary>
        /// 添加工作表
        /// </summary>
        /// <param name="workBookName">工作簿名</param>
        /// <param name="workSheetName">工作表名</param>
        /// <param name="clomus">工作表列</param>
        public static void AddWorkSheet(string workBookName, string workSheetName, Dictionary<string, OleDbType> clomus)
        {
            if (!CheckExistWorkSheet(workBookName, workSheetName))
            {
                string columsTotal = GetColumsNameAndType(clomus);
                string sql = string.Format("CREATE TABLE [{0}] {1}", workSheetName, columsTotal);
                ExecuteNonQuery(workBookName, sql);
            }
        }

        /// <summary>
        /// 往工作表添加数据行
        /// </summary>
        /// <param name="workBookName"></param>
        /// <param name="workSheetName"></param>
        /// <param name="paramDic"></param>
        public static void AddWorkSheetRows(string workBookName, string workSheetName, Dictionary<string, string> paramDic)
        {
            ValidateWorkSheetName(ref workSheetName);

            StringBuilder sql = new StringBuilder();
            StringBuilder field = new StringBuilder(string.Empty);
            StringBuilder value = new StringBuilder(string.Empty);
            sql.Append("INSERT INTO [").Append(workSheetName).Append("$]({0}) VALUES ({1})");

            foreach (KeyValuePair<string, string> item in paramDic)
            {
                field.Append("[").Append(item.Key).Append("],");
                value.Append("'").Append(item.Value).Append("',");
            }
            ExecuteNonQuery(workBookName, string.Format(sql.ToString(), field.ToString().TrimEnd(','), value.ToString().TrimEnd(',')));
        }

        #region Private
        private static string GetColumsNameAndType(Dictionary<string, OleDbType> cloums)
        {
            string columsTotal = string.Empty;
            foreach (var item in cloums)
            {
                columsTotal += string.Format("[{0}] {1}", item.Key, item.Value) + ",";
            }
            columsTotal = "(" + columsTotal.Remove(columsTotal.Length - 1) + ")";
            return columsTotal;
        }

        private static bool CheckExistWorkSheet(string workBookName, string workSheetName)
        {
            bool result = false;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Excel\" + workBookName + ".xls"; ;
            conStr = string.Format(conStr, path);
            ValidateWorkSheetName(ref workSheetName);
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                con.Open();
                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow item in dt.Rows)
                {
                    if (item["TABLE_NAME"] != null && item["TABLE_NAME"].ToString().Equals(workSheetName, StringComparison.OrdinalIgnoreCase))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        private static void ExecuteNonQuery(string workBookName, string sql)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Excel\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + workBookName + ".xls";
            string conTxt = string.Format(conStr, path);
            using (OleDbConnection con = new OleDbConnection(conTxt))
            {
                OleDbCommand cmd = new OleDbCommand(sql, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 验证工作表名是否符合写入工作簿规则，对于不符合的名称，将修改
        /// </summary>
        /// <param name="workSheetName"></param>
        private static void ValidateWorkSheetName(ref string workSheetName)
        {
            if (workSheetName.Contains("("))
            {
                workSheetName = workSheetName.Replace("(", "_").Replace(")", "_");
            }
            if (workSheetName.Contains(" "))
            {
                workSheetName = workSheetName.Replace(" ", "_");
            }
            if (workSheetName.Contains("&"))
            {
                workSheetName = workSheetName.Replace("&","_");
            }
        }
        #endregion
    }
}
