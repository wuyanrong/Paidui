using System;

namespace Evt.Framework.Common
{
	/// <summary>
	/// 数据表映射类
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.All)]
    public class TableMappingAttribute : System.Attribute
	{
		#region Properties

		private string _tableName = null;
		private string _fieldName = null;
        private string _defaultValue = null;
		private bool _primaryKey = false;
        private bool _insert = true;
        private bool _update = true;

		/// <summary>
		/// 表名
		/// </summary>
		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}

		/// <summary>
		/// 字段名
		/// </summary>
		public string FieldName
		{
			get { return _fieldName; }
			set { _fieldName = value; }
		}

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

		/// <summary>
		/// 是否是主键
		/// </summary>
		public bool PrimaryKey
		{
			get { return _primaryKey; }
			set { _primaryKey = value; }
		}

        /// <summary>
        /// 插入时是否包括
        /// </summary>
        public bool Insert
        {
            get { return _insert; }
            set { _insert = value; }
        }

        /// <summary>
        /// 更新时是否包括
        /// </summary>
        public bool Update
        {
            get { return _update; }
            set { _update = value; }
        }

        #endregion
	}
}
