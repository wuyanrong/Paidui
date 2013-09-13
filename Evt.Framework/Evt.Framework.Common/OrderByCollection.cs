using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 数据库查询结果集排序字段对象集合
    /// </summary>
    public class OrderByCollection : List<OrderBy>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        public void Add(string fieldName, SortTypeEnum sortType)
        {
            OrderBy ob = new OrderBy();
            ob.FieldName = fieldName;
            ob.SortType = sortType;
            this.Add(ob);
        }
    }
}
