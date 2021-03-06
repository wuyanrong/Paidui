//---------------------------------------------
// 版权信息：版权所有(C) 2013，EVT Tech
// 变更历史：
//      姓名          日期              说明
//---------------------------------------------
//                    2013/09/15        创建
//---------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

using Evt.Framework.Common;
using Evt.Framework.DataAccess;
using Evt.Framework.Mvc;



namespace SC.Models
{
	/// <summary>
	/// Servicecategory实体类
	/// </summary>
	[Serializable]
	[TableMapping(TableName = "ServiceCategory")]
	public class ServicecategoryDataModel : Model
	{
		/// <summary>
		/// iD
		/// </summary>
		private int? _iD = null;

		/// <summary>
		/// name
		/// </summary>
		private string _name = null;

		/// <summary>
		/// iconPath
		/// </summary>
		private string _iconPath = null;

		/// <summary>
		/// createTime
		/// </summary>
		private DateTime? _createTime = null;

		/// <summary>
		///  (主键) ID 
		/// </summary>
		[TableMapping(FieldName = "ID", PrimaryKey = true)]
		public int? Id
		{
			get { return _iD; }
			set { _iD = value; }
		}

		/// <summary>
		///  Name 
		/// </summary>
		[TableMapping(FieldName = "Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///  IconPath 
		/// </summary>
		[TableMapping(FieldName = "IconPath")]
		public string Iconpath
		{
			get { return _iconPath; }
			set { _iconPath = value; }
		}

		/// <summary>
		///  CreateTime 
		/// </summary>
		[TableMapping(FieldName = "CreateTime")]
		public DateTime? Createtime
		{
			get { return _createTime; }
			set { _createTime = value; }
		}

	}
}

