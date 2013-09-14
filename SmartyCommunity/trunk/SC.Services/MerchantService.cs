using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;

namespace SC.Services
{
    public class MerchantService
    {
        private static MerchantService _instance = new MerchantService();

        public static MerchantService Instance
        {
            get { return _instance; }
        }

        public void Add(AccountDataModel model)
        {
            string sql = "insert into cccx";
            //DbUtil.DataManager.Current.IData.
        }
    }
}
