using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    /// <summary>
    /// ���ݷ��ʽӿ�
    /// </summary>
    public interface IData
    {
        #region CommandTimeout

        /// <summary>
        /// ��ȡ����������ִֹ������ĳ��Բ����ɴ���֮ǰ�ĵȴ�ʱ�䡣
        /// �ȴ�����ִ�е�ʱ�䣨����Ϊ��λ����Ĭ��Ϊ 30 �롣
        /// </summary>
        int CommandTimeout { get; set; }

        #endregion

        #region Connection

        /// <summary>
        /// ��ʾ������
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// ��ʾ�ر�����
        /// </summary>
        void CloseConnection();

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// ִ�� Transact-SQL ��䲢������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>SQL�����Ӱ��ļ�¼��</returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// ִ�� Transact-SQL ��䲢������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>SQL�����Ӱ��ļ�¼��</returns>
        int ExecuteNonQuery(string sql, ParameterCollection parameters);

        /// <summary>
        /// ִ�� Transact-SQL ��䲢������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL��������</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>SQL�����Ӱ��ļ�¼��</returns>
        int ExecuteNonQuery(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>��һ�е�һ�е�ֵ</returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>��һ�е�һ�е�ֵ</returns>
        object ExecuteScalar(string sql, ParameterCollection parameters);

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����Զ�����л��С�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL��������</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>��һ�е�һ�е�ֵ</returns>
        object ExecuteScalar(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region ExecuteDataTable

        #region ����ҳ

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql, ParameterCollection parameters);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion

        #region ��ҳ

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql, int pageIndex, int pageSize);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql, ParameterCollection parameters, int pageIndex, int pageSize);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݱ�</returns>
        DataTable ExecuteDataTable(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize);

        #endregion

        #endregion

        #region ExecuteDataSet

        #region ����ҳ

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>���ݼ�</returns>
        DataSet ExecuteDataSet(string sql);

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݼ�</returns>
        DataSet ExecuteDataSet(string sql, ParameterCollection parameters);

        /// <summary>
        /// ��ȡ���ݼ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݼ�</returns>
        DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters);

        #endregion
        
        #region ��ҳ

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        DataSet ExecuteDataSet(string sql, int pageIndex, int pageSize);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">��ز���</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <returns>���ݱ�</returns>
        DataSet ExecuteDataSet(string sql, ParameterCollection parameters, int pageIndex, int pageSize);

        /// <summary>
        /// ��ȡ���ݱ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="type">SQL�������</param>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ�ļ�¼��</param>
        /// <param name="parameters">��ز���</param>
        /// <returns>���ݱ�</returns>
        DataSet ExecuteDataSet(string sql, CommandTypeEnum type, ParameterCollection parameters, int pageIndex, int pageSize);

        #endregion

        #endregion

        #region Paging

        /// <summary>
        /// ִ�в�ѯ�����ؼ�¼����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>��¼����</returns>
        int GetRecordCount(string sql, ParameterCollection pc);

        /// <summary>
        /// ִ�в�ѯ��������ҳ��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <returns>��ҳ��</returns>
        int GetPageCount(string sql, ParameterCollection pc, int pageSize);

        #endregion
    }
}
