using System;

namespace LuTool
{
    /// <summary>
    /// 单例模式基类
    /// 具有双重锁
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        private static volatile T instance;
        private static object objSync = new object();

        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null) //第一层锁，保证每次不适用线程的私有缓存区，从公共区域读取，保证最新
                {
                    lock (objSync) //第二层锁，强制多个线程进行排队，一个接一个的去访问
                    {
                        if (instance == null)
                        {
                            instance = (T)Activator.CreateInstance(typeof(T), true);
                        }
                    }
                }
                return instance;
            }
        }
    }
}