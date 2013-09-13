using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TDR.Common.Utils
{
    public class ConfigUtil
    {
        private static string GetValue(string key)
        {
            var obj = ConfigurationManager.AppSettings[key];
            return obj == null ? null : obj.ToString();
        }

        public static int MaxCityId
        {
            get { return int.Parse(GetValue("MaxCityId")); }
        }

        public static int MinCityId
        {
            get { return int.Parse(GetValue("MinCityId")); }
        }

        public static int PageNo
        {
            get { return int.Parse(GetValue("PageNo")); }
        }

        public static int PageSize
        {
            get { return int.Parse(GetValue("PageSize")); }
        }

        public static int RunTime
        {
            get { return int.Parse(GetValue("RumTime")); }
        }

        public static string UserId
        {
            get { return GetValue("UserId"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetAllMerchantUrl
        {
            get
            {
                if (string.IsNullOrEmpty(UserId) || PageSize == 0)
                {
                    throw new ArgumentNullException("请在配置文件中填写正确的UserId,PageSize节点值！");
                }
                return string.Format(GetValue("GetAllMerchantUrl"), "{0}", "{1}", PageSize, UserId);
            }
        }

        public static string GetMerchantAllMenusUrl
        {
            get
            {
                return GetValue("GetMerchantAllMenusUrl");
            }
        }

        public static string BeginMerchantName 
        {
            get {
                return GetValue("BeginMerchantName");
            }
        }
    }
}
