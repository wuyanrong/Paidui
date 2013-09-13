using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TDR.Common.Utils;
using Newtonsoft.Json.Linq;
using TDR.Common.Entity;

namespace TDR.Services
{
    public class MerchantService
    {
        private static MerchantService _instance = new MerchantService();

        public static MerchantService Instance
        {
            get
            {
                return _instance;
            }
        }

        public IList<MerchantEntity> GetAllMerchantByCityId(int cityId, int pageNo)
        {
            string requestUrl = string.Format(ConfigUtil.GetAllMerchantUrl, cityId, pageNo);
            string jsonResult = HttpUtil.Get(requestUrl);
            JObject obj = JObject.Parse(jsonResult);
            IList<MerchantEntity> merchantList = new List<MerchantEntity>();
            merchantList = (from p in obj["restaurantinfolist"]
                                                  select new MerchantEntity()
                                                      {
                                                          MerchantId = p["id"].ToString(),
                                                          MerchantName = p["ctname"].ToString(),
                                                          Template = p["template"].ToString()
                                                      }
            ).ToList<MerchantEntity>();
            return merchantList;
        }
    }
}
