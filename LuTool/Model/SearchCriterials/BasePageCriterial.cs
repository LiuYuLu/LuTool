namespace LuTool
{
    /// <summary>
    /// 分页内容
    /// </summary>
    public class BasePageSearch
    {
        /// <summary> 分页开始下标 </summary>
        public int Page { get; set; } = 0;

        /// <summary> 每页条数 </summary>
        public int Size { get; set; } = 0;
    }
}