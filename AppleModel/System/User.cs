using AppleDbop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppleModel
{
    [ModelClass(TableName = "WA_User")]
    public partial class User : ModelDbopBaseT<User>
    {
        #region 基本属性
        [ModelField(PrimaryKey = true, IdentityKey = true)]
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// EMail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 性别 男 | 女
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 停用状态 1 停用 | 0 正常
        /// </summary>
        public int StopFlag { get; set; }

        [ModelField(IgnoreOnInsert = true)]
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        [ModelField(IgnoreOnInsert = true, IgnoreOnUpdate = true)]
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int ModifyUserID { get; set; }

        [ModelField(IgnoreOnUpdate = true)]
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserID { get; set; }
        #endregion
    }

    public partial class User
    {

 
 


        /// <summary>
        /// 通过角色获取用户列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public List<User> getUserByRoleName(string roleName) {
            string sql = @"
            SELECT  *
            FROM    dbo.WA_User
            WHERE   UserID IN ( SELECT  UserID
                                FROM    dbo.WA_UserRole
                                WHERE   RoleID = ( SELECT   RoleID
                                                   FROM     dbo.WA_Role
                                                   WHERE    RoleName = '{0}'
                                                 ) )".ForamtString(roleName);
            return Dbop.GetDataTable(sql).ToList<User>();
        
        }

        /// <summary>
        /// 获取当前用户的角色名称
        /// </summary>
        /// <returns></returns>
        public string getRoleName() {
            UserRole ur = UserRole.Find("UserID={0}".ForamtString(this.UserID));
            if (ur == null)
            {
                return string.Empty;
            }
            var roleId = ur.RoleID;
            return Role.Find("RoleID = {0}".ForamtString(roleId)).RoleName;
        }

        
        public override KeyValuePair<bool, string> Remove()
        {
            return base.Remove();
        }

        /// <summary>
        /// 根据UserID获取用户的角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>角色名称</returns>
        public static string getUserRoleNameByUserId(int userId)
        {
            string result = string.Empty;
    

            //用户可以有多个角色
            List<UserRole> urList = UserRole.FindList("UserID = {0}".ForamtString(userId));
            if(urList.Count==0)
            {
                return result;
            }
            else
            {
                foreach (UserRole ur in urList)
                {
                    Role r = Role.Find("RoleID = {0}".ForamtString(ur.RoleID));
                    if (r != null)
                    {
                        result += r.RoleName + ",";
                    }
                }
            }


            return result.Trim(',');
        }
      

        /// <summary>
        /// 验证登录用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Key = false 时， Value 为错误信息
        /// <para>Key = true 时， Value 为登录用户</para>
        /// </returns>
        public static KeyValuePair<bool, object> LoginUser(string username, string password)
        {
            User u = Find(string.Format("UserName='{0}' AND PassWord='{1}'", username, password));
            if (u == null)
            {
                return new KeyValuePair<bool, object>(false, "用户名或密码错误");
            }
            if(u.StopFlag == 1)
            {
                return new KeyValuePair<bool, object>(false, "账户已失效");
            }
            return new KeyValuePair<bool, object>(true, u);
        }

        #region 业务用户


        /// <summary>
        /// 获取我所在的公司里面的所有人
        /// 获取送件人
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<User> GetUserListByMyCompany(string strWhere="")
        {
            string sql = string.Format(@"
            DECLARE @CompanyId INT
            SELECT  @CompanyId = c.CompanyID
            FROM    dbo.WA_CompanyUser c
            WHERE   c.UserID = {0}
            SELECT  *
            FROM    dbo.WA_User
            WHERE   UserID IN ( SELECT  UserID
                                FROM    dbo.WA_CompanyUser
                                WHERE   CompanyID = @CompanyId )"+strWhere, this.UserID);
            System.Data.DataTable dt = Dbop.GetDataTable(sql);
            return dt.ListFromDataTable<User>();
        }

        /// <summary>
        /// 获取收件人:资料接收员
        /// </summary>
        /// <returns></returns>
        public  List<User> GetAddresseeUserListByUserID()
        {
            return getUserByRoleName("资料接收员");
        }
        

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public KeyValuePair<bool, string> RePassWord(string oldPwd, string newPwd)
        {
            string sql = @"
                                        IF(SELECT 1 FROM dbo.WA_User WHERE UserId ={0} AND PassWord='{1}') =1
                                        BEGIN
	                                        UPDATE dbo.WA_User SET PassWord ='{2}' WHERE UserID =1
                                        END".ForamtString(this.UserID, oldPwd, newPwd);
            int effects = Dbop.ExecuteSql(sql);
            bool res = effects == 1 ? true : false;
            if (res)
            {
                return new KeyValuePair<bool, string>(true, "");
            }
            else {
                return new KeyValuePair<bool, string>(false, "密码错误！");
            }

        }
 
        #endregion
    }




    public class User_Roles : User
    {
        //[ModelField(Ignore = true)]
        /// <summary>
        /// 角色列表
        /// </summary>
        public string RoleIDs;
        
        /// <summary>
        /// 设置用户角色列表
        /// </summary>
        /// <param name="objList"></param>
        public static void SetRoleIDs(List<User_Roles> objList)
        {
            List<UserRole> urlist = UserRole.FindList();
            var tlist = UserRole.FindList();
            List<int> rolelist = new List<int>();

            objList.ForEach(
                m => m.RoleIDs = string.Join(",",
                    from A in urlist.FindAll(k => k.UserID == m.UserID)
                    select A.RoleID));
        }

     
    }
}
