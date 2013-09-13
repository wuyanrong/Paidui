using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    /// <summary>
    /// Sql语句生成类
    /// </summary>
    public class SqlBuilder
    {
        #region 生成主键条件

        /// <summary>
        /// 根据实体主键获取主键条件的Sql语句
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">访问接口</param>
        /// <returns>主键条件的Sql语句</returns>
        public static void BuildPK(Model m, AbstractData d, ref string sql, ref ParameterCollection pc)
        {
            sql = " WHERE 1=1";

            object val = null;
            TableMappingAttribute attr = null;

            PropertyInfo[] props = m.GetCachedPropertyInfo();

            foreach (PropertyInfo prop in props)
            {
                attr = m.GetCachedTableMappingAttribute(prop.Name);
                if (attr == null)
                {
                    continue;
                }

                if (attr.PrimaryKey)
                {
                    val = m.GetValue(prop.Name);
                    if (val == null)
                    {
                        throw new DataAccessException("The primary key cannot be NULL.");
                    }
                    else
                    {
                        sql += " AND " + attr.FieldName + "=" + d.DatabaseParameterPrefix + "PK" + prop.Name;
                        pc.Add("PK" + prop.Name, val, ParameterDirectionEnum.Input);
                    }
                }
            }
        }

        #endregion

        #region Select语句相关方法

        /// <summary>
        /// 获取实体Select语句
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">数据访问接口</param>
        /// <returns>Select语句</returns>
        public static void BuildSelect(Model m, AbstractData d, ref string sql)
        {
            //string fieldName = string.Empty;
            //PropertyInfo[] props = m.GetCachedPropertyInfo();

            //for (int i = 0; i < props.Length; i++)
            //{
            //    fieldName += "," + m.GetFieldName(props[i].Name);
            //}

            //if (fieldName.Length > 0)
            //{
            //    fieldName = fieldName.Substring(1);
            //}
            //else
            //{
            //    fieldName = "*";
            //}

            // 生成
            //string sqlCommand = "SELECT " + fieldName + " FROM " + m.GetTableName();
            
            sql = "SELECT * FROM " + m.GetCachedTableMappingAttribute("").TableName;
        }

        #endregion

        #region Insert语句相关方法



        /// <summary>
        /// 获取Insert语句
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">数据访问对象</param>
        /// <returns>Insert语句</returns>
        public static void BuildInsert(Model m, AbstractData d, ref string sql, ref ParameterCollection pc)
        {
            string fieldName = "";
            string fieldValue = "";
            object val = null;
            TableMappingAttribute attr = null;
            
            PropertyInfo[] props = m.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                attr = m.GetCachedTableMappingAttribute(props[i].Name);
                if (attr == null)
                {
                    continue;
                }

                if (!attr.Insert)
                {
                    continue;
                }

                if (attr.DefaultValue != null)
                {
                    fieldName += "," + attr.FieldName;
                    fieldValue += "," + attr.DefaultValue;
                }
                else
                {
                    val = m.GetValue(props[i].Name);

                    if (val != null)
                    {
                        fieldName += "," + attr.FieldName;
                        fieldValue += "," + d.DatabaseParameterPrefix + attr.FieldName;
                        pc.Add(attr.FieldName, val);
                    }
                }
            }

            if (fieldName.Length > 0)
            {
                fieldName = fieldName.Substring(1);
                fieldValue = fieldValue.Substring(1);
            }

            sql = "INSERT INTO " + m.GetCachedTableMappingAttribute("").TableName + " (" + fieldName + ") VALUES (" + fieldValue + ")";
        }

        /// <summary>
        /// 获取Insert语句，针对Mysql数据库
        /// </summary>
        /// <param name="m">实体</param> 
        /// <returns>Insert语句</returns>
        public static void BuildMysqlInsert(Model m,ref string sql, ref ParameterCollection pc)
        {
            string fieldName = "";
            string fieldValue = "";
            object val = null;
            TableMappingAttribute attr = null;

            PropertyInfo[] props = m.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                attr = m.GetCachedTableMappingAttribute(props[i].Name);
                if (attr == null)
                {
                    continue;
                }

                if (!attr.Insert)
                {
                    continue;
                }

                if (attr.DefaultValue != null)
                {
                    fieldName += "," + attr.FieldName;
                    fieldValue += "," + attr.DefaultValue;
                }
                else
                {
                    val = m.GetValue(props[i].Name);

                    if (val != null)
                    {
                        fieldName += "," + attr.FieldName;
                        fieldValue += ",?"+ attr.FieldName;
                        pc.Add(attr.FieldName, val);
                    }
                }
            }

            if (fieldName.Length > 0)
            {
                fieldName = fieldName.Substring(1);
                fieldValue = fieldValue.Substring(1);
            }

            sql = "INSERT INTO " + m.GetCachedTableMappingAttribute("").TableName + " (" + fieldName + ") VALUES (" + fieldValue + ")";
        }

        #endregion

        #region Update语句相关方法
        /// <summary>
        /// 获取实体Update语句
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">访问接口</param>
        /// <returns>Update语句</returns>
        public static void BuildUpdate(Model m, AbstractData d, ref string sql, ref ParameterCollection pc)
        {
            string fieldName = "";
            object val = null;
            TableMappingAttribute attr = null;

            PropertyInfo[] props = m.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                attr = m.GetCachedTableMappingAttribute(props[i].Name);
                if (attr == null)
                {
                    continue;
                }

                if (!attr.Update)
                {
                    continue;
                }

                if (!attr.PrimaryKey)
                {
                    if (attr.DefaultValue != null)
                    {
                        fieldName += "," + attr.FieldName + "=" + attr.DefaultValue;
                    }
                    else
                    {
                        val = m.GetValue(props[i].Name);

                        if (val != null)
                        {
                            fieldName += "," + attr.FieldName + "=" + d.DatabaseParameterPrefix + attr.FieldName;
                            pc.Add(attr.FieldName, m.GetValue(props[i].Name));
                        }
                    }
                }
            }

            if (fieldName.Length > 0)
            {
                fieldName = fieldName.Substring(1);
            }
            else
            {
                throw new DataAccessException("Cannot generate SQL command of UPDATE.");
            }

            sql = "UPDATE " + m.GetCachedTableMappingAttribute("").TableName + " SET " + fieldName;
        }


        /// <summary>
        /// 获取实体Update语句，针对Mysql数据库
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">访问接口</param>
        /// <returns>Update语句</returns>
        public static void BuildMysqlUpdate(Model m, ref string sql, ref ParameterCollection pc)
        {
            string fieldName = "";
            object val = null;
            TableMappingAttribute attr = null;

            PropertyInfo[] props = m.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                attr = m.GetCachedTableMappingAttribute(props[i].Name);
                if (attr == null)
                {
                    continue;
                }

                if (!attr.Update)
                {
                    continue;
                }

                if (!attr.PrimaryKey)
                {
                    if (attr.DefaultValue != null)
                    {
                        fieldName += "," + attr.FieldName + "=" + attr.DefaultValue;
                    }
                    else
                    {
                        val = m.GetValue(props[i].Name);

                        if (val != null)
                        {
                            fieldName += "," + attr.FieldName + "=?"+ attr.FieldName;
                            pc.Add(attr.FieldName, m.GetValue(props[i].Name));
                        }
                    }
                }
            }

            if (fieldName.Length > 0)
            {
                fieldName = fieldName.Substring(1);
            }
            else
            {
                throw new DataAccessException("Cannot generate SQL command of UPDATE.");
            }

            sql = "UPDATE " + m.GetCachedTableMappingAttribute("").TableName + " SET " + fieldName;
        }

        #endregion
        
        #region Delete语句相关方法

        /// <summary>
        /// 获取实体的Delete语句（不包含任何条件）
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="d">数据访问接口</param>
        /// <returns>Delete语句</returns>
        public static void BuildDelete(Model m, AbstractData d, ref string sql)
        {
            sql = "DELETE FROM " + m.GetCachedTableMappingAttribute("").TableName;
        }

        #endregion

        #region 生成Where语句

        /// <summary>
        /// 根据ParameterCollection生成条件SQL语句
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static void BuildWhere(Model m, AbstractData d, ParameterCollection pc, ref string sql, ref ParameterCollection pcWhere)
        {
            sql = " WHERE 1=1";

            if (pc != null)
            {
                foreach (Parameter p in pc)
                {
                    sql += " AND " + p.Name + "=" + d.DatabaseParameterPrefix + "WHERE" + p.Name;
                    pcWhere.Add("WHERE" + p.Name, p.Value, p.Direction);
                }
            }
        }

        #endregion

        #region 生成Order By语句

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obc"></param>
        /// <returns></returns>
        public static string GetOrderBySql(OrderByCollection obc)
        {
            string orderby = "";

            if (obc != null && obc.Count > 0)
            {
                orderby = " ORDER BY ";

                foreach (OrderBy o in obc)
                {
                    orderby += o.FieldName.Replace(";", "").Replace("'", "") + " " + o.SortType.ToString() + ",";
                }

                orderby = orderby.Substring(0, orderby.Length - 1);
            }

            return orderby;
        }

        #endregion
    }
}
