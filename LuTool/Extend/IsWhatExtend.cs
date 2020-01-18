using System;
using System.Collections.Generic;
using System.Linq;

namespace LuTool
{
    /// <summary>
    /// 所有的方法都是IS开头的判断类型，例如：判断是否为空，是否有值
    /// </summary>
    public static class IsWhatExtend
    {
        #region IsNullOrEmpty

        /// <summary>
        /// 判断一个Guid是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? value)
        {
            if (value == null) return true;
            return value == Guid.Empty;
        }

        /// <summary>
        /// 判断IEnumerable是否为null或者空值
        /// </summary>
        /// <param name="value">当前集合</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        {
            if (value == null || !value.Any()) return true;
            return false;
        }

        #endregion IsNullOrEmpty

        #region IsValuable

        /// <summary>
        /// 判断当前值，流程是否不为null且不为空
        /// </summary>
        /// <param name="value">当前值</param>
        /// <returns></returns>
        public static bool IsValuable<T>(this IEnumerable<T> value)
        {
            if (value == null || !value.Any()) return false;
            return true;
        }

        /// <summary>
        /// 判断字符串不为null,且有内容.
        /// </summary>
        /// <param name="value">字符</param>
        /// <returns></returns>
        public static bool IsValuable(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        #endregion IsValuable
    }
}
