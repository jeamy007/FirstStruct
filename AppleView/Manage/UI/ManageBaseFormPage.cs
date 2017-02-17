using System;
using System.Data;
using System.Collections.Generic;

namespace AppleView.Manage.UI
{
    using AppleDbop;
    public class ManageBaseFormPage<T> : ManageBasePage
        where T : ModelDbopBase
    {
        /// <summary>
        /// 当前对象列表
        /// </summary>
        public List<T> ObjList;

        /// <summary>
        /// 加载当前对象列表
        /// </summary>
        public bool IsLoadListData = true;
        public ManageBaseFormPage()
        {
            // 注册默认处理事件
            PostEventRegister("glb_modify", Post_OnModify);
            PostEventRegister("glb_addnew", Post_OnAddNew);
            PostEventRegister("glb_save", Post_OnSave);
            PostEventRegister("glb_delete", Post_OnRemove);
            PostEventRegister("glb_save_sortindex", Post_OnSaveSortIndex);
        }

        protected override void OnGetPage()
        {
            base.OnGetPage();
            if (IsLoadListData)
            {
                string sql = GetSelectSQL(SearchSqlExpress) ?? ModelDbopBase.GetSelectSQL<T>(SearchSqlExpress);

                DataTable dt = PagingTableAndObj(
                    AppleDbop.ModelDbopBase.Dbop, 
                    sql, SortFieldSql());

                ObjList = dt.ListFromDataTable<T>();
            }
        }

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSelectSQL(string condition)
        {
            return null;
        }

        #region 默认表单事件
        /// <summary>
        /// 执行保存事件（主键有值时执行更新事件，无值时执行新增事件）
        /// </summary>
        protected virtual void Post_OnSave()
        {
            GetPostDataObject<T>((obj) =>
            {
                if(obj.HasValueParimaryKey())
                {
                    OnUpdateExec(obj);
                }
                else
                {
                    OnAddNewExec(obj);
                }
            });
        }

        /// <summary>
        /// 执行更新事件
        /// </summary>
        protected virtual void Post_OnModify()
        {
            GetPostDataObject<T>(OnUpdateExec);
        }
        /// <summary>
        /// 执行新增事件
        /// </summary>
        protected virtual void Post_OnAddNew()
        {
            GetPostDataObject<T>(OnAddNewExec);
        }

        /// <summary>
        /// 表单执行删除事件
        /// </summary>
        protected virtual void Post_OnRemove()
        {
            GetPostDataObject<T>((obj) =>
            {
                
                KeyValuePair<bool, string> r = obj.Remove();
                if (r.Key)
                {
                    OnRemoveCompleted(obj);
                }
                ResponseWriteJsonResult(r);
            });
        }

        /// <summary>
        /// 保存排序码
        /// </summary>
        protected virtual void Post_OnSaveSortIndex()
        {
            
        }


        /// <summary>
        /// 执行新增
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddNewExec(T obj)
        {
            AddNew_Init(obj);
            var r = obj.AddNew();
            if (r.Key)
            {
                OnAddNewCompleted(obj);
            }
            ResponseWriteJsonResult(r);
        }
        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="obj"></param>
        private void OnUpdateExec(T obj)
        {
            Update_Init(obj);
            var r = obj.Update();
            if (r.Key)
            {
                OnUpdateCompleted(obj);
            }
            ResponseWriteJsonResult(r);
        }
        #endregion


        #region OnCompleted Event
        protected virtual void OnUpdateCompleted(T obj)
        {

        }

        protected virtual void OnAddNewCompleted(T obj)
        {

        }
        protected virtual void OnRemoveCompleted(T obj)
        {

        }
        #endregion

        #region OnInit Property Data
        /// <summary>
        /// 更新时前设置对象属性的值和其他初始
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void Update_Init(T obj)
        {

        }
        /// <summary>
        /// 新增前设置对象属性的值和其他初始
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void AddNew_Init(T obj)
        {

        }
        #endregion
    }
}