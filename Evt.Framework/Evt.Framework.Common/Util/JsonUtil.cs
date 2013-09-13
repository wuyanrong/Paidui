using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Json对象工具类
    /// </summary>
    public static class JsonUtil
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public static string Serialize(object value)
        {
            return _serializer.Serialize(value);
        }

        public static string Serialize(string key, object value)
        {
            string json = "{\"" + key + "\":" + Serialize(value) + "}";
            return json;
        }

        public static T Deserialize<T>(string json)
        {
            return _serializer.Deserialize<T>(json);
        }

        public static string SerializerDataTable(DataTable value)
        {
            int rowCount = value.Rows.Count;
            int colCount = value.Columns.Count;
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            for (int i = 0; i < rowCount; ++i)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append("{");
                for (int j = 0; j < colCount; ++j)
                {
                    if (j > 0)
                    {
                        sb.Append(",");
                    }
                    string v = TransferSpeicalChar(ConvertUtil.ToString(value.Rows[i][j], ""));                    
                    sb.Append("\"" + value.Columns[j].ColumnName + "\":\"" + v + "\"");
                }
                sb.Append("}");
            }

            sb.Append("]");

            return sb.ToString();
        }

        public static string SerializerDataTable(string key, DataTable value)
        {
            string json = "{\"" + key + "\":" + SerializerDataTable(value) + "}";
            return json;
        }

        public static string SerializerDataTableToArray(DataTable value)
        {
            int colCount = value.Columns.Count;
            int rowCount = value.Rows.Count;
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            for (int i = 0; i < rowCount; ++i)
            {
                DataRow row = value.Rows[i];
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append("[");
                for (int j = 0; j < colCount; ++j)
                {
                    if (j > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append("\"");
                    string v = TransferSpeicalChar(ConvertUtil.ToString(value.Rows[i][j], ""));                    
                    sb.Append(v);
                    sb.Append("\"");

                }
                sb.Append("]");
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static string SerializerDataTableToArray(string key, DataTable value)
        {
            string json = "\"" + key + "\":" + SerializerDataTableToArray(value);
            return json;
        }

        public static string SerializeFormat(string key, string arg)
        {
            string json = "{\"" + key + "\":" + TransferSpeicalChar(arg) + "}";
            return json;
        }

        public static string SerializeFormat(string key, string arg, string fix)
        {
            string json = "{\"" + key + "\":" + fix + TransferSpeicalChar(arg) + fix + "}";
            return json;
        }

        public static string SerializeFormat(string key0, string arg0, string key1, string arg1)
        {
            string json = "{\"" + key0 + "\":" + TransferSpeicalChar(arg0) + ",\"" + key1 + "\":" + TransferSpeicalChar(arg1) + "}";
            return json;
        }

        public static string SerializeFormat(string key0, string arg0, string key1, string arg1, string fix)
        {
            string json = "{\"" + key0 + "\":" + fix + TransferSpeicalChar(arg0) + fix + ",\"" + key1 + "\":" + fix + TransferSpeicalChar(arg1) + fix + "}";
            return json;
        }

        public static string TransferSpeicalChar(string value)
        {
            return value.Replace(@"\", @"\\").Replace("\r", @"\r").Replace("\n", @"\n").Replace("\"", "\\\"");
        }
    }
}
