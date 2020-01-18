using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LuTool.Tests
{
    public class SingletonTest : Singleton<SingletonTest>
    {
        public string ReturnStr()
        {
            return "Hello World";
        }
    }
}
