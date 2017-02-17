using CommonTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace AppleHelp
{
    public abstract class BasePageObj : CommonTools.UI.BasePageObj
    {
        /// <summary>
        /// json 字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string JsonString(object o)
        {
            return Json.JsonConvert.ToString(o);
        }
        /// <summary>
        /// json 字符串解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public T DeserializeJson<T>(string s)
        {
            return Json.JsonConvert.ToObject<T>(s);
        }
        //public string JsonString(DataRow dr)
        //{
        //    return JsonString(DictionaryModel.Get_SO(dr));
        //}
        #region 默认表单处理
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if(IsPost())
            {
                DefaultPostEvent(FormAction);
            }
            else
            {
                OnGetPage();
            }
        }
        protected virtual void OnGetPage() { }

        /// <summary>
        /// post 预处理
        /// </summary>
        /// <param name="action"></param>
        protected virtual void DefaultPostEvent(string action)
        {
            if (PostEventList.ContainsKey(action))
            {
                PostEventList[action]();
            }
        }
        #region 默认表单事件注册
        private Dictionary<string, Action> PostEventList = new Dictionary<string, Action>();
        /// <summary>
        /// 注册POST action事件
        /// </summary>
        /// <param name="action">动作名</param>
        /// <param name="func">执行过程</param>
        protected void PostEventRegister(string action, Action func)
        {
            PostEventList.Add(action, func);
        }
        protected void PostEventRegister(params object[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                PostEventList.Add(args[0] + "", args[1] as Action);
            }
        }
        protected void PostEventUnRegister(string action)
        {
            if (PostEventList.ContainsKey(action))
            {
                PostEventList.Remove(action);
            }
        }
        #endregion

        /// <summary>
        /// 获取表单数据(data)对象
        /// </summary>
        protected T GetPostDataObject<T>()
        {
            string data = Request.Form["data"];
            if (string.IsNullOrEmpty(data))
                return default(T);
            return DeserializeJson<T>(data);
        }
        /// <summary>
        /// 获取表单对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPostObject<T>()
        {
            return DeserializeJson<T>(Request.Form.ToString());
        }
        /// <summary>
        /// 获取表单数据对象，放置回调参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        protected void GetPostDataObject<T>(Action<T> action)
        {
            T obj = GetPostDataObject<T>();
            if (obj == null)
            {
                ResponseWriteJsonResult(false);
            }
            action(obj);
        }
        #endregion
        
        #region paging
        #region op
        private int? _pagingSizeNumber;
        /// <summary>
        /// 行数
        /// </summary>
        protected int? PagingSizeNumber
        {
            get
            {
                if (_pagingSizeNumber == null)
                {
                    string r = Request.QueryString[PagingHelper.ParamName_Size];
                    if (!string.IsNullOrEmpty(r))
                        _pagingSizeNumber = SafeObjectInt(r);
                }
                return _pagingSizeNumber;
            }
        }

        /// <summary>
        public virtual void SetPagingSize()
        {
            if (PagingSizeNumber != null)
            {
                PagingSize = PagingSizeNumber.Value;
                PagingParams.Add(PagingHelper.ParamName_Size, PagingSizeNumber.Value);
            }
        }
        /// 简单分页 获取sql行数和页码数据集 并输出json字符串
        /// </summary>
        protected string PaingSizeParams
        {
            get
            {
                return PagingParamsStrEscape(PagingHelper.ParamName_Size);
            }
        }
        /// <param name="sql"></param>
        /// <param name="sqlOrder"></param>
        /// <param name="pagingsize"></param>
        /// <param name="pagingnumber"></param>
        /// <returns></returns>
        protected string PagingParamsStrEscape(params string[] names)
        {
            if (names.Length == 0)
            {
                return PagingParamsStr;
            }
            if (PagingParams == null || PagingParams.Count == 0)
            {
                return "";
            }
            var _p = new Dictionary<string, object>(PagingParams);
            foreach (var key in names)
            {
                _p.Remove(key);
            }
            return PagingParamsKeyValueStr(_p);
        }
        private int pagingNumber = 0;
        protected int PagingNumber
        {
            get
            {
                if (pagingNumber == 0)
                {
                    pagingNumber = SafeQueryInt(PagingHelper.ParamName);
                    if (pagingNumber <= 0)
                        pagingNumber = 1;
                }
                return pagingNumber;
            }
        }
        protected PagingHelperObj PagingObj;
        /// <summary>
        protected int PagingSize = 20;
        /// <summary>
        /// 获取分页表格 和 分页导航链接
        /// </summary>
        /// <param name="dbop"></param>
        /// <param name="sql">sql语句 不含排序</param>
        /// <param name="sqlOrder">排序sql</param>
        protected DataTable PagingTableAndObj(IDbHandle.IDbop dbop, string sql, string sqlOrder)
        {
            DataTable r = PagingTable(dbop, sql, sqlOrder);
            PagingObj = new PagingHelperObj(RowCount, PagingSize, PagingParamsStr);
            return r;
        }
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="dbop"></param>
        /// <param name="sql">sql语句 不含排序</param>
        /// <param name="sqlOrder">排序sql</param>
        protected DataTable PagingTable(IDbHandle.IDbop dbop, string sql, string sqlOrder)
        {
            return PagingTable(dbop, sql, sqlOrder, PagingSize, PagingNumber, out RowCount);
        }

        /// <summary>
        /// 链接附带参数 列表（不含页码）
        /// </summary>
        protected Dictionary<string, object> PagingParams = new Dictionary<string, object>();
        /// <summary>
        /// 链接附带参数 字符串（不含页码）
        /// </summary>
        protected string PagingParamsStr
        {
            get
            {
                if (PagingParams == null || PagingParams.Count == 0)
                {
                    return "";
                }
                return PagingParamsKeyValueStr(PagingParams);
            }
        }
        protected string PagingParamsKeyValueStr(Dictionary<string, object> p)
        {
            var _t = new List<string>();
            foreach (var item in p)
            {
                _t.Add(string.Format("{0}={1}", item.Key, item.Value));
            }
            return string.Join("&", _t.ToArray());
        }
        #endregion

        /// <summary>
        /// 行数
        /// </summary>
        protected int RowCount;
        /// <summary>
        /// 简单分页 获取sql行数和页码数据集
        /// </summary>
        /// <param name="dbop"></param>
        /// <param name="sql"></param>
        /// <param name="sqlOrder"></param>
        /// <param name="pagingsize"></param>
        /// <param name="pagingnumber"></param>
        protected PagingHelperBase PagingSingle(IDbHandle.IDbop dbop, string sql, string sqlOrder, int pagingsize, int pagingnumber)
        {
            return new PagingHelperBase(PagingTable(dbop, sql, sqlOrder, pagingsize, pagingnumber, out RowCount), RowCount);
        }
        public DataTable PagingTable(IDbHandle.IDbop dbop, string sql, string sqlOrder, int pagingsize, int pagingnumber, out int rowcount)
        {
            rowcount = dbop.GetSingleInt(string.Format("SELECT COUNT(*) FROM({0}) tmp_tb_pagingt", sql));
            if (rowcount == 0)
                return new DataTable();
            string pagingSql;
            if (rowcount <= pagingsize)
            {
                // 不接受 PagingNumber 参数 直接查询
                pagingSql = string.Format("{0} {1}", sql, sqlOrder);
            }
            else
            {
                // sql 异常处理
                if (!string.IsNullOrEmpty(sqlOrder) && sqlOrder.IndexOf('.') >= 0)
                {
                    // 剔除字段前缀
                    sqlOrder = Regex.Replace(sqlOrder, "\\s+\\w+[.]", " ");
                    sqlOrder = Regex.Replace(sqlOrder, "[,]\\w+[.]", ",");
                }
                pagingSql =
@"WITH tmp_tb_paging AS
 (SELECT *, ROW_NUMBER() OVER ({0})as t_rownumber FROM ({1}) t) 
 SELECT * FROM tmp_tb_paging WHERE t_rownumber between {2} and {3}";
                pagingSql = string.Format(pagingSql,
                    sqlOrder,
                    sql,
                    (pagingnumber - 1) * pagingsize + 1,
                    pagingnumber * pagingsize);
            }
            return dbop.GetDataTable(pagingSql);
        }
        #endregion
    }
}