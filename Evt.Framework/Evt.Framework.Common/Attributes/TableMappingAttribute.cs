using System;

namespace Evt.Framework.Common
{
	/// <summary>
	/// ���ݱ�ӳ����
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
		/// ����
		/// </summary>
		public string TableName
		{
			get { return _tableName; }
			set { _tableName = value; }
		}

		/// <summary>
		/// �ֶ���
		/// </summary>
		public string FieldName
		{
			get { return _fieldName; }
			set { _fieldName = value; }
		}

        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

		/// <summary>
		/// �Ƿ�������
		/// </summary>
		public bool PrimaryKey
		{
			get { return _primaryKey; }
			set { _primaryKey = value; }
		}

        /// <summary>
        /// ����ʱ�Ƿ����
        /// </summary>
        public bool Insert
        {
            get { return _insert; }
            set { _insert = value; }
        }

        /// <summary>
        /// ����ʱ�Ƿ����
        /// </summary>
        public bool Update
        {
            get { return _update; }
            set { _update = value; }
        }

        #endregion
	}
}
