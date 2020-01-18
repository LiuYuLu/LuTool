using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class DataPage
    {
        #region 字段

        /// <summary>分页页大小0.不分页</summary>
        public const string PageSizeField = "PageSize";

        /// <summary>显示页编码</summary>
        public const string PageIndexField = "PageIndex";

        /// <summary>返回总信息量</summary>
        public const string RowCountField = "RowCount";

        /// <summary>返回总页数 </summary>
        public const string PageCountField = "PageCount";

        /// <summary>排序字段</summary>
        public const string OrderFieldName = "OrderField";

        /// <summary>对象属性名集合</summary>
        public static List<string> Columns => new List<string> { "PageSize", "PageIndex", "RowCount", "PageCount", "OrderField" };

        #endregion 字段

        #region 属性

        private int _pageSize = 20;

        /// <summary>分页页大小</summary>
        [DataMember]
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                if (_pageSize != 0)
                {
                    _pageCount = (int)(_rowCount / _pageSize);
                    if (_rowCount % _pageSize != 0)
                        _pageCount = _pageCount + 1;
                }
            }
        }

        private int _pageIndex = 1;

        /// <summary>显示页编码 </summary>
        [DataMember]
        public int PageIndex
        {
            get
            {
                if (_pageIndex <= 0)
                    _pageIndex = 1;
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        private long _rowCount;

        /// <summary>返回总信息量</summary>
        [DataMember]
        public long RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
                if (_pageSize != 0)
                {
                    _pageCount = (int)(_rowCount / _pageSize); //取模，求整
                    if (_rowCount % _pageSize != 0) _pageCount = _pageCount + 1; //求余
                    if (_pageIndex > _pageCount) _pageIndex = _pageCount;
                }
            }
        }

        private int _pageCount = 1;

        /// <summary>返回总页数</summary>
        [DataMember]
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
            }
        }

        /// <summary>
        /// 本次返回的数据数量
        /// </summary>
        public int CurrentSize { get; set; }

        /// <summary>
        /// 是否是第一页
        /// </summary>
        public bool IsFirst
        {
            get { return this.PageIndex == 1; }
        }

        /// <summary>
        /// 是否是最后一页
        /// </summary>
        public bool IsLast
        {
            get { return this.PageIndex * this.PageSize >= this.RowCount; }
        }

        /// <summary>
        /// 可以往前翻页
        /// </summary>
        public bool HasPrevious
        {
            get { return this.PageIndex > 1; }
        }

        /// <summary>
        /// 可以往后翻页
        /// </summary>
        public bool HasNext
        {
            get { return this.PageIndex * this.PageSize < this.RowCount; }
        }

        /// <summary>查寻是要获的字段,号分隔</summary>
        public string ShowFieldsOnly { get; set; }

        private int _rowSkip;

        /// <summary>返回跳过的记录数</summary>
        [DataMember]
        public int RowSkip
        {
            get
            {
                _rowSkip = (PageIndex - 1) * PageSize;
                return _rowSkip;
            }
            set { _rowSkip = value; }
        }

        /// <summary>排序字段</summary>
        [DataMember]
        public string OrderField { get; set; }

        /// <summary>执行时间 </summary>
        [DataMember]
        [JsonIgnore]
        public double ExeTime { get; set; }

        /// <summary> 错误消息 </summary>
        [DataMember]
        [JsonIgnore]
        public string Message { get; set; }

        #endregion 属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataPage()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="total"></param>
        public DataPage(long total) => this.RowCount = total;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        public DataPage(int page, int size) {
            this.PageIndex = page;
            this.PageSize = size;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        public DataPage(int page, int size, long total)
        {
            this.PageIndex = page;
            this.PageSize = size;
            this.RowCount = total;
        }
    }
}