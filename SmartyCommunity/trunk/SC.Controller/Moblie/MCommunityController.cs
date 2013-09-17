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
        private static ViewResultUitl viewResult = new ViewResultUitl("Mobile", "Account");

        public ActionResult Detail(string communityId)
        {
            CommunityDataModel model = CommunityServics.Instance.GetDetail(communityId);
            return viewResult.View(this,"Detail");
        }
    }
}
