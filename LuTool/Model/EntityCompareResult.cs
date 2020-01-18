namespace LuTool
{
    /// <summary>
    /// 实体比较结果
    /// </summary>
    public class EntityCompareResult
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段描述，Ddesc
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 现值
        /// </summary>
        public string PresentValue { get; set; }
    }
}