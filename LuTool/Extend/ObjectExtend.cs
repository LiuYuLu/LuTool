using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace LuTool
{
    /// <summary>
    /// object类型数据的拓展方法
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="objValue">要转换的字符串</param>
        /// <param name="defValue">缺省值[默认是0]</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(this object objValue, int defValue = 0)
        {
            var str = objValue.ToString();
            if (string.IsNullOrWhiteSpace(str))
                return defValue;
            int.TryParse(str, out defValue);
            return defValue;
        }

        /// <summary>
        /// 返回Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象  例:T.ToJson()</param>
        /// <param name="isNullValueHandling">是否忽略Null字段，最终字符串中是否包含Null字段</param>
        /// <param name="indented">是否展示为具有Json格式的字符串</param>
        /// <param name="isLowCase">是否忽略大小写</param>
        /// <param name="dateTimeFormat">时间转换格式</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, bool isNullValueHandling = false, bool indented = false, bool isLowCase = false, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            var options = new JsonSerializerSettings();

            if (indented)
                options.Formatting = Formatting.Indented;

            if (isLowCase)
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();

            if (isNullValueHandling)
                options.NullValueHandling = NullValueHandling.Ignore;

            options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.Converters = new List<JsonConverter> { new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat } };
            return obj.ToJson(options);
        }

        /// <summary>
        /// Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象  例:T.ToJson(settings)</param>
        /// <param name="settings">Json序列化设置</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 返回相关设定格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format">格式 例：yyyy-MM-dd HH:mm:ss</param>
        /// <returns>设定格式字符串</returns>
        public static string ToDateTimeString(this object obj, string format)
        {
            DateTime.TryParse(obj?.ToString(), out var dateTime);
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 判断值是否为基础类型的默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool DefaultValue(this object obj, Type type)
        {
            if (type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(long))
                return obj.ToString() == "0";
            return obj == null;
        }
    }
}
