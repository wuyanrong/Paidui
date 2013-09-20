using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SC.Common.Util
{
    public class SessionUtil
    {
        private static SessionUtil _instance = new SessionUtil();

        public static SessionUtil Instance
        {
            get { return _instance; }
        }

        public static int AccountId
        {
            get
            {
                var obj = System.Web.HttpContext.Current.Session["AccountId"];
                return obj == null ? 0 : int.Parse(obj.ToString());
            }
        }
    }
}
