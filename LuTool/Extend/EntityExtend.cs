using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuTool
{
    /// <summary>
    /// 实体帮助类
    /// </summary>
    public static class EntityExtend
    {
        /// <summary>
        /// 父类的值赋给子类
        /// </summary>
        /// <typeparam name="Child">子类类型，需要返回的数据类型</typeparam>
        /// <typeparam name="Parnt">父类类型</typeparam>
        /// <param name="parent">父类数据实体</param>
        /// <returns></returns>
        public static Child AutoCopy<Child, Parnt>(this Parnt parent) where Child : Parnt, new() where Parnt : new()
        {
            Child child = new Child();
            var parentType = typeof(Parnt);

            var properties = parentType.GetProperties();

            foreach (var propertie in properties)
            {
                if (propertie.CanRead && propertie.CanWrite)
                {
                    propertie.SetValue(child, propertie.GetValue(parent, null), null);
                }
            }

            return child;
        }

        /// <summary>
        /// 用另外一个数据值 更新实体数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="ignoreCase">忽略大小写 默认false</param>
        /// <returns></returns>
        public static T1 AutoCopy<T1, T2>(this T1 t1, T2 t2, bool ignoreCase = false) where T1 : new() where T2 : new()
        {
            var t1properties = typeof(T1).GetProperties();
            var t2properties = typeof(T2).GetProperties();

            foreach (var t1propertie in t1properties)
            {
                try
                {
                    var t2Filter = ignoreCase ? t2properties.FirstOrDefault(m => m.Name.ToLower() == t1propertie.Name.ToLower()) : t2properties.FirstOrDefault(m => m.Name == t1propertie.Name);

                    if (t2Filter == null)
                        continue;
                    if (t2Filter.GetValue(t2, null).DefaultValue(t2Filter.PropertyType))
                        continue;
                    t1propertie.SetValue(t1, t2Filter.GetValue(t2, null), null);
                }
                catch (Exception ex)
                {
                    throw new Exception($"属性名称：{t1propertie.Name}，赋值错误。", ex);
                }
            }
            return t1;
        }

        /// <summary>
        /// 用json对象填充数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="jObj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T JsonCopy<T>(this T t, JObject jObj, StringFormatEnum type)
        {
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties)
            {
                if (jObj.ContainsKey(propertie.Name.FirstLetterToLow()))
                {
                    if (propertie.PropertyType.IsValueType)
                    {
                        if (propertie.PropertyType == typeof(bool))
                        {
                            propertie.SetValue(t, jObj[propertie.Name.FirstLetterToLow()].ToString().ToBool(), null);
                        }
                        else
                        {
                            propertie.SetValue(t, jObj[propertie.Name.FirstLetterToLow()].ToString().ToInt(), null);
                        }
                    }
                    else if (propertie.PropertyType.Name.StartsWith("String"))
                    {
                        propertie.SetValue(t, jObj[propertie.Name.FirstLetterToLow()].ToString(), null);
                    }
                    else if (propertie.PropertyType.Name.StartsWith("String"))
                    {
                        propertie.SetValue(t, jObj[propertie.Name.FirstLetterToLow()].ToString(), null);
                    }
                }
            }
            return t;
        }

        // todo:抽象成方法
        public static object GetValue(string value, Type type)
        {
            return value;
        }

        /// <summary>
        /// 获取修改的字段信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="present">更新后的值</param>
        /// <param name="original">未更新前的值</param>
        /// <returns></returns>
        public static List<EntityCompareResult> AutoCompare<T>(this T present, T original) where T : new()
        {
            var result = new List<EntityCompareResult>();
            var properties = typeof(T).GetProperties();

            foreach (var propertie in properties)
            {
                var presentObj = propertie.GetValue(present);
                var originalObj = propertie.GetValue(original);
                var originalValue = originalObj?.ToString() ?? "";
                var presentValue = presentObj?.ToString() ?? "";

                if (originalValue == presentValue) continue;

                var tempRslt = new EntityCompareResult
                {
                    Field = propertie.Name,
                    FieldName = propertie.Name,
                    OriginalValue = originalValue,
                    PresentValue = presentValue
                };

                if (originalValue != presentValue)
                {
                    result.Add(tempRslt);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取修改的字段信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="present">更新后的值</param>
        /// <param name="original">未更新前的值</param>
        /// <returns></returns>
        public static List<EntityCompareResult> AutoCompare<T>(this T present, List<EntityFieldData> original) where T : new()
        {
            var result = new List<EntityCompareResult>();
            var properties = typeof(T).GetProperties();

            foreach (var propertie in properties)
            {
                var presentObj = propertie.GetValue(present);
                var presentValue = presentObj?.ToString() ?? "";
                var originalValue = original.FirstOrDefault(m => m.FieldName == propertie.Name)?.FieldValue ?? "";

                if (presentValue == originalValue) continue;
                var tempRslt = new EntityCompareResult
                {
                    Field = propertie.Name,
                    FieldName = propertie.Name,
                    OriginalValue = originalValue,
                    PresentValue = presentValue
                };

                if (originalValue != presentValue)
                {
                    result.Add(tempRslt);
                }
            }
            return result;
        }

        /// <summary>
        /// 将对象数据扁平化输出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static List<EntityFieldData> GetFieldDatas<T>(this T val) where T : new()
        {
            var result = new List<EntityFieldData>();
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties)
            {
                var fieldVal = propertie.GetValue(val, null);
                if (fieldVal == null)
                {
                    result.Add(new EntityFieldData(propertie.Name, "", true));
                }
                else
                {
                    result.Add(new EntityFieldData(propertie.Name, fieldVal.ToString()));
                }
            }

            return result;
        }
    }
}