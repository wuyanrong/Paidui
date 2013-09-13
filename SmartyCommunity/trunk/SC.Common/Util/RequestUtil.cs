using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Evt.Framework.Common;
using System.Data;


namespace SC.Common.Util
{
    /// <summary>
    /// Request请求
    /// </summary>
    public class RequestUtil
    {
        /// <summary>
        /// 取路由中的ID值
        /// </summary>
        public static string Id
        {
            get
            {
                string id = GetRouteData("id");
                return id == null ? string.Empty : id;
            }
        }

        /// <summary>
        /// 取路由中的pageIndex值
        /// </summary>
        public static int PageIndex
        {
            get
            {
                return ConvertUtil.ToInt(HttpContext.Current.Request.QueryString["PageIndex"]);
            }
        }
        //public static string PageIndex
        //{
        //    get
        //    {
        //        string id = GetRouteData("pageIndex");
        //        return id == null ? string.Empty : id;
        //    }
        //}

        /// <summary>
        /// 取路由中的pageSize值
        /// </summary>
        public static int PageSize
        {
            get
            {
                return ConvertUtil.ToInt(HttpContext.Current.Request.QueryString["PageSize"]);
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public static string SortField
        {
            get
            {
                return HttpContext.Current.Request.QueryString["SortField"];
            }
        }

        /// <summary>
        /// 升序/降序
        /// </summary>
        public static string SortOrder
        {
            get
            {
                return HttpContext.Current.Request.QueryString["SortOrder"];
            }
        }

        /// <summary>
        /// 取路由中的控制器名称
        /// </summary>
        public static string Controller
        {
            get
            {
                string controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as string;
                return controller == null ? string.Empty : controller;
            }
        }

        /// <summary>
        ///  取路由中的Action名称
        /// </summary>
        public static string Action
        {
            get
            {
                string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as string;
                return action == null ? string.Empty : action;
            }
        }

        public static bool IsAjaxIframe
        {
            get
            {
                return RequestUtil.GetUrlData("iframe") == "1";
            }
        }

        /// <summary>
        /// 取路由中参数值
        /// </summary>
        /// <param name="key">参数key</param>
        /// <returns>参数值</returns>
        public static string GetRouteData(string key)
        {
            string data = HttpContext.Current.Request.RequestContext.RouteData.Values[key] as string;
            return data;
        }

        /// <summary>
        /// 取URL地址中QueryString参数值
        /// </summary>
        /// <param name="key">参数key</param>
        /// <returns>参数值</returns>
        public static string GetUrlData(string key)
        {
            return HttpContext.Current.Request.QueryString[key];
        }

        /// <summary>
        /// 取URL地址中参数值
        /// </summary>
        /// <param name="key">参数key</param>
        /// <returns>参数值</returns>
        public static string GetFormData(string key)
        {
            return HttpContext.Current.Request.Form[key];
        }

    }
}
