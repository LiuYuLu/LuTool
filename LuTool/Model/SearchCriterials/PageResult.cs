using System.Collections.Generic;

namespace LuTool
{
    /// <summary>
    /// 分页泛型返回
    /// </summary>
    public class PageResult<T> : List<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 分页开始下标
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageResult()
        {
            this.Page = 0;
            this.Size = 0;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="req"></param>
        public PageResult(BasePageSearch req)
        {
            this.Page = req.Page;
            this.Size = req.Size;
        }
    }
}