using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Utility;
using WY.Common.Message;
using Library.Dao;
using NHibernate.Expression;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using Castle.ActiveRecord;
using WY.Common.Framework;
using WY.Common;
using WY.Library.Model;
using System.Collections;

namespace WY.Library.Business
{
    public class CableBusiness
    {
        #region 保存电路代码
        public static int cableSave(Cable cable)
        {
            try
            {
                cable.Save();
                return cable.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return -1;
            }
        }
        #endregion

        #region 更新电路代码
        public static void cableUpdate(Cable cable)
        {
            try
            {
                if (cable.Updatetime == null)
                {
                    cable.Updatetime = DateTime.Now;
                }
                cable.Update();
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update cable set updateTime=@time where id=@id";
                    DbParameter[] parmar = { db.CreateParameter("@time", cable.Updatetime), db.CreateParameter("@id", cable.Id) };
                    db.ExecuteNonQuery(sql, parmar);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        #endregion

        #region 根据客户ID查询电路代码
        public static Cable[] getCablesByCustomerId(int cusId)
        {
            try
            {
                if (Global.g_usergroupid == (int)EnmUserRole.销售总监)
                {
                    Cable[] c = CableDao.FindAll(new Order("Createtime",false), new EqExpression("Customerid", cusId), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                    return c;
                }
                else if (Global.g_usergroupid == (int)EnmUserRole.录入人员)
                {
                    Cable[] c = CableDao.FindAll(new Order("Createtime", false), new EqExpression("Customerid", cusId), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), new EqExpression("Controluserid", Global.g_userid));
                    return c;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region 根据电路代码ID查询
        public static Cable getCalbeByCableId(int cableid)
        {
            try
            {
                return CableDao.FindFirst(new EqExpression("Id", cableid));
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region 根据电路代码查询
        public static Cable getCalbeByCableNumber(string Cablenumber)
        {
            try
            {
                return CableDao.FindFirst(new EqExpression("Cablenumber", Cablenumber), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取全部已完工的电路代码
        public static DataTable getCompleteCalbe(int year, int month)
        {
            string str = year.ToString() + "-"+month.ToString() + "-1";
            DateTime date = DateTime.Parse(str).Date; //当月第一天
            string startDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = date.AddMonths(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");   //当月最后一天
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select h.cableid,h.cablenumber,h.customerid from cablehistory as h  "
                               + "where ("
                               + "(h.Startdate<='" + startDate + "' and  h.Enddate>='" + endDate + "')"
                               + "or"
                               + "(h.Startdate<='" + startDate + "' and  h.Enddate<='" + endDate + "' and h.Enddate>='" + startDate + "')"
                               + "or"
                               + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate>='" + endDate + "')"
                               + "or"
                               + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate<='" + endDate + "'))"
                               + "and (h.Cablestatus<>" + (int)EnmCableStatus.未完工 + " and h.Cablestatus<>" + (int)EnmCableStatus.取消 + " and h.Isdeleted=" + (int)EnmIsdeleted.使用中 + ") ";
                    DataTable dt = db.GetDataSet(sql).Tables[0];
                    return dt;
                }

                //AndExpression and1 = new AndExpression(new AndExpression(new GeExpression("Startdate", startDate), new LeExpression("Startdate", endDate)), new GeExpression("Enddate", endDate));

                //AndExpression and2 = new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", endDate));

                //AndExpression and3 = new AndExpression(new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", startDate)), new LeExpression("Enddate", endDate));

                //AndExpression and4 = new AndExpression(new GeExpression("Enddate", startDate), new LeExpression("Enddate", endDate));

                
                //OrExpression orExpress = new OrExpression(new OrExpression(new OrExpression(and1, and2), and3), and4);
                //Cable[] cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                //                              new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
                //                              //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
                //                              new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)),
                //                              orExpress);
                //return cb;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取全部已完工的电路代码
        public static DataTable getCompleteCalbe(int year, int month, int cableid, int customerid)
        {
            string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            DateTime startDate = DateTime.Parse(strStartDate).Date; //当月第一天
            int days = DateTime.DaysInMonth(year, month);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);   //当月最后一天
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select h.* from cablehistory as h  "
                               + "where "
                               + "((h.Startdate<='" + startDate + "' and  h.Enddate>='" + endDate + "')"
                               + "or"
                               + "(h.Startdate<='" + startDate + "' and  h.Enddate<='" + endDate + "' and h.Enddate>='" + startDate + "')"
                               + "or"
                               + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate>='" + endDate + "')"
                               + "or"
                               + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate<='" + endDate + "'))"
                               + "and h.Cablestatus<>" + (int)EnmCableStatus.未完工 + " and h.Cablestatus<>" + (int)EnmCableStatus.取消 + " and h.Isdeleted=" + (int)EnmIsdeleted.使用中 + " "
                               + "and h.cableid=" + cableid + " and h.customerid=" + customerid + " order by h.createtime desc ";
                    DataTable dt = db.GetDataSet(sql).Tables[0];
                    return dt;
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


        /// <summary>
        /// 查询完工电路代码
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="month">月度</param>
        /// <param name="cable">电路代码</param>
        /// <returns></returns>
        public static DataTable getCompleteCalbe(int year, int month, string cable,int Id)
        {
            DataTable result = null;
            string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            DateTime startDate = DateTime.Parse(strStartDate).Date; //当月第一天
            int days = DateTime.DaysInMonth(year, month);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);   //当月最后一天
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select distinct(c.id) from cable as c left join cablehistory as h on h.cableid=c.id "
                              + "left join commission as m on m.cableid=c.id where m.userid="+Id+" and "
                              + "((h.Startdate<='" + startDate + "' and  h.Enddate>='" + endDate + "')"
                              + "or"
                              + "(h.Startdate<='" + startDate + "' and  h.Enddate<='" + endDate + "' and h.Enddate>='" + startDate + "')"
                              + "or"
                              + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate>='" + endDate + "')"
                              + "or"
                              + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate<='" + endDate + "'))"
                              + "and h.Cablestatus<>" + (int)EnmCableStatus.未完工 + " and h.Cablestatus<>" + (int)EnmCableStatus.取消 + " and h.Isdeleted=" + (int)EnmIsdeleted.使用中 + " "
                              + "and c.cablenumber like '%" + cable + "%' and c.Isdeleted="+(int)EnmIsdeleted.使用中+" order by h.createtime desc ";
                    if (Id < 1)
                    {
                        sql = "select distinct(c.id) from cable as c left join cablehistory as h on h.cableid=c.id "
                              + "where "
                              + "((h.Startdate<='" + startDate + "' and  h.Enddate>='" + endDate + "')"
                              + "or"
                              + "(h.Startdate<='" + startDate + "' and  h.Enddate<='" + endDate + "' and h.Enddate>='" + startDate + "')"
                              + "or"
                              + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate>='" + endDate + "')"
                              + "or"
                              + "(h.Startdate>='" + startDate + "' and h.Startdate<='" + endDate + "' and  h.Enddate<='" + endDate + "'))"
                              + "and h.Cablestatus<>" + (int)EnmCableStatus.未完工 + " and h.Cablestatus<>" + (int)EnmCableStatus.取消 + " and h.Isdeleted=" + (int)EnmIsdeleted.使用中 + " "
                              + "and c.cablenumber like '%" + cable + "%' and c.Isdeleted=" + (int)EnmIsdeleted.使用中 + " order by h.createtime desc ";
                    }
                    DataTable dt = db.GetDataSet(sql).Tables[0];
                    result = dt;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
            }
            return result;
            //string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            //DateTime startDate = DateTime.Parse(strStartDate).Date; //当月第一天
            //int days = DateTime.DaysInMonth(year, month); 
            //DateTime endDate = startDate.AddMonths(1).AddDays(-1).Date;   //当月最后一天
            //try
            //{
            //    Cable[] cb;
            //    AndExpression and1 = new AndExpression(new AndExpression(new GeExpression("Startdate", startDate), new LeExpression("Startdate", endDate)), new GeExpression("Enddate", endDate));

