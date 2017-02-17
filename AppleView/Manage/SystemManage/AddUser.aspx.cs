 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppleModel;

namespace AppleView.Manage.SystemManage
{
    public partial class AddUser : Manage.UI.ManageBaseFormPage<User>
    {
        //protected Company Usercompany;
        public AddUser()
        {
            //默认加载当前数据
           IsLoadListData = true;
            #region 排序
            SortDefaultIndex = 1;
            SortDefaultDESC = true;
            SortFieldName = new List<string> {
                "UserName","CreateTime"
            };
            #endregion
            #region 搜索
            SearchConfigList = new List<UI.Search> {
                new UI.Search("UserName LIKE'%{0}%'",SearchKeys.skey)
            };
            #endregion
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ObjList = user.GetUserListByMyCompany();
        }

        /// <summary>
        /// 添加时记录添加人信息
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddNew_Init(User obj)
        {
            BaseCheck(obj);
            obj.StopFlag = 0;//不让登录
            obj.CreateUserID = user.UserID;
            obj.CreateTime = DateTime.Now;
            base.AddNew_Init(obj);
           
        }
        /// <summary>
        /// 更新时记录修改人信息
        /// </summary>
        /// <param name="obj"></param>
        protected override void Update_Init(User obj)
        {
            BaseCheck(obj);
            obj.StopFlag = 1;//不让登录
            obj.ModifyUserID = user.UserID;
            obj.ModifyTime = DateTime.Now;
            base.Update_Init(obj);
           
        }

        protected override void OnAddNewCompleted(User obj)
        {
            base.OnAddNewCompleted(obj);
            CompanyUser cu = new CompanyUser();
            cu.UserID = obj.UserID;
           // cu.CompanyID = Usercompany.CompanyID;
            cu.CreateTime = DateTime.Now;
            cu.AddNew();
        }

        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="obj"></param>
        private void BaseCheck(User obj)
        {
            //Usercompany = Company.getCompanyModelByUserID(user.UserID.ToString());
            //if (Usercompany == null)
            //{
            //    ResponseWriteJsonResult(false, "您的所属单位还没有设定，请先设置您所属的单位再添加用户！");
            //}


            if (string.IsNullOrEmpty(obj.UserName.Trim()))
            {
                ResponseWriteJsonResult(false, "名称不能为空");
            }
            if (Role.ExistsName(obj.UserName, obj.UserID))
            {
                ResponseWriteJsonResult(false, "名称已存在");
            }
        }
    }
}