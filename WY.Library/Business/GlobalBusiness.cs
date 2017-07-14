using System;
using System.Collections.Generic;
using System.Text;
using WY.Common.Framework;
using WY.Common.Data;
using WY.Common.Utility;

namespace WY.Library.Business
{
    public class GlobalBusiness
    {
        public static string getCableStatus(int cablestatus)
        {
            string status = "";
            switch (cablestatus)
            {
                case (int)EnmCableStatus.未完工:
                    status = "未完工";
                    break;
                case (int)EnmCableStatus.已拆机:
                    status = "已拆机";
                    break;
                case (int)EnmCableStatus.已完工:
                    status = "已完工";
                    break;
                case (int)EnmCableStatus.降速价格下降:
                    status = "降速价格下降";
                    break;
                case (int)EnmCableStatus.升速价格不变:
                    status = "升速价格不变";
                    break;
                case (int)EnmCableStatus.升速价格上升:
                    status = "升速价格上升";
                    break;
                case (int)EnmCableStatus.升速价格下降:
                    status = "升速价格下降";
                    break;
                case (int)EnmCableStatus.延续上年不变:
                    status = "延续上年不变 ";
                    break;
                case (int)EnmCableStatus.取消:
                    status = "取消";
                    break;
                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// 获取提成比例中人员类型
        /// </summary>
        /// <param name="salerType">类型编号</param>
        /// <returns></returns>
        public static string getSalerType(int salerType)
        {
            string sale = "";
            switch (salerType)
            {
                case (int)EnmDataType.完工录入:
                    sale = "完工录入";
                    break;
                case (int)EnmDataType.销售渠道:
                    sale = "销售渠道";
                    break;
                case (int)EnmDataType.主销售渠道:
                    sale = "主销售渠道";
                    break;
                default:
                    break;
            }
            return sale;
        }

        /// <summary>
        /// 获取人员角色类型
        /// </summary>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public static string getUserRoleType(int UserType)
        {
            string sale = "";
            switch (UserType)
            {
                case (int)EnmUserRole.财务:
                    sale = "财务";
                    break;
                case (int)EnmUserRole.录入人员:
                    sale = "录入人员";
                    break;
                case (int)EnmUserRole.系统管理员:
                    sale = "系统管理员";
                    break;
                case (int)EnmUserRole.销售渠道:
                    sale = "销售渠道";
                    break;
                case (int)EnmUserRole.销售总监:
                    sale = "销售总监";
                    break;
                default:
                    break;
            }
            return sale;
        }
        /// <summary>
        /// 获取电路支付类型
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static string getPayType(int payType)
        {
            string pay = "";
            switch (payType)
            { 
                case (int)EnmPayType.一次性付:
                    pay = "一次性付";
                    break;
                case (int)EnmPayType.季付:
                    pay = "季付";
                    break;
                case (int)EnmPayType.半年付:
                    pay = "半年付";
                    break;
                case (int)EnmPayType.月付:
                    pay = "月付";
                    break;
                default:
                    break;
            }
            return pay;
        }

        public static int getPayTypeForMonth(int payType)
        {
            int months = 1;
            switch (payType)
            {
                case (int)EnmPayType.一次性付:
                    months = 1;
                    break;
                case (int)EnmPayType.季付:
                    months = 3;
                    break;
                case (int)EnmPayType.半年付:
                    months = 2;
                    break;
                case (int)EnmPayType.月付:
                    months = 12;
                    break;
                default:
                    break;
            }
            return months;
        }

        public static string getBusinessType(int type)
        {
            switch (type)
            {
                case (int)EnmCableStatus.已完工:
                    return "已完工";
                case (int)EnmCableStatus.已拆机:
                    return "已拆机";
                case (int)EnmCableStatus.未完工:
                    return "未完工";
                case (int)EnmCableStatus.取消:
                    return "取消";
                case (int)EnmCableStatus.降速价格下降:
                    return "降速价格下降";
                case (int)EnmCableStatus.升速价格不变:
                    return "升速价格不变";
                case (int)EnmCableStatus.升速价格上升:
                    return "升速价格上升";
                case (int)EnmCableStatus.升速价格下降:
                    return "升速价格下降";
                case (int)EnmCableStatus.延续上年不变:
                    return "延续上年不变";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 获取提成比例中人员类型
        /// </summary>
        /// <param name="salerType">类型编号</param>
        /// <returns></returns>
        public static string getChangeType(int changeType)
        {
            string type = "";
            switch (changeType)
            {
                case (int)EnmChangeType.电路状态变更:
                    type = "电路状态变更";
                    break;
                case (int)EnmChangeType.全部变更:
                    type = "全部变更";
                    break;
                case (int)EnmChangeType.新增完工记录:
                    type = "新增完工记录";
                    break;
                case (int)EnmChangeType.主销售渠道变更:
                    type = "主销售渠道变更";
                    break;
                default:
                    break;
            }
            return type;
        }



        public static int selectEnm(string enm)
        {
            Type business = typeof(EnmCableStatus);
            Array Arrays = Enum.GetValues(business);
            for (int i = 0; i < Arrays.LongLength; i++)
            {
                if (Arrays.GetValue(i).ToString() == enm)
                {
                    return i;
                }
            }
            return -1;
        }

        #region 判断网络是否连接
        public static bool isConnServer()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    db.Open();
                }
                //TableManager.DBServerTime();
                return true;
            }
            catch(Exception ex)
            {
                Log.Info(DateTime.Now.ToString() + "登录失败！" + ex.Message);
                Log.Info("================================================");
                return false;
            }
        }
        #endregion
    }
}
