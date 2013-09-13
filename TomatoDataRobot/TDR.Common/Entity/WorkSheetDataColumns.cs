using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDR.Common.Entity
{
    public class WorkSheetDataColumns
    {
        public static  string MenuName = "菜品名称(*)";

        public static  string MenuId = "菜品编号(*)";

        public static  string MenuParentCategory = "菜品大类(*)";

        public static  string MenuSubCategory = "菜品小类(*)";

        public static  string MenuDefaultPrice = "菜品默认价格(*)";

        /// <summary>
        /// 菜品单位
        /// </summary>
        public static  string MenuUnits = "菜品单位";

        public static  string MenuDiscount = "菜品折扣(98折则输入:98,不填则默认100)";

        public static  string MenuCookieTime = "上菜耗时(分钟)";

        /// <summary>
        /// 时价菜品
        /// </summary>
        public static  string OnTimePrice = "时价菜品";

        /// <summary>
        /// 特色菜
        /// </summary>
        public static  string  SpecialDish = "特色菜";

        public static  string ConfirmWeight = "称重确认";

        public static  string SinglePoint = "单点";

        /// <summary>
        /// 提成类型
        /// </summary>
        public static  string Commission = "提成类型";

        /// <summary>
        /// 提成额度(保留两位小数,比例提成则输入数字,5%则输入5)
        /// </summary>
        public static  string CommissionAmount = "提成额度(保留两位小数,比例提成则输入数字,5%则输入5)";



    }
}
