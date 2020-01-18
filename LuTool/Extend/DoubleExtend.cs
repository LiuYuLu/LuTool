using System;

namespace LuTool
{
    /// <summary>
    /// DoubleD的拓展方法
    /// </summary>
    public static class DoubleExtend
    {
        /// <summary>
        /// 精确Double小数位数，返回double类型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="length">小数点后位数</param>
        /// <param name="multiple">倍数,如果是百分制则乘以100</param>
        /// <returns></returns>
        public static double Round(this double value, int length, int multiple = 1)
        {
            return Math.Round(value * multiple, length);
        }

        /// <summary>
        /// 精确Double小数位数，返回double类型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="length">小数点后位数</param>
        /// <param name="multiple">倍数,如果是百分制则乘以100</param>
        /// <returns></returns>
        [Obsolete("推荐使用Round方法替代")]
        public static double RoundV1(this double value, int length, int multiple = 1)
        {
            var str = (value * multiple).ToString($"F{length}");
            return Double.Parse(str);
        }

        /// <summary>
        /// 判断Double类型是 0/0 的接口，不是一个数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNaN(this double value)
        {
            return double.IsNaN(value);
        }

        /// <summary>
        /// 修复Double类型当被除数为0时的异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static double FixValue(this double value, double defValue = 0D)
        {
            if (double.IsNaN(value))
                return defValue;
            if (double.IsInfinity(value))
                return defValue;
            return value;
        }
    }
}
