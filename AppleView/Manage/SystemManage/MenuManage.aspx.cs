using System;

namespace AppleView.Manage.SystemManage
{
    using AppleModel;
    using System.Collections.Generic;
    using System.Linq;
    public partial class MenuManage : AppleView.Manage.UI.ManageBaseFormPage<Menu_Roles>
    {
        public MenuManage()
        {
            PagingSize = 1000;
            #region 排序配置
            SortFieldName = new List<string> { "SortIndex" };
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu_Roles.SetRoleIDs(ObjList);
        }
        /// <summary>
        /// 添加时记录添加人信息
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddNew_Init(Menu_Roles obj)
        {
            BaseCheck(obj);
            base.AddNew_Init(obj);
            obj.CreateUserID = user.UserID;
            obj.CreateTime = DateTime.Now;
        }
        /// <summary>
        /// 更新时记录修改人信息
        /// </summary>
        /// <param name="obj"></param>
        protected override void Update_Init(Menu_Roles obj)
        {
            BaseCheck(obj);
            base.Update_Init(obj);
            obj.ModifyUserID = user.UserID;
            obj.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 新增完成时 处理菜单角色
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnAddNewCompleted(Menu_Roles obj)
        {
            base.OnAddNewCompleted(obj);
            this.OnUpdateCompleted(obj);
        }

        /// <summary>
        /// 修改完成时 处理菜单角色
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnUpdateCompleted(Menu_Roles obj)
        {
            base.OnUpdateCompleted(obj);
            List<int> rolelist;
            if (string.IsNullOrEmpty(obj.RoleIDs))
            {
                rolelist = new List<int>();
            }
            else
            {
                rolelist = Array.ConvertAll<string, int>(obj.RoleIDs.Split(','), m => int.Parse(m)).ToList();
            }
            MenuRole.ResetRole(obj.MenuID, rolelist);
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="obj"></param>
        private void BaseCheck(Menu obj)
        {
            if (string.IsNullOrEmpty(obj.MenuName.Trim()))
            {
                ResponseWriteJsonResult(false, "名称不能为空");
            }
            if (AppleModel.Role.ExistsName(obj.MenuName, obj.MenuID))
            {
                ResponseWriteJsonResult(false, "名称已存在");
            }
        }

        /// <summary>
        /// 保存排序码
        /// </summary>
        protected override void Post_OnSaveSortIndex()
        {
            base.Post_OnSaveSortIndex();
            ResponseWriteJsonResult(false, "未实现");
        }

        #region 表单数据
        public List<Menu_Single> ParentManuList()
        {
            return Menu_Single.FindList(0);
        }

        public List<Role_Single> RoleList()
        {
            return Role_Single.FindList();
        }
        #endregion
    }
}