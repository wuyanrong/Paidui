using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC.Models
{
    public class CommunityViewModel
    {
        public int CommunityId { get; set; }

        public string CommunityName { get; set; }

        /// <summary>
        /// 存放社区提供的
        /// </summary>
        public List<ServicecategoryDataModel> ServiceCategory { get; set; }
    }
}
