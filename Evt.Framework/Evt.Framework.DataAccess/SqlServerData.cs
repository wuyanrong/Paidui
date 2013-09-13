using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
	/// <summary>
	/// 封装SqlServer的访问方法
	/// </summary>
	internal sealed class SqlServerData : AbstractData, IData, IDisposable
	{
		#region Properties

		private string _connectionString = null;
        private bool _impersonated = false;
		private SqlConnection _cnn = null;
		private SqlTransaction _trans = null;
		private bool _disposed = false;
		private int _transCounter = 0;
        private int _commandTimeout = 30;

		#endregion

		#region Constructor

		/// <summary>
        ///  构造函数
		/// </summary>
        /// <param name="connectionString">连接字符串</param>
		public SqlServerData(string connectionString, bool impersonated)
		{
            this._connectionString = connectionString;
            this._impersonated = impersonated;
		}

		/// <summary>
		/// 析构函数
		/// </summary>
		~SqlServerData()
		{
			Dispose(false);
		}

		#endregion

		#region Connection

		/// <summary>
        /// 打开数据连接对象。(在实际使用时，不需调用此方法，这个方法将由类内部调用)
		/// </summary>
		public void OpenConnection()
		{
            SqlConnection cnn = null;

            if (this._impersonated)
            {
                using (Impersonator im = new Impersonator())
                {
                    cnn = new SqlConnection(this._connectionString);

                    try
                    {
                        cnn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException(ex.Message, "打开数据库失败, 使用的数据库连接字符串为: " + this._connectionString, ex);
                    }
                }
            }
            else
            {
                cnn = new SqlConnection(this._connectionString);

                try
                {
                    cnn.Open();
                }
                catch (Exception ex)
                {
                    throw new DataAccessException(ex.Message, "打开数据库失败, 使用的数据库连接字符串为: " + this._connectionString, ex);
                }
            }

            this._cnn = cnn;
		}

        /// <summary>
        /// 关闭数据连接对象。(在实际使用时，不需调用此方法，这个方法将由类内部调用)
        /// </summary>
        public void CloseConnection()
        {
            if (this._cnn != null)
            {
                this._cnn.Close();
                this._cnn = null;
            }
        }

		#endregion

        #region 抽像类 AbstractData

        /// <summary>
        /// SQL执行接口
        /// </summary>
        public override IData IData
        {
            get { return this; }
        }

        /// <summary>
        /// 参数前缀符
        /// </summary>
        public override string DatabaseParameterPrefix
        {
            get { return "@"; }
        }

        /// <summary>
        /// 启动默认级别事务
        /// </summary>
        public override void BeginTransaction()
        {
            BeginTransaction(IsolationLevelEnum.ReadCommitted);
        }

        /// <summary>
        /// 启动指定级别事务
        /// </summary>
        public override void BeginTransaction(IsolationLevelEnum level)
        {
            try
            {
                this._transCounter++;
                if (this._transCounter == 1)
                {
                    OpenConnection();

                    IsolationLevel il = IsolationLevel.ReadCommitted;

                    switch (level)
                    {
                        case IsolationLevelEnum.Chaos:
                            il = IsolationLevel.Chaos;
                            break;
                        case IsolationLevelEnum.ReadCommitted:
                            il = IsolationLevel.ReadCommitted;
                            break;
                        case IsolationLevelEnum.ReadUncommitted:
                            il = IsolationLevel.ReadUncommitted;
                            break;
                        case IsolationLevelEnum.RepeatableRead:
                            il = IsolationLevel.RepeatableRead;
                            break;
                        case IsolationLevelEnum.Serializable:
                            il = IsolationLevel.Serializable;
                            break;
                        case IsolationLevelEnum.Snapshot:
                            il = IsolationLevel.Snapshot;
                            break;
                        case IsolationLevelEnum.Unspecified:
                            il = IsolationLevel.Unspecified;
                            break;
                        default:
                            il = IsolationLevel.ReadCommitted;
                            break;
                    }

                    this._trans = this._cnn.BeginTransaction(il);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "打开事务失败, 事务级别：" + level.ToString(), ex);
            }
        }

        /// <summary>
        /// 递交事务
        /// </summary>
        public override void CommitTransaction()
        {
            try
            {
                this._transCounter--;
                if (this._transCounter == 0)
                {
                    this._trans.Commit();
                    this._trans.Dispose();
                    this._trans = null;

                    this._cnn.Close();
                    this._cnn = null;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "提交事务失败, 当前事务记数：" + this._transCounter.ToString(), ex);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public override void RollbackTransaction()
        {
            try
            {
                if (this._trans != null)
                {
                    this._trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "回滚事务失败, 当前事务记数：" + this._transCounter.ToString(), ex);
            }
            finally
            {
                this._transCounter = 0;
                if (this._trans != null)
                {
                    this._trans.Dispose();
                    this._trans = null;
                }
                if (this._cnn != null)
                {
                    this._cnn.Close();
                    this._cnn = null;
                }
            }
        }

        #endregion

        #region 接口 IData

        #region CommandTimeout

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        #endregion

		#region ExecuteNonQuery

		/// <summary>
		/// 执行 Transact-SQL 语句并返回受影响的行数
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>SQL语句所影响的记录数</returns>
		public int ExecuteNonQuery(string sql)
		{
            return ExecuteNonQuery(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// 执行 Transact-SQL 语句并返回受影响的行数
		/// </summary>
		/// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
		/// <returns>SQL语句所影响的记录数</returns>
        public int ExecuteNonQuery(string sql, ParameterCollection parameters)
		{
            return ExecuteNonQuery(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// 执行数据库操作(Insert,Update,Delete)
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="type">SQL语句的类型</param>
		/// <param name="parameters">相关参数</param>
		/// <returns>SQL语句所影响的记录数</returns>
		public int ExecuteNonQuery(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			int ret = 0;
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// 判断是否有事务
				if (this._transCounter == 0)
				{
					OpenConnection();
				}
				else
				{
					oleCmd.Transaction = this._trans;
				}

				oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

				if (type == CommandTypeEnum.Text)
				{
					oleCmd.CommandType = CommandType.Text;
				}
				else if (type == CommandTypeEnum.StoredProcedure)
				{
					oleCmd.CommandType = CommandType.StoredProcedure;
				}
				else if (type == CommandTypeEnum.TableDirect)
				{
					oleCmd.CommandType = CommandType.TableDirect;
				}

				// 设置参数
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
						param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
						oleCmd.Parameters.Add(param);
					}
				}

				// 执行Sql
				ret = oleCmd.ExecuteNonQuery();

				// 获取参数返回值
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
                        if (parameters[i].Direction != ParameterDirectionEnum.Input)
                        {
                            parameters[i].Value = oleCmd.Parameters[DatabaseParameterPrefix + parameters[i].Name].Value;
                        }
					}
				}
			}
			catch (Exception ex)
			{
				throw new DataAccessException(ex.Message, "执行ExecuteNonQuery失败, SQL语句/SQL参数为: " + sql + "/" + (parameters == null ? "" : parameters.ToString()) , ex);
			}
			finally
			{
				if (this._transCounter == 0 && this._cnn != null)
				{
					this._cnn.Close();
					this._cnn = null;
				}
			}

			return ret;
		}

		#endregion

		#region ExecuteScalar

        /// <summary>
        /// 执行查询，返回记录总数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>记录总数</returns>
        public int ExecuteRecordCount(string sql)
        {
            string sqlCount = "select count(*) as c from (" + sql + ") as t;";
            object ret = ExecuteScalar(sqlCount, CommandTypeEnum.Text, null);
            return ConvertUtil.ToInt(ret);
        }

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>第一行第一列的值</returns>
		public object ExecuteScalar(string sql)
		{
            return ExecuteScalar(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
		/// </summary>
		/// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
		/// <returns>第一行第一列的值</returns>
        public object ExecuteScalar(string sql, ParameterCollection parameters)
		{
            return ExecuteScalar(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="type">SQL语句的类型</param>
		/// <param name="parameters">相关参数</param>
		/// <returns>第一行第一列的值</returns>
		public object ExecuteScalar(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			object ret = null;
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// 判断是否有事务
				if (this._transCounter == 0)
				{
					OpenConnection();
				}
				else
				{
					oleCmd.Transaction = this._trans;
				}

				oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

				if (type == CommandTypeEnum.Text)
				{
					oleCmd.CommandType = CommandType.Text;
				}
				else if (type == CommandTypeEnum.StoredProcedure)
				{
					oleCmd.CommandType = CommandType.StoredProcedure;
				}
				else if (type == CommandTypeEnum.TableDirect)
				{
					oleCmd.CommandType = CommandType.TableDirect;
				}

				// 设置参数
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
						param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
						oleCmd.Parameters.Add(param);

					}
				}

				// 执行
				ret = oleCmd.ExecuteScalar();

				// 获取参数返回值
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
                        if (parameters[i].Direction != ParameterDirectionEnum.Input)
                        {
                            parameters[i].Value = oleCmd.Parameters[DatabaseParameterPrefix + parameters[i].Name].Value;
                        }
					}
				}
			}
			catch (Exception ex)
			{
				throw new DataAccessException(ex.Message, "执行ExecuteScalar失败, SQL语句/SQL参数为: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
			}
			finally
			{
				if (this._transCounter == 0 && this._cnn != null)
				{
					this._cnn.Close();
					this._cnn = null;
				}
			}

			return ret;
		}

		#endregion

		#region ExecuteDataTable

        #region 不分页的获取

        /// <summary>
		/// 获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// 获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql, ParameterCollection parameters)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// 获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="type">SQL语句类型</param>
        /// <param name="parameters">相关参数</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			DataTable ret = new DataTable();

			SqlDataAdapter da = new SqlDataAdapter();
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// 判断是否有事务
				if (this._transCounter == 0)
				{
					OpenConnection();
				}
				else
				{
					oleCmd.Transaction = this._trans;
				}

				oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

				if (type == CommandTypeEnum.Text)
				{
					oleCmd.CommandType = CommandType.Text;
				}
				else if (type == CommandTypeEnum.StoredProcedure)
				{
					oleCmd.CommandType = CommandType.StoredProcedure;
				}
				else if (type == CommandTypeEnum.TableDirect)
				{
					oleCmd.CommandType = CommandType.TableDirect;
				}

				// 设置参数
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
                        param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
						oleCmd.Parameters.Add(param);
					}
				}

				// 执行
				da.SelectCommand = oleCmd;
				da.Fill(ret);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(ex.Message, "执行ExecuteDataTable失败, SQL语句/SQL参数为: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
			}
			finally
			{
				if (this._transCounter == 0 && this._cnn != null)
				{
					this._cnn.Close();
					this._cnn = null;
				}
			}

			return ret;
        }

        #endregion

        #region 分页的获取

        /// <summary>
		/// 以DataReader方式分页获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="pageIndex">当前页数</param>
		/// <param name="pageSize">当前页显示的记录数</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql, int pageIndex, int pageSize)
		{
			return ExecuteDataTable(sql, CommandTypeEnum.Text, null, pageIndex, pageSize);
		}

		/// <summary>
		/// 以DataReader方式分页获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
		/// <param name="pageIndex">当前页数</param>
		/// <param name="pageSize">当前页显示的记录数</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql, ParameterCollection parameters, int pageIndex, int pageSize)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, parameters, pageIndex, pageSize);
		}

		/// <summary>
		/// 获取数据表
		/// </summary>
		/// <param name="sql">SQL语句</param>
		/// <param name="type">SQL语句类型</param>
		/// <param name="pageIndex">当前页数</param>
		/// <param name="pageSize">当前页显示的记录数</param>
        /// <param name="parameters">相关参数</param>
		/// <returns>数据表</returns>
		public DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
		{
            return GetDataTableByDataReader(sql, type, parameters, pageIndex, pageSize);
        }

        // 用rownum方式
        private DataTable GetDataTableByRowNum(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            //SELECT * 
            //FROM (select *,ROW_NUMBER() Over(order by id) as rowNum from table_info )as myTable
            //WHERE rowNum between 50 and 60;

            //注：SQL Server 的ROW_NUMBER()函数性能，通用性都不够，暂不使用

            return null;
        }

        // 用Reader方式
        private DataTable GetDataTableByDataReader(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            DataTable ret = new DataTable();

            int startIndex = 0;
            int endIndex = 0;
            int rowIndex = 0;

            SqlCommand oleCmd = new SqlCommand();

            try
            {
                startIndex = pageSize * (pageIndex - 1);
                endIndex = startIndex + pageSize - 1;

                // 判断是否有事务
                if (this._transCounter == 0)
                {
                    OpenConnection();
                }
                else
                {
                    oleCmd.Transaction = this._trans;
                }

                oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

				if (type == CommandTypeEnum.Text)
				{
					oleCmd.CommandType = CommandType.Text;
				}
				else if (type == CommandTypeEnum.StoredProcedure)
				{
					oleCmd.CommandType = CommandType.StoredProcedure;
				}
				else if (type == CommandTypeEnum.TableDirect)
				{
					oleCmd.CommandType = CommandType.TableDirect;
				}

				// 设置参数
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
						param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
						oleCmd.Parameters.Add(param);
					}
				}

				// 执行
                SqlDataReader dr = oleCmd.ExecuteReader();

                for (int i = 0; i < dr.FieldCount; i++)
                {
                    ret.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
                }

                while (dr.Read())
                {
                    rowIndex++;
                    if (rowIndex > startIndex)
                    {
                        DataRow row = ret.NewRow();
                        for (int i = 0; i < ret.Columns.Count; i++)
                        {
                            row[i] = dr.GetValue(i);
                        }
                        ret.Rows.Add(row);
                    }
                    if (rowIndex > endIndex)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
				throw new DataAccessException(ex.Message, "执行ExecuteDataTable失败, SQL语句/SQL参数为: " + sql + "/" +	(parameters == null ? "" : parameters.ToString()), ex);
			}
            finally
            {
                if (this._transCounter == 0 && this._cnn != null)
                {
                    this._cnn.Close();
                    this._cnn = null;
                }
            }

            return ret;
        }

        #endregion

        #endregion

        #region ExecuteDataSet

        #region 不分页的获取

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, null);
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string sql, ParameterCollection parameters)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, parameters);
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters)
        {
            DataSet ret = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand oleCmd = new SqlCommand();

            try
            {
                // 判断是否有事务
                if (this._transCounter == 0)
                {
                    OpenConnection();
                }
                else
                {
                    oleCmd.Transaction = this._trans;
                }

                oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

                if (type == CommandTypeEnum.Text)
                {
                    oleCmd.CommandType = CommandType.Text;
                }
                else if (type == CommandTypeEnum.StoredProcedure)
                {
                    oleCmd.CommandType = CommandType.StoredProcedure;
                }
                else if (type == CommandTypeEnum.TableDirect)
                {
                    oleCmd.CommandType = CommandType.TableDirect;
                }

                // 设置参数
                if (parameters != null && parameters.Count > 0)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
                        param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
                        oleCmd.Parameters.Add(param);
                    }
                }

                // 执行
                da.SelectCommand = oleCmd;
                da.Fill(ret);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "执行ExecuteDataSet失败, SQL语句/SQL参数为: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
            }
            finally
            {
                if (this._transCounter == 0 && this._cnn != null)
                {
                    this._cnn.Close();
                    this._cnn = null;
                }
            }

            return ret;
        }

        #endregion

        #region 分页的获取

        /// <summary>
        /// 以DataReader方式分页获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        public DataSet ExecuteDataSet(string sql, int pageIndex, int pageSize)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, null, pageIndex, pageSize);
        }

        /// <summary>
        /// 以DataReader方式分页获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">相关参数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <returns>数据表</returns>
        public DataSet ExecuteDataSet(string sql, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, parameters, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句类型</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">当前页显示的记录数</param>
        /// <param name="parameters">相关参数</param>
        /// <returns>数据表</returns>
        public DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            DataTable ret = new DataTable();

            int startIndex = 0;
            int endIndex = 0;
            int rowIndex = 0;

            SqlCommand oleCmd = new SqlCommand();

            try
            {
                startIndex = pageSize * (pageIndex - 1);
                endIndex = startIndex + pageSize - 1;

                // 判断是否有事务
                if (this._transCounter == 0)
                {
                    OpenConnection();
                }
                else
                {
                    oleCmd.Transaction = this._trans;
                }

                oleCmd.Connection = this._cnn;
                oleCmd.CommandText = GetNativeSQL(sql, CommandTypeEnum.Text);
                oleCmd.CommandTimeout = _commandTimeout;

                if (type == CommandTypeEnum.Text)
                {
                    oleCmd.CommandType = CommandType.Text;
                }
                else if (type == CommandTypeEnum.StoredProcedure)
                {
                    oleCmd.CommandType = CommandType.StoredProcedure;
                }
                else if (type == CommandTypeEnum.TableDirect)
                {
                    oleCmd.CommandType = CommandType.TableDirect;
                }

                // 设置参数
                if (parameters != null && parameters.Count > 0)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = DatabaseParameterPrefix + parameters[i].Name;
                        param.Value = parameters[i].Value;
                        param.Direction = (ParameterDirection)parameters[i].Direction;
                        oleCmd.Parameters.Add(param);
                    }
                }

                // 执行
                SqlDataReader dr = oleCmd.ExecuteReader();

                for (int i = 0; i < dr.FieldCount; i++)
                {
                    ret.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
                }

                while (dr.Read())
                {
                    rowIndex++;
                    if (rowIndex > startIndex)
                    {
                        DataRow row = ret.NewRow();
                        for (int i = 0; i < ret.Columns.Count; i++)
                        {
                            row[i] = dr.GetValue(i);
                        }
                        ret.Rows.Add(row);
                    }
                    if (rowIndex > endIndex)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "执行ExecuteDataSet失败, SQL语句/SQL参数为: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
            }
            finally
            {
                if (this._transCounter == 0 && this._cnn != null)
                {
                    this._cnn.Close();
                    this._cnn = null;
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(ret);

            return ds;
        }

        #endregion

        #endregion

        #region Paging

        /// <summary>
        /// 执行查询，返回记录总数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>记录总数</returns>
        public int GetRecordCount(string sql, ParameterCollection pc)
        {
            string sqlCount = "select count(*) as c from (" + sql + ") as t;";
            int count = ConvertUtil.ToInt(ExecuteScalar(sqlCount, CommandTypeEnum.Text, pc));
            return count;
        }

        /// <summary>
        /// 执行查询，返回总页数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>总页数</returns>
        public int GetPageCount(string sql, ParameterCollection pc, int pageSize)
        {
            int count = (int)Math.Ceiling(GetRecordCount(sql, pc) * 1.0 / pageSize);
            return count;
        }

        #endregion

        #endregion

        #region IDisposable

        /// <summary>
		/// 销毁资源
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if(!this._disposed)
			{
				if(disposing)
				{
					if (this._trans != null)
					{
						this._trans.Dispose();
						this._trans = null;
					}
					if (this._cnn != null)
					{
						this._cnn.Close();
						this._cnn = null;
					}
				}
			}
			this._disposed = true;         
		}

		#endregion

        #region NativeSQL

        private string GetImpersonatedDomainUser()
        {
            string domainUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            //if (_domain.Length == 0)
            //{
            //    OleDbConnection sqlCon = new OleDbConnection(this._connectionString);
            //    sqlCon.Open();

            //    OleDbCommand oleCmd = new OleDbCommand();
            //    oleCmd.Connection = sqlCon;
            //    oleCmd.CommandText = "select suser_sname();";
            //    oleCmd.CommandType = CommandType.Text;

            //    _domain = oleCmd.ExecuteScalar().ToString().Split('\\')[0];
            //}

            //if (string.Compare(_domain.ToUpper(), "NT AUTHORITY", true) != 0)
            //{
            //    string[] names = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');

            //    domainUserName = _domain + "\\" + names[1];
            //}

            return domainUserName;
        }

        private string GetNativeSQL(string srcSql, CommandTypeEnum type)
        {
            // 替换参数
            string sql = ParseParameter(srcSql);

            // 用户模拟
            if (this._impersonated)
            {
                if (type == CommandTypeEnum.StoredProcedure)
                {
                    sql = "execute as user = '" + GetImpersonatedDomainUser() + "';exec " + srcSql + ";revert;";
                }
                else
                {
                    sql = "execute as user = '" + GetImpersonatedDomainUser() + "';" + srcSql + ";revert;";
                }
            }

            return sql;
        }

        private string ParseParameter(string sql)
        {
            string pattern = @"(?<!')\$(?<value>[^\$']+)\$(?<!')";
            return Regex.Replace(sql, pattern, new MatchEvaluator(MatchParameter));
        }

        private string MatchParameter(Match m)
        {
            return DatabaseParameterPrefix + m.Groups["value"].Value;
        }

        #endregion
    }
}