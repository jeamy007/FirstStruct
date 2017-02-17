using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

namespace AppleDbop
{
    public partial class ModelDbopBase
    {
        /// <summary>
        /// 获取模型对应数据库表名
        /// </summary>
        /// <returns></returns>
        protected static string GLB_GetDBTableName(System.Type type)
        {
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(type))
            {
                return GLB_CACHE_MODELPROPERTYS[type].TableName;
            }
            else
            {
                return GetPropertyTableName(type);
            }
        }
        /// <summary>
        /// 获取主键条件表达式
        /// </summary>
        private string GetPrimaykeySQLConditionExpress()
        {
            return GetPrimaykeySQLConditionExpress(this);
        }
        /// <summary>
        /// 获取主键条件表达式
        /// </summary>
        private static string GetPrimaykeySQLConditionExpress<T>(T model) where T : ModelDbopBase
        {
            Cache_ModelInfor infor = LoadModelProperty(model.GetType());
            if (string.IsNullOrEmpty(infor.PrimaryKeyFieldName))
            {
                return string.Empty;
            }
            object val = GetPropertyValue<T>(model, infor.PrimaryKeyFieldName);
            return string.Format(infor.PrimaryKeySQLConditionExpress, val);
        }
        /// <summary>
        /// 默认通过模型解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>

        public static T GetModelByDataRow<T>(DataRow dr) where T : ModelDbopBase
        {
            T obj = Activator.CreateInstance<T>();
            object cellValue;
            EachPropertys(typeof(T), (pi) =>
            {
                cellValue = dr[pi.FieldName];
                PropertySetValue(obj, pi.Property, cellValue);
            });
            return obj;
        }
        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <param name="val"></param>
        private static void PropertySetValue<T>(T obj, PropertyInfo p, object val)
        {
            // 暂时没有考虑泛型 IsGenericType
            if (val != null && val != System.DBNull.Value)
            {
                p.SetValue(obj, val, null);
            }
        }

        private static void _getModelSqlInsert<T>(T model, Dictionary<string, object> sqlparams, List<string> fields, List<string> values)
        {
            _getModelSql(model, sqlparams, (f, v) =>
            {
                fields.Add(f);
                values.Add(v);
            }, (pi) =>
            {
                if (pi.PropertyModelAttr != null)
                {
                    return pi.PropertyModelAttr.IgnoreOnInsert;
                }
                return false;
            });
        }
        private static void _getModelSqlUpdate<T>(T model, Dictionary<string, object> sqlparams, Action<string, string> funcEachField)
        {
            _getModelSql(model, sqlparams, funcEachField,
                (pi) =>
                {
                    if(pi.PropertyModelAttr != null)
                    {
                        return pi.PropertyModelAttr.IgnoreOnUpdate;
                    }
                    return false;
                });
        }
        private static void _getModelSql<T>(T model, Dictionary<string, object> sqlparams, Action<string, string> funcEachField, Func<Cache_ModelInfor_Property, bool> funcIgnore = null)
        {
            string propName;
            EachPropertys(model.GetType()
                , (pi) =>
                {
                    propName = pi.FieldName;
                    sqlparams.Add(propName, GetPropertyValue(model, pi.Property.Name));
                    funcEachField(propName, "@" + propName);
                }
                , (pi) =>
                {
                    if (funcIgnore == null)
                    {
                        return PropModifyIgnore(pi, null);
                    }
                    return PropModifyIgnore(pi, (attr) => { return funcIgnore(pi); });
                });
        }

        /// <summary>
        /// 遍历可用属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="funcEach"></param>
        /// <param name="funcIgnore"></param>
        private static void EachPropertys(Type type, Action<Cache_ModelInfor_Property> funcEach, Func<Cache_ModelInfor_Property, bool> funcIgnore = null)
        {
            if(funcIgnore == null)
            {
                // 默认包含
                funcIgnore = (pi) => { return false; };
            }
            LoadModelProperty(type, funcEach, funcIgnore);
        }

