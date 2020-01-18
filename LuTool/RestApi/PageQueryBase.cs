namespace LuTool
{
    /// <summary> 分页查询基类 </summary>
    public class PageQueryBase
    {
        /// <summary>
        /// 页码数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页数据
        /// </summary>
        private DataPage dp;

        /// <summary>
        ///
        /// </summary>
        public DataPage Dp
        {
            get
            {
                if (dp == null)
                {
                    dp = new DataPage(this.PageIndex, this.PageSize);
                    return dp;
                }
                return dp;
            }
        }
    }
}