/*
版权信息：版权所有(C) 2010，EVT Tech
作    者：钱明星
完成日期：2010-11-24
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace Evt.Framework.Test
{
	/// <summary>
	/// T2实体类
	/// </summary>
	[Serializable]
	[TableMapping(TableName="t2")]
	public partial class T2Model
	{
		private int? _idInt = null;
		private string _nameVarchar = null;
		private string _descVarchar = null;

		/// <summary>
		///  
		/// </summary>
		[TableMapping(FieldName="id_int",PrimaryKey=true)]
		public int? IdInt
		{
			get { return _idInt; }
			set { _idInt = value; }
		}

		/// <summary>
		///  
		/// </summary>
		[TableMapping(FieldName="name_varchar")]
		public string NameVarchar
		{
			get { return _nameVarchar; }
			set { _nameVarchar = value; }
		}

		/// <summary>
		///  
		/// </summary>
		[TableMapping(FieldName="desc_varchar", DefaultValue="NOW()")]
		public string DescVarchar
		{
			get { return _descVarchar; }
			set { _descVarchar = value; }
		}

	}
}

