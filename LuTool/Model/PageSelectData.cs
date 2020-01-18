namespace LuTool
{
    /// <summary>
    /// 页面选择数据
    /// </summary>
    public class PageSelectData
    {
        /// <summary>
        /// 选项展示文本
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Dsc { get; set; }

        public PageSelectData()
        {
            this.Label = string.Empty;
            this.Value = string.Empty;
            this.Dsc = string.Empty;
            this.Selected = false;
        }

        public PageSelectData(string label, string value, string dsc = "", bool select = false)
        {
            this.Label = label;
            this.Value = value;
            this.Dsc = dsc;
            this.Selected = select;
        }

        public PageSelectData(string label, long value, string dsc = "", bool select = false)
            : this(label, value.ToString(), dsc, select)
        {
        }

        public PageSelectData(string label, double value, string dsc = "", bool select = false)
            : this(label, value.ToString(), dsc, select)
        {
        }
    }
}