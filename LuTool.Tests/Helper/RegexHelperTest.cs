using System;
using System.Net;
using System.Text.RegularExpressions;
using Xunit;

namespace LuTool.Tests
{
    public class RegexHelperTest
    {
        [Fact]
        public void PostTest()
        {
            var str =
                "2019-12-02T10:28:13.343233997Z =Microsoft (R) Build Engine version 16.0.450+ga8dc7f1d34 for .NET Core";

            var matchResult = Regex.Matches(str, @"^[0-9-]{10}T[0-9:]{8}\.[0-9]+Z");
            if (matchResult.Count > 0)
                foreach (Match item in matchResult)
                {
                    for (var index = 0; index < item.Groups.Count; index++)
                    {
                        var matchStr = item.Groups[index].Value;
                        var datetime = Convert.ToDateTime(matchStr);
                        str = str.Replace(matchStr, $"{datetime:HH:mm:ss}");
                    }
                }

            Assert.True(!string.IsNullOrWhiteSpace(str));
        }
    }
}