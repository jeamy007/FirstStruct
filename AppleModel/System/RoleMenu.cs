using System;
using System.Collections.Generic;
using AppleDbop;

namespace AppleModel
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [ModelClass(TableName = "WA_RoleMenu")]
    public partial class RoleMenu: ModelDbopBaseT<RoleMenu>
    {
        [ModelField(PrimaryKey = true)]
        public int RoleID { get; set; }
        public int MenuID { get; set; }
    }

    public partial class RoleMenu
    {
        /// <summary>
        /// 重新设置角色菜单
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="rolelist"></param>
        /// <returns></returns>
        public static KeyValuePair<bool, string> ResetMenu(int roleid, List<int> menulist)
        {
            if (roleid > 0)
            {
                RoleMenu.Remove(roleid);
            }
            if (menulist.Count > 0)
            {
                List<RoleMenu> list = new List<RoleMenu>();
                foreach (int item in menulist)
                {
                    list.Add(new RoleMenu { RoleID = roleid, MenuID = item });
                }
                return RoleMenu.AddNewBatch(list);
            }
            return new KeyValuePair<bool, string>(true, null);
        }

        /// <summary>
        /// 获取角色对应的菜单id列表
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public static List<int> GetMenuList(int roleid)
        {
            List<int> menulist = new List<int>();
            FindList(roleid).ForEach(m =>
            {
                menulist.Add(m.MenuID);
            });
            return menulist;
        }
    }


    /// <summary>
    /// 菜单角色
    /// </summary>
    [ModelClass(TableName = "WA_RoleMenu")]
    public partial class MenuRole : ModelDbopBaseT<MenuRole>
    {
        [ModelField(PrimaryKey = true)]
        public int MenuID { get; set; }
        public int RoleID { get; set; }
    }

    public partial class MenuRole
    {
        /// <summary>
        /// 重新设置菜单角色
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="rolelist"></param>
        /// <returns></returns>
        public static KeyValuePair<bool, string> ResetRole(int menuid, List<int> rolelist)
        {
            if (menuid > 0)
            {
                MenuRole.Remove(menuid);
            }
            if (rolelist.Count > 0)
            {
                List<RoleMenu> list = new List<RoleMenu>();
                foreach (int item in rolelist)
                {
                    list.Add(new RoleMenu { MenuID = menuid, RoleID = item });
                }
                return RoleMenu.AddNewBatch(list);
            }
            return new KeyValuePair<bool, string>(true, null);
        }

        /// <summary>
        /// 获取菜单对应的角色id列表
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public static List<int> GetRoleList(int menuid)
        {
            List<int> rolelist = new List<int>();
            FindList(menuid).ForEach(m =>
            {
                rolelist.Add(m.RoleID);
            });
            return rolelist;
        }
    }
}
