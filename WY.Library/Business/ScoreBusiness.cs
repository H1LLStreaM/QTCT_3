using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using System.Data;
using System.Collections;
using WY.Common.Utility;
using WY.Common.Data;
using System.Data.Common;
using WY.Common.Message;
using WY.Common.Framework;
using Library.Dao;
using NHibernate.Expression;
using WY.Library.Model;

namespace WY.Library.Business
{
    public class ScoreBusiness
    {
        #region 计算提成金额
        #region 预计
        /// <summary>
        /// 计算预计提成金额
        /// </summary>
        /// <param name="startDate">查询起始日期</param>
        /// <param name="endDate">查询截止日期</param>
        /// <param name="cableNumber">电路代码</param>
        /// <param name="salerId">渠道工号</param>
        /// <param name="tbScore">数据表模板</param>
        public static DataTable makeExpectedScore(DateTime startDate, DateTime endDate, string cableNumber, int salerId)
        {
            DataTable tbScore = createCol();
            try
            {
                for (DateTime date = startDate; date <= endDate; date = date.AddMonths(1))
                {
                    //Cable[] cables = CableBusiness.getCompleteCalbe(date.Year, date.Month, cableNumber);  //查询电路代码信息
                    DataTable cables = CableBusiness.getCompleteCalbe(date.Year, date.Month, cableNumber,salerId);  //查询电路代码信息
                    if (cables != null && cables.Rows.Count > 0)  //判断是否为空或没值
                    {
                        for (int i = 0; i < cables.Rows.Count; i++)  //循环电路合同
                        {
                            Cable tmpCalbe =CableBusiness.getCalbeByCableId(Utils.NvInt(cables.Rows[i]["id"]));
                            if (tmpCalbe == null)
                            {
                                continue;
                            }
                            string cusName = CustomerBusiness.findCustomerById(tmpCalbe.Customerid).Customername;//cables[i].Customer.Customername; //客户名称
                            string cablenumber = tmpCalbe.Cablenumber;    //电路代码
                            string calbeType = tmpCalbe.Contracttype;    //电路类型
                            string paytype = GlobalBusiness.getPayType(tmpCalbe.Paytype);  //合同付款方式
                            if (tmpCalbe.Cablestatus == (int)EnmCableStatus.取消)
                            {
                                continue;
                            }
                            Nullable<DateTime> contractStartTime = tmpCalbe.Startdate;  //合同起始日期
                            Nullable<DateTime> contractEndTime = tmpCalbe.Enddate;      //合同截止日期
                            if (tmpCalbe.Removetime != null)
                            {
                                contractEndTime = tmpCalbe.Removetime;
                            }
                            decimal contractMoney = ScoreBusiness.isHaveWriteoff(DateTime.Parse(contractStartTime.ToString()), DateTime.Parse(contractEndTime.ToString()), tmpCalbe.Paytype, tmpCalbe.Money, date.Year, date.Month);   //cables[i].Money;      //合同金额
                            if (contractMoney == 0)
                            {
                                continue;
                            }
                            if (contractStartTime == null || contractEndTime == null)    //如果合同没有起始或结束时间直接跳过进入下一个循环
                            {
                                continue;
                            }
                            Dateinterval interval = CableBusiness.anaylisContractMonth(contractStartTime, contractEndTime, date);  //获取时间区间
                            decimal money = tmpCalbe.Money;  //电路合同金额
                            int months = GlobalBusiness.getPayTypeForMonth(tmpCalbe.Paytype); //根据合同类型计算使用月数
                            decimal avgmoney = money / months;  //平均每月费用
                            int cableId = tmpCalbe.Id;     //电路代码ID
                            Commission[] cs = CommissionBusiness.getCommissions(cableId, salerId);  //渠道提成信息
                            if (cs != null && cs.Length > 0)  //根据电路代码和渠道查询提成
                            {
                                for (int j = 0; j < cs.Length; j++)
                                {
                                    int year = date.Year;     //年度
                                    int month = date.Month;   //月度
                                    string saler = UserBusiness.findUserById(cs[j].Userid).USER_NAME; //渠道工号
                                    int days = ScoreBusiness.calCommissionDate(interval, cs[j].Begintime,cs[j].Endtime);//根据时间区间和提成日期计算有效天数
                                    if (days <= 0)
                                    {
                                        continue;
                                    }
                                    int daysInMonth = DateTime.DaysInMonth(year, month); //计算当月天数 
                                    decimal ratio = cs[j].Ratio / 100;
                                    decimal tax = cs[j].Tax / 100;
                                    decimal score = 0;
                                    if (tmpCalbe.Paytype == (int)EnmPayType.月付)
                                    {
                                        score = ScoreBusiness.calCommissionMoney(daysInMonth, days, avgmoney, ratio, tax); //业绩计算
                                        writeScoreTable(year, month, cusName, cablenumber, saler, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, 0, 0, ratio, tax, daysInMonth);
                                    }
                                    else
                                    {
                                        score = ScoreBusiness.calCommissionMoney(avgmoney, ratio, tax); //业绩计算
                                        writeScoreTable(interval.StartDate.Year, interval.StartDate.Month, cusName, cablenumber, saler, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, 0, 0, ratio, tax, daysInMonth);
                                    }                                    
                                    //writeScoreTable(year, month, cusName, cablenumber, saler, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, 0, 0,ratio,tax);
                                 }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //MessageHelper.ShowMessage("I005");
                        continue;
                    }
                }
                return tbScore;
            }
            catch(Exception ex)
            {
                Log.Warning(ex.Message);
                MessageHelper.ShowMessage("E999", "预计提成金额计算发生错误。");
                return tbScore;
            }            
        }

        #endregion

        #region 实际
        /// <summary>
        /// 计算实际提成金额
        /// </summary>
        /// <param name="startDate">查询起始日期</param>
        /// <param name="endDate">查询截止日期</param>
        /// <param name="cableNumber">电路代码</param>
        /// <param name="userId">渠道工号</param>
        /// <param name="tbScore">数据表模板</param>
        public static DataTable makeActualScore(DateTime startDate, DateTime endDate, string cableNumber, int userId,string customerName)
        {
            DataTable tbScore = createCol();
            try
            {
                for (DateTime date = startDate; date <= endDate; date = date.AddMonths(1))
                {
                    object value = CommissionBusiness.isExistCommissionValueByMontAndSaler(userId, date, date.AddMonths(1).AddSeconds(-1), date.Year, date.Month);
                    if (value != null)
                    {
                        DataTable dt = (DataTable)value;
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                decimal score = 0;
                                int flag= Utils.NvInt(dt.Rows[i]["flag"].ToString());
                                Cable c = CableBusiness.getCalbeByCableId(int.Parse(dt.Rows[i]["电路id"].ToString()));
                                string cusName = CustomerBusiness.findCustomerById(int.Parse(dt.Rows[i]["客户id"].ToString())).Customername; //客户名称
                                string cablenumber = c.Cablenumber;    //电路代码
                                string calbeType = c.Contracttype;    //电路类型
                                string paytype = GlobalBusiness.getPayType(c.Paytype);  //合同付款方式
                                decimal proxy = decimal.Parse(dt.Rows[i]["proxy"].ToString());  //代理费
                                decimal contractMoney = c.Money;      //合同金额
                                decimal receivable = decimal.Parse(dt.Rows[i]["Receivable"].ToString());  //代理费
                                //计算时间间隔
                                Nullable<DateTime> contractStartTime = DateTime.Parse(dt.Rows[i]["账单开始时间"].ToString());  //账单结算起始日期
                                Nullable<DateTime> contractEndTime = DateTime.Parse(dt.Rows[i]["账单结束时间"].ToString());    //账单结算截止日期
                                if (contractStartTime == null || contractEndTime == null)    //如果合同没有起始或结束时间
                                {
                                    continue;
                                }
                                Dateinterval interval = new Dateinterval();
                                //计算时间间隔
                                interval.StartDate = DateTime.Parse(contractStartTime.ToString());
                                interval.EndDate = DateTime.Parse(contractEndTime.ToString());                                
                                Nullable<DateTime> begindate = DateTime.Parse(dt.Rows[i]["提成开始时间"].ToString());
                                Nullable<DateTime> enddate = null;
                                if (dt.Rows[i]["提成结束时间"] != null && !string.IsNullOrEmpty(dt.Rows[i]["提成结束时间"].ToString()))
                                {
                                    enddate = DateTime.Parse(dt.Rows[i]["提成结束时间"].ToString());
                                }
                                int days = ScoreBusiness.calCommissionDate(interval, begindate, enddate);//根据时间区间和提成日期计算有效天数
                                if (days <= 0)
                                {
                                    continue;
                                }
                                int daysInBill = new TimeSpan(DateTime.Parse(dt.Rows[i]["账单开始时间"].ToString()).Ticks).Subtract(new TimeSpan(DateTime.Parse(dt.Rows[i]["账单结束时间"].ToString()).Ticks)).Duration().Days + 1;  //计算间隔天数//计算账单结算区间天数
                                decimal ratio = decimal.Parse(dt.Rows[i]["提成比例"].ToString()) / 100;
                                decimal tax = decimal.Parse(dt.Rows[i]["税率"].ToString()) / 100;
                                decimal money = decimal.Parse(dt.Rows[i]["提成金额"].ToString());   //账单金额
                                int year = date.Year;    //年度
                                int month = date.Month;   //月度
                                //TB_User saler = UserBusiness
                                if (flag == (int)EnmWriteOffFlag.正常结算)
                                {
                                    score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //业绩计算
                                    writeScoreTable(year, month, cusName, cablenumber, UserBusiness.findUserById(userId).USER_NAME, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, proxy, receivable, ratio, tax, daysInBill);
                                }
                                else if (flag == (int)EnmWriteOffFlag.补结算)
                                {
                                    score = SuppmentWriteoffScore(interval.StartDate, interval.EndDate, c, money, userId, date.Year, date.Month);
                                    writeScoreTable(interval.StartDate.Year, interval.StartDate.Month, cusName, cablenumber, UserBusiness.findUserById(userId).USER_NAME, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, proxy, receivable, ratio, tax, daysInBill);
                                }                                
                                //writeScoreTable(year, month, cusName, cablenumber, UserBusiness.findUserById(userId).LogName, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, proxy, receivable,ratio,tax);
                            }
                        }
                    }
                }
                return tbScore;
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                MessageHelper.ShowMessage("E999", "预计提成金额计算发生错误。");
                return tbScore;
            }      
        }
        #endregion

