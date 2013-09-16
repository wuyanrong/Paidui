using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SC.Common.Util;
using SC.Services;

namespace SC.Controllers
{
    public class PAccountController : Controllers
    {
        private static ViewResultUitl _viewResult = new ViewResultUitl("PC", "Account");
        public ActionResult Add(string name, string pwd)
        {
            //MerchantService.Instance.Add(AccountDataModel);
            //return _viewResult.View(this, "Main");
            return null;
        }

        public ActionResult DoAdd()
        {
            //MerchantService.Instance.Add(AccountDataModel);

            //return null;
            return null;
        }
    }
}
