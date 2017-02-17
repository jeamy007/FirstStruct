using System;
using System.Data;
using System.Collections.Generic;

namespace AppleDbop
{
    public partial class ModelDbopBase
    {
        #region sql语句
        protected string GLB_GetDBTableName()
        {
            return GLB_GetDBTableName(this.GetType());
        }
        protected virtual string SQLInsert(Dictionary<string, object> sqlparams)
        {
            return SQLInsert(this, sqlparams);
        }
        protected virtual string SQLUpdate(string wh, Dictionary<string, object> sqlparams)
        {
            return SQLUpdate(this, wh, sqlparams);
        }



        protected virtual string SQLDeleteModel (string wh) 
        {
            return SQLDelete(this,wh); 
        }
        protected static string SQLDelete<T>(T t,string wh) where T : ModelDbopBase
        {
            return SQLDelete<T>(wh);
        }

 


        protected static string SQLInsert<T>(T model, Dictionary<string, object> sqlparams) where T : ModelDbopBase
        {
            return _getModelSql_Insert<T>(model, sqlparams);
        }

        protected static string SQLUpdate<T>(T model, string wh, Dictionary<string, object> sqlparams) where T : ModelDbopBase
        {
            return _getModelSql_Update<T>(model, wh, sqlparams);
        }
        /// <summary>
        /// 删除sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkvalue"></param>
        /// <returns></returns>
        protected static string SQLDelete<T>(object pkvalue) where T:ModelDbopBase
        {
            Cache_ModelInfor minfor = LoadModelProperty(typeof(T));
            string pkexp = minfor.PrimaryKeySQLConditionExpress;
            if(string.IsNullOrEmpty(pkexp))
            {
                throw new System.Exception("无主键");
            }
            string cn = string.Format(minfor.PrimaryKeySQLConditionExpress, pkvalue);
            return SQLDelete(minfor.TableName, cn);
        }
        protected static string SQLDelete(string tableName, string where)
        {
            return string.Format("DELETE {0} WHERE {1}", tableName, where);
        }
        protected static string SQLSelect<T>() where T : ModelDbopBase
        {
            return "SELECT * FROM " + GLB_GetDBTableName(typeof(T));
        }


        public static string GetSelectSQL<T>(string wh) where T : ModelDbopBase
        {
            string sql = SQLSelect<T>().TrimStart();
            if (string.IsNullOrEmpty(sql))
            {
                return "";
            }
            #region
            sql = sql + _getWhereSQL<T>(wh);
            #endregion
            return sql;
        }
        private static string _getWhereSQL<T>(string wh) where T : ModelDbopBase
        {
            if (!string.IsNullOrEmpty(wh))
            {
                wh = wh.TrimStart();
                string _t = wh.ToUpper();
                if (_t.StartsWith("WHERE ") || _t.StartsWith("ORDER "))
                {
                    return " " + wh;
                }
                else if (_t.StartsWith("AND "))
                {
                    wh = wh.Substring(4);
                }
                return " WHERE " + wh;
            }
            return wh;
        }

        private static string _getModelSql_Insert<T>(T model, Dictionary<string, object> sqlparams) where T : ModelDbopBase
        {
            List<string>
                fileds = new List<string>(),
                values = new List<string>();
            _getModelSqlInsert(model, sqlparams, fileds, values);
            return string.Format("INSERT {0}({1}) VALUES({2})", GLB_GetDBTableName(model.GetType()), string.Join(",", fileds), string.Join(",", values));
        }

        private static string _getModelSql_Update<T>(T model, string wh, Dictionary<string, object> sqlparams) where T : ModelDbopBase
        {
            List<string>
                fileds = new List<string>(),
                values = new List<string>(),
                sqlprt = new List<string>();
            _getModelSqlUpdate(model, sqlparams, (field, val) =>
            {
                sqlprt.Add(string.Format("{0}={1}", field, val));
            });
            return string.Format("UPDATE {0} SET {1}{2}", GLB_GetDBTableName(model.GetType()), string.Join(",", sqlprt), _getWhereSQL<T>(wh));
        }

        #endregion
    }
}
