using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LuTool
{
    /// <summary>
    /// 正则表达式帮助类
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 获取匹配正则表达式Key的值
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="regex">正则必须包含分组</param>
        /// <param name="key">key值</param>
        /// <returns></returns>
        public static string GetMatchValue(string text, string regex, string key)
        {
            Regex rex = new Regex(regex, RegexOptions.IgnoreCase);
            var result = rex.Match(text);
            return result.Groups[key].Value;
        }

        /// <summary>
        /// 获取匹配正则表达式Key的值,这个方法不安全，建议使用加参数List的
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="regex">正则必须包含分组</param>
        /// <returns></returns>
        public static List<List<string>> GetMatchValueList(string text, string regex)
        {
            var result = new List<List<string>>();
            Regex rex = new Regex(regex, RegexOptions.IgnoreCase);
            var matchResult = rex.Matches(text);
            if (matchResult.Count > 0)
            {
                foreach (Match item in matchResult)
                {
                    var tempValue = new List<string>();
                    for (int groupCtr = 1; groupCtr < item.Groups.Count; groupCtr++)
                    {
                        Group group = item.Groups[groupCtr];
                        tempValue.Add(group.Value);
                    }
                    result.Add(tempValue);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取匹配正则表达式Key的值
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="regex">正则必须包含分组</param>
        /// <param name="keys">key值</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetMatchValueList(string text, string regex, List<string> keys)
        {
            var result = new List<Dictionary<string, string>>();
            Regex rex = new Regex(regex, RegexOptions.IgnoreCase);
            var matchResult = rex.Matches(text);
            if (matchResult.Count > 0)
            {
                foreach (Match item in matchResult)
                {
                    var divValue = new Dictionary<string, string>();
                    foreach (var key in keys)
                    {
                        divValue.Add(key, item.Groups[key] == null ? string.Empty : item.Groups[key].Value);
                    }
                    result.Add(divValue);
                }
            }
            return result;
        }

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsMatch(string str, string regex)
        {
            Regex rex = new Regex(regex, RegexOptions.IgnoreCase);
            return rex.IsMatch(str);
        }
    }
}