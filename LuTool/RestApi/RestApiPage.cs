using System.Collections.Generic;

namespace LuTool
{
    /// <summary>
    /// 分页数据返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestApiPage<T> : RestApi
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public DataPage Page { get; set; }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        public RestApiPage()
        {
            this.Data = new List<T>();
            this.Page = new DataPage();
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        public RestApiPage(long total)
        {
            this.Data = new List<T>();
            this.Page = new DataPage(total);
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="t">实体</param>
        public RestApiPage(List<T> t)
        {
            this.Data = t;
            this.Page = new DataPage();
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="total">总量</param>
        public RestApiPage(List<T> t, long total)
        {
            this.Data = t;
            var dp = new DataPage(total)
            {
                CurrentSize = t?.Count ?? 0
            };
            this.Page = dp;
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="dp">分页数据</param>
        public RestApiPage(List<T> t, DataPage dp)
        {
            this.Data = t;
            dp.CurrentSize = t?.Count ?? 0;
            this.Page = dp;
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="rslt">实体</param>
        public RestApiPage(PageResult<T> rslt)
        {
            this.Data = rslt;
            this.Page = new DataPage(rslt.Page, rslt.Size, rslt.TotalCount)
            {
                CurrentSize = rslt?.Count ?? 0
            };
        }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="rslt">页数</param>
        public RestApiPage(List<T> t, PageResult<T> rslt) : this(t, rslt.Page, rslt.Size, rslt.TotalCount) { }

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="page">页数</param>
        /// <param name="size">大小</param>
        /// <param name="total">总量</param>
        public RestApiPage(List<T> t, int page, int size, long total)
        {
            this.Data = t;
            var dp = new DataPage(total)
            {
                PageIndex = page,
                PageSize = size,
                CurrentSize = t?.Count ?? 0
            };
            this.Page = dp;
        }
    }
}