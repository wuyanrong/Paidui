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
    /// 类型验证
    /// </summary>
    [Serializable]
    public class TypeAttribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TypeAttribute()
        {
        }

        public static ValidationResult ValidateType(object value)
        {
            bool isValid = false;

            switch (value.GetType().ToString())
            {
                case "int":
                    isValid = true;
                    break;
                case "Decimal":
                    isValid = true;
                    break;
                default:
                    break;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("类型错误");
            }
        }
    }
}
