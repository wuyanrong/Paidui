using System;
using System.Collections.Generic;
using System.Text;

using Evt.Framework.Common;

namespace Evt.Framework.DataAccess
{
	/// <summary>
	/// Data���󹤳�
	/// </summary>
    internal sealed class DataFactory
	{
        /// <summary>
        /// ��ȡʵ����Data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="impersonated"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        internal static AbstractData GetAbstractData(string connection, bool impersonated, DatabaseTypeEnum dbType)
        {
            AbstractData data = null;

            switch (dbType)
            {
                case DatabaseTypeEnum.SqlServer:
                    data = new SqlServerData(connection, impersonated);
                    break;

                case DatabaseTypeEnum.Access:
                    data = new AccessData(connection, impersonated);
                    break;

                case DatabaseTypeEnum.MySql:
                    data = new MySqlData(connection, impersonated);
                    break;

                case DatabaseTypeEnum.Oracle:
                    throw new DataAccessException("δʵ��Oracle...");
            }

            return data;
        }
	}
}
