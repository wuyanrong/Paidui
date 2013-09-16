using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;

namespace SC.Services
{
    public class CommunityServics
    {
        private static CommunityServics _instance = new CommunityServics();

        public static CommunityServics Instance 
        {
            get { return _instance; }
        }

        public CommunityDataModel GetDetail(string communityId)
        {
            string sql = @"";
        }
    }
}
