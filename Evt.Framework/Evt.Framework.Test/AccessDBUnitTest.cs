//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using Evt.Framework.Common;
//using Evt.Framework.DataAccess;

//namespace Evt.Framework.Test
//{
//    /// <summary>
//    /// AccessDBUnitTest 的摘要说明
//    /// </summary>
//    [TestClass]
//    public class AccessDBUnitTest
//    {
//        public AccessDBUnitTest()
//        {
//            //
//            //TODO: 在此处添加构造函数逻辑
//            //
//        }

//        private TestContext testContextInstance;

//        /// <summary>
//        ///获取或设置测试上下文，该上下文提供
//        ///有关当前测试运行及其功能的信息。
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region 附加测试属性
//        //
//        // 编写测试时，还可使用以下附加属性:
//        //
//        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
//        // [ClassInitialize()]
//        // public static void MyClassInitialize(TestContext testContext) { }
//        //
//        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
//        // [ClassCleanup()]
//        // public static void MyClassCleanup() { }
//        //
//        // 在运行每个测试之前，使用 TestInitialize 来运行代码
//        // [TestInitialize()]
//        // public void MyTestInitialize() { }
//        //
//        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
//        // [TestCleanup()]
//        // public void MyTestCleanup() { }
//        //
//        #endregion

//        [TestMethod]
//        public void Create()
//        {
//            CategoryModel cate = new CategoryModel();
//            cate.CategoryID = 300;
//            cate.CategoryName = "money";
//            cate.CategoryDesc = "money desc";
//            cate.Deleted = true;

//            DbUtil.DataManager.Create(cate);
//        }

//        [TestMethod]
//        public void Retrieve()
//        {
//            CategoryModel cate = new CategoryModel();
//            cate.CategoryID = 13;
//            cate.CreatedOn = DateTime.Now;

//            string s = DbUtil.DataManager.ConnectionString;
//            DataTable dt = DbUtil.DataManager.Retrieve(cate);
//            DbUtil.DataManager.ConvertFrom(cate, dt);
//            DataTable dt2 = DbUtil.DataManager.ConvertTo(cate);

//            //Assert.AreEqual(dt.Rows[0]["CategoryName"].ToString(), "money");
//        }

//        [TestMethod]
//        public void RetrieveMultiple()
//        {
//            CategoryModel cate = new CategoryModel();

//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("CategoryName", "money");

//            DataTable dt = DbUtil.DataManager.RetrieveMultiple(cate, pc);
//            //foreach (DataRow row in dt.Rows)
//            //{
//            //    Assert.AreEqual(row["CategoryName"].ToString(), "money");
//            //}
//        }

//        [TestMethod]
//        public void RetrieveMultipleOrderBy()
//        {
//            CategoryModel cate = new CategoryModel();

//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("CategoryName", "money");

//            OrderByCollection obc = new OrderByCollection();
//            obc.Add("CategoryName", SortTypeEnum.Asc);

//            DataTable dt = DbUtil.DataManager.RetrieveMultiple(cate, pc, obc);

//            //foreach (DataRow row in dt.Rows)
//            //{
//            //    Assert.AreEqual(row["CategoryName"].ToString(), "money");
//            //}
//        }

//        [TestMethod]
//        public void Update()
//        {
//            CategoryModel cate = new CategoryModel();
//            cate.CategoryID = 8;
//            cate.CategoryName = "moneymoney";

//            DbUtil.DataManager.Update(cate);
//        }

//        [TestMethod]
//        public void UpdateMultiple()
//        {
//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("CategoryName", "money");

//            CategoryModel cate = new CategoryModel();
//            cate.CategoryName = "money";

//            DbUtil.DataManager.UpdateMultiple(cate, pc);
//        }

//        [TestMethod]
//        public void Delete()
//        {
//            CategoryModel cate = new CategoryModel();
//            cate.CategoryID = 8;

//            DbUtil.DataManager.Delete(cate);
//        }

//        [TestMethod]
//        public void DeleteMultiple()
//        {
//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("CategoryName", "Money");

//            CategoryModel cate = new CategoryModel();

//            DbUtil.DataManager.DeleteMultiple(cate, pc);
//        }

//        [TestMethod]
//        public void ExecuteSQL()
//        {
//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("CategoryName", "money");

//            DataTable dt = DbUtil.DataManager.IData.ExecuteDataTable("select * from Category where CategoryName=$CategoryName$", CommandTypeEnum.Text, pc);
            
//            CategoryModel cate = new CategoryModel();
//            DbUtil.DataManager.ConvertFrom(cate, dt);
            

//        }

//        [TestMethod]
//        public void M()
//        {
//            //DbUtil.DataManager.BeginTransaction();

//            DbUtil.DataManager.IData.ExecuteNonQuery("select * from Category");

//            Do1();
//            Do2();

//            //DbUtil.DataManager.CommitTransaction();
//        }

//        [TestMethod]
//        public void Do1()
//        {
//            DbUtil.DataManager.IData.ExecuteNonQuery("select * from Category");
//        }

//        [TestMethod]
//        public void Do2()
//        {
//            DbUtil.DataManager.IData.ExecuteNonQuery("select * from Category");
//        }


//    }
//}
