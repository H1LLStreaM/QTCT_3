using System;
using System.Collections.Generic;
using System.Text;
using WY.Common.Data;

namespace WY.Common
{
    public class Global
    {
        public const string DEFAULT_DATE_FORMAT = "yyyy-MM-dd";
        public const string DEFAULT_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string SHORT_DATE_FORMAT = "yy-M-d";
        public const string SHORT_TIME_FORMAT = "yy-M-d HH:mm";
        public const int MAX_SALER_COUNT = 5;       //每条电路代码最多添加5个业务员其中包括（主销售和完工录入者）
        public const int MIN_SALER_COUNT = MAX_SALER_COUNT - 2;
        public const string ERR_CONNECTION = "can not connection";
        public const int ALERT_MONTH = 2;    //默认6个月后开始提醒
        /// <summary>
        /// 管理员工号
        /// </summary>
        public const string ADMIN_USER_NUMBER = "ADMIN";

        public const string DB_PWDKEY = "wewin321";  //加密ｋｅｙ，８位

        /// <summary>
        /// 是否记录sql执行日志
        /// </summary>
        public static bool _allowsqllog = false;

        public static DbHelper g_db = null;
        public static int g_userid = -1;                    //当前用户Id
        public static string g_username = string.Empty;     //当前用户名
        public static string g_usercode = string.Empty;     //当前用工号
        public static int g_userrole = -1;     //当前用权限
        public static string g_password = string.Empty;     //当前用户名
        public static string g_dept = string.Empty;     //当前用户部门

        public static decimal g_ratio1 = 0;
        public static decimal g_ratio2 = 0;
    }
}
