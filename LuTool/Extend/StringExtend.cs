using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LuTool
{
    /// <summary>
    /// String类型拓展方法
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 判断字符串为null或无内容.
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串为null或空白字符串.
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 得到字符串的Byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Byte</returns>
        public static byte[] GetBytes(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(byte[]);
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值[默认是0]</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(this string str, int defValue = 0)
        {
            int.TryParse(str, out defValue);
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Long类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int64类型结果</returns>
        public static long ToLong(this string str, long defValue = 0)
        {
            long.TryParse(str, out defValue);
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Long类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int64类型结果</returns>
        public static bool ToBool(this string str, bool defValue = false)
        {
            bool.TryParse(str, out defValue);
            return defValue;
        }

        /// <summary>
        /// 将字符串转换为short类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">缺省值</param>
        /// <returns></returns>
        public static short ToShort(this string str, short defaultValue = 0)
        {
            short.TryParse(str, out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换为Double类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">缺省值</param>
        /// <returns></returns>
        public static double ToDouble(this string str, double defaultValue = 0)
        {
            double.TryParse(str, out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换为枚举类型
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <param name="defaultValue">缺省值</param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase = true, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            Enum.TryParse(str, ignoreCase, out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 去除字符串重复，并且还是以因为逗号隔开
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="splitChar">分割字符</param>
        /// <param name="removeEmpty">是否去除空格</param>
        /// <returns></returns>
        public static string RemoveStrRepeat(this string str, char splitChar, bool removeEmpty = true)
        {
            var array = str.Split(splitChar);
            if (removeEmpty)
                array.Where(m => !string.IsNullOrEmpty(m));
            return string.Join(splitChar.ToString(), array.Distinct().ToArray());
        }

        /// <summary>
        /// 判断字符串是否是符合要求长度的数字
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="length">要求长度</param>
        /// <returns></returns>
        public static bool MatchNum(this string value, int length)
        {
            var regex = string.Concat("^[0-9]{", length, "}$");
            return Regex.IsMatch(value, regex);
        }

        /// <summary>
        /// 去除HTML标签
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="allHtmlTag">是否包含小括号和换行</param>
        /// <returns></returns>
        public static string NoHtmlTag(this string text, bool allHtmlTag = false)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            //删除脚本
            text = Regex.Replace(text, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            text = Regex.Replace(text, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"-->", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<!--.*", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            if (allHtmlTag)
            {
                text.Replace("<", "");
                text.Replace(">", "");
                text.Replace("\r\n", "");
            }
            return text;
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIpAddress(this string value)
        {
            return Regex.IsMatch(value, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        /// <summary>
        /// 字符串瘦身，获取前N个字符串，超过则截取并添加后缀
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">长度</param>
        /// <param name="suffix">后缀，默认是不加后缀</param>
        /// <returns></returns>
        public static string Slim(this string str, int length, string suffix = "")
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);

            char[] stringChar = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + suffix;
            else
                return sb.ToString();
        }

        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T">返回数据实体</typeparam>
        /// <param name="xmlString">xml字符串</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString) where T : class, new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStr">时间戳</param>
        /// <param name="showSecond">是否显示秒级时间戳，否->毫秒级</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string timeStr)
        {
            try
            {
                // 判断是不是字段串
                if (RegexHelper.IsMatch(timeStr, "^[0-9]+$"))
                {
                    var longStamp = timeStr.ToLong();
                    return longStamp.ToDateTime();
                }
                else
                {
                    return Convert.ToDateTime(timeStr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"传入的参数参数错误，参数timeStamp：{timeStr}", ex);
            }
        }

        #region 对字符串进行编码和解码

        /// <summary>
        /// 对字符串进行UrlEncode编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return WebUtility.UrlEncode(str);
        }

        /// <summary>
        /// 对字符串进行UrlDecode解码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return WebUtility.UrlDecode(str);
        }

        /// <summary>
        /// 对字符串进行HtmlEncode编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(this string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 对字符串进行HtmlDecode解码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 根据Regex正则表达式，对匹配的内容进行UrlEncode编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="regex">Regex正则表达式</param>
        /// <returns></returns>
        public static string UrlEncodeByRegex(this string value, string regex)
        {
            var list = RegexHelper.GetMatchValueList(value, regex);
            list.ForEach(m => m.ForEach(n => value = value.Replace(n, n.UrlEncode())));
            return value;
        }

        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jerry，如果不编码，可能得到的网址： http://a.com/?tvshow=Tom&Jerry&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jerry&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AsUrlData(this string data)
        {
            if (data == null)
            {
                return null;
            }
            return Uri.EscapeDataString(data);
        }

        #endregion 对字符串进行编码和解码

        /// <summary>
        /// 对字符串反序列化成JSON对象
        /// </summary>
        /// <typeparam name="T">JSON对象</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="isIgnoreNull">是否忽略NULL值</param>
        /// <param name="isIgnoreEx">是否忽略异常，忽略异常返回default(T)</param>
        /// <returns></returns>
        public static T ToObject<T>(this string str, bool isIgnoreNull = true, bool isIgnoreEx = false)
        {
            var setting = new JsonSerializerSettings
            {
                NullValueHandling = isIgnoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, setting);
                }
            }
            catch (Exception ex)
            {
                if (!isIgnoreEx)
                    throw new Exception($"将字符串格转成对象出错，str:{str}", ex);
                return default(T);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">JSON对象</typeparam>
        /// <param name="str">字符串</param>
        /// <param name="settings">反序列化配置</param>
        /// <param name="isIgnoreEx">是否忽略异常，忽略异常返回default(T)</param>
        /// <returns></returns>
        public static T ToObject<T>(this string str, JsonSerializerSettings settings, bool isIgnoreEx = false)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, settings);
                }
            }
            catch (Exception ex)
            {
                if (!isIgnoreEx)
                    throw new Exception($"将字符串格转成对象出错，str:{str}", ex);
                return default(T);
            }
        }

        /// <summary>
        /// 美化Tsdb的Tag标签内容
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        [Obsolete("规范名称，请使用PrettifyTsdbTag", true)]
        public static string TsdbTagPrettify(this string str)
        {
            return Regex.Replace(str, @"[^-_a-zA-Z0-9\.\u4e00-\u9fa5]", "_", RegexOptions.Multiline);
        }

        /// <summary>
        /// 美化Tsdb的Tag标签内容
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string PrettifyTsdbTag(this string str)
        {
            return Regex.Replace(str, @"[^-_a-zA-Z0-9\.\u4e00-\u9fa5]", "_", RegexOptions.Multiline);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string CamelToUnderline(this string str)
        {
            return "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string FormatToSnack(this string str)
        {
            while (str.Contains("_"))
            {
            }
            return "";
        }

        /// <summary>
        /// 将字符串首字母小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string FirstLetterToLow(this string str)
        {
            if (str.IsNullOrWhiteSpace() || str.Length == 0)
                return str;
            var firstLetter = str.Substring(0, 1);
            var charLetter = firstLetter.ToCharArray()[0];
            if (charLetter >= 'A' && charLetter <= 'Z')
            {
                return firstLetter.ToLower() + str.Substring(1, str.Length - 1);
            }
            return str;
        }

        /// <summary>
        /// 将字符串转换成List字符集合
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static List<string> ToList(this string str, char separator)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();
            else
                return str.Split(',').ToList();
        }
    }
}