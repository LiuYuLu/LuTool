using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// OpenTsdb查询请求
    /// </summary>
    public class OpenTsdbQueryRequest
    {
        /// <summary>
        /// 开始的时间，可以是时间戳、24h-ago、2018/11/01-10:00、2018/11/01 10:00
        /// </summary>
        [JsonProperty("start")]
        public object Start { get; set; }

        /// <summary>
        /// 结束时间（非必须），可以是时间戳、24h-ago、2018/11/01-10:00、2018/11/01 10:00
        /// </summary>
        [JsonProperty("end")]
        public object End { get; set; }

        /// <summary>
        /// 查询条件项
        /// </summary>
        [JsonProperty("queries")]
        public List<Query> Queries { get; set; }

        /// <summary>
        /// 是否返回注释信息。默认为false。
        /// </summary>
        [JsonProperty("noAnnotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool NoAnnotations { get; set; } = false;

        /// <summary>
        /// 是否返回时间跨度内的全局注释。默认为false。
        /// </summary>
        [JsonProperty("globalAnnotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool GlobalAnnotations { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回与时间序列关联的TSUID。 true：如果多个时间序列被聚合成一个集合，则多个TSUID将以排序方式返回。false：不返回。 默认为false。
        /// </summary>
        [JsonProperty("showTSUIDs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ShowTSUIDs { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回时间摘要，默认为false。
        /// </summary>
        [JsonProperty("showSummary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ShowSummary { get; set; } = false;

        /// <summary>
        /// 是否在结果中返回详细时间，默认为false。
        /// </summary>
        [JsonProperty("showStats", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ShowStats { get; set; } = false;

        /// <summary>
        /// 是否返回带查询结果的原始子查询。默认为false。
        /// </summary>
        [JsonProperty("showQuery", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ShowQuery { get; set; } = false;

        /// <summary>
        /// 默认情况下，查询结果中的时间戳是以秒为单位的。如果设置为true，则查询结果中的时间戳以毫秒为单位。
        /// </summary>
        [JsonProperty("msResolution", DefaultValueHandling = DefaultValueHandling.Ignore)]
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
            /// 标签筛选条件
            /// </summary>
            [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
            public Dictionary<string, string> Tags { get; set; }

            /// <summary>
            /// 是否返回倾斜率。默认为false。
            /// </summary>
            [JsonProperty("rate", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool Rate { get; set; } = false;

            /// <summary>
            /// 选项
            /// </summary>
            [JsonProperty("rateOptions", NullValueHandling = NullValueHandling.Ignore)]
            public Dictionary<string, string> RateOptions { get; set; }

            /// <summary>
            /// 降时间精度采样
            /// </summary>
            [JsonProperty("downsample", NullValueHandling = NullValueHandling.Ignore)]
            public string Downsample { get; set; }

            /// <summary>
            /// 是否返回仅包含过滤器中提供的标记键的系列。默认为false。
            /// </summary>
            [JsonProperty("explicitTags", DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool ExplicitTags { get; set; } = false;

            /// <summary>
            /// 获取度量的直方图数据，并计算数据上给定的百分位数列表。百分位数是从0到100的浮点值。更多详细信息如下
            /// </summary>
            [JsonProperty("percentiles", NullValueHandling = NullValueHandling.Ignore)]
            public List<double> Percentiles { get; set; }
        }
    }
}