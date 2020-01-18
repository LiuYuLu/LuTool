using System;
using Xunit;

namespace LuTool.Tests
{
    public class EncryptHelperTest
    {
        [Fact]
        public void GzipCompressAndDecompressTest()
        {
            var inputStr = "zlex@zlex.org,snowolf@zlex.org,zlex.snowolf@zlex.org";
            var compress = EncryptHelper.GzipCompress(inputStr);
            Console.WriteLine(compress);
            var decompress = EncryptHelper.GzipDecompress(compress);
            Console.WriteLine(decompress);

            Assert.Equal(inputStr, decompress);
        }
    }
}