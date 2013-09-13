using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Reflection;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    [Serializable]
    public abstract class DataManager
    {
        #region 实例获取相关

        private const string CONTEXT_ID = "{00C0D20F-2F0E-493b-B327-4B48DE66294D}"; // 默认的内部上下文ID

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        private AbstractData Data
        {
            get
            {
                AbstractData data = null;

                if (AutoContext)
                {
                    data = CallContext.GetData(ContextID) as AbstractData;

                    if (data == null)
                    {
                        data = DataFactory.GetAbstractData(ConnectionString, Impersonated, DatabaseType);
                        CallContext.SetData(ContextID, data);
                    }
                }
                else
                {
                    data = DataFactory.GetAbstractData(ConnectionString, Impersonated, DatabaseType);
                }

                return data;
            }
        }

        /// <summary>
        /// Get database connection string
        /// This method can be overrided if you want to change the default behavior.
        /// </summary>
        /// <returns></returns>
        public  abstract string ConnectionString
        {
            get;
        }

        /// <summary>
        /// Indicate whether needs to impersonate Windows Authentication.
        /// This property can be overrided if you want to change the default behavior.
        /// </summary>
        protected virtual bool Impersonated
        {
            get { return false; }
        }

        /// <summary>
        /// Indicate whether using the connection object which is in CallContext.
        /// </summary>
        protected virtual bool AutoContext
        {
            get { return true; }
        }

        /// <summary>
        /// Internal context id, can be overrided.
        /// </summary>
        protected virtual string ContextID
        {
            get { return CONTEXT_ID; }
        }

        /// <summary>
        /// Internal the DB type.
        /// </summary>
        protected virtual DatabaseTypeEnum DatabaseType
        {
            get { return DatabaseTypeEnum.SqlServer; }
        }

        #endregion

        #region 事务控制相关

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            Data.BeginTransaction();
        }

        /// <summary>
        /// 开始指定事务级别的事务
        /// </summary>
        /// <param name="level">事务级别</param>
        public void BeginTransaction(IsolationLevelEnum level)
        {
            Data.BeginTransaction(level);
        }

        /// <summary>
        /// 递交事务
        /// </summary>
        public void CommitTransaction()
        {
            Data.CommitTransaction();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            Data.RollbackTransaction();
        }

        #endregion

        #region 实体操作相关

        /// <summary>
        /// SQL语句执行接口
        /// </summary>
        public IData IData
        {
            get { return Data.IData; }
        }

        #region Create

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public int Create(Model model)
        {
            return Data.Create(model);
        }

        #endregion

        #region Retrieve

        /// <summary>
        /// 以实体的主键值为条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>实体</returns>
        public DataTable Retrieve(Model model)
        {
            return Data.Retrieve(model);
        }

        /// <summary>
        /// 以条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>实体</returns>
        public DataTable RetrieveMultiple(Model model, ParameterCollection pc)
        {
            return Data.RetrieveMultiple(model, pc, null);
        }

        /// <summary>
        /// 以条件获取
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <param name="obc"></param>
        /// <returns>实体</returns>
        public DataTable RetrieveMultiple(Model model, ParameterCollection pc, OrderByCollection obc)
        {
            return Data.RetrieveMultiple(model, pc, obc);
        }

        #endregion

        #region Update

        /// <summary>
        /// 以实体的主键值为条件更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public int Update(Model model)
        {
            return Data.Update(model);
        }

        /// <summary>
        /// 以实体的主键值为条件更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>影响的行数</returns>
        public int UpdateMultiple(Model model, ParameterCollection pc)
        {
            return Data.UpdateMultiple(model, pc);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 以实体的主键值为条件删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>影响的行数</returns>
        public int Delete(Model model)
        {
            return Data.Delete(model);
        }

        /// <summary>
        /// 以条件删除
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="pc"></param>
        /// <returns>影响的行数</returns>
        public int DeleteMultiple(Model model, ParameterCollection pc)
        {
            return Data.DeleteMultiple(model, pc);
        }

        #endregion

        #endregion

        #region 实现与DataTable之间的转换功能实现

        /// <summary>
        /// 将Model转换成DataTable
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable ConvertTo(Model model)
        {
            return Data.ConvertTo(model);
        }

        /// <summary>
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        public virtual void ConvertFrom(Model model, DataTable table)
        {
            Data.ConvertFrom(model, table);
        }

        /// <summary>
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="rowIndex">数据行</param>
        public virtual void ConvertFrom(Model model, DataTable table, int rowIndex)
        {
            Data.ConvertFrom(model, table, rowIndex);
        }

        #endregion
    }
}
