using CommonTools;
using System.Collections.Generic;

namespace AppleHelp
{
    /// <summary>
    /// Session池
    /// </summary>
    public class SessionBase//:ICache
    {
        private static List<string> SessionNames = new List<string>();
        /// <summary>
        /// 获取指定Session名称的值
        /// </summary>
        public static object Get(string name)
        {
            return System.Web.HttpContext.Current.Session[name];
        }
        /// <summary>
        /// 设置指定Session名称的值
        /// </summary>
        public static void Set(string name, object val)
        {
            if(!SessionNames.Contains(name))
            {
                SessionNames.Add(name);
            }
            System.Web.HttpContext.Current.Session[name] = val;
        }
        /// <summary>
        /// 获取指定Session名称的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            return System.Web.HttpContext.Current.Session[name].ConvertType<T>();
            //return CommonTools.Extension.ConvertType<T>(Context.Current.Session[name]);
        }
        /// <summary>
        /// 清除Session
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void ClearAll(params string[] args)
        {
            foreach (var name in args)
            {
                System.Web.HttpContext.Current.Session[name] = null;
            }
        }

        /// <summary>
        /// 清除Session 所有设置过的
        /// </summary>
        public static void ClearAll()
        {
            ClearAll(SessionNames.ToArray());
        }
        //public object Get(string name)
        //{
        //    return Context.Current.Session[name];
        //}

        //public void Set(string name, object val)
        //{
        //    Context.Current.Session[name] = val;
        //}

        //public T Get<T>(string name)
        //{
        //    return this.Get(name).ConvertType<T>();
        //}

        //public void Clear(string name)
        //{
        //    this.Set(name, null);
        //}
    }
}