        #region 根据合同类型计算补结提成金额
        public static decimal SuppmentWriteoffScore(DateTime startDate, DateTime endDate, Cable cable, decimal money, int userId, int year, int month)
        {
            try
            {
                string strPayType; //支付方式
                List<Dateinterval> list = new List<Dateinterval>();
                Dateinterval d;
                switch (cable.Paytype)
                {
                    case (int)EnmPayType.月付:
                        strPayType = "月付";
                        d = new Dateinterval();
                        d.StartDate = startDate;
                        d.EndDate = endDate;
                        d.Money = money;
                        list.Add(d);
                        break;
                    case (int)EnmPayType.季付:
                        strPayType = "季付";
                        for (DateTime tmpDate = startDate; tmpDate < endDate; tmpDate = tmpDate.AddMonths(3))
                        {
                            int tmp_year = tmpDate.Year;
                            int tmp_month = tmpDate.Month;
                            d = new Dateinterval();
                            if (tmpDate == startDate)
                            {
                                d.StartDate = tmpDate;
                                d.EndDate = DateTime.Parse(tmp_year.ToString() + "-" + tmp_month.ToString() + "-01").Date.AddMonths(1).AddDays(-1);
                            }
                            else
                            {
                                d.StartDate = DateTime.Parse(tmp_year.ToString() + "-" + tmp_month.ToString() + "-01").Date;
                                d.EndDate = startDate.AddMonths(1).AddDays(-1).Date;
                                if (d.EndDate > endDate)
                                {
                                    d.EndDate = endDate;
                                }
                            }
                            d.Money = money / 4;
                            list.Add(d);
                        }
                        break;
                    case (int)EnmPayType.半年付:
                        strPayType = "半年付";
                        for (DateTime tmpDate = startDate; tmpDate < endDate; tmpDate = tmpDate.AddMonths(6))
                        {
                            int tmp_year = tmpDate.Year;
                            int tmp_month = tmpDate.Month;
                            d = new Dateinterval();
                            if (tmpDate == startDate)
                            {
                                d.StartDate = tmpDate;
                                d.EndDate = DateTime.Parse(tmp_year.ToString() + "-" + tmp_month.ToString() + "-01").Date.AddMonths(1).AddDays(-1);
                            }
                            else
                            {
                                d.StartDate = DateTime.Parse(tmp_year.ToString() + "-" + tmp_month.ToString() + "-01").Date;
                                d.EndDate = startDate.AddMonths(1).AddDays(-1).Date;
                                if (d.EndDate > endDate)
                                {
                                    d.EndDate = endDate;
                                }
                            }
                            d.Money = money / 2;
                            list.Add(d);
                        }
                        break;
                    case (int)EnmPayType.一次性付:
                        strPayType = "一次性付";
                        d = new Dateinterval();
                        d.StartDate = startDate;
                        d.EndDate = DateTime.Parse(startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-01").Date.AddMonths(1).AddDays(-1);
                        d.Money = money;
                        list.Add(d);
                        break;
                    default:
                        return 0;
                }
                //根据区间历史计算补结金额
                for (int i = 0; i < list.Count; i++)
                {
                    object value = CommissionBusiness.isExistCommissionValueByMontAndSaler(userId, list[i].StartDate, list[i].EndDate, year, month);
                    if (value != null)
                    {
                        DataTable dt = (DataTable)value;
                        DataRow[] rows = dt.Select("电路id=" + cable.Id + " and flag=" + (int)EnmWriteOffFlag.补结算 + "");
                        for (int j = 0; j < rows.Length; j++)
                        {
                            Nullable<DateTime> begindate = DateTime.Parse(rows[j]["提成开始时间"].ToString());
                            Nullable<DateTime> enddate = null;
                            if (dt.Rows[j]["提成结束时间"] != null && !string.IsNullOrEmpty(rows[j]["提成结束时间"].ToString()))
                            {
                                enddate = DateTime.Parse(rows[j]["提成结束时间"].ToString());
                            }
                            int days = ScoreBusiness.calCommissionDate(list[i], begindate, enddate);//根据时间区间和提成日期计算有效天数
                            if (days <= 0)
                            {
                                continue;
                            }
                            int daysInBill = new TimeSpan(list[i].StartDate.Ticks).Subtract(new TimeSpan(list[i].EndDate.Ticks)).Duration().Days + 1;  //计算间隔天数//计算账单结算区间天数
                            decimal ratio = decimal.Parse(rows[i]["提成比例"].ToString()) / 100;
                            decimal tax = decimal.Parse(rows[i]["税率"].ToString()) / 100;
                            //decimal money = decimal.Parse(dt.Rows[i]["提成金额"].ToString());
                            decimal score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //业绩计算
                            //int year = date.Year;    //年度
                            //int month = date.Month;   //月度
                            //TB_User saler = UserBusiness
                            //writeScoreTable(year, month, cusName, cablenumber, UserBusiness.findUserById(userId).LogName, Math.Round(score, 2), tbScore, 
                            //    calbeType, contractMoney, strPayType, money, contractStartTime, contractEndTime, proxy, receivable);
                            return score;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 财务统计报表
        public static decimal AccountSummaryReport(DateTime startDate, DateTime endDate, int userId,int month,int year)
        {
            decimal totalScore=0; //合计提成金额
            try
            {
                object obj = CommissionBusiness.isExistCommissionValueByMontAndSaler(userId, startDate, endDate, year, month);
                if (obj != null)
                {
                    DataTable dt = (DataTable)obj;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try
                            {
                                int flag = Utils.NvInt(dt.Rows[i]["flag"].ToString());  //支付方式
                                Cable c = CableBusiness.getCalbeByCableId(int.Parse(dt.Rows[i]["电路id"].ToString()));
                                Dateinterval interval = new Dateinterval();
                                //计算时间间隔
                                interval.StartDate = DateTime.Parse(dt.Rows[i]["账单开始时间"].ToString());
                                interval.EndDate = DateTime.Parse(dt.Rows[i]["账单结束时间"].ToString());
                                Nullable<DateTime> begindate = DateTime.Parse(dt.Rows[i]["提成开始时间"].ToString());
                                Nullable<DateTime> enddate = null;
                                if (dt.Rows[i]["提成结束时间"] != null && !string.IsNullOrEmpty(dt.Rows[i]["提成结束时间"].ToString()))
                                {
                                    enddate = DateTime.Parse(dt.Rows[i]["提成结束时间"].ToString());
                                }
                                int days = ScoreBusiness.calCommissionDate(interval, begindate, enddate);//根据时间区间和提成日期计算有效天数
                                if (days <= 0)
                                {
                                    continue;
                                }
                                int daysInBill = new TimeSpan(DateTime.Parse(dt.Rows[i]["账单开始时间"].ToString()).Ticks).Subtract(new TimeSpan(DateTime.Parse(dt.Rows[i]["账单结束时间"].ToString()).Ticks)).Duration().Days + 1;  //计算间隔天数//计算账单结算区间天数
                                decimal ratio = decimal.Parse(dt.Rows[i]["提成比例"].ToString()) / 100;
                                decimal tax = decimal.Parse(dt.Rows[i]["税率"].ToString()) / 100;
                                decimal money = decimal.Parse(dt.Rows[i]["提成金额"].ToString());
                                if (money == 0)
                                {
                                    continue;
                                }
                                decimal score = 0; //业绩计算
                                if (flag == (int)EnmWriteOffFlag.正常结算)
                                {
                                    score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //业绩计算
                                }
                                else if (flag == (int)EnmWriteOffFlag.补结算)
                                {
                                    score = SuppmentWriteoffScore(interval.StartDate, interval.EndDate,c,money, userId,year,month);
                                }
                                totalScore += score;
                            }
                            catch (Exception ex)
                            {
                                Log.Warning(ex.Message);
                                continue;
                            }
                        }
                    }
                }
                return totalScore;
            }
            catch (Exception ex)
            {
                Log.Warning(ex.Message);
                return -1;
            }
        }
        #endregion

        #region 计算补结提成金额
        //public decimal getSupplementScore(int year,int month)
        //{
        //    try
        //    {
        //        object obj = CommissionBusiness.getSupplementByMonth(year, month);
        //        if (obj != null)
        //        {
        //            DataTable dt = (DataTable)obj;
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                DateTime Settlementstart = DateTime.Parse(dt.Rows[i]["Settlementstart"].ToString());  //结算开始时间
        //                DateTime Settlementend = DateTime.Parse(dt.Rows[i]["Settlementend"].ToString());      //结算截止时间
        //                int cableId = Utils.NvInt(dt.Rows[i]["cableid"].ToString());
        //                if (cableId > 0)
        //                {
        //                    Cable cable = CableBusiness.getCalbeByCableId(cableId);
        //                    if (cable != null)
        //                    {
        //                        int payType = cable.Paytype;  //支付方式
        //                        //根据支付方式统计获得补结的提成金额。
        //                        decimal writeoff = Utils.NvDecimal(dt.Rows[i]["writeoff"].ToString());  //销账金额

        //                    }
        //                    else
        //                    {
        //                        return 0;
        //                    }
        //                }
        //                else
        //                {
        //                    return 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        #endregion

        #region 填写业绩的datatable
        private static void writeScoreTable(int year, int month, string cusName, string cablenumber, string salernumber, decimal score, DataTable tbScore, 
            string cableType, decimal contractMoney, string paytype, decimal money, Nullable<DateTime> startDate, Nullable<DateTime> endDate, 
            decimal proxy, decimal receivable,decimal ratio,decimal tax,int days)
        {
            DataRow[] r = tbScore.Select("年度=" + year + " and 月度=" + month + " and 客户='" + cusName + "' and 销售渠道='" + salernumber + "' and 电路代码='" + cablenumber + "' and 提成比例='" + ratio + "' and 税率='" + tax + "'"); //根据条件查找datatable对应数据行
            if (r.Length > 0)  //如果找到数据行，将提成金额累加
            {
                r[0]["提成金额"] = Utils.NvDecimal(r[0]["提成金额"].ToString()) + score;  //累加提成金额
            }
            else   //新增行数据
            {
                DataRow row = tbScore.NewRow();
                row["年度"] = year;
                row["月度"] = month;
                row["电路代码"] = cablenumber;
                row["客户"] = cusName;
                row["应收月租费"] = receivable;
                row["付款方式"] = paytype;
                row["电路类型"] = cableType;
                row["销售渠道"] = salernumber;
                row["销账金额"] = money;
                row["结算起始日期"] = startDate;
                row["结算截止日期"] = endDate;
                row["代理费"] = proxy;
                row["提成比例"] = ratio * 100;
                row["税率"] = tax;
                row["实际天数"] = days;
                row["提成金额"] = score;
                tbScore.Rows.Add(row);
            }
        }
        #endregion
        #endregion

        #region 根据条件查询业绩信息
        public static DataTable getScoreByCondition(Hashtable hs)
        {
            try
            {
                string sql = "select c.cablenumber as 电路代码, cu.customerName as 客户名称,u.logname as 主销售渠道,c.completeTime as 完工日期, RemoveTime as 拆机日期,"
                        + "c.contractType as 电路类型,  c.money as 合同金额,s.writeoff as 销账金额,s.month as 月度,s.year as 年度, s.Settlementstart as 结算起始日期, s.Settlementend as 结算截止日期,"
                        + "(case c.paytype when " + (int)EnmPayType.一次性付 + " then '一次性付' when " + (int)EnmPayType.季付 + " then '季付' when " + (int)EnmPayType.半年付 + " then '半年付' "
                        + "when " + (int)EnmPayType.月付 + " then '月付' end) as 付款类型,c.Limittime as 合同期限 "
                        + "from salebills as s left join cable as c on s.cableid = c.id left join customer as cu on s.customerid = cu.id "
                        + "left join tb_user as u on c.userid = u.id where 1=1 ";
                string condition = "";
                string orderby = "";
                if (hs.Count > 0)
                {
                    if (hs.ContainsKey("客户"))
                    {
                        condition += "and cu.CustomerName like '%" + hs["客户"].ToString() + "%' ";
                    }
                    if (hs.ContainsKey("完工起始"))
                    {
                        condition += "and c.CompleteTime >='" + hs["完工起始"].ToString() + "' ";
                    }
                    if (hs.ContainsKey("完工截止"))
                    {
                        condition += "and c.CompleteTime <='" + hs["完工截止"].ToString() + "' ";
                    }
                    if (hs.ContainsKey("结算方式"))
                    {
                        condition += "and s.flag = " + Utils.NvInt(hs["结算方式"]) + " ";
                    }
                    if (hs.ContainsKey("导入起始年度"))
                    {
                        condition += " and ((s.year>=" + Utils.NvInt(hs["导入起始年度"]) + " ";
                    }
                    if (hs.ContainsKey("导入起始月度"))
                    {
                        condition += " and s.month>=" + Utils.NvInt(hs["导入起始月度"]) + ") ";
                    }
                    if (hs.ContainsKey("导入截止年度"))
                    {
                        condition += " and (s.year<=" + Utils.NvInt(hs["导入截止年度"]) + " ";
                    }
                    if (hs.ContainsKey("导入截止月度"))
                    {
                        condition += " and s.month<=" + Utils.NvInt(hs["导入截止月度"]) + ")) ";
                    }
                    condition += " and s.Isdeleted=" + (int)EnmIsdeleted.使用中 + " and cu.Isdeleted =" + (int)EnmIsdeleted.使用中 + " and c.Isdeleted=" + (int)EnmIsdeleted.使用中 + " ";
                    orderby = "order by s.month , s.year desc";
                }
                using (DbHelper db = new DbHelper())
                {
                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql + condition + orderby, paramlist).Tables[0]; //查询结果表
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "查询账单发生错误。");
                return null;
            }
        }
        #endregion   

        #region 查询并计算实绩提成收入
        public static DataTable getActual(string condition, DateTime endTime)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select b.year as 年度,b.month as 月度,c.cablenumber as 电路代码, cu.customername as 客户名称, "
                    + "c.ContractType as 电路类型, u.name as 渠道名称, c.CompleteTime as 完工日期,b.writeoff as 合同金额,b.receivable as 应收账款, "
                    + "cm.ratio as 提成比率,cm.tax as 税率, b.settlementstart as 起始日期, b.settlementend as 截止日期,b.proxy as 代理费, "
                    + "c.paytype as 付款类型,cm.beginTime ,cm.endtime,'' as 提成金额  from  salebills as b left join cable as c on b.cableid = c.id "
                    + "left join commission as cm on cm.CableId = b.cableid left join tb_User as u on cm.userid = u.id left join customer as cu on b.customerid = cu.id "
                    + "where c.isDeleted=" + (int)EnmIsdeleted.使用中 + " and u.isDeleted =" + (int)EnmIsdeleted.使用中 + " and b.writeoff>0 "
                    + "and cu.isDeleted =" + (int)EnmIsdeleted.使用中 + " and c.CableStatus<>" + (int)EnmCableStatus.未完工 + " and c.CableStatus<>" + (int)EnmCableStatus.已拆机 + ""
                    + condition + " order by year,month ";
                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                    DataTable result = new DataTable();
                    if (tb.Rows.Count > 0)
                    {
                        //result = calActualMoney(tb, endTime);  //计算提成金额
                    }
                    if (result.Rows.Count > 0)
                    {
                        return result;
                    }
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new DataTable();
            }
        }

        //public static DataTable getActual(string salerName,DateTime start,DateTime end, DateTime endTime)
        //{
        //    DataTable result = new DataTable();
        //    try
        //    {
        //        using (DbHelper db = new DbHelper())
        //        {
        //            string sql = "select b.year as 年度,b.month as 月度,c.cablenumber as 电路代码, cu.customername as 客户名称, "
        //            + "c.ContractType as 电路类型, u.name as 渠道名称, c.CompleteTime as 完工日期,b.writeoff as 合同金额,b.receivable as 应收账款, "
        //            + "cm.ratio as 提成比率,cm.tax as 税率, b.settlementstart as 起始日期, b.settlementend as 截止日期,b.proxy as 代理费, "
        //            + "c.paytype as 付款类型,cm.beginTime ,cm.endtime,'' as 提成金额  from  salebills as b left join cable as c on b.cableid = c.id "
        //            + "left join commission as cm on cm.CableId = b.cableid left join tb_User as u on cm.userid = u.id left join customer as cu on b.customerid = cu.id "
        //            + "where c.isDeleted=" + (int)EnmIsdeleted.使用中 + " and u.isDeleted =" + (int)EnmIsdeleted.使用中 + " and b.writeoff>0 "
        //            + "and cu.isDeleted =" + (int)EnmIsdeleted.使用中 + " and c.CableStatus<>" + (int)EnmCableStatus.未完工 + " and c.CableStatus<>" + (int)EnmCableStatus.已拆机 + " and c.CableStatus<>" + (int)EnmCableStatus.取消 + " "
        //            + "and (b.year>=" + start.Year + " and b.year <=" + end.Year + ") and (b.month>=" + start.Month + " and b.month<=" + end.Month + ") ";
        //            if (!string.IsNullOrEmpty(salerName))
        //            {
        //                sql += "and u.name like '%" + salerName + "%' ";
        //            }
        //            sql += "order by year,month ";
        //            DbParameter[] paramlist = { };
        //            DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];                    
        //            if (tb.Rows.Count > 0)
        //            {
        //                result = calActualMoney(tb,endTime);  //计算提成金额
        //            }
        //            //if (result.Rows.Count > 0)
        //            //{
        //            //    return result;
        //            //}
        //            //return new DataTable();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        return result;
        //    }
        //}

        #endregion

        #region 计算提成有效日期
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="begindate">账单结算开始时间</param>
        /// <param name="enddate">账单结算截止时间</param>
        /// <returns></returns>
        private static int calCommissionDate(Dateinterval interval, Nullable<DateTime> begindate, Nullable<DateTime> enddate)
        {           
            DateTime startinterval = interval.StartDate; //区间开始日期
            DateTime endinterval = interval.EndDate;  //区间截止日期

            //Nullable<DateTime> begindate = cm.Begintime;  //提成开始时间;
            //Nullable<DateTime> enddate = cm.Endtime;  //提成截止时间
            int days = 0;
            if (begindate == null)
            {
                return days;
            }
            if (begindate > endinterval)
            {
                return days;
            }
            //判断提成有效区间
            if (begindate <= startinterval)
            {
                if (enddate == null || enddate >= endinterval)  //没有“提成截止日期”或者“提成截止日期”晚于等于“区间截止日期”
                {
                    days = calDays(startinterval, endinterval); //用“区间开始日期”和“区间截止日期”计算间隔天数
                }
                else if (enddate < endinterval && enddate>startinterval)   //“提成截止日期”早于“区间截止日期”
                {
                    days = calDays(startinterval, enddate.Value);   //用“提成起始日期”和“提成截止日期”计算间隔天数
                }
            }
            else if(begindate>startinterval)  //"提成起始日期" 晚于"区间起始日期"
            {
                if (enddate > endinterval && begindate < endinterval)    //"提成截止日期" 晚于"区间截止日期"
                {
                    days = calDays(begindate.Value, endinterval);   //用“提成起始日期”和“区间截止日期”计算间隔天数
                }
                else if (enddate <= endinterval)   //"提成截止日期" 早于等于于"区间截止日期"
                {
                    days = calDays(begindate.Value, enddate.Value);   //用“提成起始日期”和“提成截止日期”计算间隔天数
                }
                else if (enddate == null && begindate <= startinterval) //未设置“提成截止日期”并且“提成起始日期”早于等于“区间起始日期”
                {
                    days = calDays(begindate.Value, endinterval);   //用“提成起始日期”和“区间截止日期”计算间隔天数
                }
                else if (enddate == null && begindate > startinterval) //未设置“提成截止日期”并且“提成起始日期”早于等于“区间起始日期”
                {
                    days = calDays(begindate.Value, endinterval);   //用“提成起始日期”和“区间截止日期”计算间隔天数
                }
            }
            //else if (enddate<startinterval) //“提成截止日期”早于“区间起始日期”
            //{
            //    return days;
            //}
            //else if (begindate > endinterval) //“提成起始日期”晚于“区间截止日期”
            //{
            //    return days;
            //}
            return days;
        }
        #endregion

        #region 初始化datatable列
        private static DataTable createCol()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("年度", typeof(int));
            tb.Columns.Add("月度", typeof(int));
            tb.Columns.Add("客户", typeof(string));
            tb.Columns.Add("电路代码", typeof(string));
            tb.Columns.Add("电路类型", typeof(string));            
            tb.Columns.Add("合同金额", typeof(decimal));
            tb.Columns.Add("付款方式", typeof(string));
            tb.Columns.Add("应收月租费", typeof(decimal));
            tb.Columns.Add("销账金额", typeof(decimal));
            tb.Columns.Add("结算起始日期", typeof(string));
            tb.Columns.Add("结算截止日期", typeof(string));
            tb.Columns.Add("销售渠道", typeof(string));
            tb.Columns.Add("代理费",typeof(decimal));
            tb.Columns.Add("提成比例", typeof(decimal));
            tb.Columns.Add("税率", typeof(decimal));
            tb.Columns.Add("实际天数", typeof(int));
            tb.Columns.Add("提成金额", typeof(decimal));
            
            return tb;
        }
        #endregion

