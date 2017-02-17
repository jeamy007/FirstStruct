using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppleDbop;

namespace AppleModel
{
    [ModelClass(TableName = "WA_CompanyUser")]
    public partial class CompanyUser : ModelDbopBaseT<CompanyUser>
    {
        #region 基本属性
        [ModelField(PrimaryKey=true)]
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// CompanyID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
        #endregion
    }

    public partial class Company_User : ModelDbopBaseT<Company_User>
    {
        public int CompanyID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
      
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

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        public static string SelectSQL(string condition)
        {
            return @"
SELECT  u.UserID ,
        u.UserName ,
        u.Email ,
        u.Gender ,
        u.StopFlag ,
        UserPhone ,
        cu.CompanyID ,
        c.CompanyName,
        u.CreateTime
FROM    dbo.WA_User u
        LEFT JOIN dbo.WA_CompanyUser cu ON u.UserID = cu.UserID
        LEFT JOIN dbo.WA_Company c ON cu.CompanyID = c.CompanyID
WHERE   u.StopFlag = 0"+ condition;
        }
    }


}
