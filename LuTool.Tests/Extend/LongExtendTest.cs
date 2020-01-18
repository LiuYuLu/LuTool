using System;
using Xunit;

namespace LuTool.Tests
{
    public class LongExtendTest
    {
        [Fact]
        public void TestToDateTime()
        {
            var stamp = 1544507100000L;
            var result = stamp.ToDateTime();
            Console.WriteLine($"测试结果：{result}");
            Assert.True(result < DateTime.Now);
        }
    }
}