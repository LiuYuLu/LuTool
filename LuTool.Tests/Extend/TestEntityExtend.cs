using Xunit;

namespace LuTool.Tests
{
    public class TestEntityExtend
    {
        [Fact]
        public void AutoCopyTest()
        {
            var parent = new ParentClass { id = 1, name = "父亲" };
            var child = parent.AutoCopy<ChildClass, ParentClass>();
            Assert.True(child != null);
        }
    }

    public class ParentClass
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ChildClass : ParentClass
    {
        public int age { get; set; }
        public string email { get; set; }
    }
}