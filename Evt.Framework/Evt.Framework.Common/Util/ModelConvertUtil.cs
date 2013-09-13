//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/20       创建

using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Model实体转换工具类
    /// </summary>
    public class ModelConvertUtil//<T> where T : new()
    {
        /// <summary>
        /// 将DataTable转换为List〈T〉
        /// </summary>
        /// <example>
        /// <code>
        /// // 获得商家列表
        /// public DataTable GetList()
        /// {
        ///     string tradeID = HttpContext.Current.Request["tradeID"];
        ///     string districtID = HttpContext.Current.Request["districtID"];
        /// 
        ///     string sql = @" SELECT DISTINCT m.merchant_id,m.merchant_name,m.status_code
        ///                     FROM    merchant AS m INNER JOIN 
        ///                             merchant_bind AS mb ON m.merchant_id = mb.merchant_id
        ///                     WHERE 1 = 1 ";
        ///     ParameterCollection pc = new ParameterCollection();
        ///     if (!String.IsNullOrEmpty(tradeID))
        ///     {
        ///         sql += " AND m.trade_id = $trade_id$ ";
        ///         pc.Add("trade_id", tradeID);
        ///     }
        ///     if (!String.IsNullOrEmpty(districtID))
        ///     {
        ///         sql += " AND mb.district_id = $district_id$ AND mb.status_code = 1 ";
        ///         pc.Add("district_id", districtID);
        ///     }
        ///     sql += " ORDER BY m.merchant_name ASC ";
        /// 
        ///     DataTable dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql, pc);
        ///     IList〈MerchantDataModel〉 list = ModelConvertUtil.ConvertToModel〈MerchantDataModel〉(dt);
        /// 
        ///     return list;
        /// }
        /// </code>
        /// </example>
        /// <param name="dt">待转换的DataTable对象</param>
        /// <returns>List〈T〉表现形式</returns>
        public static IList<T> DataTableToList<T>(DataTable dt)
        {
            IList<T> ts = new List<T>();
            Type modelType = typeof(T);
            string tempName = String.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                T t = Activator.CreateInstance<T>();

                PropertyInfo[] propertys = modelType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    TableMappingAttribute attr = (TableMappingAttribute)Attribute.GetCustomAttribute(pi, typeof(TableMappingAttribute));

                    if (attr != null && dt.Columns.Contains(attr.FieldName))
                    {
                        LogUtil.Error("ModelConvertUtil——Property：" + attr.FieldName);
                        if (pi.CanWrite)
                        {
                            object value = dr[attr.FieldName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                            else
                                LogUtil.Error("ModelConvertUtil——DBNull");
                        }
                        else
                            LogUtil.Error("ModelConvertUtil——NoCanWrite");
                    }
                    else
                        LogUtil.Error("ModelConvertUtil——NoContain");
                }

                ts.Add(t);
            }

            return ts;
        }

        /// <summary>
        /// 将DataReader转换为泛型T
        /// </summary>
        /// <typeparam name="T">具体的ViewModel类型</typeparam>
        /// <param name="dr"></param>
        /// <returns>泛型T</returns>
        public static T ReaderToModel<T>(IDataReader dr)
        {
            using (dr)
            {
                if (dr.Read())
                {
                    Type modelType = typeof(T);
                    int count = dr.FieldCount;
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < count; i++)
                    {
                        if (!IsNullOrDBNull(dr[i]))
                        {
                            PropertyInfo pi = modelType.GetProperty(GetPropertyName(dr.GetName(i)), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi != null)
                            {
                                pi.SetValue(model, HackType(dr[i], pi.PropertyType), null);
                            }
                        }
                    }
                    return model;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 将DataReader转换为List<T>
        /// </summary>
        /// <typeparam name="T">具体的ViewModel类型</typeparam>
        /// <param name="dr"></param>
        /// <returns>List<T>表现形式</returns>
        public static IList<T> ReaderToList<T>(IDataReader dr)
        {
            using (dr)
            {
                List<T> list = new List<T>();
                Type modelType = typeof(T);
                int count = dr.FieldCount;
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();

                    for (int i = 0; i < count; i++)
                    {
                        if (!IsNullOrDBNull(dr[i]))
                        {
                            PropertyInfo pi = modelType.GetProperty(GetPropertyName(dr.GetName(i)), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi != null)
                            {
                                pi.SetValue(model, HackType(dr[i], pi.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }

        //这个类对可空类型进行判断转换，要不然会报错   
        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        private static bool IsNullOrDBNull(object obj)
        {
            return (obj == null || (obj is DBNull)) ? true : false;
        }

        //取得DB的列对应bean的属性名
        private static string GetPropertyName(string column)
        {
            column = column.ToLower();
            string[] narr = column.Split('_');
            column = "";

            for (int i = 0; i < narr.Length; i++)
            {
                if (narr[i].Length > 1)
                {
                    column += narr[i].Substring(0, 1).ToUpper() + narr[i].Substring(1);
                }
                else
                {
                    column += narr[i].Substring(0, 1).ToUpper();
                }
            }

            return column;
        }
    }
}
