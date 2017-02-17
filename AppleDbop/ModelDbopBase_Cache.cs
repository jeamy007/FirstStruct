using System;
using System.Reflection;
using System.Collections.Generic;

namespace AppleDbop
{
    /// <summary>
    /// 属性自定义扩展属性
    /// </summary>
    public partial class ModelDbopBase 
    {
        /// <summary>
        /// 缓存 模型属性信息类
        /// </summary>
        private class Cache_ModelInfor
        {
            private string _TableName;
            private string _PrimaryKeyFieldName;
            private PropertyInfo _IdentityProperty;
            private PropertyInfo _PrimaryKeyProperty;
            /// <summary>
            /// 模型字段属性列表
            /// </summary>
            public List<Cache_ModelInfor_Property> PropertyInfoList = new List<Cache_ModelInfor_Property>();

            /// <summary>
            /// 模型数据库表名
            /// </summary>
            public string TableName
            {
                get { return _TableName; }
                set { _TableName = value; }
            }
            /// <summary>
            /// 模型数据库表主键字段名
            /// <para>智能用于更新和查找</para>
            /// </summary>
            public string PrimaryKeyFieldName
            {
                get { return _PrimaryKeyFieldName; }
                set { _PrimaryKeyFieldName = value; }
            }

            /// <summary>
            /// 主键条件表达式
            /// </summary>
            public string PrimaryKeySQLConditionExpress;

            /// <summary>
            /// 模型自增属性
            /// </summary>
            public PropertyInfo IdentityProperty
            {
                get { return _IdentityProperty; }
                set { _IdentityProperty = value; }
            }
            /// <summary>
            /// 模型主键属性
            /// </summary>
            public PropertyInfo PrimaryKeyProperty
            {
                get { return _PrimaryKeyProperty; }
                set { _PrimaryKeyProperty = value; }
            }
            /// <summary>
            /// 主键为数值类型
            /// </summary>
            public bool IsIntPrimaryKey;
        }
        /// <summary>
        /// 缓存 模型属性信息 属性类
        /// </summary>
        private class Cache_ModelInfor_Property
        {
            /// <summary>
            /// 字段名
            /// </summary>
            public string FieldName { get; set; }
            /// <summary>
            /// 属性
            /// </summary>
            public PropertyInfo Property { get; set; }
            /// <summary>
            /// 属性的自定义属性
            /// </summary>
            public ModelFieldAttribute PropertyModelAttr { get; set; }
        }
    }
}