        /// <summary>
        /// 装载模型属性
        /// </summary>
        private Cache_ModelInfor LoadModelProperty()
        {
            return LoadModelProperty(this.GetType());
        }
        /// <summary>
        /// 装载模型属性
        /// </summary>
        private static Cache_ModelInfor LoadModelProperty(Type type)
        {
            return LoadModelProperty(type, (pi) => { }, (pi) => { return false; });
        }
        /// <summary>
        /// 装载模型属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="funcEach"></param>
        /// <param name="funcIgnore"></param>
        private static Cache_ModelInfor LoadModelProperty(Type type, Action<Cache_ModelInfor_Property> funcEach, Func<Cache_ModelInfor_Property, bool> funcIgnore)
        {
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(type))
            {
                Cache_ModelInfor cminfo = GLB_CACHE_MODELPROPERTYS[type];
                List<Cache_ModelInfor_Property> t_props = cminfo.PropertyInfoList;
                foreach (Cache_ModelInfor_Property pi in t_props)
                {
                    if (!funcIgnore(pi))
                    {
                        funcEach(pi);
                    }
                }
                return cminfo;
            }
            else
            {
                List<Cache_ModelInfor_Property> t_props;
                t_props = new List<Cache_ModelInfor_Property>();
                string 
                    fieldname,
                    pkname = string.Empty,
                    pksqlexp = string.Empty;
                bool b_pkint = false;
                Cache_ModelInfor_Property porpinfor;
                PropertyInfo prop_idf = null, prop_pk = null;
                ModelFieldAttribute attr;
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.CanWrite)
                    {
                        #region 属性处理
                        object[] attrs = p.GetCustomAttributes(false);
                        if (attrs.Length > 0)
                        {
                            attr = (ModelFieldAttribute)attrs[0];
                            // 直接忽略
                            if (attr.Ignore)
                            {
                                continue;
                            }
                            // 获取自定义字段名
                            if (string.IsNullOrEmpty(attr.FieldName))
                            {
                                fieldname = p.Name;
                            }
                            else
                            {
                                fieldname = attr.FieldName;
                            }
                            // 获取自增键
                            if (attr.IdentityKey)
                            {
                                prop_idf = p;
                            }
                            // 获取主键
                            if (attr.PrimaryKey)
                            {
                                prop_pk = p;
                                pkname = fieldname;
                                // 主键仅识别字符串类型和非字符串类型
                                if (p.PropertyType == typeof(string))
                                {
                                    pksqlexp = pkname + "='{0}'";
                                }
                                else
                                {
                                    b_pkint = true;
                                    pksqlexp = pkname + "={0}";
                                }
                            }
                        }
                        else
                        {
                            fieldname = p.Name;
                            attr = null;
                        }
                        #endregion
                        porpinfor = new Cache_ModelInfor_Property
                        {
                            FieldName = fieldname,
                            Property = p,
                            PropertyModelAttr = attr
                        };

                        t_props.Add(porpinfor);

                        try
                        {
                            if (!funcIgnore(porpinfor))
                            {
                                funcEach(porpinfor);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                string key_tablename = GLB_GetDBTableName(type);
                Cache_ModelInfor r =  new Cache_ModelInfor
                    {
                        TableName = key_tablename,
                        PrimaryKeyProperty = prop_pk,
                        PrimaryKeyFieldName = pkname,
                        PrimaryKeySQLConditionExpress = pksqlexp,
                        IdentityProperty = prop_idf,
                        PropertyInfoList = t_props,
                        IsIntPrimaryKey = b_pkint
                };
                GLB_CACHE_MODELPROPERTYS.Add(type, r);
                return r;
            }
        }

        /// <summary>
        /// 属性修改时忽略
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static bool PropModifyIgnore(Cache_ModelInfor_Property propinfor, Func<ModelFieldAttribute, bool> func)
        {
            if(func == null)
            {
                func = (a) => { return false; };
            }
            ModelFieldAttribute attr = propinfor.PropertyModelAttr;
            if (attr != null)
            {
                if (attr.IdentityKey || attr.UseDatabaseDefaultValue || func(attr))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 根据属性名获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        private static object GetPropertyValue<T>(T Model, string PropertyName)
        {
            Type theType = Model.GetType();
            PropertyInfo pi = theType.GetProperty(PropertyName);
            return pi.GetValue(Model, null);
        }
        private static SqlDbType GetDbType(Type type)
        {
            if (type == typeof(int))
            {
                return SqlDbType.Int;
            }
            return SqlDbType.NVarChar;
        }

        protected static string GetPropertyTableName(Type type)
        {
            var props = type.GetCustomAttributes(true);
            if (props.Length > 0)
            {
                return ((ModelClassAttribute)(props[0])).TableName;
            }
            return type.Name;
        }
    }
}
