using System.Collections.Generic;

namespace LuTool.Model
{
    /// <summary>
    /// 带有分页的数据集合
    /// </summary>
    public class PageList<T> : List<T>
    {
        /// <summary>
        /// 总数据量
        /// </summary>
        public long TotalCount { get; set; }
    }
}