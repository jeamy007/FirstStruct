using AppleModel;
using System;
using System.Collections.Generic;
using AppleHelp;

namespace AppleView.Manage.UI
{
    public class SessionPanel : SessionBase
    {
        public void SetCurrUser(User user)
        {
            CurrUser = user;
        }
        public void SetCurrMenuList(List<Menu> menulist)
        {
            CurrMenuList = menulist;
        }


        /// <summary>
        /// 当前用户
        /// </summary>
        public static User CurrUser
        {
            get
            {
                return Get(EM_SessionNames.SYS_USER.ToString()) as User;
            }
            private set
            {
                Set(EM_SessionNames.SYS_USER.ToString(), value);
            }
        }
        /// <summary>
        /// 当前角色菜单
        /// </summary>
        public static List<Menu> CurrMenuList
        {
            get
            {
                return Get(EM_SessionNames.SYS_MENU.ToString()) as List<Menu>;
            }
            private set
            {
                Set(EM_SessionNames.SYS_MENU.ToString(), value);
            }
        }


        private enum EM_SessionNames
        {
            SYS_USER,
            SYS_MENU
        }

        /// <summary>
        /// 清除
        /// </summary>
        public static void Clear()
        {
            ClearAll(Enum.GetNames(typeof(EM_SessionNames)));
        }
    }
}