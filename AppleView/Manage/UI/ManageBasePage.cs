using System;
using System.Data;
using System.Collections.Generic;
using DbHandle;
using CommonTools;
using AppleModel;
using AppleHelp;

namespace AppleView.Manage.UI
{
    public class ManageBasePage : BasePageObj
    {
        #region 权限变量
        /// <summary>
        /// 页面权限标识
        /// </summary>
        protected string PageKey = string.Empty;
        /// <summary>
        /// 页面状态 （暂未使用）
        /// </summary>
        //protected string PageState = string.Empty;

        /// <summary>
        /// 页面检测权限
        /// </summary>
        protected bool CheckRights = true;
        /// <summary>
        /// 系统页面 管理员可直接访问
        /// </summary>
        protected bool IsSystemPage = false;

        /// <summary>
        /// 始终初始化数据帮助（搜索栏，分页栏）
        /// </summary>
        protected bool AlwaysInitDataHelper = false;

        #endregion

        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected User user;

        protected override void OnInit(EventArgs e)
        {
            object o = SessionPanel.CurrUser;
            if (o == null)
            {
                RedirectLoginPage();
                // 测试
                //user = new User() { UserName = "admin_T", UserID = 1 };
                //SessionPanel _pd = new SessionPanel();
                //_pd.SetCurrUser(user);
                //_pd.SetCurrMenuList(Menu.FindList());
                //o = user;
            }

            user = (User)o;
            if (!IsPost())
            {
                SetPagingSize();

                InitSearch();

                InitSortParams();
            }
            else
            {
                if (AlwaysInitDataHelper)
                {
                    InitSearch();

                    InitSortParams();
                }
            }
            base.OnInit(e);
        }

