//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/13       创建

using System;
using System.Configuration;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 公共配置工具类。
    /// </summary>
    public class ConfigUtil
    {
        /// <summary>
        /// 获取是否记录公司底层框架的日志信息。
        /// 在Web.config中配置“TraceFramework”AppSettings配置项
        /// 
        /// </summary>
        public static bool TraceFramework
        {
            get
            {
                bool traceFramework;
                if (!Boolean.TryParse(ConfigurationManager.AppSettings["TraceFramework"], out traceFramework))
                {
                    traceFramework = false;
                }

                return traceFramework;
            }
        }

        /// <summary>
        /// 获取跟踪日志是否只记录精简信息。
        /// 在Web.config中配置“TraceSimpleContent”AppSettings配置项
        /// 缺省值值为false
        /// </summary>
        public static bool TraceSimpleContent
        {
            get
            {
                bool traceSimpleContent;
                if (!Boolean.TryParse(ConfigurationManager.AppSettings["TraceSimpleContent"], out traceSimpleContent))
                {
                    traceSimpleContent = false;
                }

                return traceSimpleContent;
            }
        }

        /// <summary>
        /// 获取缓存过期时间（单位为秒）。
        /// 在Web.config中配置“CacheTimeout”AppSettings配置项
        /// 缺省值值为3秒
        /// </summary>
        public static int CacheTimeout
        {
            get
            {
                int cacheTimeout;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["CacheTimeout"], out cacheTimeout))
                {
                    cacheTimeout = 3;
                }

                return cacheTimeout;
            }
        }

        /// <summary>
        /// 缓存处理警告时长，Memcached Set/Get数据超过设定时长时记录警告信息（单位为秒）。
        /// 在Web.config中配置“WarnDuration”AppSettings配置项
        /// 缺省值值为2秒
        /// </summary>
        public static int WarnDuration
        {
            get
            {
                int warnDuration;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["WarnDuration"], out warnDuration))
                {
                    warnDuration = 2;
                }

                return warnDuration;
            }
        }

        /// <summary>
        /// 获取保存数据到Memcached的失败重试次数。
        /// 在Web.config中配置“MemcachedTryCount”AppSettings配置项
        /// 缺省值为3次。
        /// </summary>
        public static int MemcachedTryCount
        {
            get
            {
                int tryCount;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["MemcachedTryCount"], out tryCount))
                {
                    tryCount = 3;
                }

                return tryCount;
            }
        }

        /// <summary>
        /// 获取系统名称。
        /// 在Web.config中配置“SystemName”AppSettings配置项
        /// 缺省值为CRM
        /// </summary>
        public static string SystemName
        {
            get
            {
                string systemName = String.Empty;
                systemName = ConfigurationManager.AppSettings["SystemName"];

                if (string.IsNullOrEmpty(systemName))
                {
                    systemName = "CRM";
                }

                return systemName;
            }
        }

        /// <summary>
        /// 获取短信http请求地址信息。
        /// 在Web.config中配置“SMSHttpUrl”AppSettings配置项
        /// 缺省值为sms.paidui.cn/Home/SendSMS
        /// </summary>
        public static string SMSHttpUrl
        {
            get
            {
                string smsHttpUrl = string.Empty;
                smsHttpUrl = ConfigurationManager.AppSettings["SMSHttpUrl"];

                if (string.IsNullOrEmpty(smsHttpUrl))
                {
                    smsHttpUrl = "sms.paidui.cn/Home/SendSMS";
                }
                
                return smsHttpUrl;
            }
        }
    }
}
