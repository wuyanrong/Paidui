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
//    /// MySqlDBUnitTest 的摘要说明
//    /// </summary>
//    [TestClass]
//    public class MySqlDBUnitTest
//    {
//        public MySqlDBUnitTest()
//        {
//            TraceUtil.RegisterTraces("file", new MyFileTrace());
//            //TraceUtil.RegisterTraces("syslog", new MySyslogTrace());
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
//        public void MySql_TestDeserialization()
//        {
//            CategoryModel c = new CategoryModel();

            
//        }

//        [TestMethod]
//        public void MySql_RetrieveMultiple()
//        {
//            //string sql = "select * from bulletin_info";

//            //DataTable dt = DbUtil.DataManager.IData.ExecuteDataTable(sql);

//            string procResult = "";
//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("_match_number", "1");
//            pc.Add("_bet_type", "1");
//            pc.Add("_order_type", "1");
//            pc.Add("_horse_numbers", "1");
//            pc.Add("_odds", "17.6");
//            pc.Add("_amount", "100");
//            pc.Add("_commission_rate", "0.005");
//            pc.Add("_create_by", "{139E57B5-D9C5-4707-BD75-866CA8AF8C1}");
//            pc.Add("_result_value", procResult, ParameterDirectionEnum.Output);

//            DbUtil.DataManager.IData.ExecuteDataSet("p_orders", CommandTypeEnum.StoredProcedure, pc);

//            //foreach (DataRow row in dt.Rows)
//            //{
//            //    Assert.AreEqual(row["CategoryName"].ToString(), "money");
//            //}
//        }

//        [TestMethod]
//        public void MySql_GetStoredProcedureOutputParameter()
//        {
//            ParameterCollection pc = new ParameterCollection();
//            pc.Add("p1", "a");
//            pc.Add("p2", "", ParameterDirectionEnum.Output);

//            DbUtil.DataManager.IData.ExecuteDataSet("qmx_p_test_output", CommandTypeEnum.StoredProcedure, pc);

            
//        }


//        [TestMethod]
//        public void MySql_TraceDebug()
//        {
//            //while (true)
//            {
//                TraceUtil.TraceDebug("这是测试。。。");

//                System.Threading.Thread.Sleep(3000);
//            }
//        }

//        [TestMethod]
//        public void MySql_TestCommandTimeout()
//        {
//            //DbUtil.DataManager.IData.CommandTimeout
//        }


//        [TestMethod]
//        public void MySql_Insert_T2()
//        {
//            T2Model t2 = new T2Model();

//            t2.IdInt = 1;
//            t2.NameVarchar = "name";
//            t2.DescVarchar = "desc";

//            DbUtil.DataManager.Create(t2);
//        }


//        [TestMethod]
//        public void MySql_Update_T2()
//        {
//            T2Model t2 = new T2Model();

//            t2.IdInt = 1;

//            DbUtil.DataManager.Update(t2);

//        }

//    }
//}
