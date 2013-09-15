using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Models;
using SC.Common.DataAccess;
using Evt.Framework.Common;

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
            return null;

            //var obj = new ServicecategoryDataModel();
            //ParameterCollection pc = new ParameterCollection("@Id", obj.Id);
            //return DbUtil.DataManager.Current.RetrieveMultiple();
        }
    }
}