            //    AndExpression and2 = new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", endDate));

            //    AndExpression and3 = new AndExpression(new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", startDate)), new LeExpression("Enddate", endDate));

            //    AndExpression and4 = new AndExpression(new GeExpression("Enddate", startDate), new LeExpression("Enddate", endDate));

            //    OrExpression orExpress = new OrExpression(new OrExpression(new OrExpression(and1, and2), and3), and4);
            //    if (!string.IsNullOrEmpty(cable))  //判断电路代码编号是否有输入
            //    {
            //        cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
            //                                      new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
            //                                      //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
            //                                      new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)),
            //                                      new LikeExpression("Cablenumber", "%" + cable + "%"),
            //                                      orExpress);
            //    }
            //    else
            //    {
            //        cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
            //                                     new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
            //                                     //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
            //                                     new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)),
            //                                     orExpress);
            //    }               
            //    return cb;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex.Message);
            //    MessageHelper.ShowMessage("E999", ex.Message);
            //    return null;
            //}
        }
        
        #region 删除电路代码，同时删除对应提成比率
        public static void delCableAllinfo(int cableId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "Update cable set Isdeleted=@del where id =@id";
                    DbParameter[] parmar = { db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", cableId) };
                    db.ExecuteNonQuery(sql, parmar);  //电路代码
                    DateTime endtime = TableManager.DBServerTime().Date;
                    sql = "Update commission set endTime=@endTime,Isdeleted=@del where cableid=@id";

                    DbParameter[] parmar2 = {db.CreateParameter("@del",(int)EnmIsdeleted.已删除), db.CreateParameter("@id", cableId),
                                             db.CreateParameter("@endTime",endtime)};
                    db.ExecuteNonQuery(sql, parmar2);  //提成比率
                    db.TrnCommit();
                    MessageHelper.ShowMessage("I002");
                }
                catch (Exception ex)
                {
                    db.TrnRollBack();
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "电路代码删除失败。");

                }
            }
        }
        #endregion

        #region 根据客户ID，电路代码查询数据查询电路ID
        public static int getCountbyCusIdCable(int cusid, string cable)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select c.id from customer as u left join cable as c on c.customerid = u.id where c.isdeleted=@del and u.isdeleted=@del "
                               + " and u.id=@cusid and c.CableNumber=@cable";

                    DbParameter[] parmar = { db.CreateParameter("@cusid", cusid), db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@cable", cable) };
                    return Utils.NvInt(db.GetDataSet(sql, parmar).Tables[0].Rows[0][0].ToString());
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }
        #endregion

        #region 根据客户名称和电路代码查询电路ID
        public static int getCablebyCustomerandCablenumber(string customername, string cablenumber)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "select c.id from cable as c left join customer as u on c.customerid = u.id "
                           +"left join customerhistory as ch on ch.customerid = u.id "
                           + "where (u.customername=@cus or ch.customername=@cus) and cablenumber=@cable";
                DbParameter[] parmar = { db.CreateParameter("@cus", customername), db.CreateParameter("@cable", cablenumber) };
                DataTable dt = db.GetDataSet(sql, parmar).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return Utils.NvInt(dt.Rows[0][0].ToString());
                }
                else
                {
                    return -1;
                }
                
            }
        }
        #endregion

        #region 判断电路合同时间和查询时间的关系
        /// <summary>
        /// 判断电路合同时间和查询时间的关系
        /// </summary>
        /// <param name="contractStartDate">合同开始日期</param>
        /// <param name="contractEndDate">合同截止日期</param>
        /// <param name="searchDate">查询起始日期 每月1日</param>
        /// <returns></returns>
        public static Dateinterval anaylisContractMonth(Nullable<DateTime> contractStartDate, Nullable<DateTime> contractEndDate, DateTime searchDate)
        {
            Dateinterval interval = new Dateinterval();  

            //默认为整月
            interval.MonthType = (int)EnmMonth.全月;
            interval.StartDate = searchDate;
            interval.EndDate = searchDate.AddMonths(1).AddDays(-1);
            if (contractStartDate.Value.Year == searchDate.Year && contractStartDate.Value.Month == searchDate.Month)
            {
                //起始月
                interval.MonthType = (int)EnmMonth.起始月;
                interval.StartDate = contractStartDate.Value;
            }
            else if (contractEndDate.Value.Year == searchDate.Year && contractEndDate.Value.Month == searchDate.Month)
            {
                //截止月
                interval.MonthType = (int)EnmMonth.截止月;
                interval.EndDate = contractEndDate.Value;
            }
            return interval;
        }
        #endregion

        #region 根据条件查询电路代码
        public static DataTable getCableByCondition(Hashtable hs)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    List<DbParameter> parmar = new List<DbParameter>();
                    string condition = "";
                    if (hs.ContainsKey("writeid"))
                    {
                        condition += "and b.ControlUserId=@writeid ";
                        parmar.Add(db.CreateParameter("@writeid", hs["writeid"]));
                    }
                    if (hs.ContainsKey("salerid"))
                    {
                        condition += "and b.userid=@salerid ";
                        parmar.Add(db.CreateParameter("@salerid", hs["salerid"]));
                    }
                    if (hs.ContainsKey("cablestatus"))
                    {
                        condition += "and b.CableStatus=@cablestatus ";
                        parmar.Add(db.CreateParameter("@cablestatus", hs["cablestatus"]));
                    }
                    if (hs.ContainsKey("cablenumber"))
                    {
                        condition += "and b.Cablenumber like @cablenumber ";
                        parmar.Add(db.CreateParameter("@cablenumber", "%"+hs["cablenumber"]+"%"));
                    }
                    if (hs.ContainsKey("customer"))
                    {
                        condition += "and c.CustomerName like @customer ";
                        parmar.Add(db.CreateParameter("@customer", "%" + hs["customer"] + "%"));
                    }
                    if (hs.ContainsKey("IsDeleted"))
                    {
                        condition += "and b.IsDeleted = @del ";
                        parmar.Add(db.CreateParameter("@del", int.Parse(hs["IsDeleted"].ToString())));
                    }
                    if (hs.ContainsKey("startDate"))
                    {
                        condition += "and h.createtime>=@startdate ";
                        parmar.Add(db.CreateParameter("@startdate", hs["startDate"]));
                    }
                    if (hs.ContainsKey("endDate"))
                    {
                        condition += "and b.createtime <@enddate ";
                        parmar.Add(db.CreateParameter("@endDate", hs["endDate"]));
                    }
                    if (hs.ContainsKey("cableclass"))
                    {
                        condition += "and b.cableclass =@cableclass ";
                        parmar.Add(db.CreateParameter("@cableclass", int.Parse(hs["cableclass"].ToString())));
                    }
                    string sql = @"select case when b.cableclass=0 then '专网类' else '上网类' end cableclass ,b.Contracttype,
                                    b.id,b.createTime as 日期,customername as 客户名称,b.cablenumber as EIP编号, 
                                    b.cablestatus as 状态,b.userid as 渠道,c.name as 联系人,b.BoxNumber as 箱子编号,
                                    tel as 联系电话,c.address as 客户地址,b.PackageInfo as 套餐内容,b.money as 月租费,
                                    b.CompleteTime as 完工日期,b.ControlUserId as 录入人员,b.address1 as 甲端地址,b.address2 as  乙端地址, 
                                    max(h.createTime) 
                                    from cable as b left join customer as c on b.customerid = c.id 
                                    left join cablehistory as h on h.cableid=b.id
                                    where 1=1 " + condition
                                    + @"group by b.id,b.createTime,customername,b.cablenumber, 
                                    b.cablestatus ,b.userid ,c.name,b.BoxNumber,
                                    tel,c.address,b.PackageInfo,b.money,
                                    b.CompleteTime,b.ControlUserId,b.address1,b.address2 
                                    order by b.createTime desc";
                    return db.GetDataSet(sql, parmar.ToArray()).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }

        public static Cable[] getByCondition()
        {
            Cable[] cs = CableDao.FindAll(new Order("Default2",true), new NotNullExpression("Default2"), new NotExpression(new EqExpression("Default2", "")), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return cs;
        }

        public static Cable getByNumberAndCustomerId(string cablenumber, int customerid)
        {
            Cable c = CableDao.FindFirst(new EqExpression("Customerid", customerid),
                                         new EqExpression("Cablenumber", cablenumber),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
                                         new EqExpression("Isdeleted",(int)EnmIsdeleted.使用中));
            return c;
        }

        public static bool getByCableNumber(string cablenumber, int customerid,int cableid)
        {
            if (string.IsNullOrEmpty(cablenumber))
            {
                return true;
            }
            using (DbHelper db = new DbHelper())
            {
                string sql = "Select count(1) from cable where cablenumber='" + cablenumber + "' and isdeleted=" + (int)EnmIsdeleted.使用中 + "";
                int count = Utils.NvInt(db.GetDataSet(sql).Tables[0].Rows[0][0].ToString());
                if (cableid > 0)  //更新
                {
                    if (count > 0) 
                    {
                        int flag = 0;
                        sql = "Select id,customerid,cablenumber from cable where cablenumber='" + cablenumber + "' and isdeleted=" + (int)EnmIsdeleted.使用中 + "";
                        DataTable dt = db.GetDataSet(sql).Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int tmpcustomerid = Utils.NvInt(dt.Rows[i]["customerid"].ToString());  //客户id
                            int tmpcableid = Utils.NvInt(dt.Rows[i]["id"].ToString());   //电路id
                            if (tmpcustomerid != customerid)
                            {
                                return false;
                            }
                            else if(tmpcustomerid==customerid && tmpcableid!=cableid)
                            {
                                return false;
                            }
                            
                        }
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else  //新增
                {
                    if (count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }                
            }
        }
        #endregion

        #region 变更录入人员
        public static void changeWrite(int writeid,int newwriteid)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "Update Cable SET ControlUserId=@newid where ControlUserId=@id";
                    DbParameter[] parmar = { db.CreateParameter("@newid", newwriteid), db.CreateParameter("@id", writeid) };
                    db.ExecuteNonQuery(sql,parmar);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999");
                }
            }
        }
        #endregion

        #region 根据电路代码查询完工记录
        public static Cable getCompleteCalbeByCableNumber(string Cablenumber)
        {
            try
            {
                return CableDao.FindFirst(new EqExpression("Cablenumber", Cablenumber), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                          new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)), new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region 根据电路代码获取电路数量
        public static int getCableCount(string number)
        {
            Cable[] c = CableDao.FindAll(new EqExpression("Cablenumber", number), new NotExpression(new EqExpression("Isdeleted", (int)EnmIsdeleted.已删除)));
            if (c != null)
            {
                return c.Length;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        public static void processMySql()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = @"Update cable set updatetime=(select max(createtime) from cablehistory where cable.id=cablehistory.cableid)
                                   Where updatetime is null and isdeleted=@del";
                    db.ExecuteNonQuery(sql, new DbParameter[] { db.CreateParameter("del", (int)EnmIsdeleted.使用中) });
                    MessageHelper.ShowMessage("I004");
                }
            }
            catch(Exception ex)
            { 
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
            }
        }
    }
}
