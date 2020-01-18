using System;
using Xunit;

namespace LuTool.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            SingletonTest.Instance.ReturnStr();
        }
    }
}
