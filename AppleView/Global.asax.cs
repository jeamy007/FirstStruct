using System;
using AppleDbop;

namespace AppleView
{
    public class Global : System.Web.HttpApplication
    {
        public static IDbHandle.IDbop Dbop;
        protected void Application_Start(object sender, EventArgs e)
        {
            // 初始化数据库操作
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ToString();
            Dbop = new DbHandle.OperateSqlServer(connstring);
            ModelDbopBase.Dbop = Dbop;

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 程序异常记录
            string msg = string.Empty;
            //if (Manage.UI.SessionPanel.CurrUser != null)
            //{
            //    msg = string.Format("userid:{0},username:{1}"
            //        , Manage.UI.SessionPanel.CurrUser.UserID
            //        , Manage.UI.SessionPanel.CurrUser.UserName);
            //}
            msg += Server.GetLastError().ToString();
            AppleHelp.Log.WriteLog(msg, "debuglog_error_app");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}