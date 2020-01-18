using System;

namespace LuTool
{
    /// <summary>
    /// 日期拓展类
    /// </summary>
    public static class DateTimeExtend
    {
        /// <summary>
        /// 获取某个日期是第几周
        /// </summary>
        /// <param name="date">任何一个有效的日期</param>
        /// <returns>返回指定日期是第几周</returns>
        public static int WeekOfYear(this DateTime date)
        {
            DateTime firstDayOfYear = new DateTime(date.Year, 1, 1);
            int weekDay = firstDayOfYear.DayOfWeek.ToInt();
            return (date.DayOfYear + weekDay - 2) / 7 + 1;
        }

        /// <summary>
        /// DateTime时间格式转换为UTC时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <param name="isMilliseconds">是否显示毫秒级时间戳，否->秒级</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ToTimeStamp(this DateTime time, bool isMilliseconds = true)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            // DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);//等价的建议写法

            DateTimeOffset dto = new DateTimeOffset(time);

            if (isMilliseconds)
                //return (long)(time - startTime).TotalMilliseconds;
                return dto.ToUnixTimeMilliseconds();
            else
                //return (long)(time - startTime).TotalSeconds;
                return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        /// DateTime时间格式转换为yyyy-MM-dd HH:mm:ss时间格式
        /// </summary>
        /// <param name="time"> DateTime时间</param>
        /// <returns>Unix时间戳格式</returns>
        public static string ToNormalFormat(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
