using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    /// <summary>
    /// 数据访问接口
    /// </summary>
    public interface IData
    {
        #region CommandTimeout

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间。
        /// 等待命令执行的时间（以秒为单位）。默认为 30 秒。
        /// </summary>
        int CommandTimeout { get; set; }

        #endregion

        #region Connection

        /// <summary>
        /// 显示打开连接
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// 显示关闭连接
        /// </summary>
        void CloseConnection();

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>SQL语句所影响的记录数</returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>SQL语句所影响的记录数</returns>
        int ExecuteNonQuery(string sql, ParameterCollection parameters);

        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>SQL语句所影响的记录数</returns>
        int ExecuteNonQuery(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>第一行第一列的值</returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>第一行第一列的值</returns>
        object ExecuteScalar(string sql, ParameterCollection parameters);

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>第一行第一列的值</returns>
        object ExecuteScalar(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region ExecuteDataTable

        #region 不分页

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql, ParameterCollection parameters);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region 分页

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql, int pageIndex, int pageSize);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql, ParameterCollection parameters, int pageIndex, int pageSize);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据表</returns>
        DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize);

        #endregion

        #endregion

        #region ExecuteDataSet

        #region 不分页

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>数据集</returns>
        DataSet ExecuteDataSet(string sql);

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据集</returns>
        DataSet ExecuteDataSet(string sql, ParameterCollection parameters);

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据集</returns>
        DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion
        
        #region 分页

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        DataSet ExecuteDataSet(string sql, int pageIndex, int pageSize);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        DataSet ExecuteDataSet(string sql, ParameterCollection parameters, int pageIndex, int pageSize);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据表</returns>
        DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize);

        #endregion

        #endregion

        #region Paging

        /// <summary>
        /// 执行查询，返回记录总数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>记录总数</returns>
        int GetRecordCount(string sql, ParameterCollection pc);

        /// <summary>
        /// 执行查询，返回总页数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>总页数</returns>
        int GetPageCount(string sql, ParameterCollection pc, int pageSize);

        #endregion
    }
}
