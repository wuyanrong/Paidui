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
        public CommunityDataModel GetDetail(string communityId)
        {
            string sql = @"SELECT  Name,ServiceCategoryIds FROM dbo.Community WHERE ID=@ID";
            ParameterCollection pc = new ParameterCollection();
            pc.Add("ID", communityId);
            var dt = DbUtil.DataManager.Current.IData.ExecuteDataTable(sql, pc);
            CommunityDataModel model = new CommunityDataModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    model.Name = item["Name"].ToString();
                    model.Servicecategoryids = item["ServiceCategoryIds"].ToString();
                }
            }
            return model;
        }


    }
}
