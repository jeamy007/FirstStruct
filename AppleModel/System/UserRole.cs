using System;
using System.Collections.Generic;
using AppleDbop;

namespace AppleModel
{
    [ModelClass(TableName = "WA_UserRole")]
    public class UserRole : ModelDbopBaseT<UserRole>
    {
        [ModelField(PrimaryKey = true)]
        public int UserID { get; set; }
        public int RoleID { get; set; }
 
        /// <summary>
        /// 重新设置用户角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rolelist"></param>
        /// <returns></returns>
        public static KeyValuePair<bool, string> ResetRole(int userid, List<int> rolelist)
        {
            if (userid > 0)
            {
                UserRole.Remove(userid);
            }
            if (rolelist.Count > 0)
            {
                List<UserRole> list = new List<UserRole>();
                foreach (int item in rolelist)
                {
                    list.Add(new UserRole { RoleID = item, UserID = userid });
                }
                return UserRole.AddNewBatch(list);
            }
            return new KeyValuePair<bool, string>(true, null);
        }

    }
}
