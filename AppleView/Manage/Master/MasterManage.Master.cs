using AppleModel;
using System.Collections.Generic;


namespace AppleView.Manage.Master
{
    public partial class MasterManage : System.Web.UI.MasterPage
    {
        public User user;
       
        public List<Menu> menuList;
    
 

        protected void Page_Load(object sender, System.EventArgs e)
        {
            user = AppleView.Manage.UI.SessionPanel.CurrUser;

            if (user == null)
            {
                Response.Redirect("/Manage/Login.aspx?lasturl=" + Request.RawUrl);
            }


            menuList = AppleView.Manage.UI.SessionPanel.CurrMenuList; 
            
        }
    }
}