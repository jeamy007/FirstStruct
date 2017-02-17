using System.Collections.Generic;
using System.Linq;

namespace AppleView.Manage.UI
{
    public class Search
    {
        public enum KeyDataType { Int, String, Date };
        public Search() { }
        public Search(string express, object key, KeyDataType datatype = KeyDataType.String)
        {
            this.Key = key.ToString();
            this.Express = express;
            this.DataType = datatype;
        }
        public Search(string express, object key1, object key2, KeyDataType datatype = KeyDataType.Date)
        {
            this.Keys = new string[] { key1.ToString(), key2.ToString() };
            this.Values = new List<object>();
            this.Express = express;
            this.DataType = datatype;
        }
        public Search(string express, object key1, object key2, object defaultval1, object defaultval2, KeyDataType datatype = KeyDataType.Date)
        {
            this.Keys = new string[] { key1.ToString(), key2.ToString() };
            this.Values = new List<object>();
            this.DefaultValues = new List<object> { defaultval1, defaultval2 };
            this.Express = express;
            this.DataType = datatype;
        }
        public Search(string express, params string[] keys)
        {
            this.Keys = keys;
            this.Values = new List<object>();
            this.Express = express;
            this.DataType = KeyDataType.String;
        }
        /// <summary>
        /// 搜索键值
        /// </summary>
        public string Key { get; set; }
        public string[] Keys { get; set; }
        /// <summary>
        /// 搜索键对应的值
        /// </summary>
        public object Value { get; set; }
        public List<object> Values { get; set; }
        /// <summary>
        /// 搜索键对应的值 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        public List<object> DefaultValues { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public KeyDataType DataType { get; set; }
        public string Express { get; set; }
    }
}