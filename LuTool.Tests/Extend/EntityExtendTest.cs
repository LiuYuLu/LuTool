using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LuTool.Tests
{
    public class EntityExtendTest
    {
        public class TestModel1
        {
            public int Age { get; set; }
            public string Name { get; set; }
            public long Page { get; set; }
        }

        public class TestModel2
        {
            public int Age { get; set; }
            public string Name { get; set; }
            public long Page { get; set; }
            public bool Man { get; set; }
        }

        [Fact]
        public void TestMethod1()
        {
            var model = new TestModel2 { Age = 10, Name = "sadfasd", Page = 123333333333333333 };
            var model2 = new TestModel2 { Age = 10 };
            var newModel = new TestModel1 { Age = 22, Name = "22222", Page = 222222222222222 };
            var result = newModel.AutoCopy(model2);
            Console.WriteLine($"测试结果：{result.ToString()}");
            Assert.True(result != null);
        }

        [Fact]
        public void TestTsdbTagPrettify()
        {
            var model1 = new TestModel2 { Age = 10, Name = "sadfasd", Page = 123333333333333333 };
            var model2 = new TestModel2 { Age = 101, Name = "sadfasd1", Page = 4444444444444 };
            var result = model2.AutoCompare(model1);
            Console.WriteLine($"测试结果：{result.ToString()}");
            Assert.True(!result.ToString().Contains(" "));
        }

        [Fact]
        public void TestAutoCopyJobj()
        {
            var model1 = new TestModel2 { Age = 10, Name = "sadfasd", Page = 123333333333333333 ,Man = false};
            var json = @"{""age"":9,""name"":""json数据"",""man"":true}";
            var josnObj = JObject.Parse(json);
            var result = model1.JsonCopy(josnObj,StringFormatEnum.Up);
            Console.WriteLine($"测试结果：{result.ToString()}");
            Assert.True(!result.ToString().Contains(" "));
        }

        [Fact]
        public void TestToJosn()
        {
            var model1 = new WechatWorkRequest(1,11) { Content="sdfsadf" };
            var str = model1.ToJson();
            Console.WriteLine($"测试结果：{str}");
            Assert.True(!str.Contains(" "));
        }
    }
}