        #region 业务函数
        /// <summary>
        /// 获取 id,name 列表
        /// </summary>
        /// <param name="dt"></param>
        public Dictionary<int, string> SingleDictinary(DataTable dt)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(SafeObjectInt(item[0]), item[1] + "");
            }
            return list;
        }

 
 
        #endregion

        #region ajax 后台公用数据处理函数
        /// <summary>
        /// 数据库操作
        /// </summary>
        //public IDbHandle.IDbop Dbop
        //{
        //    get
        //    {
        //        return SDIHelper.DbHelper.Dbop;
        //    }
        //}
        #endregion

        #region 页面级 公用函数
        /// <summary>
        /// 跳转至登录页面
        /// </summary>
        public void RedirectLoginPage()
        {
            Response.Redirect("/Manage/Login.aspx?lasturl=" + Request.RawUrl);
        }
        /// <summary>
        /// 注销
        /// </summary>
        public void RedirectLogOutPage()
        {
            Response.Redirect("/Manage/LogOut.aspx");
        }
        public string GetNextDayStr(string sdate)
        {
            return SafeObjectDateTime(sdate).AddDays(1).ToString("yyyy-MM-dd");
        }

        public class Dic
        {
            public Dic(int _id, string _name)
            {
                this.id = _id;
                this.name = _name;
            }
            public int id { get; set; }
            public string name { get; set; }
        }
        #endregion

        #region 排序处理模块
        /// <summary>
        /// 默认排序字段
        /// </summary>
        protected int SortDefaultIndex = 0;
        /// <summary>
        /// 默认排序方式 (默认升序)
        /// </summary>
        protected bool SortDefaultDESC = false;
        /// <summary>
        /// 排序字段列表 可在SortLink中初始化
        /// </summary>
        protected List<string> SortFieldName;

        int SortFieldCount = -1;
        bool SortHaveFind = false;
        /// <summary>
        /// 获取页面需排序的索引链接 （添加在 a标签的href属性中）
        /// </summary>
        public string SortLink()
        {
            SortFieldCount++;
            var pstr = PagingParamsStrEscape("sortid", "sorttype");
            string r = "?sortid=" + SortFieldCount;
            //r += GetPagingParam();
            if (!string.IsNullOrEmpty(pstr))
            {
                r += "&" + pstr;
            }
            r += "{0}\" class=\"sort{1}";
            if (SortHaveFind)
            {
                return string.Format(r, "", "");
            }
            string sortid = Request.QueryString["sortid"];
            int si = 0;
            if (sortid == null)
            {
                // 默认排序项 相反标识
                if (SortDefaultIndex >= 0 && SortDefaultIndex == SortFieldCount)
                {
                    if (SortDefaultDESC)
                    {
                        return string.Format(r, "&sorttype=0", " asc");
                    }
                    else
                        return string.Format(r, "&sorttype=1", " desc");
                    //return string.Format(r, "&sorttype=" + (SortDefaultDESC ? 0 : 1), (SortDefaultDESC ? " asc" : " desc"));
                }
                else
                    return string.Format(r, "", "");
            }
            else if (!int.TryParse(sortid, out si) || SortFieldCount != si)
                return string.Format(r, "", "");
            SortHaveFind = true;
            string sorttype = Request.QueryString["sorttype"];
            bool SortDesc = false;
            if (sorttype == null)
            {
                SortDesc = false;
            }
            else
            {
                int it = 0;
                int.TryParse(sorttype, out it);
                SortDesc = it != 1;
            }
            return string.Format(r, "&sorttype=" + (SortDesc ? "1" : "0"), (SortDesc ? " desc" : " asc"));
        }

        /// <summary>
        /// 获取排序的 SQL
        /// </summary>
        /// <returns></returns>
        protected string SortFieldSql(string def = "")
        {
            if (SortFieldName == null || SortFieldName.Count == 0)
                return def;
            int sortid = SafeQueryInt("sortid", -1);
            string desc;
            if (sortid == -1)
            {
                //if (SortDefaultIndex == -1)
                //    return def;
                if (SortDefaultIndex < 0 || SortDefaultIndex > SortFieldName.Count)
                    SortDefaultIndex = 0;
                sortid = SortDefaultIndex;
                desc = SortDefaultDESC ? " desc" : "";
            }
            else
            {
                // 超出索引
                if (sortid >= SortFieldName.Count)
                    return def;
                desc = SafeQueryInt("sorttype", 1) == 1 ? " desc" : "";
            }
            if (sortid >= SortFieldName.Count)
            {
                sortid = SortFieldName.Count - 1;
            }
            string r = " ORDER BY " + SortFieldName[sortid];
            // 多项排序 默认首项
            int fstidx = r.IndexOf(",");
            if (fstidx == -1)
                r += desc;
            else
            {
                // 首项包含排序规则（异常初始识别）
                string _t = r.Substring(0, fstidx).ToLower();
                if (_t.IndexOf(" desc") >= 0 || _t.IndexOf(" asc") >= 0)
                    return r;
                r = r.Insert(fstidx, desc);
            }
            return r;
        }
        /// <summary>
        /// 初始化排序参数 在分页中添加排序参数
        /// </summary>
        protected void InitSortParams()
        {
            int sortid = SafeQueryInt("sortid", -1);
            if (sortid >= 0)
            {
                PagingParams.Add("sortid", sortid);
                int sorttype = SafeQueryInt("sorttype", -1);
                if (sorttype >= 0)
                {
                    PagingParams.Add("sorttype", sorttype);
                }
            }
        }
        
        #endregion

        #region 搜索模块
        /// <summary>
        /// 初始化搜索配置
        /// </summary>
        protected List<Search> SearchConfigList;
        protected List<string> SearchEscapeKeyList = new List<string>();
        /// <summary>
        /// 获取键对应的值 （前端绑定默认键值）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object SearchValue(string key)
        {
            if (SearchConfigList != null && SearchConfigList.Count > 0)
            {
                //foreach (var item in SearchConfigList.Where(p => p.Key == key))
                //{
                //    return item.Value;
                //}
                foreach (var item in SearchConfigList)
                {
                    if (item.Keys == null)
                    {
                        if (item.Key == key)
                        {
                            return item.Value == null ? item.DefaultValue : item.Value;
                        }
                    }
                    else
                    {
                        if (item.Values == null || item.Values.Count == 0)
                            return null;
                        //if (item.Keys.Length != item.Values.Count)
                        //{
                        //    PublicFunction.WriteLog("ManageBasePage.SearchValue,Keys.Length != Values.Count");
                        //    return null;
                        //}
                        for (int i = 0; i < item.Keys.Length; i++)
                        {
                            if (item.Keys[i] == key)
                            {
                                return item.Values[i];
                            }
                        }
                    }
                }
                return null;
            }
            return null;
        }
        /// <summary>
        /// 获取键对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T SearchValue<T>(string key)
        {
            return SearchValue(key).ConvertType<T>();
        }
        protected object SearchValue(SearchKeys key)
        {
            return SearchValue(key.ToString());
        }
        /// <summary>
        /// 获取搜索sql表达式 and ...
        /// </summary>
        protected string SearchSqlExpress
        {
            get
            {
                if (SearchConfigList != null && SearchConfigList.Count > 0)
                {
                    List<string> t = new List<string>();
                    string sand = " AND ";
                    foreach (var item in SearchConfigList)
                    {
                        if (string.IsNullOrEmpty(item.Express))
                        {
                            continue;
                        }
                        if (item.Keys == null)
                        {
                            if (item.Value != null)
                            {
                                t.Add(string.Format(item.Express, item.Value));
                            }
                            else if (item.DefaultValue != null)
                            {
                                t.Add(string.Format(item.Express, item.DefaultValue));
                            }
                        }
                        else
                        {
                            List<object> _vals = new List<object>();
                            foreach (object val in item.Values)
                            {
                                if (val != null && !string.IsNullOrEmpty(val + ""))
                                    _vals.Add(val);
                            }
                            if (_vals.Count > 0)
                                t.Add(string.Format(item.Express, _vals.ToArray()));
                        }
                    }
                    // 搜索 参数一般都大于0
                    return t.Count > 1 ? sand + string.Join(sand, t.ToArray()) :
                        t.Count == 0 ? "" : sand + t[0];
                    //return t.Count == 0 ? "" :
                    //    t.Count == 1 ? sand + t[0] :
                    //    sand + string.Join(sand, t.ToArray());

                    //if(t.Count == 1)
                    //{
                    //    return sand + t[0];
                    //}
                    //else if (t.Count > 1)
                    //{
                    //    return sand + string.Join(sand, t.ToArray());
                    //}
                    //return "";
                }
                return "";
            }
        }
        /// <summary>
        /// 搜索附带参数（前端页面刷新默认带的参数键值）
        /// </summary>
        protected string SearchParams
        {
            get
            {
                List<string> t = new List<string>();
                if (SearchConfigList != null && SearchConfigList.Count > 0)
                {
                    foreach (var item in SearchConfigList)
                    {
                        if (item.Keys == null)
                        {
                            t.Add(item.Key);
                        }
                        else
                        {
                            t.AddRange(item.Keys);
                        }
                    }
                }
                if (SearchEscapeKeyList != null && SearchEscapeKeyList.Count > 0)
                {
                    foreach (var item in SearchEscapeKeyList)
                    {
                        t.Add(item);
                    }
                }
                if (t.Count > 0)
                    return PagingParamsStrEscape(t.ToArray());
                else
                    return "";
            }
        }

        /// <summary>
        /// 提供默认的key值
        /// </summary>
        protected enum SearchKeys
        {
            skey,
            sstate,
            stype,
            ssdate,
            sedate,
            sstore,
            sbusiness
        };

        /// <summary>
        /// 搜索参数初始化
        /// </summary>
        /// <param name="userKeys"></param>
        protected virtual void InitSearch()
        {
            if (SearchConfigList != null && SearchConfigList.Count > 0)
            {
                foreach (var item in SearchConfigList)
                {
                    switch (item.DataType)
                    {
                        case Search.KeyDataType.Int:
                            SearchValueSetInt(item);
                            break;
                        case Search.KeyDataType.String:
                            SearchValueSetString(item);
                            break;
                        case Search.KeyDataType.Date:
                            SearchValueSetDate(item);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 通过时间种子类型获取两个时间段
        /// today;yesterday;nearweek(近一周);nearlastweek(近上周);thisweek(本周);lastweek(上周);
        /// thismonth(本月);lastmonth(上月);
        /// </summary>
        /// <param name="dateKey">时间种子类型</param>
        /// <param name="dateStart">返回开始时间</param>
        /// <param name="dateEnd">返回结束时间</param>
        /// <param name="baseTime">默认时间</param>
        public void SearchDateValue(string dateKey, out string dateStart, out string dateEnd, DateTime? baseTime = null)
        {
            dateStart = dateEnd = string.Empty;
            DateTime? dt1, dt2;
            GetDateRange(dateKey, out dt1, out dt2, baseTime);
            if (dt1 != null)
            {
                dateStart = dt1.Value.ToString("yyyy-MM-dd");
                dateEnd = dt2.Value.ToString("yyyy-MM-dd");
            }
        }

        private static void GetDateRange(string key, out DateTime? dateStart, out DateTime? dateEnd, DateTime? baseTime = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                dateStart = null;
                dateEnd = null;
            }
            DateTime now;
            if (baseTime == null)
            {
                now = DateTime.Now.Date;
            }
            else
            {
                now = baseTime.Value;
            }
            switch (key)
            {
                case "today":
                    dateStart = dateEnd = now;
                    break;
                case "yesterday":
                    dateStart = dateEnd = now.AddDays(-1);
                    break;
                case "nearweek": //近一周
                    dateStart = now.AddDays(-6);
                    dateEnd = now;
                    break;
                case "nearlastweek": //近上周
                    dateStart = now.AddDays(-13);
                    dateEnd = now.AddDays(-7);
                    break;
                case "thisweek": //本周
                    dateStart = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d")));
                    dateEnd = dateStart.GetValueOrDefault().AddDays(6);
                    break;
                case "lastweek": //上周
                    dateStart = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))).AddDays(-7);
                    dateEnd = dateStart.GetValueOrDefault().AddDays(6);
                    break;
                case "thismonth":
                    dateStart = now.AddDays(-now.Day + 1);
                    dateEnd = dateStart.Value.AddMonths(1).AddDays(-1);
                    break;
                case "lastmonth":
                    dateStart = now.AddMonths(-1).AddDays(-now.Day + 1);
                    dateEnd = now.AddDays(-now.Day);
                    break;
                default:
                    dateStart = null;
                    dateEnd = null;
                    break;
            }
        }

        private void SearchValueSetInt(Search s)
        {
            if (s.Keys == null)
            {
                var o = SafeQueryInt(s.Key);
                if (o > 0)
                {
                    PagingParams.Add(s.Key, o);
                    s.Value = o;
                }
            }
            else
            {
                foreach (var key in s.Keys)
                {
                    var o = SafeQueryInt(key);
                    if (o > 0)
                    {
                        PagingParams.Add(key, o);
                    }
                    s.Values.Add(o);
                }
            }
        }
        private void SearchValueSetString(Search s)
        {
            if (s.Keys == null)
            {
                var o = SafeQuery(s.Key);
                if (!string.IsNullOrEmpty(o))
                {
                    PagingParams.Add(s.Key, o);
                    s.Value = o;
                }
            }
            else
            {
                string key;
                for (int i = 0; i < s.Keys.Length; i++)
                {
                    key = s.Keys[i];
                    var o = SafeQuery(key);
                    if (!string.IsNullOrEmpty(o))
                    {
                        PagingParams.Add(key, o);
                    }
                    else
                    {
                        if (s.DefaultValues != null)
                        {
                            o = s.DefaultValues[i].ToString();
                            PagingParams.Add(key, o);
                        }
                    }
                    s.Values.Add(o);
                }
            }
        }
        private void SearchValueSetDate(Search s)
        {
            string _val;
            if (s.Keys == null)
            {
                var o = SafeQueryDate(s.Key);
                if (o != null)
                {
                    _val = SafeObjectDateTime(o.Value).ToString("yyyy-MM-dd");
                    PagingParams.Add(s.Key, _val);
                    s.Value = _val;
                }
                else
                {
                    if (s.DefaultValue != null)
                    {
                        _val = SafeObjectDateTime(s.DefaultValue).ToString("yyyy-MM-dd");
                        PagingParams.Add(s.Key, _val);
                        s.Value = _val;
                    }
                }
            }
            else
            {
                string key;
                for (int i = 0; i < s.Keys.Length; i++)
                {
                    key = s.Keys[i];
                    var o = SafeQueryDate(key);
                    if (o != null)
                    {
                        _val = SafeObjectDateTime(o.Value).ToString("yyyy-MM-dd");
                        PagingParams.Add(key, _val);
                        s.Values.Add(_val);
                    }
                    else
                    {
                        if (s.DefaultValues != null && s.DefaultValues[i] != null)
                        {
                            _val = SafeObjectDateTime(s.DefaultValues[i]).ToString("yyyy-MM-dd");
                            PagingParams.Add(key, _val);
                            s.Values.Add(_val);
                        }
                        else
                        {
                            s.Values.Add(null);
                        }
                    }
                }

                //foreach (var key in s.Keys)
                //{
                //    var o = SafeQueryDate(key);
                //    if (o != null)
                //    {
                //        PagingParams.Add(key, o.Value);
                //        if (o.Value == null)
                //            s.Values.Add("");
                //        else
                //            s.Values.Add(o.Value.ToString("yyyy-MM-dd"));

                //    }
                //    else
                //    {
                //        s.Values.Add("");
                //    }
                //}
            }
        }
        #endregion

    }

}