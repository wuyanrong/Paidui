using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDR.Common.Entity
{
    public class MenuEntity
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuCategory { get; set; }
        public string MenuUntis { get; set; }
        public string MenuPrice { get; set; }
        public string MenuImage { get; set; }

        /// <summary>
        /// 售出的数量，吃过的人数
        /// </summary>
        public int SaleCount { get; set; }
    }
}
