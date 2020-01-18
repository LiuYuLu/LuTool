namespace LuTool
{
    /// <summary>
    /// 实体字段信息
    /// </summary>
    public class EntityFieldData
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// 是否是默认值
        /// </summary>
        public bool DefaultValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityFieldData()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityFieldData(string name,string value, bool defaultVal=false)
        {
            FieldName = name;
            FieldValue = value;
            DefaultValue = defaultVal;
        }
    }
}