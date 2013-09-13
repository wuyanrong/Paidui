using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace Evt.Framework.Test
{
    /// <summary>
    /// 数据管理类
    /// </summary>
    public class AccessDataManager : DataManager
    {
        public  override string ConnectionString
        {
            get 
            {
                string conn = "";
                conn += "Provider=Microsoft.Jet.OLEDB.4.0;";
                conn += "Data source=" + @"F:\CN\SVN\Common\Frameworks\CSharp\Evt.Framework.V2\Evt.Framework.Test\Test.mdb" + ";";
                conn += "User ID=" + "Admin" + ";";
                conn += "Password=" + "" + ";";
                //return conn;
                return "server=localhost;uid=root;pwd=vpc;DataBase=qianmx_test;charset=utf8;allow zero datetime=true";
            }
        }

        protected override DatabaseTypeEnum DatabaseType
        {
            get
            {
                //return DatabaseTypeEnum.Access;
                return DatabaseTypeEnum.MySql;
            }
        }
    }

    /// <summary>
    /// 封装数据管理类为单态模式（WinningPostJockeyDataDataManager）
    /// </summary>
    public class DbUtil
    {
        private static AccessDataManager _adm = new AccessDataManager();

        /// <summary>
        /// 
        /// </summary>
        public static AccessDataManager DataManager
        {
            get { return _adm; }
        }
    }
}
