using System;

namespace AppleDbop
{
    public class ModelClassAttribute : Attribute
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        public String TableName;
    }
    public partial class ModelDbopBase
    {
        /// <summary>
        /// 属性自定义扩展属性
        /// </summary>
        public class ModelFieldAttribute : Attribute
        {
            /// <summary>
            /// 主键（插入和更新时忽略）
            /// <para>更新和查找使用</para>
            /// </summary>
            public Boolean PrimaryKey;
            /// <summary>
            /// 自增键 设置该属性时，有以下特性：
            /// <para>新增和修改时忽略该属性字段</para>
            /// <para>模型新增完成后，该属性自动被赋值</para>
            /// </summary>
            public Boolean IdentityKey;
            /// <summary>
            /// 忽略
            /// <para>非数据库字段或忽略该字段的所有处理</para>
            /// </summary>
            public Boolean Ignore;
            /// <summary>
            /// 插入时忽略
            /// </summary>
            public Boolean IgnoreOnInsert;
            /// <summary>
            /// 更新时忽略
            /// </summary>
            public Boolean IgnoreOnUpdate;
            /// <summary>
            /// 数据库字段名
            /// </summary>
            public String FieldName;
            /// <summary>
            /// 使用数据库默认值
            /// <para>插入和更新时忽略</para>
            /// </summary>
            public Boolean UseDatabaseDefaultValue;
        }
    }
}
