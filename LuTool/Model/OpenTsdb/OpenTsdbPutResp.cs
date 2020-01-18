using System.Collections.Generic;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// OpenTSDB推送结果
    /// </summary>
    public class OpenTsdbPutResp
    {
        /// <summary>
        /// 成功数量
        /// </summary>
        [JsonProperty("success")]
        public int Success { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        [JsonProperty("failed")]
        public int Failed { get; set; }

        /// <summary>
        /// 错误信息集合
        /// </summary>
        [JsonProperty("errors")]
        public List<ErrorDetail> Errors { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public class ErrorDetail
        {
            /// <summary>
            /// 请求保存的数据
            /// </summary>
            [JsonProperty("datapoint")]
            public OpenTsdbPutRequest Datapoint { get; set; }

            /// <summary>
            /// 失败信息
            /// </summary>
            [JsonProperty("error")]
            public string Error { get; set; }
        }
    }
}