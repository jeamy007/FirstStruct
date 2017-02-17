using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppleView
{
    public partial class _Default : UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CurrUser
            if (IsPost())
            {
                switch (FormAction)
                {
                    case "save":
                        string name = SafeForm("name");
                        ResponseWriteJsonResult(true, name);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}