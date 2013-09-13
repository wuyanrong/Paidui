//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/19       创建

using System;
using System.ComponentModel.DataAnnotations;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Decimal类型验证
    /// </summary>
    /// <example>
    /// <code>
    /// /// 编辑菜品ViewModel对象
    /// public class EditDisheModel : ViewModel
    /// {
    ///     /// 营业时间
    ///     [DateTimeType(ErrorMessage = "营业时间必须是时间类型")]
    ///     public string OpenTime { get; set; }
    /// 
    ///     /// 排序值
    ///     [Required(ErrorMessage = "排序值不能为空")]
    ///     [IntType(ErrorMessage="排序值必须是数字")]
    ///     public string OrderId { get; set; }
    /// 
    ///     /// 菜品金额
    ///     [Required(ErrorMessage = "金额值不能为空")]
    ///     [DecimalType(ErrorMessage = "金额值必须是数字")]
    ///     public string Price { get; set; }
    /// }
    /// </code>
    /// </example>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DecimalTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        public DecimalTypeAttribute()
        {
        }

        /// <summary>
        /// 确定指定的对象是否合法有效
        /// </summary>
        /// <param name="value">待验证对象</param>
        /// <returns>合法有效，则为true；否则为false</returns>
        public override bool IsValid(object value)
        {
            Decimal i;
            return Decimal.TryParse(value.ToString(), out i);
        }
    }
}
