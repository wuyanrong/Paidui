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
    /// Sql���������
    /// </summary>
    public class SqlBuilder
    {
        #region ������������

        /// <summary>
        /// ����ʵ��������ȡ����������Sql���
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ʽӿ�</param>
        /// <returns>����������Sql���</returns>
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

        #region Select�����ط���

        /// <summary>
        /// ��ȡʵ��Select���
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ݷ��ʽӿ�</param>
        /// <returns>Select���</returns>
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

            // ����
            //string sqlCommand = "SELECT " + fieldName + " FROM " + m.GetTableName();
            
            sql = "SELECT * FROM " + m.GetCachedTableMappingAttribute("").TableName;
        }

        #endregion

        #region Insert�����ط���



        /// <summary>
        /// ��ȡInsert���
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ݷ��ʶ���</param>
        /// <returns>Insert���</returns>
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
        /// ��ȡInsert��䣬���Mysql���ݿ�
        /// </summary>
        /// <param name="m">ʵ��</param> 
        /// <returns>Insert���</returns>
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

        #region Update�����ط���
        /// <summary>
        /// ��ȡʵ��Update���
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ʽӿ�</param>
        /// <returns>Update���</returns>
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
        /// ��ȡʵ��Update��䣬���Mysql���ݿ�
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ʽӿ�</param>
        /// <returns>Update���</returns>
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
        
        #region Delete�����ط���

        /// <summary>
        /// ��ȡʵ���Delete��䣨�������κ�������
        /// </summary>
        /// <param name="m">ʵ��</param>
        /// <param name="d">���ݷ��ʽӿ�</param>
        /// <returns>Delete���</returns>
        public static void BuildDelete(Model m, AbstractData d, ref string sql)
        {
            sql = "DELETE FROM " + m.GetCachedTableMappingAttribute("").TableName;
        }

        #endregion

        #region ����Where���

        /// <summary>
        /// ����ParameterCollection��������SQL���
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

        #region ����Order By���

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
