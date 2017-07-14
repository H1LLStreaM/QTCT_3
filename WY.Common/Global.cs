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
        public const int MAX_SALER_COUNT = 5;       //ÿ����·����������5��ҵ��Ա���а����������ۺ��깤¼���ߣ�
        public const int MIN_SALER_COUNT = MAX_SALER_COUNT - 2;
        public const string ERR_CONNECTION = "can not connection";
        public const int ALERT_MONTH = 2;    //Ĭ��6���º�ʼ����
        /// <summary>
        /// ����Ա����
        /// </summary>
        public const string ADMIN_USER_NUMBER = "ADMIN";

        public const string DB_PWDKEY = "wewin321";  //���ܣ�������λ

        /// <summary>
        /// �Ƿ��¼sqlִ����־
        /// </summary>
        public static bool _allowsqllog = false;

        public static DbHelper g_db = null;
        public static int g_userid = -1;                    //��ǰ�û�Id
        public static string g_username = string.Empty;     //��ǰ�û���
        public static string g_usercode = string.Empty;     //��ǰ�ù���
        public static int g_userrole = -1;     //��ǰ��Ȩ��
        public static string g_password = string.Empty;     //��ǰ�û���
        public static string g_dept = string.Empty;     //��ǰ�û�����

        public static decimal g_ratio1 = 0;
        public static decimal g_ratio2 = 0;
    }
}
