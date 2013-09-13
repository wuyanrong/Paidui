//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/15       创建

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 数据类型转换工具类
    /// </summary>
    public class ConvertUtil
    {
        #region ToBool

        /// <summary>
        /// 将指定值解析成bool类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>bool值，失败返回false</returns>
        public static bool ToBool(object o)
        {
            return ToBool(o, false);
        }

        /// <summary>
        /// 将指定值解析成bool类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns>bool值</returns>
        public static bool ToBool(object o, bool defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToBoolean(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToInt

        /// <summary>
        /// 将指定值解析成int类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>int值，失败返回常量int.MinValue</returns>
        public static int ToInt(object o)
        {
            return ToInt(o, int.MinValue);
        }

        /// <summary>
        /// 将指定值解析成int类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns>int值</returns>
        public static int ToInt(object o, int defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToTinyInt

        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <returns>返回byte，失败返回0</returns>
        public static byte ToTinyInt(object value)
        {
            return ToTinyInt(value, byte.MinValue);
        }

        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defValue">失败后默认值</param>
        /// <returns>byte类型值</returns>
        public static byte ToTinyInt(object value, byte defValue)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return defValue;
            }

            byte def = 0;
            byte.TryParse(value.ToString(), out def);

            return def == 0 ? defValue : def;
        }

        #endregion

        #region ToSmallInt

        /// <summary>
        /// 转换成short
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <returns>返回short，失败返回0</returns>
        public static short ToSmallInt(object value)
        {
            return ToSmallInt(value, 0);
        }

        /// <summary>
        /// 转换成short
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defValue">失败后默认值</param>
        /// <returns>short类型值</returns>
        public static short ToSmallInt(object value, short defValue)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return defValue;
            }

            short def = 0;
            short.TryParse(value.ToString(), out def);

            return def == 0 ? defValue : def;
        }

        #endregion

        #region ToFloat

        /// <summary>
        /// 转换成float
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <returns>返回float，失败返回常量float.MinValue</returns>
        public static float ToFloat(object value)
        {
            return ToFloat(value, float.MinValue);
        }

        /// <summary>
        /// 转换成float
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defValue">失败后默认值</param>
        /// <returns>float类型值</returns>
        public static float ToFloat(object value, float defValue)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return defValue;
            }

            float def = 0;
            float.TryParse(value.ToString(), out def);

            return def == 0 ? defValue : def;
        }

        #endregion

        #region ToBigInt

        /// <summary>
        /// 转换成Int64
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <returns>返回Int64，失败返回常量Int64.MinValue</returns>
        public static Int64 ToBigInt(object value)
        {
            return ToBigInt(value, Int64.MinValue);
        }

        /// <summary>
        /// 转换成Int64
        /// </summary>
        /// <param name="value">待转换对象</param>
        /// <param name="defValue">失败后默认值</param>
        /// <returns>Int64类型值</returns>
        public static Int64 ToBigInt(object value, Int64 defValue)
        {
            if (value == null || Convert.IsDBNull(value))
            {
                return defValue;
            }

            Int64 def = 0;
            Int64.TryParse(value.ToString(), out def);

            return def == 0 ? defValue : def;
        }

        #endregion

        #region ToDecimal

        /// <summary>
        /// 将指定值解析成decimal类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>decimal值，失败返回常量Decimal.MinValue</returns>
        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, Decimal.MinValue);
        }

        /// <summary>
        /// 将指定值解析成decimal类型，解析失败时返回传入的默认值
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">解析失败的值</param>
        /// <returns>decimal值</returns>
        public static decimal ToDecimal(object o, decimal defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDecimal(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToDouble

        /// <summary>
        /// 将指定值解析成double类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>double值，失败返回常量Double.MinValue</returns>
        public static double ToDouble(object o)
        {
            return ToDouble(o, Double.MinValue);
        }

        /// <summary>
        /// 将指定值解析成double类型，解析失败时返回传入的默认值
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">解析失败的值</param>
        /// <returns>double值</returns>
        public static double ToDouble(object o, double defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToDateTime

        /// <summary>
        /// 将指定值解析成DateTime类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>DateTime值，失败返回常量DateTime.MinValue</returns>
        public static DateTime ToDateTime(object o)
        {
            return ToDateTime(o, DateTime.MinValue);
        }

        /// <summary>
        /// 将指定值解析成DateTime类型，解析失败时返回传入的默认值
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">解析失败的值</param>
        /// <returns>DateTime值</returns>
        public static DateTime ToDateTime(object o, DateTime defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDateTime(o);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// 将指定值解析成String类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>String值，失败返回常量String.Empty</returns>
        public static string ToString(object o)
        {
            return ToString(o, String.Empty);
        }

        /// <summary>
        /// 将指定值解析成String类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns>String值</returns>
        public static string ToString(object o, string defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                return o.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToGuid

        /// <summary>
        /// 将指定值解析成Guid类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>Guid值</returns>
        public static Guid ToGuid(object o)
        {
            if (o is Guid)
            {
                return (Guid)o;
            }
            else
            {
                return new Guid(o.ToString());
            }
        }

        /// <summary>
        /// 将指定值解析成Guid类型
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns>Guid值</returns>
        public static Guid ToGuid(object o, Guid defaultValue)
        {
            if (o == null || o == DBNull.Value)
            {
                return defaultValue;
            }

            try
            {
                if (o is Guid)
                {
                    return (Guid)o;
                }
                else
                {
                    return new Guid(o.ToString());
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region ToMoneyString

        /// <summary>
        /// 将指定值显示为金额方式
        /// </summary>
        /// <param name="o">指定值</param>
        /// <returns>失败返回0.00</returns>
        public static string ToMoneyString(object o)
        {
            return ToMoneyString(o, "0.00");
        }

        /// <summary>
        /// 将指定值显示为金额方式
        /// </summary>
        /// <param name="o">指定值</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string ToMoneyString(object o, string format)
        {
            if (o == null || o == DBNull.Value)
            {
                return string.Empty;
            }

            if (o is decimal)
            {
                decimal money = (decimal)o;
                return money.ToString(format);
            }
            else if (o is float)
            {
                float money = (float)o;
                return money.ToString(format);
            }
            else if (o is double)
            {
                double money = (double)o;
                return money.ToString(format);
            }
            else if (o is int)
            {
                int money = (int)o;
                return money.ToString(format);
            }
            else if (o is long)
            {
                long money = (long)o;
                return money.ToString(format);
            }
            else
            {
                return o.ToString();
            }
        }

        #endregion

        #region ToColor

        /// <summary>
        /// 转换成颜色
        /// </summary>
        /// <param name="color">颜色值，如#fffffff</param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            color = color.Replace("#", string.Empty);

            byte a = System.Convert.ToByte("ff", 16);

            byte pos = 0;

            if (color.Length == 8)
            {
                a = System.Convert.ToByte(color.Substring(pos, 2), 16);
                pos = 2;
            }

            byte r = System.Convert.ToByte(color.Substring(pos, 2), 16);

            pos += 2;

            byte g = System.Convert.ToByte(color.Substring(pos, 2), 16);

            pos += 2;

            byte b = System.Convert.ToByte(color.Substring(pos, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }

        #endregion

        #region 数字与大写相互转换

        /// <summary>
        /// 转换数字金额主函数（包括小数）
        /// </summary>
        /// <param name="num">数字num</param>
        /// <returns>转换成中文大写后的字符串或者出错信息提示字符串</returns>
        public static string ToChineseNumber(decimal num)
        {
            string str = num.ToString();
            char[] ch = new char[1];
            ch[0] = '.';   //小数点
            string[] splitstr = null;   //定义按小数点分割后的字符串数组
            splitstr = str.Split(ch[0]);//按小数点分割字符串
            if (splitstr.Length == 1)   //只有整数部分
                return ConvertData(str) + "圆整";
            else   //有小数部分
            {
                string rstr;
                rstr = ConvertData(splitstr[0]) + "圆";//转换整数部分
                rstr += ConvertXiaoShu(splitstr[1]) + "整";//转换小数部分
                return rstr;
            }
        }

        ///   <summary>
        ///   判断是否是正数字字符串
        ///   </summary>
        ///   <param   name="str">   判断字符串</param>
        ///   <returns>如果是数字，返回true，否则返回false</returns>
        private static bool IsPositveDecimal(string str)
        {
            Decimal d;
            try
            {
                d = Decimal.Parse(str);

            }
            catch (Exception)
            {
                return false;
            }
            if (d > 0)
                return true;
            else
                return false;
        }

        ///   <summary>
        ///   转换数字（整数）
        ///   </summary>
        ///   <param   name="str">需要转换的整数数字字符串</param>
        ///   <returns>转换成中文大写后的字符串</returns>
        private static string ConvertData(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;
            if (strlen <= 4)//数字长度小于四位
            {
                rstr = ConvertDigit(str);
            }
            else
            {
                if (strlen <= 8)//数字长度大于四位，小于八位
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字
                    tmpstr = str.Substring(0, strlen - 4);//截取其余数字
                    //将两次转换的数字加上万后相连接
                    rstr = String.Concat(ConvertDigit(tmpstr) + "万", rstr);
                    rstr = rstr.Replace("零万", "万");
                    rstr = rstr.Replace("零零", "零");

                }
                else if (strlen <= 12)//数字长度大于八位，小于十二位
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字
                    tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字
                    rstr = String.Concat(ConvertDigit(tmpstr) + "万", rstr);
                    tmpstr = str.Substring(0, strlen - 8);
                    rstr = String.Concat(ConvertDigit(tmpstr) + "億", rstr);
                    rstr = rstr.Replace("零億", "億");
                    rstr = rstr.Replace("零万", "零");
                    rstr = rstr.Replace("零零", "零");
                    rstr = rstr.Replace("零零", "零");
                }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                    case "万零": rstr = rstr.Substring(0, strlen - 2) + "万"; break;
                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }
            }

            return rstr;
        }

        ///   <summary>
        ///   转换数字（小数部分）
        ///   </summary>
        ///   <param   name="str">需要转换的小数部分数字字符串</param>
        ///   <returns>转换成中文大写后的字符串</returns>
        private static string ConvertXiaoShu(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = ConvertChinese(str) + "角";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = ConvertChinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += ConvertChinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "");
                rstr = rstr.Replace("零角", "");
                return rstr;
            }
        }

        /// <summary>
        /// 转换数字
        /// </summary>
        /// <param   name="str">转换的字符串（四位以内）</param>
        /// <returns></returns>
        private static string ConvertDigit(string str)
        {
            int strlen = str.Length;
            string rstr = "";
            switch (strlen)
            {
                case 1: rstr = ConvertChinese(str); break;
                case 2: rstr = Convert2Digit(str); break;
                case 3: rstr = Convert3Digit(str); break;
                case 4: rstr = Convert4Digit(str); break;
            }
            rstr = rstr.Replace("拾零", "拾");
            strlen = rstr.Length;

            return rstr;
        }


        /// <summary>
        /// 转换四位数字
        /// </summary>
        private static string Convert4Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "仟";
            rstring += ConvertChinese(str2) + "佰";
            rstring += ConvertChinese(str3) + "拾";
            rstring += ConvertChinese(str4);
            rstring = rstring.Replace("零仟", "零");
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }

        /// <summary>
        /// 转换三位数字
        /// </summary>
        private static string Convert3Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "佰";
            rstring += ConvertChinese(str2) + "拾";
            rstring += ConvertChinese(str3);
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }

        /// <summary>
        /// 转换二位数字
        /// </summary>
        private static string Convert2Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "拾";
            rstring += ConvertChinese(str2);
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }

        /// <summary>
        /// 将一位数字转换成中文大写数字
        /// </summary>
        private static string ConvertChinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟万億圆整角分"
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "壹"; break;
                case "2": cstr = "贰"; break;
                case "3": cstr = "叁"; break;
                case "4": cstr = "肆"; break;
                case "5": cstr = "伍"; break;
                case "6": cstr = "陆"; break;
                case "7": cstr = "柒"; break;
                case "8": cstr = "捌"; break;
                case "9": cstr = "玖"; break;
            }
            return (cstr);
        }

        #endregion

        #region DataTable、DataSet转换为可序列化的List、Dictionary

        /// <summary>
        /// 将DataTable转换为List，以便使用JavaScriptSerializer将其序列化成Json。
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <returns>DataTable的List表现形式。</returns>
        public static List<Dictionary<string, object>> ConvertDataTableToList(DataTable dataTable)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        row.Add(dataColumn.ColumnName, dataRow[dataColumn]);
                    }
                    rows.Add(row);
                }
            }

            return rows;
        }

        /// <summary>
        /// 将DataSet转换为Dictionary，以便使用JavaScriptSerializer将其序列化成Json。
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>DataSet的Dictionary表现形式。</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> ConvertDataSetToDictionary(DataSet dataSet)
        {
            Dictionary<string, List<Dictionary<string, object>>> tables = new Dictionary<string, List<Dictionary<string, object>>>();
            if (dataSet != null)
            {
                foreach (DataTable dataTable in dataSet.Tables)
                {
                    List<Dictionary<string, object>> rows = ConvertDataTableToList(dataTable);
                    tables.Add(dataTable.TableName, rows);
                }
            }

            return tables;
        }

        #endregion
    }
}
