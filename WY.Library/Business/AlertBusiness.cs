using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WY.Common.Data;
using WY.Common.Framework;
using System.Data.Common;
using WY.Common.Utility;
using WY.Common.Message;
using System.Windows.Forms;
using WY.Common;

namespace WY.Library.Business
{
    /// <summary>
    /// ��������
    /// </summary>
    public class AlertBusiness
    {
        #region �ж��Ƿ���������Ϣ
        public static int isHavebusinessAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    DateTime serverTime = TableManager.DBServerTime().Date;
                    string sql = "select COUNT(1) "
                            + " from cable as c left join tb_user u on c.userid=u.id and c.userRemindTime<='" + serverTime + "' "
                            + " left join customer as cu on c.customerid=cu.id where c.isDeleted =@del and u.isdeleted=@del and cu.isdeleted=@del "
                            + "And Controluserid=@Controluserid";

                    DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.ʹ����), db.CreateParameter("Controluserid",Global.g_userid) };

                    return Utils.NvInt(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "��ȡ��·����������Ϣʧ�ܡ�");
                return -1;
            }
        }
        #endregion

        #region �ж��Ƿ�����ɱ����Ϣ
        public static int isHaveratioAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select COUNT(1) from cable where default2 is not null and isDeleted=" + (int)EnmIsdeleted.ʹ���� + " And CableStatus<>" + (int)EnmCableStatus.ȡ�� + " "
                                + "And CableStatus<>" + (int)EnmCableStatus.�Ѳ�� + " ";

                    DbParameter[] paramlist = { };

                    return Utils.NvInt(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "��ȡ��·����������Ϣʧ�ܡ�");
                return -1;
            }
        }
        #endregion

        #region ҵ������
        public static DataTable businessAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    DateTime serverTime = TableManager.DBServerTime().Date;
                    string sql = "select c.id as ��·id,cablenumber as ��·����,completeTime as �깤����,(case paytype when " + (int)EnmPayType.һ���Ը� + " then 'һ���Ը�' when " + (int)EnmPayType.���� + " then '����' when " + (int)EnmPayType.���긶 + " then '���긶' "
                    + " when " + (int)EnmPayType.�¸� + " then '�¸�' end) as ��������,cu.customerName as �ͻ�����,u.name as ��������,endDate as ��ͬ��ֹ�� "
                    + " from cable as c left join tb_user u on c.userid=u.id and c.userRemindTime<='" + serverTime + "' "
                    + " left join customer as cu on c.customerid=cu.id where c.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and u.isdeleted=" + (int)EnmIsdeleted.ʹ���� + " and cu.isdeleted=" + (int)EnmIsdeleted.ʹ���� + " "
                    + " And c.Controluserid=" + Global.g_userid + " and c.CableStatus<>" + (int)EnmCableStatus.�Ѳ�� + "";
                    //DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql).Tables[0];  //������������
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "�������ѹ��ܷ�������");
                return new DataTable();
            }
        }
        #endregion

        #region �����ܼ�����
        public static DataTable managerAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    DateTime serverTime = TableManager.DBServerTime().Date;
                    string sql = "select c.id as ��·id,cablenumber as ��·����,completeTime as �깤����,(case paytype when " + (int)EnmPayType.һ���Ը� + " then 'һ���Ը�' when " + (int)EnmPayType.���� + " then '����' when " + (int)EnmPayType.���긶 + " then '���긶' "
                               + " when " + (int)EnmPayType.�¸� + " then '�¸�' end) as ��������,cu.customerName as �ͻ�����,u.name as ��������,limitTime as ��ͬ��ֹ�� "
                               + " from cable as c left join tb_user u on c.userid=u.id and c.ManagerRemindTime<'" + serverTime + "' "
                               + " left join customer as cu on c.customerid=cu.id where c.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and u.isdeleted=" + (int)EnmIsdeleted.ʹ���� + " and cu.isdeleted=" + (int)EnmIsdeleted.ʹ���� + "";

                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "�ܼ����ѹ��ܷ�������");
                return new DataTable();
            }
        }
        #endregion      
    }
}
