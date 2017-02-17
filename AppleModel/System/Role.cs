using System;
using System.Collections.Generic;
using AppleDbop;

namespace AppleModel
{
    /// <summary>
    /// 角色
    /// </summary>
    [ModelClass(TableName = "WA_Role")]
    public partial class Role : ModelDbopBaseT<Role>
    {
        #region 基本属性
        [ModelField(PrimaryKey = true, IdentityKey = true)]
        /// <summary>
        /// RoleID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

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
    public partial class Role
    {
        /// <summary>
        /// 存在角色名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public static bool ExistsName(string name, int roleid = 0)
        {
            return false;
        }
    }

    /// <summary>
    /// 角色菜单
    /// </summary>
    public class Role_Menus : Role
    {
        /// <summary>
        /// 菜单id列表
        /// </summary>
        public string MenuIDs;

        /// <summary>
        /// 获取设置角色菜单ids
        /// </summary>
        /// <param name="objList"></param>
        public static void SetMenuIDs(List<Role_Menus> objList)
        {
            // 【可优化】
            objList.ForEach(m => m.MenuIDs = string.Join(",", RoleMenu.GetMenuList(m.RoleID)));
        }
    }

    /// <summary>
    /// 角色字典（仅有id和名称）
    /// </summary>
    [ModelClass(TableName = "WA_Role")]
    public partial class Role_Single : ModelDbopBaseT<Role_Single>
    {
        /// <summary>
        /// RoleID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}

