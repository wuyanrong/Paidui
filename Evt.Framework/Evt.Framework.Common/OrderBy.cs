using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 数据库查询结果集排序字段对象
    /// </summary>
    public class OrderBy
    {
        private string _fieldName;
        private SortTypeEnum _sortType = SortTypeEnum.Asc;

        /// <summary>
        /// 
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SortTypeEnum SortType
        {
            get { return _sortType; }
            set { _sortType = value; }
        }
    }
}
