using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace LuTool.Tests
{
    public class HttpHelperTest
    {
        [Fact]
        public void PostTest()
        {
            var url = "http://www.istr.cn/api/token";
            var jsonStr = @"{
 ""username"": ""netcore.monitormain"",
 ""password"": ""KoN2t5lOryuPxhrz""
}";
            var resp = HttpHelper.Post(url, jsonStr, out HttpStatusCode status);
            Assert.True(!string.IsNullOrWhiteSpace(resp));
        }

        [Fact]
        public void PostHeaderTest()
        {
            var url = "http://www.istr.cn/api/productLines/search";
            var jsonStr = @"{
  ""returns"": [""queryAll"",""labelDetail""],
  ""index"": 1,
  ""size"": 1,
  ""query"": """"
}";
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("X-Auth-Token", "netcore.monitormain:1527824703001:d7e4183be6c92d79f5f823b14454f0ff:57208");
            var resp = HttpHelper.Post(url, jsonStr, header, out HttpStatusCode status);
            Assert.True(!string.IsNullOrWhiteSpace(resp));
        }

        [Fact]
        public void PostWithBasicTest()
        {
            var url = "https://api.Tapd.cn/workspaces/add_member_by_nick";
            var data = "workspace_id=20000691";
            var name = "lylyl";
            var pwd = "590E11F8-65B0-5ECA-F4D6-F2F2A362C7LY";
            var contentType = "application/x-www-form-urlencoded";
            var resp = HttpHelper.PostWithBasic(url, data, name, pwd, contentType: contentType);
            Assert.True(!string.IsNullOrWhiteSpace(resp));
        }

        [Fact]
        public void PostFileTest()
        {
            var token = "vfVk2PAGjLIeJXV5NdCt1tYsXJnGYXNgpmEXAg_WM7t-3xkhYByMP_mW-RwolHFVZ20mBpLRV0MLgdnoUd0NQ3ESehyR1J30-huu39t2tgeibOQW6KHWrdx4SQCwKnjLly-vV4epZROPr4-uFWq6Ol1lIDCmoOLct3JlEZbJsrzvxjuzXXSV_khE2BLgz5dxAFtWO4Z6XfE5Bxw3DwGqAA";
            var url = $"https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={token}&type=image";
            var fileUrl = "";
            var filename = "39c2c2be7a9641a0bb9860ee53b8c516image.png";
            var resp = HttpHelper.PostFile(url, fileUrl, filename);
            Assert.True(!string.IsNullOrWhiteSpace(resp));
        }

        [Fact]
        public void TestGet()
        {
            try
            {
                var result = HttpHelper.Get("http://www.baidu.com");
                Console.WriteLine(result);

                Assert.True(result.IsValuable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}