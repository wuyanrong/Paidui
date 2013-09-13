//  ----------------------------------------
//  版权信息：版权所有(C) 2012，PAIDUI.CN
//  变更历史：
//      姓名         日期          说明
//  ----------------------------------------
//     杨欣文     2012/03/31       创建
//  ----------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using Evt.Framework.Common;
using Evt.Framework.DataAccess;
using System.Web;

namespace SC.Common.DataAccess
{
    /// <summary>
    /// 数据库工具类。
    /// </summary>
    public class DataManagerCollection
    {
        /// <summary>
        /// _dataManagers
        /// </summary>
        private Dictionary<string, DataManager> _dataManagers = new Dictionary<string, DataManager>();

        /// <summary>
        /// DataManager集合对象
        /// </summary>
        public DataManagerCollection()
        {
            _dataManagers["SC"] = new SCDataManager();
        }

        /// <summary>
        /// 获取DataManager索引器
        /// </summary>
        /// <param name="company">索引键</param>
        /// <returns>DataManager对象</returns>
        public DataManager this[string company]
        {
            get
            {
                if (!_dataManagers.ContainsKey(company))
                {
                    throw new Exception("公司名称" + company + "不存在。");
                }

                return _dataManagers[company];
            }
        }

        /// <summary>
        /// 当前DataManager对象
        /// </summary>
        public DataManager Current
        {
            get
            {
                return _dataManagers["SC"];
            }
        }
    }
}
