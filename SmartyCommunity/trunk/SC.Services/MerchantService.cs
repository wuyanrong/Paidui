using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;

namespace SC.Services
{
    public class MerchantService:BaseService<MerchantService>
    {

        public void Add(AccountDataModel model)
        {
            string sql = "insert into cccx";
            //DbUtil.DataManager.Current.IData.
        }

        /// <summary>
        /// 获取特定小区，特定类别的所有商家
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="communityId"></param>
        /// <returns></returns>
        public List<MerchantDataModel> GetList(int categoryId, int communityId)
        {
            return null;
        }


        public MerchantDataModel GetDetail(int merchantId)
        {
            return null;
        }

        /// <summary>
        /// 根据用户Id,获取收藏的商家
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<MerchantDataModel> GetFavoriteMerchant(int accountId)
        {
            return null;
        }
    }
}
