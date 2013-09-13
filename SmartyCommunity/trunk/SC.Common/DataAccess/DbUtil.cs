
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC.Common.DataAccess
{
    /// <summary>
    /// Db Util
    /// </summary>
    public static class DbUtil
    {
        /// <summary>
        /// DataManagerCollection
        /// </summary>
        private static DataManagerCollection _dataManagers = new DataManagerCollection();

        /// <summary>
        /// DataManagerCollection对象
        /// </summary>
        public static DataManagerCollection DataManager
        {
            get
            {
                return _dataManagers;
            }
        }
    }
}
