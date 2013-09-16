using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC.Models.ViewModel
{
    public class CommunityViewModel
    {
        public int CommunityId { get; set; }

        public int CommunityName { get; set; }

        /// <summary>
        /// 存放社区提供的
        /// </summary>
        public Tuple<int, string, string> ServiceCategory { get; set; }
    }
}
