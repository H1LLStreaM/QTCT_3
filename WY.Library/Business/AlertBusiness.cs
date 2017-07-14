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
    /// 报警提醒
    /// </summary>
    public class AlertBusiness
    {
        #region 判断是否有提醒消息
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

                    DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.使用中), db.CreateParameter("Controluserid",Global.g_userid) };

                    return Utils.NvInt(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "获取电路到期提醒信息失败。");
                return -1;
            }
        }
        #endregion

        #region 判断是否有提成变更消息
        public static int isHaveratioAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select COUNT(1) from cable where default2 is not null and isDeleted=" + (int)EnmIsdeleted.使用中 + " And CableStatus<>" + (int)EnmCableStatus.取消 + " "
                                + "And CableStatus<>" + (int)EnmCableStatus.已拆机 + " ";

                    DbParameter[] paramlist = { };

                    return Utils.NvInt(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "获取电路到期提醒信息失败。");
                return -1;
            }
        }
        #endregion

        #region 业务提醒
        public static DataTable businessAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    DateTime serverTime = TableManager.DBServerTime().Date;
                    string sql = "select c.id as 电路id,cablenumber as 电路代码,completeTime as 完工日期,(case paytype when " + (int)EnmPayType.一次性付 + " then '一次性付' when " + (int)EnmPayType.季付 + " then '季付' when " + (int)EnmPayType.半年付 + " then '半年付' "
                    + " when " + (int)EnmPayType.月付 + " then '月付' end) as 付款类型,cu.customerName as 客户名称,u.name as 销售渠道,endDate as 合同截止日 "
                    + " from cable as c left join tb_user u on c.userid=u.id and c.userRemindTime<='" + serverTime + "' "
                    + " left join customer as cu on c.customerid=cu.id where c.isDeleted =" + (int)EnmIsdeleted.使用中 + " and u.isdeleted=" + (int)EnmIsdeleted.使用中 + " and cu.isdeleted=" + (int)EnmIsdeleted.使用中 + " "
                    + " And c.Controluserid=" + Global.g_userid + " and c.CableStatus<>" + (int)EnmCableStatus.已拆机 + "";
                    //DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql).Tables[0];  //销售渠道数据
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "渠道提醒功能发生错误！");
                return new DataTable();
            }
        }
        #endregion

        #region 销售总监提醒
        public static DataTable managerAlert()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    DateTime serverTime = TableManager.DBServerTime().Date;
                    string sql = "select c.id as 电路id,cablenumber as 电路代码,completeTime as 完工日期,(case paytype when " + (int)EnmPayType.一次性付 + " then '一次性付' when " + (int)EnmPayType.季付 + " then '季付' when " + (int)EnmPayType.半年付 + " then '半年付' "
                               + " when " + (int)EnmPayType.月付 + " then '月付' end) as 付款类型,cu.customerName as 客户名称,u.name as 销售渠道,limitTime as 合同截止日 "
                               + " from cable as c left join tb_user u on c.userid=u.id and c.ManagerRemindTime<'" + serverTime + "' "
                               + " left join customer as cu on c.customerid=cu.id where c.isDeleted =" + (int)EnmIsdeleted.使用中 + " and u.isdeleted=" + (int)EnmIsdeleted.使用中 + " and cu.isdeleted=" + (int)EnmIsdeleted.使用中 + "";

                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "总监提醒功能发生错误！");
                return new DataTable();
            }
        }
        #endregion      
    }
}
