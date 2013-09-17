using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC.Common.Util;

namespace SC.Controllers
{
    public class BaseController<TParentDirector, TSubDirector>
        where TParentDirector : class
        where TSubDirector : class
    {
        private static ViewResultUitl viewResult = new ViewResultUitl(TParentDirector.toString(), TSubDirector.toString());
    }
}
