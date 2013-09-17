using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;
using System.Data;
using Evt.Framework.Common;

namespace SC.Services
{
    public class AccountService:BaseService<AccountService>
    {

        public void Register(AccountDataModel model)
        {
            var obj = DbUtil.DataManager.Current.Retrieve(new AccountDataModel() { Email = model.Email });
            if (obj.Rows.Count > 0)
            {
                throw new MessageException("邮箱已经注册！");
            }
            this.Create(model);
        }

        public bool Login(AccountDataModel model)
        {
            var obj = DbUtil.DataManager.Current.Retrieve(new AccountDataModel() { Email = model.Email, Passsword = model.Passsword });
            return obj.Rows.Count > 0 ? true : false;
        }

        public void Create(AccountDataModel model)
        {
            DbUtil.DataManager.Current.Create(model);
        }

        public AccountDataModel GetModel(int accountId)
        {

            var obj = DbUtil.DataManager.Current.Retrieve(new AccountDataModel() { Id = accountId });
            AccountDataModel model = new AccountDataModel();
            foreach (DataRow item in obj.Rows)
            {
                model.Id = item.Field<int>("ID");
                model.Mobphone = item.Field<string>("MobPhone");
                model.NikeName = item.Field<string>("NikeName");
                model.Email = item.Field<string>("Email");
                model.Potopath = item.Field<string>("Potopath");
                model.Createtime = item.Field<DateTime>("CreateTime");
            }
            return model;
        }

        public void Update(AccountDataModel model)
        {
            DbUtil.DataManager.Current.Update(model);
        }
    }
}
