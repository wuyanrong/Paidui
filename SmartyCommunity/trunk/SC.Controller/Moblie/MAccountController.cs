using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SC.Common.Util;

namespace SC.Controllers
{
    public class MAccountController : Controller
    {
        private static ViewResultUitl _viewResult = new ViewResultUitl("Mobile", "Account");

        public ActionResult Add() //ViewModel
        {
            return _viewResult.View(this, "Main");
        }

        public ActionResult AddFavoriteMerchant(string name, string phone, string comment) //Entity
        {
            return null;

        }

        public ActionResult GetFavoriteMerchant(string accountId)
        {
            return null;
        }
    }
}
