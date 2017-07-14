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
        #region 保存提成比率
        public static bool saveRatio(Cable cable,DateTime startTime,int businesstypeid,RatioDate ratio)
        {
            try
            {
                int CableId = cable.Id;         //电路代码ID
                int MainUserId = cable.Userid;  //主销售人员
                int WriteUserId = cable.Controluserid;  //录入人员ID
                if (getCommissRatio(CableId, MainUserId) == 0)
                {
                    //写入主销售渠道提成比例
                    save(CableId, MainUserId, (int)EnmDataType.主销售渠道, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid,ratio);
                }
                if (getCommissRatio(CableId, WriteUserId) == 0)
                {
                    //写入主销售渠道提成比例
                    save(CableId, WriteUserId, (int)EnmDataType.完工录入, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid,ratio);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "保存提成信息失败。");
                return false;
            }

        }

        public static void saveRatio(Commission c)
        {
            c.Save();
        }
        #endregion 

        #region 变更主销售人员提成信息
        //public static void saveMainSaler(Cable cable,DateTime startTime,int businesstypeid)
        //{
        //    try
        //    {
        //        int mainId = getCommissMainRatio(cable.Id);  //原电路代码主销售ID
        //        if (mainId != cable.Userid)
        //        {
        //            save(cable.Id, cable.Userid, (int)EnmDataType.主销售渠道, cable.Cablestatus, startTime, cable.Cableclass, businesstypeid);                   
        //        }               
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //    }
        //}
        #endregion

        #region 查询主销售人员提成比率
        private static int getCommissMainRatio(int cableid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select UserId from Commission where Cableid=@Cableid and Isdeleted = @Isdeleted and endTime is null and usertype=@role";
                    DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid),  db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.使用中),
                                                db.CreateParameter("@role",(int)EnmDataType.主销售渠道) };
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

        #region 判断渠道是否重复
        public static int getCommissRatio(int cableid, int salerid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {                    
                    string sql = "select count(1) from Commission where Cableid=@Cableid and UserId=@salerid and Isdeleted = @Isdeleted";
                    DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid), db.CreateParameter("@salerid", salerid), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.使用中) };
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

        #region 根据提成比例和税率判断渠道是否重复
        public static int getCommissRatio(int cableid, int salerid, decimal ratio, decimal tax)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "select count(1) from Commission where Cableid=@Cableid and UserId=@salerid and ratio=@ratio and tax=@tax and Isdeleted = @Isdeleted";
                DbParameter[] paramlist = { db.CreateParameter("@Cableid", cableid), db.CreateParameter("@salerid", salerid), db.CreateParameter("@ratio", ratio),
                                        db.CreateParameter("@tax", tax), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.使用中) };
                DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                return Utils.NvInt(tb.Rows[0][0].ToString());
            }
        }
        #endregion

        #region
        private static void save(int cableid, int mainId,int userType,int cstatus,DateTime startTime,int cableclass,int businessTypeid,RatioDate ratio)
        {
            //新建渠道比例
            string cablestatus = GlobalBusiness.getCableStatus(cstatus);
            saveCommission(cableid, mainId, userType, cablestatus, startTime, cableclass, businessTypeid,ratio);
        }
        #endregion

        #region 保存提成
        /// <summary>
        /// 保存提成
        /// </summary>
        /// <param name="cableid">电路ID</param>
        /// <param name="salerid">销售ID</param>
        /// <param name="salerType">销售人员类型</param>
        public static void saveCommission(int cableid,int salerid,int salerType,string cablestatus,DateTime startTime,int cableclass,int businessTypeid,RatioDate ratio)
        {
            try
            {
                decimal ratiovalue = 0;
                decimal taxvalue = 0;
                string dataname = cablestatus;
                //DataTable tb = getCableRatio(cableclass,businessTypeid);                
                if (salerType == (int)EnmDataType.主销售渠道)
                {
                    //dataname = "主销售渠道" + dataname;
                    //DataRow row = tb.Select("dataname='" + dataname + "'")[0];
                    ratiovalue = ratio.RatioMian;
                    taxvalue = ratio.TaxMain;

                }
                else if (salerType == (int)EnmDataType.完工录入)
                {
                    //dataname = "完工录入" + dataname;
                    //DataRow row = tb.Select("dataname='" + dataname + "'")[0];
                    ratiovalue = ratio.RatioWrite;
                    taxvalue = ratio.TaxWrite;
                }
                else if (salerType == (int)EnmDataType.销售渠道)
                {
                    //dataname = "销售渠道" + dataname;
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
                c.Isdeleted = (int)EnmIsdeleted.使用中;
                c.Create();
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999",ex.Message);
            }
        }

        /// <summary>
        /// 保存提成、税率
        /// </summary>
        /// <param name="cableid">电路ID</param>
        /// <param name="salerid">渠道ID</param>
        /// <param name="ratio">提成率</param>
        /// <param name="tax">税率</param>
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
                c.Isdeleted = (int)EnmIsdeleted.使用中;
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
        /// 保存提成、税率
        /// </summary>
        /// <param name="cableid">电路ID</param>
        /// <param name="salerid">渠道ID</param>
        /// <param name="ratio">提成率</param>
        /// <param name="tax">税率</param>
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
                c.Isdeleted = (int)EnmIsdeleted.使用中;
                c.Create();
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "保存提成信息发生错误。");
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

        #region 删除提成信息
        public static bool delSaler(int cableid, int salerid, DateTime endDate)
        {
            try
            {
                Commission c = CommissionDao.FindFirst(new EqExpression("Userid", salerid), new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                if (c != null)
                {
                    c.Endtime = endDate;
                    c.Isdeleted = (int)EnmIsdeleted.已删除;
                    c.Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "删除渠道比例失败！");
                return false;
            }
        }

        public static bool delSaler(int cableid, DateTime endDate)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                if (cs != null)
                {
                    for (int i = 0; i < cs.Length; i++)
                    {
                        cs[i].Endtime = endDate;
                        cs[i].Isdeleted = (int)EnmIsdeleted.已删除;
                        cs[i].Update();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "删除渠道比例失败！");
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
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.已删除),db.CreateParameter("@endtime",end),
                                               db.CreateParameter("cableid",cableId) };
                    db.ExecuteNonQuery(sql,paramlist);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "更新提成比率发生错误！");
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
                    DbParameter[] paramlist = { db.CreateParameter("@isDeleted", (int)EnmIsdeleted.已删除),db.CreateParameter("@endtime",endtime),
                                               db.CreateParameter("cableid",cableId) };
                    db.ExecuteNonQuery(sql, paramlist);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "更新提成比率发生错误！");
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

        #region 获取预先设置的提成比率
        private static DataTable getCableRatio(int cableclass,int businesstypeid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select * from datamanage where  businessid=@bid";
                    DbParameter[] paramlist = {db.CreateParameter("@bid",businesstypeid)};
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
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

        #region 获取提成比率信息
        public static Commission[] getCommissions(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new Order("Usertype",true), new EqExpression("Cableid", cableId), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return cs;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "获取提成信息错误。");
                return null;
            }
        }

        public static Commission[] getCommissionsWithNoEndDate(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new Order("Usertype", true), new EqExpression("Cableid", cableId), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),new NullExpression("Endtime"));
                return cs;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "获取提成信息错误。");
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
                MessageHelper.ShowMessage("E999", "获取提成信息错误。");
                return null;
            }
        }

        public static Commission[] getCommissions(int cableid, int userid)
        {
            try
            {
                if (userid == 0)
                {
                    Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                    return cs;
                }
                else
                {
                    Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid), new EqExpression("Userid", userid), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                    return cs;
                }
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageHelper.ShowMessage("E999", "获取提成信息错误。");
                return null;
            }
        }
        #endregion

        #region 更新提成比率
        public static bool UpdataCommission(int cableId, int userId, decimal ratio, decimal tax,DateTime end,DateTime start)
        {
            try
            {
                Commission c = CommissionDao.FindFirst(new EqExpression("Cableid", cableId), new EqExpression("Userid", userId), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                if (c != null)
                {
                    c.Endtime = end;
                    c.Isdeleted = (int)EnmIsdeleted.已删除;
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

        #region 获取主销售人员和完工录入者提成信息
        public static Commission[] getMainsalerAndWriterInfo(int cableId)
        {
            try
            {
                Commission[] cs = CommissionDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), new EqExpression("Cableid", cableId));
                return cs;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "获取主销售和完工录入人员信息发生错误。");
                return null;
            }
        }
        #endregion

        #region 判断提成截止日期是否有效
        public static bool isLegitimateData(int cableid, int salerid, DateTime endDate)
        {
            try
            {
                Commission cs = CommissionDao.FindFirst(new EqExpression("Cableid", cableid), new EqExpression("Userid",salerid),
                                                        new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
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
                MessageHelper.ShowMessage("E999", "渠道比例变更失败！");
                return false;
            }
        }
        #endregion

        #region 判断电路是否包含主销售和完工录入
        public static bool isIncludeMianandWriter(int cableid)
        {
            Commission[] cs = CommissionDao.FindAll(new EqExpression("Cableid", cableid),new OrExpression(new EqExpression("Usertype",(int)EnmDataType.完工录入),new EqExpression("Usertype",(int)EnmDataType.主销售渠道)),
                                                        new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
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

        #region 判断电路提成信息是否包含指定月度和渠道的提成
        /// <summary>
        /// 财务汇总
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
                    string sql = @"Select c.ratio 提成比例,c.tax 税率,c.cableid 电路id, s.writeoff 提成金额,begintime 提成开始时间,endtime 提成结束时间,s.flag,
                                    s.Settlementstart 账单开始时间,s.Settlementend 账单结束时间,s.customerid as 客户id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable  
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime<=@startDate  
                                    and (endtime>=@endDate or endtime is null)
                                    And userid=@userId and s.isdeleted=@isdel and s.year=@dateYear and s.month=@dateMonth 
                                    union 
                                    Select c.ratio 提成比例,c.tax 税率,c.cableid 电路id, s.writeoff 提成金额,begintime 提成开始时间,endtime 提成结束时间,s.flag,
                                    s.Settlementstart 账单开始时间,s.Settlementend 账单结束时间,s.customerid as 客户id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime<=@startDate 
                                    and endtime<=@endDate and endtime>@startDate 
                                    And userid=@userId and s.isdeleted=@isdel  and s.year=@dateYear and s.month=@dateMonth 
                                    union  
                                    Select c.ratio 提成比例,c.tax 税率,c.cableid 电路id, s.writeoff 提成金额,begintime 提成开始时间,endtime 提成结束时间,s.flag,
                                    s.Settlementstart 账单开始时间,s.Settlementend 账单结束时间,s.customerid as 客户id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
                                    From commission as c join salebills as s on c.cableid=s.cableid 
                                    Where begintime>=@startDate  and begintime<=@endDate 
                                    and endtime<=@endDate 
                                    And userid=@userId and s.isdeleted=@isdel and s.year=@dateYear and s.month=@dateMonth  
                                    union 
                                    Select c.ratio 提成比例,c.tax 税率,c.cableid 电路id, s.writeoff 提成金额,begintime 提成开始时间,endtime 提成结束时间,s.flag,
                                    s.Settlementstart 账单开始时间,s.Settlementend 账单结束时间,s.customerid as 客户id,IFNULL(s.proxy,0) as proxy,IFNULL(s.Receivable,0) as Receivable   
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
                    //param[5] = db.CreateParameter("@flag", (int)EnmWriteOffFlag.正常结算);
                    param[5] = db.CreateParameter("@isdel", (int)EnmIsdeleted.使用中);
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

        #region 获取月度补结
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
                    param[2] = db.CreateParameter("flag", (int)EnmWriteOffFlag.补结算);
                    param[4] = db.CreateParameter("del", (int)EnmIsdeleted.使用中);
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
