using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// OpenTsdb查询返回的数据明细
    /// </summary>
    public class OpenTsdbQueryDetail
    {
        /// <summary>
        /// 度规
        /// </summary>
        [JsonProperty("metric")]
        public string Metric { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// 聚合的可以，可以理解为没有进行操作的tag集合
        /// </summary>
        [JsonProperty("aggregateTags")]
        public List<string> AggregateTags { get; set; }

        /// <summary>
        /// 命中数据
        /// </summary>
        [JsonProperty("dps")]
        public Dictionary<string, double> Dps { get; set; }
    }
}