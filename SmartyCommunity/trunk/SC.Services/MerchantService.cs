using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC.Services
{
    public class MerchantService
    {
        private static MerchantService _instance = new MerchantService();

        public static MerchantService Instance
        {
            get { return _instance; }
        }
    }
}
