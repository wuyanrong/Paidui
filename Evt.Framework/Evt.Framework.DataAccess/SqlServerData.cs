using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
	/// <summary>
	/// ��װSqlServer�ķ��ʷ���
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
        ///  ���캯��
		/// </summary>
        /// <param name="connectionString">�����ַ���</param>
		public SqlServerData(string connectionString, bool impersonated)
		{
            this._connectionString = connectionString;
            this._impersonated = impersonated;
		}

		/// <summary>
		/// ��������
		/// </summary>
		~SqlServerData()
		{
			Dispose(false);
		}

		#endregion

		#region Connection

		/// <summary>
        /// ���������Ӷ���(��ʵ��ʹ��ʱ��������ô˷�������������������ڲ�����)
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
                        throw new DataAccessException(ex.Message, "�����ݿ�ʧ��, ʹ�õ����ݿ������ַ���Ϊ: " + this._connectionString, ex);
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
                    throw new DataAccessException(ex.Message, "�����ݿ�ʧ��, ʹ�õ����ݿ������ַ���Ϊ: " + this._connectionString, ex);
                }
            }

            this._cnn = cnn;
		}

        /// <summary>
        /// �ر��������Ӷ���(��ʵ��ʹ��ʱ��������ô˷�������������������ڲ�����)
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

        #region ������ AbstractData

        /// <summary>
        /// SQLִ�нӿ�
        /// </summary>
        public override IData IData
        {
            get { return this; }
        }

        /// <summary>
        /// ����ǰ׺��
        /// </summary>
        public override string DatabaseParameterPrefix
        {
            get { return "@"; }
        }

        /// <summary>
        /// ����Ĭ�ϼ�������
        /// </summary>
        public override void BeginTransaction()
        {
            BeginTransaction(IsolationLevelEnum.ReadCommitted);
        }

        /// <summary>
        /// ����ָ����������
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
                throw new DataAccessException(ex.Message, "������ʧ��, ���񼶱�" + level.ToString(), ex);
            }
        }

        /// <summary>
        /// �ݽ�����
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
                throw new DataAccessException(ex.Message, "�ύ����ʧ��, ��ǰ���������" + this._transCounter.ToString(), ex);
            }
        }

        /// <summary>
        /// �ع�����
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
                throw new DataAccessException(ex.Message, "�ع�����ʧ��, ��ǰ���������" + this._transCounter.ToString(), ex);
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

        #region �ӿ� IData

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
		/// ִ�� Transact-SQL ��䲢������Ӱ�������
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <returns>SQL�����Ӱ��ļ�¼��</returns>
		public int ExecuteNonQuery(string sql)
		{
            return ExecuteNonQuery(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// ִ�� Transact-SQL ��䲢������Ӱ�������
		/// </summary>
		/// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
		/// <returns>SQL�����Ӱ��ļ�¼��</returns>
        public int ExecuteNonQuery(string sql, ParameterCollection parameters)
		{
            return ExecuteNonQuery(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// ִ�����ݿ����(Insert,Update,Delete)
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="type">SQL��������</param>
		/// <param name="parameters">��ز���</param>
		/// <returns>SQL�����Ӱ��ļ�¼��</returns>
		public int ExecuteNonQuery(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			int ret = 0;
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// �ж��Ƿ�������
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

				// ���ò���
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

				// ִ��Sql
				ret = oleCmd.ExecuteNonQuery();

				// ��ȡ��������ֵ
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
				throw new DataAccessException(ex.Message, "ִ��ExecuteNonQueryʧ��, SQL���/SQL����Ϊ: " + sql + "/" + (parameters == null ? "" : parameters.ToString()) , ex);
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
        /// ִ�в�ѯ�����ؼ�¼����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>��¼����</returns>
        public int ExecuteRecordCount(string sql)
        {
            string sqlCount = "select count(*) as c from (" + sql + ") as t;";
            object ret = ExecuteScalar(sqlCount, CommandTypeEnum.Text, null);
            return ConvertUtil.ToInt(ret);
        }

		/// <summary>
		/// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <returns>��һ�е�һ�е�ֵ</returns>
		public object ExecuteScalar(string sql)
		{
            return ExecuteScalar(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
		/// </summary>
		/// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
		/// <returns>��һ�е�һ�е�ֵ</returns>
        public object ExecuteScalar(string sql, ParameterCollection parameters)
		{
            return ExecuteScalar(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="type">SQL��������</param>
		/// <param name="parameters">��ز���</param>
		/// <returns>��һ�е�һ�е�ֵ</returns>
		public object ExecuteScalar(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			object ret = null;
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// �ж��Ƿ�������
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

				// ���ò���
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

				// ִ��
				ret = oleCmd.ExecuteScalar();

				// ��ȡ��������ֵ
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
				throw new DataAccessException(ex.Message, "ִ��ExecuteScalarʧ��, SQL���/SQL����Ϊ: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
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

        #region ����ҳ�Ļ�ȡ

        /// <summary>
		/// ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, null);
		}

		/// <summary>
		/// ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql, ParameterCollection parameters)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, parameters);
		}

		/// <summary>
		/// ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="type">SQL�������</param>
        /// <param name="parameters">��ز���</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters)
		{
			DataTable ret = new DataTable();

			SqlDataAdapter da = new SqlDataAdapter();
			SqlCommand oleCmd = new SqlCommand();

			try
			{
				// �ж��Ƿ�������
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

				// ���ò���
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

				// ִ��
				da.SelectCommand = oleCmd;
				da.Fill(ret);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(ex.Message, "ִ��ExecuteDataTableʧ��, SQL���/SQL����Ϊ: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
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

        #region ��ҳ�Ļ�ȡ

        /// <summary>
		/// ��DataReader��ʽ��ҳ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="pageIndex">��ǰҳ��</param>
		/// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql, int pageIndex, int pageSize)
		{
			return ExecuteDataTable(sql, CommandTypeEnum.Text, null, pageIndex, pageSize);
		}

		/// <summary>
		/// ��DataReader��ʽ��ҳ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
		/// <param name="pageIndex">��ǰҳ��</param>
		/// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql, ParameterCollection parameters, int pageIndex, int pageSize)
		{
            return ExecuteDataTable(sql, CommandTypeEnum.Text, parameters, pageIndex, pageSize);
		}

		/// <summary>
		/// ��ȡ���ݱ�
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="type">SQL�������</param>
		/// <param name="pageIndex">��ǰҳ��</param>
		/// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <param name="parameters">��ز���</param>
		/// <returns>���ݱ�</returns>
		public DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
		{
            return GetDataTableByDataReader(sql, type, parameters, pageIndex, pageSize);
        }

        // ��rownum��ʽ
        private DataTable GetDataTableByRowNum(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            //SELECT * 
            //FROM (select *,ROW_NUMBER() Over(order by id) as rowNum from table_info )as myTable
            //WHERE rowNum between 50 and 60;

            //ע��SQL Server ��ROW_NUMBER()�������ܣ�ͨ���Զ��������ݲ�ʹ��

            return null;
        }

        // ��Reader��ʽ
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

                // �ж��Ƿ�������
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

				// ���ò���
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

				// ִ��
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
				throw new DataAccessException(ex.Message, "ִ��ExecuteDataTableʧ��, SQL���/SQL����Ϊ: " + sql + "/" +	(parameters == null ? "" : parameters.ToString()), ex);
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

        #region ����ҳ�Ļ�ȡ

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>���ݼ�</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, null);
        }

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݼ�</returns>
        public DataSet ExecuteDataSet(string sql, ParameterCollection parameters)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, parameters);
        }

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݼ�</returns>
        public DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters)
        {
            DataSet ret = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand oleCmd = new SqlCommand();

            try
            {
                // �ж��Ƿ�������
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

                // ���ò���
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

                // ִ��
                da.SelectCommand = oleCmd;
                da.Fill(ret);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message, "ִ��ExecuteDataSetʧ��, SQL���/SQL����Ϊ: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
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

        #region ��ҳ�Ļ�ȡ

        /// <summary>
        /// ��DataReader��ʽ��ҳ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        public DataSet ExecuteDataSet(string sql, int pageIndex, int pageSize)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, null, pageIndex, pageSize);
        }

        /// <summary>
        /// ��DataReader��ʽ��ҳ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        public DataSet ExecuteDataSet(string sql, ParameterCollection parameters, int pageIndex, int pageSize)
        {
            return ExecuteDataSet(sql, CommandTypeEnum.Text, parameters, pageIndex, pageSize);
        }

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݱ�</returns>
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

                // �ж��Ƿ�������
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

                // ���ò���
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

                // ִ��
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
                throw new DataAccessException(ex.Message, "ִ��ExecuteDataSetʧ��, SQL���/SQL����Ϊ: " + sql + "/" + (parameters == null ? "" : parameters.ToString()), ex);
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
        /// ִ�в�ѯ�����ؼ�¼����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>��¼����</returns>
        public int GetRecordCount(string sql, ParameterCollection pc)
        {
            string sqlCount = "select count(*) as c from (" + sql + ") as t;";
            int count = ConvertUtil.ToInt(ExecuteScalar(sqlCount, CommandTypeEnum.Text, pc));
            return count;
        }

        /// <summary>
        /// ִ�в�ѯ��������ҳ��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <returns>��ҳ��</returns>
        public int GetPageCount(string sql, ParameterCollection pc, int pageSize)
        {
            int count = (int)Math.Ceiling(GetRecordCount(sql, pc) * 1.0 / pageSize);
            return count;
        }

        #endregion

        #endregion

        #region IDisposable

        /// <summary>
		/// ������Դ
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
            // �滻����
            string sql = ParseParameter(srcSql);

            // �û�ģ��
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