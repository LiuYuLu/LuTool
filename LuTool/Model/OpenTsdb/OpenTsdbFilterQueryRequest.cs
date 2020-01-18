using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// OpenTsdb查询请求
    /// </summary>
    public class OpenTsdbFilterQueryRequest
    {
        /// <summary>
        /// 开始的时间戳，秒级(建议)或毫秒级
        /// </summary>
        [JsonProperty("start")]
        public long Start { get; set; }

        /// <summary>
        /// 结束时间戳，秒级(建议)或毫秒级
        /// </summary>
        [JsonProperty("end")]
        public long End { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        [JsonProperty("queries")]
        public List<Query> Queries { get; set; }

        /// <summary>
        /// 是否返回注释信息。默认为false。
        /// </summary>
        [JsonProperty("noAnnotations")]
        public bool NoAnnotations { get; set; } = false;

        /// <summary>
        /// 是否返回时间跨度内的全局注释。默认为false。
        /// </summary>
        [JsonProperty("globalAnnotations")]
        public bool GlobalAnnotations { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回与时间序列关联的TSUID。 true：如果多个时间序列被聚合成一个集合，则多个TSUID将以排序方式返回。false：不返回。 默认为false。
        /// </summary>
        [JsonProperty("showTSUIDs")]
        public bool ShowTSUIDs { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回时间摘要，默认为false。
        /// </summary>
        [JsonProperty("showSummary")]
        public bool ShowSummary { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回详细时间，默认为false。
        /// </summary>
        [JsonProperty("showStats")]
        public bool ShowStats { get; set; } = false;

        /// <summary>
        /// 是否返回带查询结果的原始子查询。默认为false。
        /// </summary>
        [JsonProperty("showQuery")]
        public bool ShowQuery { get; set; } = false;

        /// <summary>
        /// 默认情况下，查询结果中的时间戳是以秒为单位的。如果设置为true，则查询结果中的时间戳以毫秒为单位。
        /// </summary>
        [JsonProperty("msResolution")]
        public bool MsResolution { get; set; } = false;

        /// <summary>
        /// 查询条件
        /// </summary>
        public class Query
        {
            /// <summary>
            /// 聚合函数
            /// </summary>
            [JsonProperty("aggregator")]
            public string Aggregator { get; set; } = "avg";

            /// <summary>
            /// 查询的Metric指标项
            /// </summary>
            [JsonProperty("metric")]
            public string Metric { get; set; }

            /// <summary>
            /// 标签过滤条件
            /// </summary>
            [JsonProperty("filters")]
            public List<QueryFilter> Filters { get; set; }

            /// <summary>
            /// 是否返回倾斜率。默认为false。
            /// </summary>
            [JsonProperty("rate")]
            public bool Rate { get; set; } = false;

            /// <summary>
            /// 选项
            /// </summary>
            [JsonProperty("rateOptions")]
            public Dictionary<string, string> RateOptions { get; set; }

            /// <summary>
            /// 降时间精度采样
            /// </summary>
            [JsonProperty("downsample")]
            public string Downsample { get; set; }

            /// <summary>
            /// 是否返回仅包含过滤器中提供的标记键的系列。默认为false。
            /// </summary>
            [JsonProperty("explicitTags")]
            public bool ExplicitTags { get; set; } = false;

            /// <summary>
            /// 获取度量的直方图数据，并计算数据上给定的百分位数列表。百分位数是从0到100的浮点值。更多详细信息如下
            /// </summary>
            [JsonProperty("percentiles")]
            public List<double> Percentiles { get; set; }
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        public class QueryFilter
        {
            /// <summary>
            /// 要调用的过滤器的名称
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// 用于调用过滤器的标记键
            /// </summary>
            [JsonProperty("tagk")]
            public string Tagk { get; set; }

            /// <summary>
            /// 要评估的过滤器表达式，并取决于所使用的过滤器
            /// </summary>
            [JsonProperty("filter")]
            public string Filter { get; set; }

            /// <summary>
            /// 是否按过滤器匹配的每个值对结果进行分组。默认情况下，与筛选器匹配的所有值都将聚合到一个系列中
            /// </summary>
            [JsonProperty("groupBy")]
            public bool GroupBy { get; set; } = false;
        }
    }
}