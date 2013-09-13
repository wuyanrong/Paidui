using Evt.Framework.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace Evt.Framework.Test
{


    /// <summary>
    ///这是 JsonUtilTest 的测试类，旨在
    ///包含所有 JsonUtilTest 单元测试
    ///</summary>
    [TestClass()]
    public class JsonUtilTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Deserialize 的测试
        ///</summary>
        public void DeserializeTestHelper<T>()
        {
            string json = string.Empty; // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = JsonUtil.Deserialize<T>(json);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeserializeTest()
        {
            DeserializeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///Serialize 的测试
        ///</summary>
        [TestMethod()]
        public void SerializeTest()
        {
            Model m = new Model();
            string s = JsonUtil.Serialize(m);
            Assert.AreNotEqual(s, null);
        }

        /// <summary>
        ///Serialize 的测试
        ///</summary>
        [TestMethod()]
        public void SerializeTest1()
        {
            string key = string.Empty; // TODO: 初始化为适当的值
            object value = null; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = JsonUtil.Serialize(key, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SerializerDataTable 的测试
        ///</summary>
        [TestMethod()]
        public void SerializerDataTableTest()
        {
            string key = string.Empty; // TODO: 初始化为适当的值
            DataTable value = null; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = JsonUtil.SerializerDataTable(key, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SerializerDataTable 的测试
        ///</summary>
        [TestMethod()]
        public void SerializerDataTableTest1()
        {
            DataTable value = null; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = JsonUtil.SerializerDataTable(value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        // 新加入的测试 2012.9.25 ye.k

        private const string TitleStr = @"\Test\\k s\""f \\sf\@#235421^&^@^%#$#&@()\\s\fds\~!@#$%^&*()_+|}
{POIUYTREQW/,.?><:;'""?><";
        [TestMethod]
        public void TestDataTableToJson()
        {
            using (var dt = new DataTable())
            {
                dt.TableName = "TableName";
                var title = new DataColumn("Title", typeof(string));
                dt.Columns.Add(title);
                DataRow dataRow = dt.NewRow();
                dataRow["Title"] = TitleStr;
                dt.Rows.Add(dataRow);
                var result = JsonUtil.SerializerDataTable(dt);
                Assert.AreEqual(result, @"[{""Title"":""\\Test\\\\k s\\\""f \\\\sf\\@#235421^&^@^%#$#&@()\\\\s\\fds\\~!@#$%^&*()_+|}\r\n{POIUYTREQW/,.?><:;'\""?><""}]");
            }
        }

        [TestMethod]
        public void TestSerialize()
        {
            var key = "Title";
            var value = TitleStr;
            var result = JsonUtil.Serialize(key, value);
            Assert.AreEqual(result, @"{""Title"":""\\Test\\\\k s\\\""f \\\\sf\\@#235421^&^@^%#$#&@()\\\\s\\fds\\~!@#$%^&*()_+|}\r\n{POIUYTREQW/,.?\u003e\u003c:;\u0027\""?\u003e\u003c""}");
        }
    }

    public class Model
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
