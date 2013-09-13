using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using TDR.Common.Utils;
using Newtonsoft.Json.Linq;
using TDR.Common.Entity;

namespace TDR.Services
{
    public class MenuService
    {
        private static MenuService _instance = new MenuService();

        public static MenuService Instance
        {
            get
            {
                return _instance;
            }
        }

        public IList<MenuEntity> GetAllMenuByMerchantId(string merchantId, int cityId, int template)
        {
            var paramList = new WebHeaderCollection() 
            {
                {"city",cityId.ToString()},
                {"user",ConfigUtil.UserId},
            };
            Dictionary<string, string> dic = new Dictionary<string, string>() 
            {
                {"restaurant",merchantId},
                {"template",template.ToString()}
            };
            string jsonResult = HttpUtil.Post(ConfigUtil.GetMerchantAllMenusUrl, paramList, dic);
            JObject jObject = JObject.Parse(jsonResult);
            var categoryList = (from p in jObject["c"][1]["d"][0]["d"].Children() select p).ToDictionary(p => p["b"], a => a["c"]);
            IList<MenuEntity> menuList = new List<MenuEntity>();
            menuList = (from p in jObject["b"].Children()
                        select new MenuEntity()
                            {
                                MenuId = p["a"] == null ? string.Empty : p["a"].ToString(),
                                MenuName = p["b"] == null ? string.Empty : p["b"].ToString(),
                                MenuCategory = categoryList[int.Parse(p["w"].ToString())].Value<string>(),
                                MenuUntis = p["e"] == null ? string.Empty : p["e"].ToString(),
                                MenuPrice = p["c"] == null ? string.Empty : p["c"].ToString(),
                                MenuImage = p["g"] == null ? string.Empty : p["g"].ToString(),
                                SaleCount = int.Parse(p["q"].ToString())
                            }
                                          ).ToList<MenuEntity>();
            return menuList;
        }
    }
}
