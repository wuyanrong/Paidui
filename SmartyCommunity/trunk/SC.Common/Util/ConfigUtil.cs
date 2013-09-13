using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SC.Common.Util
{

    public class ConfigUtil
    {
        private static T GetValue<T>(string key, T defaultValue) where T : class
        {
            var obj = ConfigurationManager.AppSettings[key];
            return obj == null ? defaultValue : (obj as T);
        }

        public static string SCConnectionStr
        {
            get { return GetValue<string>("SC_DB", null); }
        }
    }
}
