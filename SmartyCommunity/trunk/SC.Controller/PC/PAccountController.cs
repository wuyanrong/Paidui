using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SC.Common.Util;

namespace SC.Controllers
{
    public class PAccountController : Controller
    {
        private static ViewResultUitl _viewResult = new ViewResultUitl("PC", "Account");
        public ActionResult Add()
        {
            return _viewResult.View(this, "Main");
        }

        public ActionResult DoAdd()
        {
            return null;
        }
    }
}
