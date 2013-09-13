using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using MySql.Data.Types;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
    /// <summary>
    /// ʵ�����
    /// </summary>
    [Serializable]
    public abstract class Model : IDeserializationCallback 
    {
        #region ��������

        // ����Ԫ���ݵĿ���
        private static Hashtable _types = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _prop = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _props = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _tableAttrs = Hashtable.Synchronized(new Hashtable());

        private string _name = null;

        /// <summary>
        /// ���캯��
        /// </summary>
        public Model()
        {
            CacheMetadata();
        }
        
        /// <summary>
        /// ��ȡ�����Typeʵ��
        /// </summary>
        /// <returns>ʵ��</returns>
        internal Type GetCachedType()
        {
            return (Type)_types[this._name];
        }

        /// <summary>
        /// ��ȡ�����������Ϣ
        /// </summary>
        /// <returns>��������</returns>
        internal PropertyInfo[] GetCachedPropertyInfo()
        {
            return (PropertyInfo[])_props[this._name];
        }

        /// <summary>
        /// ��ȡ�����Ԫ����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns>Ԫ����</returns>
        internal TableMappingAttribute GetCachedTableMappingAttribute(string propertyName)
        {
            return (TableMappingAttribute)_tableAttrs[this._name + propertyName];
        }

        private void CacheMetadata()
        {
            this._name = this.ToString();

            // �ж��Ƿ񱻻���
            if (!_types.ContainsKey(this._name))
            {
                Type t = this.GetType();
                PropertyInfo[] props = t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                TableMappingAttribute tattr = null;

                // ����Typeʵ��
                _types[this._name] = t;

                // ����Type��GetProperties����
                _props[this._name] = props;
                for (int i = 0; i < props.Length; i++)
                {
                    _prop[string.Concat(this._name, props[i].Name)] = props[i];
                }

                // ������Ԫ����
                tattr = (TableMappingAttribute)Attribute.GetCustomAttribute(t, typeof(TableMappingAttribute));
                _tableAttrs[this._name] = tattr;
                for (int i = 0; i < props.Length; i++)
                {
                    tattr = (TableMappingAttribute)Attribute.GetCustomAttribute(props[i], typeof(TableMappingAttribute));
                    _tableAttrs[string.Concat(this._name, props[i].Name)] = tattr;
                }
            }
        }

        #endregion

        #region ��ȡʵ��ֵ�Ĺ���ʵ��

        /// <summary>
        /// ��ȡָ����������ֵ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns>����ֵ</returns>
        public virtual object GetValue(string propertyName)
        {
            Type t = (Type)_types[this._name];
            object ret = t.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, this, null);
            return ret;
        }

        /// <summary>
        /// ����ָ����������ֵ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="propertyValue">����ֵ</param>
        public virtual void SetValue(string propertyName, object propertyValue)
        {
            Type t = (Type)_types[this._name];
            t.GetProperty(propertyName).SetValue(this, propertyValue, null);
            //t.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, this, new object[] { propertyValue });
        }


        #endregion

        #region ʵ����DataTable֮���ת������ʵ��

        /// <summary>
        /// ��ʵ��ת����DataTable��DataTable�е���������ʵ����ֶ���Ϊ׼
        /// </summary>
        /// <returns>���ݱ�</returns>
        [Obsolete("��ʹ��DataManager�����еķ���")]
        public virtual DataTable ConvertTo()
        {
            DataTable table = new DataTable();
            DataRow row = null;
            object ret = null;

            Type t = (Type)_types[this._name];
            PropertyInfo[] props = (PropertyInfo[])_props[this._name];

            // ������ṹ
            for (int i = 0; i < props.Length; i++)
            {
                table.Columns.Add(props[i].Name);
            }

            // �½���
            row = table.NewRow();
            for (int i = 0; i < props.Length; i++)
            {
                ret = t.InvokeMember(props[i].Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, this, null);
                if (ret != null)
                {
                    row[i] = ret;
                }
            }
            table.Rows.Add(row);

            return table;
        }

        /// <summary>
        /// ͨ���ֶ�����DataTable������ʵ��
        /// </summary>
        /// <param name="table">���ݱ�</param>
        [Obsolete("��ʹ��DataManager�����еķ���")]
        public virtual void ConvertFrom(DataTable table)
        {
            ConvertFrom(table, 0, true);
        }

        /// <summary>
        /// ͨ���ֶ�����DataTable������ʵ��
        /// </summary>
        /// <param name="table">���ݱ�</param>
        /// <param name="rowIndex">������</param>
        [Obsolete("��ʹ��DataManager�����еķ���")]
        public virtual void ConvertFrom(DataTable table, int rowIndex)
        {
            ConvertFrom(table, rowIndex, true);
        }

        private void ConvertFrom(DataTable table, int rowIndex, bool byProperty)
        {
            if (table == null || table.Rows.Count == 0 || table.Rows.Count < rowIndex)
            {
                return;
            }

            object ret = null;

            Type t = (Type)_types[this._name];
            PropertyInfo[] props = (PropertyInfo[])_props[this._name];

            for (int i = 0; i < props.Length; i++)
            {
                TableMappingAttribute attr = (TableMappingAttribute)_tableAttrs[this._name + props[i].Name];
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

                    if (props[i].PropertyType == typeof(bool?))
                    {
                        ret = Convert.ToBoolean(ret);
                    }

                    t.InvokeMember(props[i].Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, this, new object[] { ret });
                }
            }
        }

        #endregion

        #region IDeserializationCallback ��Ա

        public void OnDeserialization(object sender)
        {
            CacheMetadata();
        }

        #endregion
    }
}
