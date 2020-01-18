using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// 实体帮助类
    /// </summary>
    public class EntityHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体内容</param>
        /// <returns></returns>
        public string GetProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1},", name, value);
                }
                else
                {
                    GetProperties(value);
                }
            }
            return tStr;
        }

        /// <summary>
        /// 把实体转换成Post提交的数据,返回值是空值时，请加上验证
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体内容</param>
        /// <returns></returns>
        public static string ConvetToPostdata<T>(T t)
        {
            StringBuilder sb = new StringBuilder();
            if (t == null)
            {
                return sb.ToString();
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return sb.ToString();
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    sb.Append(string.Concat(name, "=", value, "&"));
                }
                else
                {
                    if (value == null)
                    {
                        sb.Append(string.Concat(name, "=&"));
                    }
                    else
                    {
                        //ConvetToPostdata(value);
                    }
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 把实体转换成Post提交的数据,返回值是空值时，请加上验证
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体内容</param>
        /// <param name="dataKey">需要的key</param>
        /// <returns></returns>
        /// <remarks>请加Try Catch()</remarks>
        public static string ConvetToPostdataFromModel<T>(T t, List<string> dataKey)
        {
            StringBuilder sb = new StringBuilder();
            if (t == null)
            {
                return string.Empty;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return string.Empty;
            }
            //todo:应该将需要序列化的数据转成必填和可选类型，这样就不会出错
            foreach (var keyName in dataKey.Distinct())
            {
                string name = keyName;
                var modelItem = properties.FirstOrDefault(m => m.Name == name);
                if (modelItem == null)
                {
                    //RecordSystemLog.WriteErrorLog(new Exception("把实体转换成Post提交的数据,字段：" + name + "没有匹配的值。"), requestParams: JsonConvert.SerializeObject(t));
                    return string.Empty;
                }
                object value = modelItem.GetValue(t, null);
                if (modelItem.PropertyType.IsValueType || modelItem.PropertyType.Name.StartsWith("String"))
                {
                    sb.Append(string.Concat(name, "=", value, "&"));
                }
                else
                {
                    //RecordSystemLog.WriteErrorLog(new Exception("把实体转换成Post提交的数据,字段：" + name + "的值有误"), requestParams: JsonConvert.SerializeObject(t));
                    return string.Empty;
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 执行自定义方法
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>Tuple<bool, string></returns>
        public object ExecuteFlowCustomEvent(string eventName, object[] eventParams, string dllName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(dllName);
            string typeName = System.IO.Path.GetFileNameWithoutExtension(eventName);
            string methodName = eventName.Substring(typeName.Length + 1);
            Type type = assembly.GetType(typeName, true);

            object obj = System.Activator.CreateInstance(type, false);
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                return method.Invoke(obj, eventParams);
            }
            else
            {
                var errorMsg = string.Format(@"DLL名称：{0}，方法名称：{1}，请求参数：{2}", dllName, eventName, JsonConvert.SerializeObject(eventParams));
                return new Tuple<bool, string>(false, "找不到方法异常了");
            }
        }

        /// <summary>
        /// 把Request.Form 转换成实体数据保存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">Form表单</param>
        /// <returns></returns>
        public static T ConvertRequestFormToModel<T>(NameValueCollection form) where T : class, new()
        {
            T t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return t;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                if (form[name] != null)
                {
                    Type type = item.PropertyType;
                    if (type == typeof(int))
                    {
                        item.SetValue(t, form[name].ObjToInt(), null);
                    }
                    else if (type == typeof(string))
                    {
                        item.SetValue(t, form[name].ToString(), null);
                    }
                }
            }
            return t;
        }
    }
}