using System;
using LuTool;

namespace LuTool
{
    /// <summary>
    /// String类型帮助类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 生成GUID（全局统一标识符）并返回值
        /// </summary>
        /// <param name="replace">是否替换“-”</param>
        /// <returns></returns>
        public static string GetGuid(bool replace = false)
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return replace ? guid.ToString().Replace("-", "") : guid.ToString();
        }

        /// <summary>
        /// 字符串操作委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Func<string,string> StringFormat(StringFormatEnum type)
        {
            switch (type)
            {
                case StringFormatEnum.Up: return str => { return str.ToLower(); };
                default: return str => { return str; };
            }
        }
    }
}