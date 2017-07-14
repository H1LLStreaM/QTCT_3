using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Data;
using WY.Common.Utility;
using WY.Common.Message;
using System.Data;
using Library.Dao;
using NHibernate.Expression;
using System.Data.Common;
using WY.Common.Framework;
using Castle.ActiveRecord;
using WY.Library.Model;

namespace WY.Library.Business
{
    public class CommissionBusiness
    {
        #region ������ɱ���
        public static bool saveRatio(Cable cable,DateTime startTime,int businesstypeid,RatioDate ratio)
        {
            try
            {
                int CableId = cable.Id;         //��·����ID
                int MainUserId = cable.Userid;  //��������Ա
                int WriteUserId = cable.Controluserid;  //¼����ԱID
                if (getCommissRatio(CableId, MainUserId) == 0)
                {
                    //д��������������ɱ���
                    save(CableId, MainUserId, (int)EnmDataType.����������, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid,ratio);
                }
                if (getCommissRatio(CableId, WriteUserId) == 0)
                {
                    //д��������������ɱ���
                    save(CableId, WriteUserId, (int)EnmDataType.�깤¼��, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid,ratio);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "���������Ϣʧ�ܡ�");
                return false;
            }

        }

        public static void saveRatio(Commission c)
        {
            c.Save();
        }
        #endregion 

        #region �����������Ա�����Ϣ
        //public static void saveMainSaler(Cable cable,DateTime startTime,int businesstypeid)
        //{
        //    try
        //    {
        //        int mainId = getCommissMainRatio(cable.Id);  //ԭ��·����������ID
        //        if (mainId != cable.Userid)
        //        {
        //            save(cable.Id, cable.Userid, (int)EnmDataType.����������, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid);                   
        //        }               
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //    }
        //}
        #endregion

