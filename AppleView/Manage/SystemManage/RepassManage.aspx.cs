using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppleView.Manage.SystemManage
{
    public partial class RepassManage : AppleView.Manage.UI.ManageBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if(IsPost())
           {
               switch (FormAction.ToLower())
               {
                   case "remark":
                       {

                           RepassStruct rs = DeserializeJson<RepassStruct>(Request.Form["data"]);                          
                           ResponseWriteJsonResult(user.RePassWord(rs.oldPwd, rs.newPwd));

                       } break;
               }
           }
        }
    }
    class RepassStruct
    {
        public string oldPwd;
        public string newPwd;
    }

}