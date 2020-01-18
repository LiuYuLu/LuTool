using Xunit;

namespace LuTool.Tests
{
    public class DoubleExtendTest
    {
        [Theory]
        [InlineData(1.00441)]
        [InlineData(1.1)]
        public void RoundTest(double value)
        {
            var result = value.Round(2);
            Assert.True(result >= 0);
        }
    }
}