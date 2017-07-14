using System;
using System.Collections.Generic;
using System.Text;
using WY.Common.Data;

namespace WY.Common
{
    public class WYGlobal
    {
        public static string COOKIE_LOGIN_NAME = "LOGINNAME";   //自动登录Cookie名称

        public static string COOKIE_LOGIN_USER = "LOGINEDUSER";   //登录的用户

        public static bool _allowsqllog = false;
        public static bool _allowerrorlog = true;


        public static int guserid;
        public static string gusername;

    } 
}
