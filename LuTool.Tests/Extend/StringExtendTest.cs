using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LuTool.Tests
{
    public class StringExtendTest
    {
        [Fact]
        public void TestMethod1()
        {
            var dateTime = DateTime.Now;
            var result = dateTime.WeekOfYear();
            Console.WriteLine($"测试结果：{result}");
            Assert.True(result >= 0);
        }

        [Fact]
        public void TestTsdbTagPrettify()
        {
            var str = "POST /pay/accountbalan:ce";
            var result = str.PrettifyTsdbTag();
            Console.WriteLine($"测试结果：{result}");
            Assert.True(!result.Contains(" "));
        }

        [Fact]
        public void TestAndOr()
        {
            var str = " A && (B || C)";
            var exe = str.Clone().ToString();

            str = str.Replace(" ", "");
            var rslt = true && (true || false);

            var array = new List<string> { "&", "|", "(", ")" };
            var dataList = new List<bool> { true, true, false };
            var exeStr = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            var resl = true;


            int flag = 0;
            while (str.Length <=flag)
            {
                var strtem = str.Substring(flag, 1);
                if (isfirst(strtem))
                {

                }
            }



            bool isfirst(string a)
            {
                return array.Contains(a);
            }

            Expression<Func<bool>> express = () => true;
        }
    }
}