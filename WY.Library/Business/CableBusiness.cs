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
        #region �����·����
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

        #region ���µ�·����
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

        #region ���ݿͻ�ID��ѯ��·����
        public static Cable[] getCablesByCustomerId(int cusId)
        {
            try
            {
                if (Global.g_usergroupid == (int)EnmUserRole.�����ܼ�)
                {
                    Cable[] c = CableDao.FindAll(new Order("Createtime",false), new EqExpression("Customerid", cusId), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
                    return c;
                }
                else if (Global.g_usergroupid == (int)EnmUserRole.¼����Ա)
                {
                    Cable[] c = CableDao.FindAll(new Order("Createtime", false), new EqExpression("Customerid", cusId), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), new EqExpression("Controluserid", Global.g_userid));
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

        #region ���ݵ�·����ID��ѯ
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

        #region ���ݵ�·�����ѯ
        public static Cable getCalbeByCableNumber(string Cablenumber)
        {
            try
            {
                return CableDao.FindFirst(new EqExpression("Cablenumber", Cablenumber), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region ��ȡȫ�����깤�ĵ�·����
        public static DataTable getCompleteCalbe(int year, int month)
        {
            string str = year.ToString() + "-"+month.ToString() + "-1";
            DateTime date = DateTime.Parse(str).Date; //���µ�һ��
            string startDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = date.AddMonths(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");   //�������һ��
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
                               + "and (h.Cablestatus<>" + (int)EnmCableStatus.δ�깤 + " and h.Cablestatus<>" + (int)EnmCableStatus.ȡ�� + " and h.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + ") ";
                    DataTable dt = db.GetDataSet(sql).Tables[0];
                    return dt;
                }

                //AndExpression and1 = new AndExpression(new AndExpression(new GeExpression("Startdate", startDate), new LeExpression("Startdate", endDate)), new GeExpression("Enddate", endDate));

                //AndExpression and2 = new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", endDate));

                //AndExpression and3 = new AndExpression(new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", startDate)), new LeExpression("Enddate", endDate));

                //AndExpression and4 = new AndExpression(new GeExpression("Enddate", startDate), new LeExpression("Enddate", endDate));

                
                //OrExpression orExpress = new OrExpression(new OrExpression(new OrExpression(and1, and2), and3), and4);
                //Cable[] cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                //                              new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
                //                              //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
                //                              new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)),
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

        #region ��ȡȫ�����깤�ĵ�·����
        public static DataTable getCompleteCalbe(int year, int month, int cableid, int customerid)
        {
            string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            DateTime startDate = DateTime.Parse(strStartDate).Date; //���µ�һ��
            int days = DateTime.DaysInMonth(year, month);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);   //�������һ��
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
                               + "and h.Cablestatus<>" + (int)EnmCableStatus.δ�깤 + " and h.Cablestatus<>" + (int)EnmCableStatus.ȡ�� + " and h.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " "
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
        /// ��ѯ�깤��·����
        /// </summary>
        /// <param name="year">���</param>
        /// <param name="month">�¶�</param>
        /// <param name="cable">��·����</param>
        /// <returns></returns>
        public static DataTable getCompleteCalbe(int year, int month, string cable,int Id)
        {
            DataTable result = null;
            string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            DateTime startDate = DateTime.Parse(strStartDate).Date; //���µ�һ��
            int days = DateTime.DaysInMonth(year, month);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);   //�������һ��
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
                              + "and h.Cablestatus<>" + (int)EnmCableStatus.δ�깤 + " and h.Cablestatus<>" + (int)EnmCableStatus.ȡ�� + " and h.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " "
                              + "and c.cablenumber like '%" + cable + "%' and c.Isdeleted="+(int)EnmIsdeleted.ʹ����+" order by h.createtime desc ";
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
                              + "and h.Cablestatus<>" + (int)EnmCableStatus.δ�깤 + " and h.Cablestatus<>" + (int)EnmCableStatus.ȡ�� + " and h.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " "
                              + "and c.cablenumber like '%" + cable + "%' and c.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " order by h.createtime desc ";
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
            //DateTime startDate = DateTime.Parse(strStartDate).Date; //���µ�һ��
            //int days = DateTime.DaysInMonth(year, month); 
            //DateTime endDate = startDate.AddMonths(1).AddDays(-1).Date;   //�������һ��
            //try
            //{
            //    Cable[] cb;
            //    AndExpression and1 = new AndExpression(new AndExpression(new GeExpression("Startdate", startDate), new LeExpression("Startdate", endDate)), new GeExpression("Enddate", endDate));

            //    AndExpression and2 = new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", endDate));

            //    AndExpression and3 = new AndExpression(new AndExpression(new LtExpression("Startdate", startDate), new GeExpression("Enddate", startDate)), new LeExpression("Enddate", endDate));

            //    AndExpression and4 = new AndExpression(new GeExpression("Enddate", startDate), new LeExpression("Enddate", endDate));

            //    OrExpression orExpress = new OrExpression(new OrExpression(new OrExpression(and1, and2), and3), and4);
            //    if (!string.IsNullOrEmpty(cable))  //�жϵ�·�������Ƿ�������
            //    {
            //        cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
            //                                      new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
            //                                      //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
            //                                      new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)),
            //                                      new LikeExpression("Cablenumber", "%" + cable + "%"),
            //                                      orExpress);
            //    }
            //    else
            //    {
            //        cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
            //                                     new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
            //                                     //new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
            //                                     new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)),
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
        
        #region ɾ����·���룬ͬʱɾ����Ӧ��ɱ���
        public static void delCableAllinfo(int cableId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "Update cable set Isdeleted=@del where id =@id";
                    DbParameter[] parmar = { db.CreateParameter("@del", (int)EnmIsdeleted.��ɾ��), db.CreateParameter("@id", cableId) };
                    db.ExecuteNonQuery(sql, parmar);  //��·����
                    DateTime endtime = TableManager.DBServerTime().Date;
                    sql = "Update commission set endTime=@endTime,Isdeleted=@del where cableid=@id";

                    DbParameter[] parmar2 = {db.CreateParameter("@del",(int)EnmIsdeleted.��ɾ��), db.CreateParameter("@id", cableId),
                                             db.CreateParameter("@endTime",endtime)};
                    db.ExecuteNonQuery(sql, parmar2);  //��ɱ���
                    db.TrnCommit();
                    MessageHelper.ShowMessage("I002");
                }
                catch (Exception ex)
                {
                    db.TrnRollBack();
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "��·����ɾ��ʧ�ܡ�");

                }
            }
        }
        #endregion

        #region ���ݿͻ�ID����·�����ѯ���ݲ�ѯ��·ID
        public static int getCountbyCusIdCable(int cusid, string cable)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select c.id from customer as u left join cable as c on c.customerid = u.id where c.isdeleted=@del and u.isdeleted=@del "
                               + " and u.id=@cusid and c.CableNumber=@cable";

                    DbParameter[] parmar = { db.CreateParameter("@cusid", cusid), db.CreateParameter("@del", (int)EnmIsdeleted.��ɾ��), db.CreateParameter("@cable", cable) };
                    return Utils.NvInt(db.GetDataSet(sql, parmar).Tables[0].Rows[0][0].ToString());
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }
        #endregion

        #region ���ݿͻ����ƺ͵�·�����ѯ��·ID
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

        #region �жϵ�·��ͬʱ��Ͳ�ѯʱ��Ĺ�ϵ
        /// <summary>
        /// �жϵ�·��ͬʱ��Ͳ�ѯʱ��Ĺ�ϵ
        /// </summary>
        /// <param name="contractStartDate">��ͬ��ʼ����</param>
        /// <param name="contractEndDate">��ͬ��ֹ����</param>
        /// <param name="searchDate">��ѯ��ʼ���� ÿ��1��</param>
        /// <returns></returns>
        public static Dateinterval anaylisContractMonth(Nullable<DateTime> contractStartDate, Nullable<DateTime> contractEndDate, DateTime searchDate)
        {
            Dateinterval interval = new Dateinterval();  

            //Ĭ��Ϊ����
            interval.MonthType = (int)EnmMonth.ȫ��;
            interval.StartDate = searchDate;
            interval.EndDate = searchDate.AddMonths(1).AddDays(-1);
            if (contractStartDate.Value.Year == searchDate.Year && contractStartDate.Value.Month == searchDate.Month)
            {
                //��ʼ��
                interval.MonthType = (int)EnmMonth.��ʼ��;
                interval.StartDate = contractStartDate.Value;
            }
            else if (contractEndDate.Value.Year == searchDate.Year && contractEndDate.Value.Month == searchDate.Month)
            {
                //��ֹ��
                interval.MonthType = (int)EnmMonth.��ֹ��;
                interval.EndDate = contractEndDate.Value;
            }
            return interval;
        }
        #endregion

        #region ����������ѯ��·����
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
                    string sql = @"select case when b.cableclass=0 then 'ר����' else '������' end cableclass ,b.Contracttype,
                                    b.id,b.createTime as ����,customername as �ͻ�����,b.cablenumber as EIP���, 
                                    b.cablestatus as ״̬,b.userid as ����,c.name as ��ϵ��,b.BoxNumber as ���ӱ��,
                                    tel as ��ϵ�绰,c.address as �ͻ���ַ,b.PackageInfo as �ײ�����,b.money as �����,
                                    b.CompleteTime as �깤����,b.ControlUserId as ¼����Ա,b.address1 as �׶˵�ַ,b.address2 as  �Ҷ˵�ַ, 
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
            Cable[] cs = CableDao.FindAll(new Order("Default2",true), new NotNullExpression("Default2"), new NotExpression(new EqExpression("Default2", "")), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return cs;
        }

        public static Cable getByNumberAndCustomerId(string cablenumber, int customerid)
        {
            Cable c = CableDao.FindFirst(new EqExpression("Customerid", customerid),
                                         new EqExpression("Cablenumber", cablenumber),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)),
                                         new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
                                         new EqExpression("Isdeleted",(int)EnmIsdeleted.ʹ����));
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
                string sql = "Select count(1) from cable where cablenumber='" + cablenumber + "' and isdeleted=" + (int)EnmIsdeleted.ʹ���� + "";
                int count = Utils.NvInt(db.GetDataSet(sql).Tables[0].Rows[0][0].ToString());
                if (cableid > 0)  //����
                {
                    if (count > 0) 
                    {
                        int flag = 0;
                        sql = "Select id,customerid,cablenumber from cable where cablenumber='" + cablenumber + "' and isdeleted=" + (int)EnmIsdeleted.ʹ���� + "";
                        DataTable dt = db.GetDataSet(sql).Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int tmpcustomerid = Utils.NvInt(dt.Rows[i]["customerid"].ToString());  //�ͻ�id
                            int tmpcableid = Utils.NvInt(dt.Rows[i]["id"].ToString());   //��·id
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
                else  //����
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

        #region ���¼����Ա
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

        #region ���ݵ�·�����ѯ�깤��¼
        public static Cable getCompleteCalbeByCableNumber(string Cablenumber)
        {
            try
            {
                return CableDao.FindFirst(new EqExpression("Cablenumber", Cablenumber), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                          new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)), new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        #region ���ݵ�·�����ȡ��·����
        public static int getCableCount(string number)
        {
            Cable[] c = CableDao.FindAll(new EqExpression("Cablenumber", number), new NotExpression(new EqExpression("Isdeleted", (int)EnmIsdeleted.��ɾ��)));
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
                    db.ExecuteNonQuery(sql, new DbParameter[] { db.CreateParameter("del", (int)EnmIsdeleted.ʹ����) });
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
