using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Library.Model;
using WY.Common;
using WY.Common.Utility;
using WY.Common.Data;
using System.Data.Common;
using WY.Common.Message;
using Library.Dao;
using NHibernate.Expression;

namespace WY.Library.Business
{
    /// <summary>
    /// 电信账单
    /// </summary>
    public class SaleBillBusiness
    {
        public static Hashtable losths;  //缺失电路代码信息        
        public static Hashtable errhs;  //Excel错误信息        
        public static List<Tmpsalebills> list;  //Excel临时账单信息
        public static List<int> delRowIndex;  //统计缺失代码及错误账单信息的行标
        public static Hashtable addhs;  //账单补结电路代码信息

        #region 判断当前月度账单是否已经导入
        public static int isHaveBills(int year, int month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select count(1) from salebills where year=@year and month=@month ";
                    DbParameter[] param = { db.CreateParameter("@year", year), db.CreateParameter("@month", month) };
                    return Utils.NvInt(db.GetDataSet(sql, param).Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "获取"+year.ToString()+"年"+month.ToString()+"月度账单发生错误。");
                return -1;
            }
        }
        #endregion

        #region 删除选择月度的所有账单
        public static bool delAllBillsInDate(int year,int month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update salebills set Isdeleted="+(int)EnmIsdeleted.已删除+" where year=@year and month=@month ";
                    DbParameter[] param = { db.CreateParameter("@year", year), db.CreateParameter("@month", month) };
                    db.ExecuteNonQuery(sql,param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "删除"+year.ToString()+"年"+month.ToString()+"月度账单发生错误。");
                return false;
            }
        }
        #endregion

        #region 读取账单所有信息
        public static void readAllExcelBills(DataTable bill)
        {
            string errorItem = "";
            for (int i = Global.row_Startline; i < bill.Rows.Count - Global.row_Startline; i++)
            {
                try
                {
                    errorItem = "应收账款";
                    decimal receivable = Utils.NvDecimal(bill.Rows[i][Global.col_Receivable].ToString());  //应收账款
                    errorItem = "销账金额";
                    decimal writeoff = Utils.NvDecimal(bill.Rows[i][Global.col_Writeoff].ToString());      //销账金额
                    string excalbenumber = Utils.NvStr(bill.Rows[i][Global.col_exCalbenumber].ToString());   //原电路代码
                    string contract = Utils.NvStr(bill.Rows[i][Global.col_contractnumber].ToString());   //合同编号
                    string speed = Utils.NvStr(bill.Rows[i][Global.col_Speed].ToString());   //现速率
                    string exspeed = Utils.NvStr(bill.Rows[i][Global.col_exSpeed].ToString());   //原速率
                    string nature = Utils.NvStr(bill.Rows[i][Global.col_cableNature].ToString());  //电路性质
                    string range = Utils.NvStr(bill.Rows[i][Global.col_Range].ToString());   //通信范围
                    string completedate = Utils.NvStr(bill.Rows[i][Global.col_Completedate].ToString());  //完工日期
                    string removedata = Utils.NvStr(bill.Rows[i][Global.col_Removedate].ToString());  //拆机日期
                    string settlementstart = Utils.NvStr(bill.Rows[i][Global.col_Settlementstart].ToString());  //结算起始日期
                    string settlementend = Utils.NvStr(bill.Rows[i][Global.col_Settlementend].ToString());  //结算截止日期
                }
                catch
                {
                    errhs.Add(i, errorItem+"错误。");
                    continue;
                }
            }
        }
        #endregion

        #region 检测账单信息
        /// <summary>
        /// 检测账单信息
        /// </summary>
        /// <param name="bill">导入的电信账单表</param>
        public static void checkBillInfo(DataTable bill,int year,int month)
        {
            list = new List<Tmpsalebills>();
            for (int i = Global.row_Startline; i < bill.Rows.Count; i++)
            {
                string cabelNumber = Utils.NvStr(bill.Rows[i][Global.col_Calbenumber].ToString());  //电路代码
                string cusName = Utils.NvStr(bill.Rows[i][Global.col_Cusname].ToString());     //客户名称
                cusName = cusName.Replace("（", "(");
                cusName = cusName.Replace("）", ")");
                if (string.IsNullOrEmpty(cabelNumber) && string.IsNullOrEmpty(cusName))  //如果电路代码及客户名称都为空，认为Excel账单读到最末行并停止读取
                {
                    break;
                }
                #region 读取账单信息
                string receivable = Utils.NvStr(bill.Rows[i][Global.col_Receivable].ToString());  //应收账款
                string writeoff = Utils.NvStr(bill.Rows[i][Global.col_Writeoff].ToString());      //销账金额
                string excalbenumber = Utils.NvStr(bill.Rows[i][Global.col_exCalbenumber].ToString());   //原电路代码
                string contract = Utils.NvStr(bill.Rows[i][Global.col_contractnumber].ToString());   //合同编号
                string speed = Utils.NvStr(bill.Rows[i][Global.col_Speed].ToString());   //现速率
                string exspeed = Utils.NvStr(bill.Rows[i][Global.col_exSpeed].ToString());   //原速率
                string nature = Utils.NvStr(bill.Rows[i][Global.col_cableNature].ToString());  //电路性质
                string range = Utils.NvStr(bill.Rows[i][Global.col_Range].ToString());   //通信范围
                string completedate = Utils.NvStr(bill.Rows[i][Global.col_Completedate].ToString());  //完工日期
                string removedata = Utils.NvStr(bill.Rows[i][Global.col_Removedate].ToString());  //拆机日期
                string settlementstart = Utils.NvStr(bill.Rows[i][Global.col_Settlementstart].ToString());  //结算起始日期
                string settlementend = Utils.NvStr(bill.Rows[i][Global.col_Settlementend].ToString());  //结算截止日期
                string ratio = Utils.NvStr(bill.Rows[i][Global.col_Ratio].ToString());  //结算比例?? 需要判定
                string proxy = Utils.NvStr(bill.Rows[i][Global.col_Proxy].ToString());  //代理费
                string writeoffStatus = Utils.NvStr(bill.Rows[i][Global.col_WriteoffStatus].ToString());  //销账情况               
                Tmpsalebills tmpbill = new Tmpsalebills();  //电信账单类
                tmpbill.Contract = contract;
                tmpbill.Cable = cabelNumber;
                tmpbill.Customername = cusName;
                tmpbill.Contract = contract;
                tmpbill.Oldcable = excalbenumber;
                tmpbill.Cablenature = nature;
                tmpbill.Speed = speed;
                tmpbill.Speedold = exspeed;
                tmpbill.Range = range;
                tmpbill.Completedate = completedate;
                tmpbill.Removedate = removedata;
                tmpbill.Settlementend = settlementend;  //结算起始日期
                tmpbill.Settlementstart = settlementstart;  //结算截止日期
                tmpbill.Receivable = receivable;
                tmpbill.Writeoff = writeoff;
                tmpbill.Ratio = ratio;
                tmpbill.Proxy = proxy;
                tmpbill.Writeoffstatus = writeoffStatus;
                #endregion

                #region 判断错误信息
                //DateTime dtStart;   //账单结算起始日期
                //DateTime dtEnd;     //账单结算截止日期     
                //if (string.IsNullOrEmpty(cabelNumber) || string.IsNullOrEmpty(cusName))  //如果电路代码及客户名称都为空，认为Excel账单读到最末行并停止读取
                //{
                //    errhs.Add(i, "客户名或电路代码不能为空。请重新核对。");
                //    tmpbill.Errinfo = "客户名或电路代码不能为空。请重新核对。";
                //    list.Add(tmpbill);
                //    continue;
                //}
                ////判断客户名及电路代码是否已经录入
                //int resultcount = getCount(cusName, cabelNumber);
                //if (resultcount > 0)
                //{
                //    if (resultcount == 1)
                //    {
                //        try
                //        {
                //            dtStart = DateTime.Parse(settlementstart);
                //        }
                //        catch
                //        {
                //            errhs.Add(i, "结算起始日期格式不符，请重新核对。");
                //            tmpbill.Errinfo = "结算起始日期格式不符，请重新核对。";
                //            list.Add(tmpbill);
                //            continue;
                //        }
                //        try
                //        {
                //            dtEnd = DateTime.Parse(settlementend);
                //        }
                //        catch
                //        {
                //            errhs.Add(i, "账单结算截止日期格式不符，请重新核对。");
                //            tmpbill.Errinfo = "账单结算截止日期格式不符，请重新核对。";
                //            list.Add(tmpbill);
                //            continue;
                //        }

                //        string tmp = ratio.Substring(0, ratio.Length - 1);  //去掉结算比例最后的%号
                //        decimal tmpratio = 0;
                //        try
                //        {
                //            tmpratio = decimal.Parse(tmp);
                //        }
                //        catch
                //        {
                //            errhs.Add(i, "账单结算比例格式不符，请重新确认");  //如果电路结算比例不符合要求就直接跳过循环下一条记录
                //            tmpbill.Errinfo = "账单结算比例格式不符，请重新确认";
                //            list.Add(tmpbill);
                //            continue;
                //        }

                //        int cusId = 0;  //客户ID
                //        int cableId = 0; //电路ID
                //        int salerid = 0;   //主销售ID
                //        DataTable tb = getID(cusName, cabelNumber);  //查询电路ID，客户ID，主销售员ID
                //        if (tb.Rows.Count > 0)
                //        {
                //            cusId = Utils.NvInt(tb.Rows[0]["id"].ToString());
                //            cableId = Utils.NvInt(tb.Rows[0]["id1"].ToString());
                //            salerid = Utils.NvInt(tb.Rows[0]["userid"].ToString());
                //            if (tb.Rows[0]["startdate"].ToString() == "")
                //            {
                //                errhs.Add(i, "电路未查到完工日期，请重新核对。");
                //                tmpbill.Errinfo = "电路未查到完工日期，请重新核对。";
                //                list.Add(tmpbill);
                //                continue;
                //            }
                //            DateTime contractStartData = DateTime.Parse(tb.Rows[0]["startdate"].ToString()).Date;  //完工电路代码合同起始日期
                //            if (contractStartData > dtStart)
                //            {
                //                errhs.Add(i, "账单结算日起始期早于电路合同起始日期，请重新核对。");
                //                tmpbill.Errinfo = "账单结算日起始期早于电路合同起始日期，请重新核对。";
                //                list.Add(tmpbill);
                //                continue;
                //            }
                //            //根据账单比例对比完工录入电路比率，如果超出则报警，但是依旧保存记录，下期账单会有负值抵扣
                //            Cable cb = CableBusiness.getCalbeByCableId(cableId);
                //            if (tmpratio > cb.Writeoffratio)
                //            {
                //                errhs.Add(i, "账单结算比例大于电路代码设定比率，请查看对应记录，该数据仍将会保存到数据库。");
                //            }
                //        }
                //        string result = isAddBillinfo(year, month, dtStart.Year, dtStart.Month, i);
                //        if (result == "补结")
                //        {
                //            //判断是否多次补结
                //            DateTime starttime = DateTime.Parse(tmpbill.Settlementstart).Date;
                //            DateTime endtime = DateTime.Parse(tmpbill.Settlementend).Date;
                //            //if (isHaveBillsInfo(cusId, cableId, starttime,endtime)) //根据结算起始日期判断电路代码是否有重复
                //            //{
                //            //    tmpbill.Flag = ((int)EnmRepeat.多次补结算).ToString();
                //            //}
                //            //else
                //            //{
                //            //    tmpbill.Flag = ((int)EnmRepeat.补结算).ToString();
                //            //}
                //            addhs.Add(i, tmpbill);  //补结电路信息
                //            continue;
                //        }
                //        else if (result == "错误")
                //        {
                //            continue;
                //        }
                //        getLostCable(losths, cusName, cabelNumber, i);
                //        //if (getLostCable(losths, cusName, cabelNumber, i))
                //        //{
                //        //    //list.Add(tmpbill);   //添加正常电信账单
                //        //}
                //    }
                //    else
                //    {
                //        errhs.Add(i, "根据电路代码及客户名称查询到多个完工信息，请重新核对。");
                //        tmpbill.Errinfo = "根据电路代码及客户名称查询到多个完工信息，请重新核对。";
                //        list.Add(tmpbill);
                //        //delRowIndex.Add(i);
                //        continue;
                //    }
                //}
                //else
                //{
                //    errhs.Add(i, "根据电路代码及客户名称没有查到完工信息，请重新核对。");
                //    tmpbill.Errinfo = "根据电路代码及客户名称没有查到完工信息，请重新核对。";
                //    list.Add(tmpbill);
                //    continue;
                //}
                #endregion
                list.Add(tmpbill);
            } 


            ////先获取在这次导入月度内有效的完工电路代码
            //if(CableBusiness.getCompleteCalbe(year,month)!=null)
            //{
            //    Cable[] cb = CableBusiness.getCompleteCalbe(year,month);                
            //    for (int i = 0; i < cb.Length; i++)
            //    {                   
            //        losths.Add(cb[i].Cablenumber+cb[i].Customer.Customername, cb[i]);  //预先添加已完工电路代码，用于后期统计缺失电路代码信息
            //    }
            //}
            ////循环账单信息对比信息
            //if (Global.row_Startline < 0)
            //{
            //    MessageHelper.ShowMessage("E038");
            //    return;
            //}
                   
        }
        #endregion

        #region 判断电路代码和客户名称是否已经录入
        public static int getCount(string cusname, string cablenumber)
        {
            try
            {
                int result = -1;
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select count(1) from customer as c inner join cable as b on c.id = b.CustomerId "
                               //+"left join customerhistory as ch on ch.customerid=c.id "
                               + "where c.CustomerName=@cusname and b.CableNumber=@cablenumber "
                               + "and c.Isdeleted=@del1 and b.Isdeleted=@del2";
                    DbParameter[] paramlist = { db.CreateParameter("@cusname", cusname), db.CreateParameter("@cablenumber", cablenumber),
                                               db.CreateParameter("@del1",(int)EnmIsdeleted.使用中),db.CreateParameter("@del2",(int)EnmIsdeleted.使用中) };

                    result = Utils.NvInt(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                    if(result>0)
                    {
                        return result;
                    }
                    else
                    {
                        sql = "select count(1) from customerhistory as c inner join cable as b on c.customerid = b.CustomerId "
                           //+ "left join customerhistory as ch on ch.customerid=c.id "
                           + "where c.CustomerName=@cusname and b.CableNumber=@cablenumber "
                           + "and c.Isdeleted=@del1 and b.Isdeleted=@del2";
                        DbParameter[] paramlist2 = { db.CreateParameter("@cusname", cusname), db.CreateParameter("@cablenumber", cablenumber),
                                               db.CreateParameter("@del1",(int)EnmIsdeleted.使用中),db.CreateParameter("@del2",(int)EnmIsdeleted.使用中) };

                        result = Utils.NvInt(db.GetDataSet(sql, paramlist2).Tables[0].Rows[0][0].ToString());
                        return result;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return -1;
            }
        }
        #endregion

        #region 根据结算起始日期判断电路代码是否有重复
        public static bool isHaveBillsInfo(int cusid, int cableid, DateTime start, DateTime end)
        {
            try
            {
                //string date = year.ToString() + "/" + month.ToString() + "/";
                Salebills[] bills = SalebillsDao.FindAll(new EqExpression("Settlementstart", start), new EqExpression("Settlementend",end), 
                                                         new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), new EqExpression("Cableid", cableid));
                if(bills.Length>0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取客户及电路ID
        public static DataTable getIDs(string cusname, string cablenumber)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Select ch.customerid as 客户id,b.cableid as 电路id,cb.userid as 主销售渠道 "
                               + "FROM customerhistory AS ch INNER JOIN cablehistory AS b ON ch.customerid = b.customerid  "
                               +" LEFT JOIN customer AS c ON c.id=ch.customerid "
                               +" LEFT JOIN cable AS cb ON cb.id=b.cableid "
                               + "where ch.CustomerName=@cusname and b.CableNumber=@cablenumber and ch.Isdeleted=@del1 and b.Isdeleted=@del2 "
                               + "AND c.Isdeleted=0 AND cb.isdeleted=0 "
                               + "and b.cablestatus<>@cablestatus and b.cablestatus<>@cablestatus2 ";
                    DbParameter[] paramlist = { db.CreateParameter("@cusname", cusname), db.CreateParameter("@cablenumber", cablenumber),
                                                db.CreateParameter("@del1",(int)EnmIsdeleted.使用中),db.CreateParameter("@del2",(int)EnmIsdeleted.使用中), 
                                                db.CreateParameter("@cablestatus",(int)EnmCableStatus.取消),db.CreateParameter("@cablestatus2",(int)EnmCableStatus.未完工)};

                    return db.GetDataSet(sql, paramlist).Tables[0];
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

        #region 查找缺失电路代码查找
        private static bool getLostCable(Hashtable hs, string custName, string cablenumber,int rowindex)
        {
            if (hs.ContainsKey(cablenumber + custName))
            {
                hs.Remove(cablenumber + custName);
                return true;
            }
            else
            {
                delRowIndex.Add(rowindex);
                return false;
            }
        }
        #endregion

        #region 判断是否为补结账单信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">导入年度</param>
        /// <param name="month">导入月度</param>
        /// <param name="tmpyear">账单结算年度</param>
        /// <param name="tmpmonth">账单结算月度</param>
        /// <param name="rowindex">行标</param>
        /// <returns></returns>
        private static string isAddBillinfo(int year, int month, int tmpyear, int tmpmonth,int rowindex)
        {
            if (year > tmpyear)
            {
                return "补结";
            }
            else if (year == tmpyear)
            {
                if (month > tmpmonth)
                {
                    return "补结";
                }
                else if (month == tmpmonth)
                {
                    return "正常";
                }
                else
                {
                    errhs.Add(rowindex, "电路结算账单结算时间大于导入设定时间，请重新核对");
                    return "错误";
                }
            }
            else
            {
                errhs.Add(rowindex, "电路结算账单结算时间大于导入设定时间，请重新核对");
                return "错误";
            }
        }
        #endregion

        #region 保存销售账单信息
        public static void saveBill(Salebills bill)
        {
            try
            {
                bill.Save();               
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "账单信息保存失败。");
            }
        }
        #endregion

        #region 保存补结账单信息
        public static void saveAddBill(Salebills bill)
        {
            try
            {
                //根据结算起始日期判断电路代码是否有重复
                DateTime time = DateTime.Parse(bill.Settlementstart).Date;
                bill.Save();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
            }
        }
        #endregion       

        #region 查询账单信息
        public static Salebills getBillByCondition(int cusid,int cableid,int month,int year)
        {
            try
            {
                Salebills bills = SalebillsDao.FindFirst(new EqExpression("Cableid", cableid), new EqExpression("Customerid", cusid), new EqExpression("Year", year),
                                    new EqExpression("Month", month), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return bills;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }

        public static Salebills[] getBillsByCondition(int year,int month,string cable,string customername)
        {            
            try
            {
                Cable[] cb;
                Salebills[] bills;
                Customer cus;
                
                if (!string.IsNullOrEmpty(cable))  //根据电路代码模糊查询
                {
                    cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)),
                                               new LikeExpression("Cablenumber", "%" + cable + "%"));
                }
                else //查找全部电路代码
                {
                    cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.未完工)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.已拆机)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.取消)));                                              
                }
                if (cb != null || cb.Length > 0)
                {
                    object[] cableids = new object[cb.Length];
                    for (int i = 0; i < cb.Length; i++)
                    {
                        cableids[i]=cb[i].Id;
                    }
                    InExpression inexpress = new InExpression("Cableid", cableids);  //将所有电路代码作为查询条件
                    
                    if (!string.IsNullOrEmpty(customername))
                    {
                        cus = CustomerBusiness.findCustomerByName(customername);
                        if (cus != null)
                        {
                            EqExpression cuseqex = new EqExpression("Customerid", cus.Id);
                            //只查正常结算和预结算的账单信息
                            bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                                         new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.正常结算), new EqExpression("Flag", (int)EnmWriteOffFlag.预结算)),
                                                         new EqExpression("Year", year),
                                                         new EqExpression("Month", month),
                                                         inexpress, cuseqex);
                        }
                        else
                        {
                            //只查正常结算和预结算的账单信息
                            bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                                         new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.正常结算), new EqExpression("Flag", (int)EnmWriteOffFlag.预结算)),
                                                         new EqExpression("Year", year),
                                                         new EqExpression("Month", month),
                                                         inexpress);
                        }
                    }
                    else
                    {
                        //只查正常结算和预结算的Excel账单信息
                        bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中),
                                                     new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.正常结算), new EqExpression("Flag", (int)EnmWriteOffFlag.预结算)),
                                                     new EqExpression("Year", year),
                                                     new EqExpression("Month", month),
                                                     inexpress);
                    }
                    return bills;
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

        #region 根据企业id查询是否有导入的账单
        public static int getCountByCustomerId(int customerid)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select count(1) from salebills where customerid=@cusid and isdeleted=@del";
                    DbParameter[] paramlist = { db.CreateParameter("@cusid", customerid), db.CreateParameter("@del", (int)EnmIsdeleted.使用中) };
                    return int.Parse(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                                               
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region 查询公司企业销售业绩
        public static DataTable getCompanyTrendcy(int startYear, int endYear, int startMonth, int endMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select  sum(writeoff) as amount,CONCAT(year,'年',month,'月') as yearmonth" +
                                            " from SaleBills " +
                                            " where (year>@syear or (year=@syear and month>=@smonth)) and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                            " and isDeleted=@del " +
                                            " group by CONCAT(year,'年',month,'月')" +
                                            " order by year,month ";
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.使用中),
                                               db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth) };

                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "销售走势图导出失败。");
                    return new DataTable();
                }

            }

        }
        #endregion     



        #region 智能网佣金业务模块
        public static DataTable netmoney(int salserid, int startYear, int startMonth, int endYear, int endMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "Select Id, year, month, dailishanghao, dailishangmingcheng, zhangqi, chanpingmingcheng, shebeihao, "+
	                             "kehumingcheng, fanyihaoma, anzhuangshijian,xiaozhangriqi,	xiaozhangjine,choujinbili,choujinzonge,bendixiaozhangjine,"+ 
	                             "bendichoujinbilv,bendichoujinjine,guoneixiaozhangjine,guoneichoujinbilv,guoneichoujinjine,qitaxiaozhangjine, "+
	                             "qitachoujiniblv,qitachoujinjine,hetonghao,choujinmingcheng,choujinleixing,kehuId,shebeiId,salerId, "+
	                             "Isdeleted,CreateTime,CreateUser, UpdateTime,UpdateUser from dt_netmoney " +
                                 " where (year>@syear or (year=@syear and month>=@smonth)) and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                 " and isDeleted=@del and salerId = @salerId" +
                                 " order by year,month ";
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.使用中),
                                               db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth),db.CreateParameter("@salerId",salserid) };

                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999");
                    return new DataTable();
                }

            }
        }

        public static DataTable netnomoney(int salserid, int startYear, int startMonth, int endYear, int endMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "Select Id, year, month, dailishanghao, dailishangmingcheng, zhangqi, chanpingmingcheng, "+
	                             "shebeihao,kehumingcheng,chuzhangjine,yuandailishanghao,kehuId,shebeiId,salerId From dt_netnomoney " +                                
                                 " where (year>@syear or (year=@syear and month>=@smonth)) and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                 " and isDeleted=@del and salerId=@salerid" +
                                 " order by year,month ";
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.使用中),
                                               db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth),db.CreateParameter("@salerid",salserid) };

                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999");
                    return new DataTable();
                }

            }
        }
        #endregion
    }    
}
