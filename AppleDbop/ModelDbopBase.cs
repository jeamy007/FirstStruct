using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppleDbop
{
    public abstract partial class ModelDbopBase //: SDIModelDbop.Interface.IModelDbop
    {
        //public ModelDbopBase()
        //{
        //}
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        public static IDbHandle.IDbop Dbop;

        #region 缓存变量
        /// <summary>
        /// 模型类属性信息缓存
        /// </summary>
        private static Dictionary<Type, Cache_ModelInfor>
            GLB_CACHE_MODELPROPERTYS = new Dictionary<Type, Cache_ModelInfor>();
        #endregion
        /// <summary>
        /// 添加新模型
        /// </summary>
        public virtual KeyValuePair<bool, string> AddNew()
        {
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            //return new KeyValuePair<bool, string>(false, this.SQLInsert(paramList));
            bool b_exec;
            string sql = this.SQLInsert(paramList);

            #region 自增属性赋值处理
            Type ttp = this.GetType();
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(ttp))
            {
                var idfprop = GLB_CACHE_MODELPROPERTYS[ttp].IdentityProperty;
                if (idfprop != null)
                {
                    sql += " SELECT SCOPE_IDENTITY() c";
                    int idfval = Dbop.GetSingleInt(sql, paramList);
                    if (idfval > 0)
                    {
                        PropertySetValue(this, idfprop,idfval);
                        b_exec = true;
                    }
                    else
                    {
                        b_exec = false;
                    }
                    return new KeyValuePair<bool, string>(b_exec, null);
                }
            }
            #endregion

            b_exec = Dbop.ExecuteSqlSuc(sql, paramList);
            return new KeyValuePair<bool, string>(b_exec, null);
        }

        /// <summary>
        /// 更新模型
        /// </summary>
        /// <param name="wh">条件</param>
        /// <returns></returns>
        public virtual KeyValuePair<bool, string> Update(string wh)
        {
            Dictionary<string, object> paramList = new Dictionary<string, object>();
            //return new KeyValuePair<bool, string>(false, this.SQLUpdate(wh, paramList));
            return new KeyValuePair<bool, string>(Dbop.ExecuteSqlSuc(this.SQLUpdate(wh, paramList), paramList), null);
        }
        /// <summary>
        /// 更新模型
        /// <para>根据模型主键和主键已赋的值更新</para>
        /// </summary>
        /// <returns></returns>
        public virtual KeyValuePair<bool, string> Update()
        {
            string w = this.GetPrimaykeySQLConditionExpress();
            if(string.IsNullOrEmpty(w))
            {
                throw new Exception(GLB_GetDBTableName() + "未设置模型主键，请设置或使用Update(string)");
                //return new KeyValuePair<bool, string>(false, "未设置模型主键，请设置或使用Update(string)");
            }
            return Update(w);
        }
        /// <summary>
        /// 移除模型
        /// </summary>
        public virtual KeyValuePair<bool, string> Remove(string wh)
        {
            string sql = SQLDelete(this.GLB_GetDBTableName(), wh);
            return new KeyValuePair<bool, string>(Dbop.ExecuteSqlSuc(sql), null);
        }
        /// <summary>
        /// 移除模型
        /// <para>根据模型主键和主键已赋的值移除</para>
        /// </summary>
        public virtual KeyValuePair<bool, string> Remove()
        {
            string w = this.GetPrimaykeySQLConditionExpress();
            if (string.IsNullOrEmpty(w))
            {
                throw new Exception(GLB_GetDBTableName() + "未设置模型主键，请设置或使用Remove(string)");
                
            }
            return Remove(w);
        }
        #region 辅助 [待优化]
        /// <summary>
        /// 获取主键值
        /// </summary>
        /// <returns></returns>
        public int GetPrimaryKeyValueInt()
        {
            Type ttp = this.GetType();
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(ttp))
            {
                var pkprop = GLB_CACHE_MODELPROPERTYS[ttp].PrimaryKeyProperty;
                if (pkprop != null)
                {
                    int _r;
                    int.TryParse((pkprop.GetValue(this, null) ?? "") + "", out _r);
                    return _r;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取主键值
        /// </summary>
        public string GetPrimaryKeyValue()
        {
            Type ttp = this.GetType();
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(ttp))
            {
                var pkprop = GLB_CACHE_MODELPROPERTYS[ttp].PrimaryKeyProperty;
                if (pkprop != null)
                {
                    return (pkprop.GetValue(this, null) ?? string.Empty) + "";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 主键值有值
        /// </summary>
        public bool HasValueParimaryKey()
        {
            Type ttp = this.GetType();
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(ttp))
            {
                var _mp = GLB_CACHE_MODELPROPERTYS[ttp];
                var pkprop = _mp.PrimaryKeyProperty;
                if (pkprop != null)
                {
                    object v = pkprop.GetValue(this, null);
                    if (_mp.IsIntPrimaryKey)
                    {
                        return (int)v != 0;
                    }
                    return v != null && !string.IsNullOrEmpty(v + "");
                }
            }
            return false;
        }
        /// <summary>
        /// 获取主键条件语句表达式
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryKeySQLConditionExpress<T>() where T : ModelDbopBase
        {
            Type ttp = typeof(T);
            if (GLB_CACHE_MODELPROPERTYS.ContainsKey(ttp))
            {
                var _mp = GLB_CACHE_MODELPROPERTYS[ttp];
                return _mp.PrimaryKeySQLConditionExpress;
            }
            return string.Empty;
        }
        //public string GetPrimaryKeySQLConditionExpress()
        //{
        //    return GetPrimaryKeySQLConditionExpress<>()
        //}
        #endregion
    }
}
