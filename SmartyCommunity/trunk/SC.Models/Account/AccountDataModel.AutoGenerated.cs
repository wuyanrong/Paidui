//---------------------------------------------
// 版权信息：版权所有(C) 2013，EVT Tech
// 变更历史：
//      姓名          日期              说明
//---------------------------------------------
//                    2013/09/06        创建
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

//using SC.Model.Common;

namespace SC.Models
{
    /// <summary>
    /// Account实体类
    /// </summary>
    [Serializable]
    [TableMapping(TableName = "Account")]
    public class AccountDataModel : Model
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
        /// password
        /// </summary>
        private string _password = null;

        /// <summary>
        ///  ID 
        /// </summary>
        [TableMapping(FieldName = "ID")]
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
        ///  Password 
        /// </summary>
        [TableMapping(FieldName = "Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

    }
}

