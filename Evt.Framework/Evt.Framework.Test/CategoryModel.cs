/*
版权信息：版权所有(C) 2009，ChuangNa Tech
作    者：钱明星
完成日期：2009-10-10
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace Evt.Framework.Test
{
	/// <summary>
	/// 实体类
	/// </summary>
	[Serializable]
    [TableMapping(TableName = "Category")]
    public partial class CategoryModel : Model
	{
        private int? _CategoryID = null;
        private string _CategoryName = null;
        private string _CategoryDesc = null;
        private bool? _Deleted = null;
        private DateTime? _createdOn = null;

        [TableMapping(FieldName = "CategoryID", PrimaryKey = true)]
        public int? CategoryID
		{
            get { return _CategoryID; }
            set { _CategoryID = value; }
		}

        [TableMapping(FieldName = "CategoryName")]
        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        [TableMapping(FieldName = "CategoryDesc")]
        public string CategoryDesc
        {
            get { return _CategoryDesc; }
            set { _CategoryDesc = value; }
        }

        [TableMapping(FieldName = "Deleted")]
        public bool? Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

        [TableMapping(FieldName = "CreatedOn")]
        public DateTime? CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
    }
}

