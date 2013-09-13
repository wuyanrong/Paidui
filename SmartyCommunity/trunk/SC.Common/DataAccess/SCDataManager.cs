
using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace SC.Common.DataAccess
{
    /// <summary>
    /// SC数据库管理器
    /// </summary>
    public class SCDataManager : DataManager
    {
        /// <summary>
        /// 数据连接字符串
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                return SC.Common.Util.ConfigUtil.SCConnectionStr;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        protected override DatabaseTypeEnum DatabaseType
        {
            get
            {
                return DatabaseTypeEnum.SqlServer;
            }
        }

        protected override string ContextID
        {
            get
            {
                return "19409EA8-136E-469D-B29C-87BEE5EE38C1";
            }
        }
    }
}
