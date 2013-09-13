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
    /// 实体基类
    /// </summary>
    [Serializable]
    public abstract class Model : IDeserializationCallback 
    {
        #region 缓存设置

        // 缓存元数据的开销
        private static Hashtable _types = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _prop = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _props = Hashtable.Synchronized(new Hashtable());
        private static Hashtable _tableAttrs = Hashtable.Synchronized(new Hashtable());

        private string _name = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Model()
        {
            CacheMetadata();
        }
        
        /// <summary>
        /// 获取缓存的Type实例
        /// </summary>
        /// <returns>实例</returns>
        internal Type GetCachedType()
        {
            return (Type)_types[this._name];
        }

        /// <summary>
        /// 获取缓存的属性信息
        /// </summary>
        /// <returns>属性数组</returns>
        internal PropertyInfo[] GetCachedPropertyInfo()
        {
            return (PropertyInfo[])_props[this._name];
        }

        /// <summary>
        /// 获取缓存的元数据
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>元数据</returns>
        internal TableMappingAttribute GetCachedTableMappingAttribute(string propertyName)
        {
            return (TableMappingAttribute)_tableAttrs[this._name + propertyName];
        }

        private void CacheMetadata()
        {
            this._name = this.ToString();

            // 判断是否被缓存
            if (!_types.ContainsKey(this._name))
            {
                Type t = this.GetType();
                PropertyInfo[] props = t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                TableMappingAttribute tattr = null;

                // 缓存Type实例
                _types[this._name] = t;

                // 缓存Type的GetProperties内容
                _props[this._name] = props;
                for (int i = 0; i < props.Length; i++)
                {
                    _prop[string.Concat(this._name, props[i].Name)] = props[i];
                }

                // 缓存类元数据
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

        #region 获取实体值的功能实现

        /// <summary>
        /// 获取指定属性名的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        public virtual object GetValue(string propertyName)
        {
            Type t = (Type)_types[this._name];
            object ret = t.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, this, null);
            return ret;
        }

        /// <summary>
        /// 设置指定属性名的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        public virtual void SetValue(string propertyName, object propertyValue)
        {
            Type t = (Type)_types[this._name];
            t.GetProperty(propertyName).SetValue(this, propertyValue, null);
            //t.InvokeMember(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, this, new object[] { propertyValue });
        }


        #endregion

        #region 实现与DataTable之间的转换功能实现

        /// <summary>
        /// 将实体转换成DataTable，DataTable中的列名称以实体的字段名为准
        /// </summary>
        /// <returns>数据表</returns>
        [Obsolete("请使用DataManager对象中的方法")]
        public virtual DataTable ConvertTo()
        {
            DataTable table = new DataTable();
            DataRow row = null;
            object ret = null;

            Type t = (Type)_types[this._name];
            PropertyInfo[] props = (PropertyInfo[])_props[this._name];

            // 创建表结构
            for (int i = 0; i < props.Length; i++)
            {
                table.Columns.Add(props[i].Name);
            }

            // 新建行
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
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        [Obsolete("请使用DataManager对象中的方法")]
        public virtual void ConvertFrom(DataTable table)
        {
            ConvertFrom(table, 0, true);
        }

        /// <summary>
        /// 通过字段名将DataTable解析成实体
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="rowIndex">数据行</param>
        [Obsolete("请使用DataManager对象中的方法")]
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

        #region IDeserializationCallback 成员

        public void OnDeserialization(object sender)
        {
            CacheMetadata();
        }

        #endregion
    }
}
