using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Reflection;

using MySql.Data.Types;
using MySql.Data.MySqlClient;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    public abstract class AbstractData
    {
        #region Abstract

        /// <summary>
        /// SQL执行接口
        /// </summary>
        public abstract IData IData
        {
            get;
        }

        /// <summary>
        /// 参数占位符
        /// </summary>
        public abstract string DatabaseParameterPrefix
        {
            get;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public abstract void BeginTransaction();

        /// <summary>
        /// 开始指定级别的事务
        /// </summary>
        /// <param name="level">事务级别</param>
        public abstract void BeginTransaction(IsolationLevelEnum level);

        /// <summary>
        /// 递交事务
        /// </summary>
        public abstract void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        public abstract void RollbackTransaction();

        #endregion

        #region Virtual

        #region Create

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public virtual int Create(Model model)
        {
            int ret = 0;

            List<PropertyInfo> pkProp = new List<PropertyInfo>();
            List<bool> setValue = new List<bool>();

            PropertyInfo[] props = model.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                TableMappingAttribute attr = model.GetCachedTableMappingAttribute(prop.Name);
                if (attr == null)
                {
                    continue;
                }
                if (attr.PrimaryKey)
                {
                    pkProp.Add(prop);
                    setValue.Add(true);
                }
            }

            try
            {
                BeginTransaction();

                for (int i = 0; i < pkProp.Count; i++)
                {
                    if (model.GetValue(pkProp[i].Name) == null)
                    {
                        if (pkProp[i].PropertyType.FullName == "System.String")
                        {
                            model.SetValue(pkProp[i].Name, System.Guid.NewGuid().ToString());
                        }
                        else
                        {
                            setValue[i] = false;
                        }
                    }
                }

                string sql = null;
                ParameterCollection pc = new ParameterCollection();
                SqlBuilder.BuildInsert(model, this, ref sql, ref pc);

                ret = IData.ExecuteNonQuery(sql, CommandTypeEnum.Text, pc);

                for (int i = 0; i < pkProp.Count; i++)
                {
                    if (!setValue[i])
                    {
                        if (pkProp[i].PropertyType.FullName.IndexOf("System.Int") != -1)
                        {
                            object pkValue = IData.ExecuteScalar("SELECT @@identity");
                            model.SetValue(pkProp[i].Name,Convert.ToInt32(pkValue));
                        }
                    }
                }

                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }

            return ret;
        }

        #endregion

        #region Retrieve

        /// <summary>
        /// 以实体的主键值为条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>实体</returns>
        public virtual DataTable Retrieve(Model model)
        {
            DataTable dt = null;

            ParameterCollection pcRetrieve = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildSelect(model, this, ref sql);

            string sqlWhere = null;
            SqlBuilder.BuildPK(model, this, ref sqlWhere, ref pcRetrieve);

            dt = IData.ExecuteDataTable(sql + sqlWhere, CommandTypeEnum.Text, pcRetrieve);

            return dt;
        }

        /// <summary>
        /// 以条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>实体</returns>
        public virtual DataTable RetrieveMultiple(Model model, ParameterCollection pc)
        {
            return RetrieveMultiple(model, pc, null);
        }

        /// <summary>
        /// 以条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <param name="obc"></param>
        /// <returns>实体</returns>
        public virtual DataTable RetrieveMultiple(Model model, ParameterCollection pc, OrderByCollection obc)
        {
            DataTable dt = null;

            ParameterCollection pcRetrieve = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildSelect(model, this, ref sql);

            string sqlWhere = null;
            SqlBuilder.BuildWhere(model, this, pc, ref sqlWhere, ref pcRetrieve);

            dt = IData.ExecuteDataTable(sql + sqlWhere + SqlBuilder.GetOrderBySql(obc), CommandTypeEnum.Text, pcRetrieve);

            return dt;
        }

        #endregion

        #region Update

        /// <summary>
        /// 以实体的主键值为条件更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public virtual int Update(Model model)
        {
            int ret = 0;

            ParameterCollection pcUpdate = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildUpdate(model, this, ref sql, ref pcUpdate);

            string sqlWhere = null;
            SqlBuilder.BuildPK(model, this, ref sqlWhere, ref pcUpdate);

            ret = IData.ExecuteNonQuery(sql + sqlWhere, CommandTypeEnum.Text, pcUpdate);

            return ret;
        }

        /// <summary>
        /// 以实体的主键值为条件更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>影响的行数</returns>
        public virtual int UpdateMultiple(Model model, ParameterCollection pc)
        {
            int ret = 0;

            ParameterCollection pcUpdate = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildUpdate(model, this, ref sql, ref pcUpdate);

            string sqlWhere = null;
            SqlBuilder.BuildWhere(model, this, pc, ref sqlWhere, ref pcUpdate);

            ret = IData.ExecuteNonQuery(sql + sqlWhere, CommandTypeEnum.Text, pcUpdate);

            return ret;
        }

        #endregion

        #region Delete

        /// <summary>
        /// 以实体的主键值为条件删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public virtual int Delete(Model model)
        {
            int ret = 0;

            ParameterCollection pcDelete = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildDelete(model, this, ref sql);

            string sqlWhere = null;
            SqlBuilder.BuildPK(model, this, ref sqlWhere, ref pcDelete);

            ret = IData.ExecuteNonQuery(sql + sqlWhere, CommandTypeEnum.Text, pcDelete);

            return ret;
        }

        /// <summary>
        /// 以条件删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>影响的行数</returns>
        public virtual int DeleteMultiple(Model model, ParameterCollection pc)
        {
            int ret = 0;

            ParameterCollection pcDelete = new ParameterCollection();

            string sql = null;
            SqlBuilder.BuildDelete(model, this, ref sql);

            string sqlWhere = null;
            SqlBuilder.BuildWhere(model, this, pc, ref sqlWhere, ref pcDelete);

            ret = IData.ExecuteNonQuery(sql + sqlWhere, CommandTypeEnum.Text, pcDelete);

            return ret;
        }

        #endregion

        #endregion

        #region 实现与DataTable之间的转换功能实现

        /// <summary>
        /// 将实体转换成DataTable，DataTable中的列名称以实体的字段名为准
        /// </summary>
        /// <returns>数据表</returns>
        public virtual DataTable ConvertTo(Model model)
        {
            DataTable table = new DataTable();
            DataRow row = null;
            object ret = null;

            Type t = model.GetCachedType();
            PropertyInfo[] props = model.GetCachedPropertyInfo();

            // 创建表结构
            for (int i = 0; i < props.Length; i++)
            {
                table.Columns.Add(props[i].Name);
            }

            // 新建行
            row = table.NewRow();
            for (int i = 0; i < props.Length; i++)
            {
                ret = t.InvokeMember(props[i].Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, model, null);
                if (ret != null)
                {
                    row[i] = ret;
                }
            }
            table.Rows.Add(row);

            return table;
        }

        /// <summary>
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        public virtual void ConvertFrom(Model model, DataTable table)
        {
            ConvertFrom(model, table, 0);
        }

        /// <summary>
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="rowIndex">数据行</param>
        public virtual void ConvertFrom(Model model, DataTable table, int rowIndex)
        {
            if (table == null || table.Rows.Count == 0 || table.Rows.Count < rowIndex)
            {
                return;
            }

            object ret = null;

            Type t = model.GetCachedType();
            PropertyInfo[] props = model.GetCachedPropertyInfo();

            for (int i = 0; i < props.Length; i++)
            {
                TableMappingAttribute attr = model.GetCachedTableMappingAttribute(props[i].Name);
                if (attr != null && table.Columns.Contains(attr.FieldName))
                {
                    ret = table.Rows[rowIndex][attr.FieldName];
                    if (ret == DBNull.Value)
                    {
                        ret = null;
                    }
                    else if (ret is MySqlDateTime)
                    {
                        ret = ((MySqlDateTime)ret).GetDateTime();
                    }
                    else if (ret is Guid)
                    {
                        ret = ret.ToString();
                    }

                    if (props[i].PropertyType == typeof(bool?))
                    {
                        ret = Convert.ToBoolean(ret);
                    }

                    t.InvokeMember(props[i].Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, model, new object[] { ret });
                }
            }
        }

        #endregion
    }
}