        #region ��ѯ��������Ա��ɱ���
        private static int getCommissMainRatio(int cableid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select UserId from Commission where Cableid=@Cableid and Isdeleted = @Isdeleted and endTime is null and usertype=@role";
                    DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid),  db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.ʹ����),
                                                db.CreateParameter("@role",(int)EnmDataType.����������) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                    if (tb.Rows.Count == 0)
                    {
                        return -1;
                    }
                    return Utils.NvInt(tb.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return -1;
            }
        }
        #endregion

        #region �ж������Ƿ��ظ�
        public static int getCommissRatio(int cableid, int salerid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {                    
                    string sql = "select count(1) from Commission where Cableid=@Cableid and UserId=@salerid and Isdeleted = @Isdeleted";
                    DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid), db.CreateParameter("@salerid", salerid), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.ʹ����) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  
                    return Utils.NvInt(tb.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return -1;
            }
        }
        #endregion

        #region ������ɱ�����˰���ж������Ƿ��ظ�
        public static int getCommissRatio(int cableid, int salerid, decimal ratio, decimal tax)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "select count(1) from Commission where Cableid=@Cableid and UserId=@salerid and ratio=@ratio and tax=@tax and Isdeleted = @Isdeleted";
                DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid), db.CreateParameter("@salerid", salerid), db.CreateParameter("@ratio", ratio),
                                        db.CreateParameter("@tax", tax), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.ʹ����) };
                DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                return Utils.NvInt(tb.Rows[0][0].ToString());
            }
        }
        #endregion

        #region
        private static void save(int cableid, int mainId,int userType,int cstatus,DateTime startTime,int cableclass,int businessTypeid,RatioDate ratio)
        {
            //�½���������
            string cablestatus = GlobalBusiness.getCableStatus(cstatus);
            saveCommission(cableid, mainId, userType, cablestatus, startTime, cableclass, businessTypeid,ratio);
        }
        #endregion

        #region �������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="cableid">��·ID</param>
        /// <param name="salerid">����ID</param>
        /// <param name="salerType">������Ա����</param>
        public static void saveCommission(int cableid,int salerid,int salerType,string cablestatus,DateTime startTime,int cableclass,int businessTypeid,RatioDate ratio)
        {
            try
            {
                decimal ratiovalue = 0;
                decimal taxvalue = 0;
                string dataname = cablestatus;
                //DataTable tb = getCableRatio(cableclass,businessTypeid);                
                if (salerType == (int)EnmDataType.����������)
                {
                    //dataname = "����������" + dataname;
                    //DataRow row = tb.Select("dataname='" + dataname + "'")[0];
                    ratiovalue = ratio.RatioMian;
                    taxvalue = ratio.TaxMain;

                }
                else if (salerType == (int)EnmDataType.�깤¼��)
                {
                    //dataname = "�깤¼��" + dataname;
                    //DataRow row = tb.Select("dataname='" + dataname + "'")[0];
                    ratiovalue = ratio.RatioWrite;
                    taxvalue = ratio.TaxWrite;
                }
                else if (salerType == (int)EnmDataType.��������)
                {
                    //dataname = "��������" + dataname;
                    //DataRow row = tb.Select("dataname='" + dataname + "'")[0];
                    ratiovalue = ratio.RatioSaler;
                    taxvalue = ratio.TaxSaler;
                }
                Commission c = new Commission();
                c.Cableid = cableid;
                c.Userid = salerid;
                c.Usertype = salerType;
                c.Tax = taxvalue;
                c.Ratio = ratiovalue;
                c.Begintime = startTime.Date;
                c.Isdeleted = (int)EnmIsdeleted.ʹ����;
                c.Create();
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999",ex.Message);
            }
        }

        /// <summary>
        /// ������ɡ�˰��
        /// </summary>
        /// <param name="cableid">��·ID</param>
        /// <param name="salerid">����ID</param>
        /// <param name="ratio">�����</param>
        /// <param name="tax">˰��</param>
        public static bool saveCommission(int cableid, int salerid, decimal ratio,decimal tax,DateTime start,int userType)
        {
            try
            {
                Commission c = new Commission();
                c.Cableid = cableid;
                c.Userid = salerid;
                c.Usertype = userType;
                c.Tax = tax;
                c.Ratio = ratio;
                c.Begintime = start;
                c.Isdeleted = (int)EnmIsdeleted.ʹ����;
                c.Create();
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ������ɡ�˰��
        /// </summary>
        /// <param name="cableid">��·ID</param>
        /// <param name="salerid">����ID</param>
        /// <param name="ratio">�����</param>
        /// <param name="tax">˰��</param>
        public static bool saveCommission(int cableid, int salerid,int usertype, decimal ratio, decimal tax, DateTime start)
        {
            try
            {
                Commission c = new Commission();
                c.Cableid = cableid;
                c.Userid = salerid;
                c.Usertype = usertype;
                c.Tax = tax;
                c.Ratio = ratio;
                c.Begintime = start;
                c.Isdeleted = (int)EnmIsdeleted.ʹ����;
                c.Create();
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "���������Ϣ��������");
                return false;
            }
        }

        public static void saveCommission(List<Commission> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Save();
            }
        }
        #endregion

        #region ɾ�������Ϣ
        public static bool delSaler(int cableid, int salerid, DateTime endDate)
        {
            try
            {
                Commission c = CommissionDao.FindFirst(new EqExpression("Userid", salerid), new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                if (c != null)
                {
                    c.Endtime = endDate;
                    c.Isdeleted = (int)EnmIsdeleted.��ɾ��;
                    c.Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "ɾ����������ʧ�ܣ�");
                return false;
            }
        }

        public static bool delSaler(int cableid, DateTime endDate)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                if (cs != null)
                {
                    for (int i = 0; i < cs.Length; i++)
                    {
                        cs[i].Endtime = endDate;
                        cs[i].Isdeleted = (int)EnmIsdeleted.��ɾ��;
                        cs[i].Update();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "ɾ����������ʧ�ܣ�");
                return false;
            }
        }

        public static bool delAllSaler(int cableId)
        {
            try
            {
                DateTime end = TableManager.DBServerTime().Date;
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update commission set Isdeleted = @isDeleted,EndTime=@endtime where EndTime is not null and cableid=@cableid";
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.��ɾ��),db.CreateParameter("@endtime",end),
                                               db.CreateParameter("cableid",cableId) };
                    db.ExecuteNonQuery(sql,paramlist);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "������ɱ��ʷ�������");
                return false;
            }
        }

        public static bool delAllSaler(int cableId,DateTime endtime)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update commission set Isdeleted = @isDeleted,EndTime=@endtime where cableid=@cableid and endTime is null";
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.��ɾ��),db.CreateParameter("@endtime",endtime),
                                               db.CreateParameter("cableid",cableId) };
                    db.ExecuteNonQuery(sql, paramlist);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "������ɱ��ʷ�������");
                return false;
            }
        }

        public static void delCommission(int commId)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "DELETE FROM Commission where id=" + commId;
                db.ExecuteNonQuery(sql);
            }
        }
        #endregion

        #region ��ȡԤ�����õ���ɱ���
        private static DataTable getCableRatio(int cableclass,int businesstypeid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select * from datamanage where  businessid=@bid";
                    DbParameter[] paramlist = {db.CreateParameter("@bid",businesstypeid)};
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return new DataTable();
            }
        }
        #endregion

        #region ��ȡ��ɱ�����Ϣ
        public static Commission[] getCommissions(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new Order("Usertype",true), new EqExpression("Cableid", cableId), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                return cs;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "��ȡ�����Ϣ����");
                return null;
            }
        }

        public static Commission[] getCommissionsWithNoEndDate(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new Order("Usertype", true), new EqExpression("Cableid", cableId), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),new NullExpression("Endtime"));
                return cs;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "��ȡ�����Ϣ����");
                return null;
            }
        }

        public static Commission[] getAllCommissions(int cableId)
        {
            try
            {
                Order[] orders = { new Order("Endtime", true), new Order("Userid", true), new Order("Begintime", true) };
                Commission[] cs = CommissionDao.FindAll(orders, new EqExpression("Cableid", cableId));
                return cs;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "��ȡ�����Ϣ����");
                return null;
            }
        }

        public static Commission[] getCommissions(int cableid, int userid)
        {
            try
            {
                if (userid == 0)
                {
                    Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                    return cs;
                }
                else
                {
                    Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Userid", userid), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                    return cs;
                }
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "��ȡ�����Ϣ����");
                return null;
            }
        }
        #endregion

        #region ������ɱ���
        public static bool UpdataCommission(int cableId, int userId, decimal ratio, decimal tax,DateTime end,DateTime start)
        {
            try
            {
                Commission c = CommissionDao.FindFirst(new EqExpression("Cableid", cableId), new EqExpression("Userid", userId), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                if (c != null)
                {
                    c.Endtime = end;
                    c.Isdeleted = (int)EnmIsdeleted.��ɾ��;
                    c.Update();
                    if (!saveCommission(cableId, userId, ratio, tax,start,c.Usertype))
                    {
                        return false;
                    }
                }                
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", ex.Message);
                return false;
            }
        }
        #endregion

        #region ��ȡ��������Ա���깤¼���������Ϣ
        public static Commission[] getMainsalerAndWriterInfo(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), new EqExpression("Cableid", cableId));
                return cs;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��ȡ�����ۺ��깤¼����Ա��Ϣ��������");
                return null;
            }
        }
        #endregion

        #region �ж���ɽ�ֹ�����Ƿ���Ч
        public static bool isLegitimateData(int cableid, int salerid, DateTime endDate)
        {
            try
            {
                Commission cs = CommissionDao.FindFirst(new EqExpression("Cableid", cableid), new EqExpression("Userid",salerid),
                                                        new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                if (cs != null)
                {
                    if (cs.Begintime <= endDate)
                    {
                        return true;
                    }
                    else
                    {
                        MessageHelper.ShowMessage("E031");
                        return false;
                    }                    
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "�����������ʧ�ܣ�");
                return false;
            }
        }
        #endregion

        #region �жϵ�·�Ƿ���������ۺ��깤¼��
        public static bool isIncludeMianandWriter(int cableid)
        {
            Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid),new OrExpression(new EqExpression("Usertype",(int)EnmDataType.�깤¼��),new EqExpression("Usertype",(int)EnmDataType.����������)),
                                                        new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            if (cs != null)
            {
                if (cs.Length==2)
                {
                    return true;
                }
                else if(cs.Length==0)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion 

        #region �жϵ�·�����Ϣ�Ƿ����ָ���¶Ⱥ����������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataTable isExistCommissionValueByMontAndSaler(int userId, DateTime startDate, DateTime endDate,int year,int month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = @"Select c.ratio ��ɱ���,c.tax ˰��,c.cableid ��·id, s.writeoff ��ɽ��,begintime ��ɿ�ʼʱ��,endtime ��ɽ���ʱ��,s.flag,
                                    s.Settlementstart �˵���ʼʱ��,s.Settlementend �˵�����ʱ��,s.customerid as �ͻ�id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable  
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime<=@startDate  
                                    and (endtime>=@endDate or endtime is null)
                                    And userid=@userId and s.isdeleted=@isdel and s.year=@dateYear and s.month=@dateMonth 
                                    union 
                                    Select c.ratio ��ɱ���,c.tax ˰��,c.cableid ��·id, s.writeoff ��ɽ��,begintime ��ɿ�ʼʱ��,endtime ��ɽ���ʱ��,s.flag,
                                    s.Settlementstart �˵���ʼʱ��,s.Settlementend �˵�����ʱ��,s.customerid as �ͻ�id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime<=@startDate 
                                    and endtime<=@endDate and endtime>@startDate 
                                    And userid=@userId and s.isdeleted=@isdel  and s.year=@dateYear and s.month=@dateMonth 
                                    union  
                                    Select c.ratio ��ɱ���,c.tax ˰��,c.cableid ��·id, s.writeoff ��ɽ��,begintime ��ɿ�ʼʱ��,endtime ��ɽ���ʱ��,s.flag,
                                    s.Settlementstart �˵���ʼʱ��,s.Settlementend �˵�����ʱ��,s.customerid as �ͻ�id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime>=@startDate  and begintime<=@endDate 
                                    and endtime<=@endDate 
                                    And userid=@userId and s.isdeleted=@isdel and s.year=@dateYear and s.month=@dateMonth  
                                    union 
                                    Select c.ratio ��ɱ���,c.tax ˰��,c.cableid ��·id, s.writeoff ��ɽ��,begintime ��ɿ�ʼʱ��,endtime ��ɽ���ʱ��,s.flag,
                                    s.Settlementstart �˵���ʼʱ��,s.Settlementend �˵�����ʱ��,s.customerid as �ͻ�id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime>=@startDate and begintime<=@endDate 
                                    and (endtime>=@endDate or endtime is null)
                                    And userid=@userId and s.isdeleted=@isdel  and s.year=@dateYear and s.month=@dateMonth ";

                    DbParameter[] param = new DbParameter[6];
                    param[0] = db.CreateParameter("@startDate", startDate);
                    param[1] = db.CreateParameter("@endDate", endDate);
                    param[2] = db.CreateParameter("@userId", userId);
                    param[3] = db.CreateParameter("@dateYear", year);
                    param[4] = db.CreateParameter("@dateMonth", month);
                    //param[5] = db.CreateParameter("@flag", (int)EnmWriteOffFlag.��������);
                    param[5] = db.CreateParameter("@isdel", (int)EnmIsdeleted.ʹ����);
                    DataSet ds = db.GetDataSet(sql, param);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region ��ȡ�¶Ȳ���
        public static DataTable getSupplementByMonth(int year, int month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = @"Select * from salebills where year=@year and month=@month and flag=@flag and idDeleted=@del";
                    DbParameter[] param = new DbParameter[4];
                    param[0] = db.CreateParameter("year", year);
                    param[1] = db.CreateParameter("month", month);
                    param[2] = db.CreateParameter("flag", (int)EnmWriteOffFlag.������);
                    param[4] = db.CreateParameter("del", (int)EnmIsdeleted.ʹ����);
                    DataSet ds = db.GetDataSet(sql, param);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
