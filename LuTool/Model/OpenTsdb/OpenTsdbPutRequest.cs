using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// OpenTSDB 推送的一条消息，推送应该是集合
    /// </summary>
    public class OpenTsdbPutRequest
    {
        /// <summary>
        /// 度规，规定变量含义
        /// </summary>
        [JsonProperty("metric")]
        public string Metric { get; set; }

        /// <summary>
        /// 时间戳，秒级(建议)或毫秒级
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// 值，浮点或整型
        /// </summary>
        [JsonProperty("value")]
        public double Value { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }
}