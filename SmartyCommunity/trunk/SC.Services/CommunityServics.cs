using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using Evt.Framework.Common;
using SC.Common.DataAccess;
using System.Data;

namespace SC.Services
{
    public class CommunityServics : BaseService<CommunityServics>
    {
        public CommunityViewModel GetDetail(string communityId)
        {
            string sql = @"SELECT Id, Name,ServiceCategoryIds FROM dbo.Community WHERE ID=@ID";
            ParameterCollection pc = new ParameterCollection();
            pc.Add("ID", communityId);
            var dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql, pc);
            CommunityViewModel model = new CommunityViewModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    model.CommunityId = int.Parse(item["Id"].ToString());
                    model.CommunityName = item["Name"].ToString();
                    string[] serviceCategorys = item["ServiceCategoryIds"].ToString().Split(',');
                    model.ServiceCategory = GetServiceCategory(serviceCategorys);
                }
            }
            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<ServicecategoryDataModel> GetServiceCategory(string[] ids)
        {
            List<ServicecategoryDataModel> list = new List<ServicecategoryDataModel>();
            foreach (var item in ids)
            {
                var obj = CategoryService.Instance.GetAllCategory().Where<ServicecategoryDataModel>(p => p.Id == int.Parse(item)).FirstOrDefault();
                list.Add(obj);
            }
            return list;
        }


    }
}
