using System;
using Xunit;

namespace LuTool.Tests
{
    public class DateTimeExtendTest
    {
        private DateTime time1 = new DateTime(2019,09,29);

        [Fact]
        public void WeekOfYear()
        {
            var result = time1.WeekOfYear();
            Console.WriteLine($"测试结果：{result}");
            Assert.True(result >= 0);
        }

        [Fact]
        public void TestToTimeStamp()
        {
            var dateTime = DateTime.Now;
            var result = dateTime.ToTimeStamp();
            Console.WriteLine($"测试结果：{result}");
            Assert.True(result >= 0);
        }

        [Fact]
        public void ToNormalFormat()
        {
            var result = time1.ToNormalFormat();
            Console.WriteLine($"测试结果：{result}");
            Assert.Equal("2019-09-29 00:00:00", result);
        }
    }
}