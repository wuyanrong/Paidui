using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;
using Evt.Framework.Common;
using System.Data;
using System.Web;

namespace SC.Services
{
    public class CategoryService : BaseService<CategoryService>
    {

        public void Create(ServicecategoryDataModel model)
        {
            DbUtil.DataManager.Current.Create(model);
        }

        public List<ServicecategoryDataModel> GetCommunityCategory(string[] ids)
        {
            if (HttpRuntime.Cache["ServiceCategory"] == null)
            {
                string sql = @"SELECT * FROM dbo.ServiceCategory";
                DataTable dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql);
                List<ServicecategoryDataModel> list = new List<ServicecategoryDataModel>();
                foreach (DataRow item in dt.Rows)
                {
                    ServicecategoryDataModel obj = new ServicecategoryDataModel()
                    {
                        Id = int.Parse(item["ID"].ToString()),
                        Name = item["Name"].ToString(),
                        Iconpath = item["IconPath"].ToString(),
                        Createtime = DateTime.Parse(item["CreateTime"].ToString())
                    };
                    list.Add(obj);
                }
                HttpRuntime.Cache["ServiceCategory"] = list;
                return GetCategoryByIds(list, ids);
            }
            var objList = HttpRuntime.Cache["ServiceCategory"] as List<ServicecategoryDataModel>;
            return GetCategoryByIds(objList, ids);
        }

        #region Private

        private List<ServicecategoryDataModel> GetCategoryByIds(List<ServicecategoryDataModel> list, string[] ids)
        {
            List<ServicecategoryDataModel> resultList = new List<ServicecategoryDataModel>();
            foreach (var item in ids)
            {
                ServicecategoryDataModel model = list.Where<ServicecategoryDataModel>(p => p.Id == int.Parse(item)).FirstOrDefault();
                resultList.Add(model);
            }
            return resultList;
        }
        #endregion


    }
}
