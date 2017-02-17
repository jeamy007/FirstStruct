using System;
using System.Collections.Generic;
using System.Linq;

namespace AppleView.Manage.SystemManage
{
    using AppleModel;
    public partial class RoleManage : AppleView.Manage.UI.ManageBaseFormPage<Role_Menus>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Role_Menus.SetMenuIDs(ObjList);
        }
        /// <summary>
        /// 添加时记录添加人信息
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddNew_Init(Role_Menus obj)
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
        protected override void Update_Init(Role_Menus obj)
        {
            BaseCheck(obj);
            base.Update_Init(obj);
            obj.ModifyUserID = user.UserID;
            obj.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 新增完成时 处理角色菜单
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnAddNewCompleted(Role_Menus obj)
        {
            base.OnAddNewCompleted(obj);
            this.OnUpdateCompleted(obj);
        }

        /// <summary>
        /// 修改完成时 处理角色菜单
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnUpdateCompleted(Role_Menus obj)
        {
            base.OnUpdateCompleted(obj);
            List<int> menulist;
            if (string.IsNullOrEmpty(obj.MenuIDs))
            {
                menulist = new List<int>();
            }
            else
            {
                menulist = Array.ConvertAll<string, int>(obj.MenuIDs.Split(','), s => int.Parse(s)).ToList();
            }
            RoleMenu.ResetMenu(obj.RoleID, menulist);
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="obj"></param>
        private void BaseCheck(Role obj)
        {
            if (string.IsNullOrEmpty(obj.RoleName.Trim()))
            {
                ResponseWriteJsonResult(false, "名称不能为空");
            }
            if (AppleModel.Role.ExistsName(obj.RoleName, obj.RoleID))
            {
                ResponseWriteJsonResult(false, "名称已存在");
            }
        }

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <returns></returns>
        public List<Menu_Single> MenuList()
        {
            return Menu_Single.FindList("ORDER BY SortIndex,MenuID");
        }
    }
}