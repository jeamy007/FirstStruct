using System;
using AppleModel;
using AppleHelp;

namespace AppleView.UI
{
    public class BasePage : BasePageObj
    {
        #region 登录用户
        protected User CurrUser;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #endregion
    }
}