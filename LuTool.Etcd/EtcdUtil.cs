using System.Collections.Generic;
using System.Text;
using Etcdserverpb;

namespace LuTool.Etcd
{
    public class EtcdUtil
    {
        /// <summary>
        /// Converts RangeResponse to Dictionary
        /// </summary>
        /// <returns>IDictionary corresponding the RangeResponse</returns>
        /// <param name="resp">RangeResponse received from etcd server</param>
        public static IDictionary<string, string> RangeRespondToDictionary(RangeResponse resp)
        {
            Dictionary<string, string> resDictionary = new Dictionary<string, string>();
            foreach (Mvccpb.KeyValue kv in resp.Kvs)
            {
                resDictionary.Add(kv.Key.ToStringUtf8(), kv.Value.ToStringUtf8());
            }
            return resDictionary;
        }

        /// <summary>
        /// 获取前缀的范围结尾
        /// </summary>
        /// <returns>The range end for prefix</returns>
        /// <param name="prefixKey">前缀</param>
        public static string GetRangeEnd(string prefixKey)
        {
            StringBuilder rangeEnd = new StringBuilder(prefixKey);
            rangeEnd[rangeEnd.Length - 1] = ++rangeEnd[rangeEnd.Length - 1];
            return rangeEnd.ToString();
        }
    }
}