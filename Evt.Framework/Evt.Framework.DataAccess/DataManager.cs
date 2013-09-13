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
    /// ���ݷ�����
    /// </summary>
    [Serializable]
    public abstract class DataManager
    {
        #region ʵ����ȡ���

        private const string CONTEXT_ID = "{00C0D20F-2F0E-493b-B327-4B48DE66294D}"; // Ĭ�ϵ��ڲ�������ID

        /// <summary>
        /// ִ��SQL���
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

        #region ����������

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void BeginTransaction()
        {
            Data.BeginTransaction();
        }

        /// <summary>
        /// ��ʼָ�����񼶱������
        /// </summary>
        /// <param name="level">���񼶱�</param>
        public void BeginTransaction(IsolationLevelEnum level)
        {
            Data.BeginTransaction(level);
        }

        /// <summary>
        /// �ݽ�����
        /// </summary>
        public void CommitTransaction()
        {
            Data.CommitTransaction();
        }

        /// <summary>
        /// �ع�����
        /// </summary>
        public void RollbackTransaction()
        {
            Data.RollbackTransaction();
        }

        #endregion

        #region ʵ��������

        /// <summary>
        /// SQL���ִ�нӿ�
        /// </summary>
        public IData IData
        {
            get { return Data.IData; }
        }

        #region Create

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <returns>Ӱ�������</returns>
        public int Create(Model model)
        {
            return Data.Create(model);
        }

        #endregion

        #region Retrieve

        /// <summary>
        /// ��ʵ�������ֵΪ������ȡ
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <returns>ʵ��</returns>
        public DataTable Retrieve(Model model)
        {
            return Data.Retrieve(model);
        }

        /// <summary>
        /// ��������ȡ
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <param name="pc"></param>
        /// <returns>ʵ��</returns>
        public DataTable RetrieveMultiple(Model model, ParameterCollection pc)
        {
            return Data.RetrieveMultiple(model, pc, null);
        }

        /// <summary>
        /// ��������ȡ
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <param name="pc"></param>
        /// <param name="obc"></param>
        /// <returns>ʵ��</returns>
        public DataTable RetrieveMultiple(Model model, ParameterCollection pc, OrderByCollection obc)
        {
            return Data.RetrieveMultiple(model, pc, obc);
        }

        #endregion

        #region Update

        /// <summary>
        /// ��ʵ�������ֵΪ��������
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <returns>Ӱ�������</returns>
        public int Update(Model model)
        {
            return Data.Update(model);
        }

        /// <summary>
        /// ��ʵ�������ֵΪ��������
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <param name="pc"></param>
        /// <returns>Ӱ�������</returns>
        public int UpdateMultiple(Model model, ParameterCollection pc)
        {
            return Data.UpdateMultiple(model, pc);
        }

        #endregion

        #region Delete

        /// <summary>
        /// ��ʵ�������ֵΪ����ɾ��
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <returns>Ӱ�������</returns>
        public int Delete(Model model)
        {
            return Data.Delete(model);
        }

        /// <summary>
        /// ������ɾ��
        /// </summary>
        /// <param name="model">ʵ��</param>
        /// <param name="pc"></param>
        /// <returns>Ӱ�������</returns>
        public int DeleteMultiple(Model model, ParameterCollection pc)
        {
            return Data.DeleteMultiple(model, pc);
        }

        #endregion

        #endregion

        #region ʵ����DataTable֮���ת������ʵ��

        /// <summary>
        /// ��Modelת����DataTable
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable ConvertTo(Model model)
        {
            return Data.ConvertTo(model);
        }

        /// <summary>
        /// ͨ���ֶ�����DataTable������ʵ��
        /// </summary>
        /// <param name="table">���ݱ�</param>
        public virtual void ConvertFrom(Model model, DataTable table)
        {
            Data.ConvertFrom(model, table);
        }

        /// <summary>
        /// ͨ���ֶ�����DataTable������ʵ��
        /// </summary>
        /// <param name="table">���ݱ�</param>
        /// <param name="rowIndex">������</param>
        public virtual void ConvertFrom(Model model, DataTable table, int rowIndex)
        {
            Data.ConvertFrom(model, table, rowIndex);
        }

        #endregion
    }
}
