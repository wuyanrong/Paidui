//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/21       创建

using System;
using System.Text;
using PostSharp.Aspects;
using System.Reflection;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 缓存开启标签
    /// </summary>
    /// <example>
    /// <code>
    /// [CacheEnable(CacheTimeout=CacheTimeoutEnum.FiveMinutes, CacheType=CacheTypeEnum.WebCache)] 
    /// public DataTable getUserInfo(……) 
    /// {
    ///     ……
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public class CacheEnableAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public CacheTimeoutEnum CacheTimeout { get; set; }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheTypeEnum CacheType { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public CacheEnableAttribute()
        {
        }

        /// <summary>
        /// 方法执行前Aspect
        /// </summary>
        /// <param name="args"></param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            string cacheKey = this.CreateCacheKey(args);

            object value = null;
            value = CacheUtil.Get(cacheKey);

            if (value != null)
            {
                args.ReturnValue = value;
                args.FlowBehavior = FlowBehavior.Return;
            }
            else
            {
                args.MethodExecutionTag = cacheKey;
            }
        }

        /// <summary>
        /// 方法执行成功后Aspect
        /// </summary>
        /// <param name="args"></param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            string cacheKey = (string)args.MethodExecutionTag;
            CacheUtil.Set(cacheKey, args.ReturnValue);
        }

        /// <summary>
        /// 生成符合CacheUtil规则的KEY
        /// </summary>
        /// <param name="args"></param>
        /// <returns>KEY</returns>
        private string CreateCacheKey(MethodExecutionArgs args)
        {
            MethodBase method = args.Method;

            StringBuilder sb = new StringBuilder();
            if (method.DeclaringType != null)
            {
                sb.Append(method.DeclaringType.FullName);
            }
            sb.Append(':');
            sb.Append(method.Name);
            sb.Append(':');
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                sb.Append(args.Arguments.GetArgument(i) ?? "null");
                sb.Append(";");
            }

            StringBuilder key = new StringBuilder();
            key.Append(ConfigUtil.SystemName);
            key.Append('_');
            key.Append(SecurityUtil.GetMD5Hash(sb.ToString()));
            if (this.CacheType == CacheTypeEnum.WebCache)
            {
                key.Append("_n_");
            }
            else if (this.CacheType == CacheTypeEnum.Memcached)
            {
                key.Append("_m_");
            }
            else if (this.CacheType == CacheTypeEnum.Both)
            {
                key.Append("_nm_");
            }

            key.Append((int)this.CacheTimeout);

            return key.ToString();
        }
    }
}
