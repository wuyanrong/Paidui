using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDR.Common.Utils;
using System.Data;
using System.Data.OleDb;
using TDR.Common.Entity;
using System.Threading;

namespace TDR.Services
{
    public class RobotService
    {
        private static RobotService _instance = new RobotService();

        public static RobotService Instance
        {
            get
            {
                return _instance;
            }
        }

        //public void DoWork()
        //{
        //    try
        //    {
        //        int cityId = ConfigUtil.MinCityId;
        //        int maxCityId = ConfigUtil.MaxCityId;
        //        string workBookName = DateTime.Now.ToString("yyyyMMdd");
        //        for (; cityId <= maxCityId; cityId++) //遍历所有城市
        //        {
        //            LogUtil.Instance.Info(cityId.ToString() + "号城市", "正在抓取城市编号为" + cityId.ToString() + "的数据....");
        //            int pageNo = 1; //初始化页码
        //            bool isComplete = false; //是否遍历完这个城市的所有商家
        //            workBookName = cityId.ToString();
        //            while (!isComplete)
        //            {
        //                IList<MerchantEntity> merchantList = MerchantService.Instance.GetAllMerchantByCityId(cityId, pageNo);
        //                if (merchantList.Count > 0)
        //                {
        //                    LogUtil.Instance.Info("    开始抓取菜单数据", "正在抓取城市编号为" + cityId.ToString() + "的商家菜单数据....");
        //                    foreach (MerchantEntity merchant in merchantList) //遍历每个商家
        //                    {
        //                        CreateWorkSheet(workBookName, merchant.MerchantName);
        //                        IList<MenuEntity> menuList = MenuService.Instance.GetAllMenuByMerchantId(merchant.MerchantId, cityId, int.Parse(merchant.Template));
        //                        isComplete = menuList.Count == 0 ? true : false;
        //                        AddMerchantMenu(menuList, workBookName, merchant.MerchantName); //添加菜单
        //                    }
        //                    LogUtil.Instance.Info("    完成抓取菜单数据", "完成抓取城市编号为" + cityId.ToString() + "的商家菜单数据！");
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //                pageNo = pageNo + 1;
        //                Thread.Sleep(1000 * 3);
        //            }
        //            LogUtil.Instance.Info(cityId.ToString() + "号城市", "完成抓取城市编号为" + cityId.ToString() + "的数据！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.Instance.Error(typeof(RobotService).ToString(), ex);
        //    }
        //}

        public void DoWork()
        {
            try
            {
                int cityId = ConfigUtil.MinCityId;
                int maxCityId = ConfigUtil.MaxCityId;
                string workBookName = DateTime.Now.ToString("yyyyMMdd");
                string begintMerchantName = ConfigUtil.BeginMerchantName;
                bool isContinue = true;
                for (; cityId <= maxCityId; cityId++) //遍历所有城市
                {
                    LogUtil.Instance.Info(cityId.ToString() + "号城市", "正在抓取城市编号为" + cityId.ToString() + "的数据....");
                    //int pageNo = 1; //初始化页码
                    int pageNo = ConfigUtil.PageNo;
                    bool isComplete = false; //是否遍历完这个城市的所有商家
                    workBookName = cityId.ToString();
                    //while (!isComplete)
                    //{
                    IList<MerchantEntity> merchantList = MerchantService.Instance.GetAllMerchantByCityId(cityId, pageNo);
                    if (merchantList.Count > 0)
                    {
                        LogUtil.Instance.Info("    开始抓取菜单数据", "正在抓取城市编号为" + cityId.ToString() + "的商家菜单数据....编号为：" + pageNo);
                        foreach (MerchantEntity merchant in merchantList) //遍历每个商家
                        {
                            if (!string.IsNullOrEmpty(begintMerchantName)) //是否从中间的一个商家开始
                            {
                                if (merchant.MerchantName != begintMerchantName && isContinue)
                                {
                                    continue;
                                }
                                isContinue = false;
                            }
                            CreateWorkSheet(merchant.MerchantName, merchant.MerchantName);
                            IList<MenuEntity> menuList = MenuService.Instance.GetAllMenuByMerchantId(merchant.MerchantId, cityId, int.Parse(merchant.Template));
                            isComplete = menuList.Count == 0 ? true : false;
                            AddMerchantMenu(menuList, merchant.MerchantName, merchant.MerchantName); //添加菜单
                        }
                        LogUtil.Instance.Info("    完成抓取菜单数据", "完成抓取城市编号为" + cityId.ToString() + "的商家菜单数据！");
                    }
                    else
                    {
                        break;
                    }
                    pageNo = pageNo + 1;
                    Thread.Sleep(1000 * 3);
                    // }
                    LogUtil.Instance.Info(cityId.ToString() + "号城市", "完成抓取城市编号为" + cityId.ToString() + "的数据！");
                }
            }
            catch (Exception ex)
            {
                LogUtil.Instance.Error(typeof(RobotService).ToString(), ex);
            }
        }


        #region Private

        /// <summary>
        /// 创建工作表
        /// </summary>
        /// <param name="workBookName">工作薄的名称</param>
        /// <param name="sheetName">工作表的名称</param>
        private void CreateWorkSheet(string workBookName, string sheetName)
        {
            Dictionary<string, OleDbType> sheetColmus = new Dictionary<string, OleDbType>()
            {
                {WorkSheetDataColumns.MenuName,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuId,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuParentCategory,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuSubCategory,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuDefaultPrice,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuUnits,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuDiscount,OleDbType.VarChar},
                {WorkSheetDataColumns.MenuCookieTime,OleDbType.VarChar},
                {WorkSheetDataColumns.OnTimePrice,OleDbType.VarChar},
                {WorkSheetDataColumns.SpecialDish,OleDbType.VarChar},
                {WorkSheetDataColumns.ConfirmWeight,OleDbType.VarChar},
                {WorkSheetDataColumns.SinglePoint,OleDbType.VarChar},
                {WorkSheetDataColumns.Commission,OleDbType.VarChar},
                {WorkSheetDataColumns.CommissionAmount,OleDbType.VarChar},
             };
            ExcelUtil.CreateWorkBook(workBookName, sheetName, sheetColmus);

            //ExcelUtil.AddWorkSheet(workBookName, sheetName, sheetColmus); //创建sheet(Table)
        }

        /// <summary>
        /// 添加商家菜单
        /// </summary>
        /// <param name="menuList">菜单列表</param>
        /// <param name="workBookName">工作薄名称</param>
        /// <param name="sheetName">工作表名称</param>
        private void AddMerchantMenu(IList<MenuEntity> menuList, string workBookName, string sheetName)
        {
            if (menuList.Count > 0)
            {
                foreach (MenuEntity menu in menuList)  //遍历商家的菜单
                {
                    Dictionary<string, string> menuDetial = new Dictionary<string, string>() 
                                    {
                                        {WorkSheetDataColumns.MenuName,menu.MenuName},
                                        {WorkSheetDataColumns.MenuSubCategory,menu.MenuCategory},
                                        {WorkSheetDataColumns.MenuDefaultPrice,menu.MenuPrice},
                                        {WorkSheetDataColumns.MenuUnits,menu.MenuUntis}
                                    };
                    ExcelUtil.AddWorkSheetRows(workBookName, sheetName, menuDetial); //往Excel插入数据
                }
            }
        }

        #endregion
    }
}
