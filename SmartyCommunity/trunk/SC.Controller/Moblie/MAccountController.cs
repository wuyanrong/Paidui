using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SC.Common.Util;
using SC.Models;
using SC.Services;
using System.Web.Mvc;

namespace SC.Controllers
{
    public class MAccountController : Controller
    {
        private static ViewResultUitl _viewResult = new ViewResultUitl("Mobile", "Account");


        public ActionResult Register()
        {
            ViewBag.Account = new AccountDataModel();
            return _viewResult.View(this, "Register");
        }

        public ActionResult Login()
        {
            return _viewResult.View(this, "Login");
        }

        public ActionResult DoLogin(AccountDataModel model)
        {
            if (AccountService.Instance.Login(model))
            {
                return _viewResult.View(this, "Main");
            }
            return _viewResult.View(this, "Main");
        }

        public ActionResult LoginOut(string accountId)
        {
            System.Web.HttpContext.Current.Session[accountId] = string.Empty;
            return _viewResult.View(this,"Main");
        }

        public ActionResult Add()
        {
            ViewBag.Account = new AccountDataModel();
            return _viewResult.View(this, "Main");
        }

        public ActionResult DoAdd(AccountDataModel model)
        {
            model.Mobphone = "13008862853";
            model.Createtime = DateTime.Now;
            AccountService.Instance.Create(model);
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
