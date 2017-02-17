using System;
using System.Collections.Generic;
using System.Linq;
using AppleDbop;

namespace AppleModel
{
    [ModelClass(TableName = "WA_Menu")]
    public partial class Menu : ModelDbopBaseT<Menu>
    {
        #region 基本属性
        [ModelField(PrimaryKey = true, IdentityKey = true)]
        /// <summary>
        /// MenuID
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单权限编码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// MenuUrl
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// MenuIcon
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// MenuParentID
        /// </summary>
        public int MenuParentID { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        public int SortIndex { get; set; }

        [ModelField(IgnoreOnInsert = true)]
        /// <summary>
        /// ModifyTime
        /// </summary>
        public DateTime ModifyTime { get; set; }

        [ModelField(IgnoreOnInsert = true, IgnoreOnUpdate = true)]
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// ModifyUserID
        /// </summary>
        public int ModifyUserID { get; set; }

        /// <summary>
        /// CreateUserID
        /// </summary>
        public int CreateUserID { get; set; }
        #endregion
    }
    public partial class Menu
    {
        /// <summary>
        /// 获取菜单列表（默认含排序）
        /// </summary>
        /// <returns></returns>
        public static List<Menu> FindList()
        {
            return ModelDbopBaseT<Menu>.FindList("ORDER BY SortIndex,MenuID");
        }
        /// <summary>
        /// 获取菜单列表（默认含排序）
        /// </summary>
        /// <returns></returns>
        public static List<Menu> FindListByUserID(int userid)
        {
            return ModelDbopBaseT<Menu>.FindList(
                string.Format("EXISTS(select 1 from WA_RoleMenu rm where rm.MenuID=WA_Menu.MenuID and EXISTS(select 1 from WA_UserRole ur where ur.RoleID=rm.RoleID and ur.UserID={0})) ORDER BY SortIndex,MenuID", userid)).Distinct().ToList();
        }
        //public static List<Menu> FindList(string wh)
        //{
        //    return SDIModelDbop.ModelDbopBaseT<Menu>.FindList(wh + " ORDER BY SortIndex,MenuID");
        //}

    }
    /// <summary>
    /// 菜单角色
    /// </summary>
    public class Menu_Roles : Menu
    {
        /// <summary>
        /// 菜单角色列表
        /// </summary>
        public string RoleIDs;

        /// <summary>
        /// 获取设置菜单角色ids
        /// </summary>
        public static void SetRoleIDs(List<Menu_Roles> objList)
        {
            // 【可优化】
            objList.ForEach(m => m.RoleIDs = string.Join(",", MenuRole.GetRoleList(m.MenuID)));
        }
    }

    [ModelClass(TableName = "WA_Menu")]
    public class Menu_Single : ModelDbopBaseT<Menu_Single>
    {
        [ModelField(PrimaryKey = true)]
        public int MenuParentID { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
    }
}