        #region 计算区间间隔天数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns></returns>
        private static int calDays(DateTime startDate, DateTime endDate)
        {
            int days = new TimeSpan(startDate.Ticks).Subtract(new TimeSpan(endDate.Ticks)).Duration().Days + 1;  //计算间隔天数
            return days;
        }
        #endregion

        #region 计算提成金额
        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthdays">当月(结算)天数</param>
        /// <param name="days">实际天数</param>
        /// <param name="money">每月费用</param>
        /// <param name="ratio">提成率</param>
        /// <param name="tax">税率</param>
        /// <returns></returns>
        public static decimal calCommissionMoney(int daysInMonth, int days, decimal money, decimal ratio, decimal tax)
        {
            return Math.Round(decimal.Parse(days.ToString()) / daysInMonth * money * ratio * (1 - tax),2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthdays">当月(结算)天数</param>
        /// <param name="days">实际天数</param>
        /// <param name="money">每月费用</param>
        /// <param name="ratio">提成率</param>
        /// <param name="tax">税率</param>
        /// <returns></returns>
        public static decimal calCommissionMoney(decimal money, decimal ratio, decimal tax)
        {
            return money * ratio * (1 - tax);
        }
        #endregion

        #region 根据合同类型判断是否有预结金额
        /// <summary>
        /// 根据合同类型判断是否有预结金额
        /// </summary>
        /// <param name="cable">电路合同</param>
        /// <param name="year">导入年度</param>
        /// <param name="month">导入月度</param>
        /// <returns></returns>
        public static decimal isHaveWriteoff(DateTime startDate,DateTime endDate, int paytype, decimal money, int year, int month)
        {
            //DateTime contractStart = DateTime.Parse(cable.Startdate.ToString());   //合同起始日期
            //DateTime contractEnd = DateTime.Parse(cable.Enddate.ToString());       //合同截止日期
            ////int paytype = cable.Paytype;
            //decimal money = cable.Money;   //电路合同金额
            //判断是否需要计算起始月
            switch (paytype)
            {
                case (int)EnmPayType.月付:
                    return money / 12;
                case (int)EnmPayType.季付:                  
                    for (DateTime tmpDate = startDate; tmpDate < endDate; tmpDate = tmpDate.AddMonths(3))
                    {
                        if (tmpDate.Year == year && tmpDate.Month == month)
                        {
                            return money / 4;
                        }                       
                    }
                    return 0;
                case (int)EnmPayType.一次性付:
                    if (startDate.Year == year && startDate.Month == month)
                    {
                        return money;
                    }
                    else
                    {
                        return 0;
                    }                  
                case (int)EnmPayType.半年付:                   
                    for (DateTime tmpDate = startDate; tmpDate < endDate; tmpDate = tmpDate.AddMonths(6))
                    {
                        if (tmpDate.Year == year && tmpDate.Month == month)
                        {
                            return money / 2;
                        }
                    }
                    return 0;
            }
            return 0;
        }
        #endregion

        
    }
}
