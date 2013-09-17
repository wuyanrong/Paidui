using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;
using Evt.Framework.Common;
using System.Web;
using System.Data;

namespace SC.Services
{
    public class CategoryService
    {
        private static CategoryService _instance = new CategoryService();

        public static CategoryService Instance
        {
            get { return _instance; }
        }

        public void Create(ServicecategoryDataModel model)
        {
            DbUtil.DataManager.Current.Create(model);
        }

        public List<ServicecategoryDataModel> GetAllCategory()
        {
            string sql = @"select * from dbo.ServiceCategory ";
            if (HttpRuntime.Cache["category"] != null)
            {
                return HttpRuntime.Cache["category"] as List<ServicecategoryDataModel>;
            }
            var dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql);
            List<ServicecategoryDataModel> list = new List<ServicecategoryDataModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ServicecategoryDataModel model = new ServicecategoryDataModel()
                    {
                        Id = int.Parse(item["Id"].ToString()),
                        Name = item["Name"].ToString(),
                        Iconpath = item["IconPath"].ToString(),
                        Createtime = DateTime.Parse(item["CreateTime"].ToString())
                    };
                    list.Add(model);
                }
                HttpRuntime.Cache["category"] = list;
            }
            return list;

        }

        public Dictionary<string, string> GetCategoryDetail(int categoryId)
        {
            string sql = @"select Name,IconPath from dbo.ServiceCategory where id=@id";
            ParameterCollection pc = new ParameterCollection();
            pc.Add("id", categoryId);
            var table = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql, pc);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (table.Rows.Count > 0)
            {
                dic.Add(table.Rows[0]["Name"].ToString(), table.Rows[0]["IconPath"].ToString());
            }
            return dic;
        }
    }
}
