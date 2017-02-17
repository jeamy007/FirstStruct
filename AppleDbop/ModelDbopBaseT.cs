using System;
using System.Data;
using System.Collections.Generic;

namespace AppleDbop
{
    public partial class ModelDbopBaseT<T> : ModelDbopBase where T : ModelDbopBase
    {
        #region 静态
        /// <summary>
        /// 获取单个模型
        /// <para>(可通过重写GetByDataRow函数更改填充模型数据方式)</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="wh">sql条件(where字符任意)</param>
        /// <returns></returns>
        public static T Find(string wh = "")
        {
            string sql = GetSelectSQL<T>(wh);
            if (string.IsNullOrEmpty(sql))
            {
                return default(T);
            }
            sql = sql.Insert(6, " TOP 1");
            DataTable dt = Dbop.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                return default(T);
            }
            return GetModelByDataRow<T>(dt.Rows[0]);
        }
        /// <summary>
        /// 获取列表模型
        /// <para>(可通过重写GetByDataRow函数更改填充模型数据方式)</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="wh">sql条件(where字符任意)</param>
        /// <returns></returns>
        public static List<T> FindList(string wh = "")
        {
            List<T> rl = new List<T>();
            string sql = GetSelectSQL<T>(wh);
            if (!string.IsNullOrEmpty(sql))
            {
                DataTable dt = Dbop.GetDataTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    rl.Add(GetModelByDataRow<T>(dr));
                }
            }
            return rl;
        }
        /// <summary>
        /// 查询列表通过主键值
        /// </summary>
        /// <param name="pkvalue"></param>
        /// <returns></returns>
        public static List<T> FindList(object pkvalue)
        {
            return FindList(string.Format(GetPrimaryKeySQLConditionExpress<T>(), pkvalue));
        }
        /// <summary>
        /// 批量加入
        /// </summary>
        /// <param name="objlist"></param>
        /// <returns></returns>
        public static KeyValuePair<bool, string> AddNewBatch(List<T> objlist)
        {
            Dictionary<string, object> paramList;
            foreach (T item in objlist)
            {
                paramList = new Dictionary<string, object>();
                Dbop.ExecuteSqlSuc(SQLInsert<T>(item, paramList), paramList);
            }
            return new KeyValuePair<bool, string>(true, null);
        }

        /// <summary>
        /// 删除操作（根据主键）
        /// </summary>
        /// <param name="pkvalue">主键值</param>
        /// <returns></returns>
        public static KeyValuePair<bool, string> Remove(object pkvalue)
        {
            string sql = SQLDelete<T>(pkvalue);            
            return new KeyValuePair<bool, string>(Dbop.ExecuteSqlSuc(sql), null);
        }
        /// <summary>
        /// 存在数据
        /// </summary>
        /// <param name="wh"></param>
        /// <returns></returns>
        public static bool ExistsData(string wh)
        {
            string sql = GetSelectSQL<T>(wh);
            return Dbop.HavRow(sql);
        }

      
        #endregion
    }
}
