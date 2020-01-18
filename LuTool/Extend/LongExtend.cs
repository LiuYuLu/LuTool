using System;

namespace LuTool
{
    /// <summary>
    /// Long类型的拓展方法
    /// </summary>
    public static class LongExtend
    {
        /// <summary>
        /// 转换时间戳
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            var length = timestamp.ToString().Length;
            if (length < 13)
            {
                var dto = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                return dto.ToLocalTime().DateTime;
            }
            else
            {
                var dto = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
                return dto.ToLocalTime().DateTime;
            }
        }
    }
}
