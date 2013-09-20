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
            string sql = @"SELECT ID,  Name,ServiceCategoryIds FROM dbo.Community WHERE ID=@ID";
            ParameterCollection pc = new ParameterCollection();
            pc.Add("ID", communityId);
            var dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql, pc);
            CommunityViewModel model = new CommunityViewModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    model.CommunityId = int.Parse(item["ID"].ToString());
                    model.CommunityName = item["Name"].ToString();
                    model.ServiceCategory = CategoryService.Instance.GetCommunityCategory(item["ServiceCategoryIds"].ToString().Split(','));
                }
            }
            return model;
        }


    }
}
