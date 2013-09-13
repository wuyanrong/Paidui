using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace SC.Common.Util
{
    /// <summary>
    /// MVC 视图操作类
    /// </summary>
    public class ViewResultUitl
    {
        /// <summary>
        /// 构造视图操作类
        /// </summary>
        /// <param name="viewSubFolder">子文件夹，从Views的下级文件夹开始的所有层级文件夹名称</param>
        public ViewResultUitl(params string[] viewSubFolder)
        {
            if (viewSubFolder.Length == 0)
            {
                ViewPath = "~/Views/";
            }
            else
            {
                ViewPath = string.Format("~/Views/{0}/", string.Join("/", viewSubFolder));
            }
        }

        /// <summary>
        /// 文件夹路径，如：~/Views/PHB/BusinessCooperation/List.cshtml
        /// </summary>
        public string ViewPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建视图结果
        /// </summary>
        /// <param name="c">控制器</param>
        /// <returns>视图结果</returns>
        public ActionResult View(Controller c)
        {
            return View(c, null);
        }

        /// <summary>
        /// 创建视图结果
        /// </summary>
        /// <param name="c">控制器</param>
        /// <param name="model">数据模型</param>
        /// <returns>视图结果</returns>
        public ActionResult View(Controller c, object model)
        {
            RouteValueDictionary routeValues = c.RouteData.Values;
            string action = routeValues["action"] as string;
            string path = string.Format("{0}{1}.cshtml", ViewPath, action);

            if (model != null)
            {
                c.ViewData.Model = model;
            }

            ViewResult result = new ViewResult();
            result.ViewName = path;
            result.ViewData = c.ViewData;
            result.TempData = c.TempData;

            return result;
        }

        /// <summary>
        /// 创建视图结果
        /// </summary>
        /// <param name="c">控制器</param>
        /// <param name="viewName">视图名称</param>
        /// <returns>视图结果</returns>
        public ActionResult View(Controller c, string viewName)
        {
            return View(c, viewName, null);
        }

        /// <summary>
        /// 创建视图结果
        /// </summary>
        /// <param name="c">控制器</param>
        /// <param name="viewName">视图名称</param>
        /// <param name="model">数据模型</param>
        /// <returns>视图结果</returns>
        public ActionResult View(Controller c, string viewName, object model)
        {
            RouteValueDictionary routeValues = c.RouteData.Values;
            string action = string.IsNullOrEmpty(viewName) ? routeValues["action"] as string : viewName;
            string path = string.Format("{0}{1}.cshtml", ViewPath, action);

            if (model != null)
            {
                c.ViewData.Model = model;
            }

            ViewResult result = new ViewResult();
            result.ViewName = path;
            result.ViewData = c.ViewData;
            result.TempData = c.TempData;

            return result;
        }
    }
}
