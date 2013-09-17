using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Evt.Framework.Common;
using SC.Services;
using SC.Models;
using SC.Common.Util;

namespace SC.Controllers.Moblie
{
    public class MCommunityController : Controller
    {
        private static ViewResultUitl viewResult = new ViewResultUitl("Mobile", "Community");

        public ActionResult Detail(string communityId)
        {
            CommunityViewModel model = CommunityServics.Instance.GetDetail(communityId);
            return viewResult.View(this, "Detail", model);
        }
    }
}
