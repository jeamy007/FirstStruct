using System;

namespace AppleView.Manage
{
    using AppleModel; 
    public partial class Login : AppleHelp.BasePageObj
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPost())
            {
                if (FormAction == "login")
                {
                    string username = SafeForm("username");
                    string password = SafeForm("password");
                    var r = AppleModel.User.LoginUser(username, password);
                    if(!r.Key)
                    {
                        ResponseWriteJsonResult(false, r.Value.ToString());
                    }
                    InitUserSystemData(r.Value as AppleModel.User);
                    string lasturl = SafeQuery("lasturl");
                    if (string.IsNullOrEmpty(lasturl))
                    {
                        lasturl = "Home.aspx";
                    }
                    ResponseWriteJsonResult(true, lasturl);
                }
            }
        }

        /// <summary>
        /// 初始化用户系统数据
        /// </summary>
        /// <param name="user"></param>
        public static void InitUserSystemData(User user)
        {
            UI.SessionPanel sp = new UI.SessionPanel();
            sp.SetCurrUser(user);

            sp.SetCurrMenuList(Menu.FindListByUserID(user.UserID));
        }
    }
}