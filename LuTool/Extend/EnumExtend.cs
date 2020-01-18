using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LuTool
{
    /// <summary>
    ///  枚举拓展类
    /// </summary>
    public static class EnumExtend
    {
        /// <summary>
        /// Dictionary的线程安全的集合
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Dictionary<string, EnumItem>> EnumCache
            = new ConcurrentDictionary<Type, Dictionary<string, EnumItem>>();

        /// <summary>
        /// 将枚举值转换为int值
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        #region 获取备注

        /// <summary>
        /// 获取枚举定义的DescriptionAttribute值
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetDsc(this Enum value)
        {
            if (value == null) return "";
            FieldInfo field = value.GetType().GetField(value.ToString());
            return ((DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))).Description;
        }

        /// <summary>
        /// 获取枚举定义的DescriptionAttribute值
        /// </summary>
        /// <param name="value">枚举类型</param>
        /// <param name="eunmValue">枚举Int值</param>
        public static string GetDsc(this Enum value, int eunmValue)
        {
            return value.GetDsc(eunmValue.ToString());
        }

        /// <summary>
        /// 获取枚举定义的DescriptionAttribute值
        /// </summary>
        /// <param name="value">枚举类型</param>
        /// <param name="eunmValue">枚举String值</param>
        public static string GetDsc(this Enum value, string eunmValue)
        {
            Dictionary<string, EnumItem> eItem = value.GetItemList();
            string desStr = "";
            foreach (var item in eunmValue.Split(','))
            {
                desStr += "," + (eItem.ContainsKey(item) ? eItem[item].ItemDsc : "");
            }
            return desStr.TrimStart(',');
        }

        #endregion 获取备注

        /// <summary>
        /// 获取字典项集合信息
        /// </summary>
        /// <param name="value">字典</param>
        /// <returns></returns>
        public static List<EnumItem> GetEnumList(this Enum value)
        {
            List<EnumItem> tempValue = value.GetItemList().Values.ToList();
            return tempValue;
        }

        /// <summary>
        /// 获取字典项集合信息，可从缓存数据获取
        /// </summary>
        /// <param name="value">字典</param>
        /// <returns></returns>
        public static Dictionary<string, EnumItem> GetItemList(this Enum value)
        {
            Type eType = value.GetType();
            if (!EnumCache.ContainsKey(eType))
            {
                Type valueType = Enum.GetUnderlyingType(eType);
                var enums = Enum.GetValues(eType);
                Dictionary<string, EnumItem> tmpList = new Dictionary<string, EnumItem>();
                foreach (Enum e in enums)
                    tmpList.Add(Convert.ChangeType(e, valueType).ToString(), new EnumItem
                    {
                        ItemText = e.ToString(),
                        ItemValue = Convert.ChangeType(e, valueType).ToString(),
                        ItemDsc = e.GetDsc()
                    });
                EnumCache.TryAdd(eType, tmpList);
            }
            return EnumCache[eType];
        }
    }

    /// <summary>
    /// 字典Item信息
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// 字典值字段
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// 字典英文字段
        /// </summary>
        public string ItemValue { get; set; }

        /// <summary>
        /// 字典描述
        /// </summary>
        public string ItemDsc { get; set; }

        /// <summary>
        /// 字典值字段名称
        /// </summary>
        public const string ItemValueField = "ItemValue";

        /// <summary>
        /// 字典英文字段名称
        /// </summary>
        public const string ItemTextField = "ItemText";

        /// <summary>
        /// 字典值描述字段名称
        /// </summary>
        public const string ItemDscField = "ItemDsc";
    }
}
