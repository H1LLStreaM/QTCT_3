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
                case (int)EnmCableStatus.δ�깤:
                    status = "δ�깤";
                    break;
                case (int)EnmCableStatus.�Ѳ��:
                    status = "�Ѳ��";
                    break;
                case (int)EnmCableStatus.���깤:
                    status = "���깤";
                    break;
                case (int)EnmCableStatus.���ټ۸��½�:
                    status = "���ټ۸��½�";
                    break;
                case (int)EnmCableStatus.���ټ۸񲻱�:
                    status = "���ټ۸񲻱�";
                    break;
                case (int)EnmCableStatus.���ټ۸�����:
                    status = "���ټ۸�����";
                    break;
                case (int)EnmCableStatus.���ټ۸��½�:
                    status = "���ټ۸��½�";
                    break;
                case (int)EnmCableStatus.�������겻��:
                    status = "�������겻�� ";
                    break;
                case (int)EnmCableStatus.ȡ��:
                    status = "ȡ��";
                    break;
                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// ��ȡ��ɱ�������Ա����
        /// </summary>
        /// <param name="salerType">���ͱ��</param>
        /// <returns></returns>
        public static string getSalerType(int salerType)
        {
            string sale = "";
            switch (salerType)
            {
                case (int)EnmDataType.�깤¼��:
                    sale = "�깤¼��";
                    break;
                case (int)EnmDataType.��������:
                    sale = "��������";
                    break;
                case (int)EnmDataType.����������:
                    sale = "����������";
                    break;
                default:
                    break;
            }
            return sale;
        }

        /// <summary>
        /// ��ȡ��Ա��ɫ����
        /// </summary>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public static string getUserRoleType(int UserType)
        {
            string sale = "";
            switch (UserType)
            {
                case (int)EnmUserRole.����:
                    sale = "����";
                    break;
                case (int)EnmUserRole.¼����Ա:
                    sale = "¼����Ա";
                    break;
                case (int)EnmUserRole.ϵͳ����Ա:
                    sale = "ϵͳ����Ա";
                    break;
                case (int)EnmUserRole.��������:
                    sale = "��������";
                    break;
                case (int)EnmUserRole.�����ܼ�:
                    sale = "�����ܼ�";
                    break;
                default:
                    break;
            }
            return sale;
        }
        /// <summary>
        /// ��ȡ��·֧������
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static string getPayType(int payType)
        {
            string pay = "";
            switch (payType)
            { 
                case (int)EnmPayType.һ���Ը�:
                    pay = "һ���Ը�";
                    break;
                case (int)EnmPayType.����:
                    pay = "����";
                    break;
                case (int)EnmPayType.���긶:
                    pay = "���긶";
                    break;
                case (int)EnmPayType.�¸�:
                    pay = "�¸�";
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
                case (int)EnmPayType.һ���Ը�:
                    months = 1;
                    break;
                case (int)EnmPayType.����:
                    months = 3;
                    break;
                case (int)EnmPayType.���긶:
                    months = 2;
                    break;
                case (int)EnmPayType.�¸�:
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
                case (int)EnmCableStatus.���깤:
                    return "���깤";
                case (int)EnmCableStatus.�Ѳ��:
                    return "�Ѳ��";
                case (int)EnmCableStatus.δ�깤:
                    return "δ�깤";
                case (int)EnmCableStatus.ȡ��:
                    return "ȡ��";
                case (int)EnmCableStatus.���ټ۸��½�:
                    return "���ټ۸��½�";
                case (int)EnmCableStatus.���ټ۸񲻱�:
                    return "���ټ۸񲻱�";
                case (int)EnmCableStatus.���ټ۸�����:
                    return "���ټ۸�����";
                case (int)EnmCableStatus.���ټ۸��½�:
                    return "���ټ۸��½�";
                case (int)EnmCableStatus.�������겻��:
                    return "�������겻��";
                default:
                    return "";
            }
        }
        /// <summary>
        /// ��ȡ��ɱ�������Ա����
        /// </summary>
        /// <param name="salerType">���ͱ��</param>
        /// <returns></returns>
        public static string getChangeType(int changeType)
        {
            string type = "";
            switch (changeType)
            {
                case (int)EnmChangeType.��·״̬���:
                    type = "��·״̬���";
                    break;
                case (int)EnmChangeType.ȫ�����:
                    type = "ȫ�����";
                    break;
                case (int)EnmChangeType.�����깤��¼:
                    type = "�����깤��¼";
                    break;
                case (int)EnmChangeType.�������������:
                    type = "�������������";
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

        #region �ж������Ƿ�����
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
                Log.Info(DateTime.Now.ToString() + "��¼ʧ�ܣ�" + ex.Message);
                Log.Info("================================================");
                return false;
            }
        }
        #endregion
    }
}
