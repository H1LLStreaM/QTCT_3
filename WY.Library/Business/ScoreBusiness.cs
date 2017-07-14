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
        #region ������ɽ��
        #region Ԥ��
        /// <summary>
        /// ����Ԥ����ɽ��
        /// </summary>
        /// <param name="startDate">��ѯ��ʼ����</param>
        /// <param name="endDate">��ѯ��ֹ����</param>
        /// <param name="cableNumber">��·����</param>
        /// <param name="salerId">��������</param>
        /// <param name="tbScore">���ݱ�ģ��</param>
        public static DataTable makeExpectedScore(DateTime startDate, DateTime endDate, string cableNumber, int salerId)
        {
            DataTable tbScore = createCol();
            try
            {
                for (DateTime date = startDate; date <= endDate; date = date.AddMonths(1))
                {
                    //Cable[] cables = CableBusiness.getCompleteCalbe(date.Year, date.Month, cableNumber);  //��ѯ��·������Ϣ
                    DataTable cables = CableBusiness.getCompleteCalbe(date.Year, date.Month, cableNumber,salerId);  //��ѯ��·������Ϣ
                    if (cables != null && cables.Rows.Count > 0)  //�ж��Ƿ�Ϊ�ջ�ûֵ
                    {
                        for (int i = 0; i < cables.Rows.Count; i++)  //ѭ����·��ͬ
                        {
                            Cable tmpCalbe =CableBusiness.getCalbeByCableId(Utils.NvInt(cables.Rows[i]["id"]));
                            if (tmpCalbe == null)
                            {
                                continue;
                            }
                            string cusName = CustomerBusiness.findCustomerById(tmpCalbe.Customerid).Customername;//cables[i].Customer.Customername; //�ͻ�����
                            string cablenumber = tmpCalbe.Cablenumber;    //��·����
                            string calbeType = tmpCalbe.Contracttype;    //��·����
                            string paytype = GlobalBusiness.getPayType(tmpCalbe.Paytype);  //��ͬ���ʽ
                            if (tmpCalbe.Cablestatus == (int)EnmCableStatus.ȡ��)
                            {
                                continue;
                            }
                            Nullable<DateTime> contractStartTime = tmpCalbe.Startdate;  //��ͬ��ʼ����
                            Nullable<DateTime> contractEndTime = tmpCalbe.Enddate;      //��ͬ��ֹ����
                            if (tmpCalbe.Removetime != null)
                            {
                                contractEndTime = tmpCalbe.Removetime;
                            }
                            decimal contractMoney = ScoreBusiness.isHaveWriteoff(DateTime.Parse(contractStartTime.ToString()), DateTime.Parse(contractEndTime.ToString()), tmpCalbe.Paytype, tmpCalbe.Money, date.Year, date.Month);   //cables[i].Money;      //��ͬ���
                            if (contractMoney == 0)
                            {
                                continue;
                            }
                            if (contractStartTime == null || contractEndTime == null)    //�����ͬû����ʼ�����ʱ��ֱ������������һ��ѭ��
                            {
                                continue;
                            }
                            Dateinterval interval = CableBusiness.anaylisContractMonth(contractStartTime, contractEndTime, date);  //��ȡʱ������
                            decimal money = tmpCalbe.Money;  //��·��ͬ���
                            int months = GlobalBusiness.getPayTypeForMonth(tmpCalbe.Paytype); //���ݺ�ͬ���ͼ���ʹ������
                            decimal avgmoney = money / months;  //ƽ��ÿ�·���
                            int cableId = tmpCalbe.Id;     //��·����ID
                            Commission[] cs = CommissionBusiness.getCommissions(cableId, salerId);  //���������Ϣ
                            if (cs != null && cs.Length > 0)  //���ݵ�·�����������ѯ���
                            {
                                for (int j = 0; j < cs.Length; j++)
                                {
                                    int year = date.Year;     //���
                                    int month = date.Month;   //�¶�
                                    string saler = UserBusiness.findUserById(cs[j].Userid).USER_NAME; //��������
                                    int days = ScoreBusiness.calCommissionDate(interval, cs[j].Begintime,cs[j].Endtime);//����ʱ�������������ڼ�����Ч����
                                    if (days <= 0)
                                    {
                                        continue;
                                    }
                                    int daysInMonth = DateTime.DaysInMonth(year, month); //���㵱������ 
                                    decimal ratio = cs[j].Ratio / 100;
                                    decimal tax = cs[j].Tax / 100;
                                    decimal score = 0;
                                    if (tmpCalbe.Paytype == (int)EnmPayType.�¸�)
                                    {
                                        score = ScoreBusiness.calCommissionMoney(daysInMonth, days, avgmoney, ratio, tax); //ҵ������
                                        writeScoreTable(year, month, cusName, cablenumber, saler, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, 0, 0, ratio, tax, daysInMonth);
                                    }
                                    else
                                    {
                                        score = ScoreBusiness.calCommissionMoney(avgmoney, ratio, tax); //ҵ������
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
                MessageHelper.ShowMessage("E999", "Ԥ����ɽ����㷢������");
                return tbScore;
            }            
        }

        #endregion

        #region ʵ��
        /// <summary>
        /// ����ʵ����ɽ��
        /// </summary>
        /// <param name="startDate">��ѯ��ʼ����</param>
        /// <param name="endDate">��ѯ��ֹ����</param>
        /// <param name="cableNumber">��·����</param>
        /// <param name="userId">��������</param>
        /// <param name="tbScore">���ݱ�ģ��</param>
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
                                Cable c = CableBusiness.getCalbeByCableId(int.Parse(dt.Rows[i]["��·id"].ToString()));
                                string cusName = CustomerBusiness.findCustomerById(int.Parse(dt.Rows[i]["�ͻ�id"].ToString())).Customername; //�ͻ�����
                                string cablenumber = c.Cablenumber;    //��·����
                                string calbeType = c.Contracttype;    //��·����
                                string paytype = GlobalBusiness.getPayType(c.Paytype);  //��ͬ���ʽ
                                decimal proxy = decimal.Parse(dt.Rows[i]["proxy"].ToString());  //�����
                                decimal contractMoney = c.Money;      //��ͬ���
                                decimal receivable = decimal.Parse(dt.Rows[i]["Receivable"].ToString());  //�����
                                //����ʱ����
                                Nullable<DateTime> contractStartTime = DateTime.Parse(dt.Rows[i]["�˵���ʼʱ��"].ToString());  //�˵�������ʼ����
                                Nullable<DateTime> contractEndTime = DateTime.Parse(dt.Rows[i]["�˵�����ʱ��"].ToString());    //�˵������ֹ����
                                if (contractStartTime == null || contractEndTime == null)    //�����ͬû����ʼ�����ʱ��
                                {
                                    continue;
                                }
                                Dateinterval interval = new Dateinterval();
                                //����ʱ����
                                interval.StartDate = DateTime.Parse(contractStartTime.ToString());
                                interval.EndDate = DateTime.Parse(contractEndTime.ToString());                                
                                Nullable<DateTime> begindate = DateTime.Parse(dt.Rows[i]["��ɿ�ʼʱ��"].ToString());
                                Nullable<DateTime> enddate = null;
                                if (dt.Rows[i]["��ɽ���ʱ��"] != null && !string.IsNullOrEmpty(dt.Rows[i]["��ɽ���ʱ��"].ToString()))
                                {
                                    enddate = DateTime.Parse(dt.Rows[i]["��ɽ���ʱ��"].ToString());
                                }
                                int days = ScoreBusiness.calCommissionDate(interval, begindate, enddate);//����ʱ�������������ڼ�����Ч����
                                if (days <= 0)
                                {
                                    continue;
                                }
                                int daysInBill = new TimeSpan(DateTime.Parse(dt.Rows[i]["�˵���ʼʱ��"].ToString()).Ticks).Subtract(new TimeSpan(DateTime.Parse(dt.Rows[i]["�˵�����ʱ��"].ToString()).Ticks)).Duration().Days + 1;  //����������//�����˵�������������
                                decimal ratio = decimal.Parse(dt.Rows[i]["��ɱ���"].ToString()) / 100;
                                decimal tax = decimal.Parse(dt.Rows[i]["˰��"].ToString()) / 100;
                                decimal money = decimal.Parse(dt.Rows[i]["��ɽ��"].ToString());   //�˵����
                                int year = date.Year;    //���
                                int month = date.Month;   //�¶�
                                //TB_User saler = UserBusiness
                                if (flag == (int)EnmWriteOffFlag.��������)
                                {
                                    score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //ҵ������
                                    writeScoreTable(year, month, cusName, cablenumber, UserBusiness.findUserById(userId).USER_NAME, Math.Round(score, 2), tbScore, calbeType, contractMoney, paytype, money, contractStartTime, contractEndTime, proxy, receivable, ratio, tax, daysInBill);
                                }
                                else if (flag == (int)EnmWriteOffFlag.������)
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
                MessageHelper.ShowMessage("E999", "Ԥ����ɽ����㷢������");
                return tbScore;
            }      
        }
        #endregion

        #region ���ݺ�ͬ���ͼ��㲹����ɽ��
        public static decimal SuppmentWriteoffScore(DateTime startDate, DateTime endDate, Cable cable, decimal money, int userId, int year, int month)
        {
            try
            {
                string strPayType; //֧����ʽ
                List<Dateinterval> list = new List<Dateinterval>();
                Dateinterval d;
                switch (cable.Paytype)
                {
                    case (int)EnmPayType.�¸�:
                        strPayType = "�¸�";
                        d = new Dateinterval();
                        d.StartDate = startDate;
                        d.EndDate = endDate;
                        d.Money = money;
                        list.Add(d);
                        break;
                    case (int)EnmPayType.����:
                        strPayType = "����";
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
                    case (int)EnmPayType.���긶:
                        strPayType = "���긶";
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
                    case (int)EnmPayType.һ���Ը�:
                        strPayType = "һ���Ը�";
                        d = new Dateinterval();
                        d.StartDate = startDate;
                        d.EndDate = DateTime.Parse(startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-01").Date.AddMonths(1).AddDays(-1);
                        d.Money = money;
                        list.Add(d);
                        break;
                    default:
                        return 0;
                }
                //����������ʷ���㲹����
                for (int i = 0; i < list.Count; i++)
                {
                    object value = CommissionBusiness.isExistCommissionValueByMontAndSaler(userId, list[i].StartDate, list[i].EndDate, year, month);
                    if (value != null)
                    {
                        DataTable dt = (DataTable)value;
                        DataRow[] rows = dt.Select("��·id=" + cable.Id + " and flag=" + (int)EnmWriteOffFlag.������ + "");
                        for (int j = 0; j < rows.Length; j++)
                        {
                            Nullable<DateTime> begindate = DateTime.Parse(rows[j]["��ɿ�ʼʱ��"].ToString());
                            Nullable<DateTime> enddate = null;
                            if (dt.Rows[j]["��ɽ���ʱ��"] != null && !string.IsNullOrEmpty(rows[j]["��ɽ���ʱ��"].ToString()))
                            {
                                enddate = DateTime.Parse(rows[j]["��ɽ���ʱ��"].ToString());
                            }
                            int days = ScoreBusiness.calCommissionDate(list[i], begindate, enddate);//����ʱ�������������ڼ�����Ч����
                            if (days <= 0)
                            {
                                continue;
                            }
                            int daysInBill = new TimeSpan(list[i].StartDate.Ticks).Subtract(new TimeSpan(list[i].EndDate.Ticks)).Duration().Days + 1;  //����������//�����˵�������������
                            decimal ratio = decimal.Parse(rows[i]["��ɱ���"].ToString()) / 100;
                            decimal tax = decimal.Parse(rows[i]["˰��"].ToString()) / 100;
                            //decimal money = decimal.Parse(dt.Rows[i]["��ɽ��"].ToString());
                            decimal score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //ҵ������
                            //int year = date.Year;    //���
                            //int month = date.Month;   //�¶�
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

        #region ����ͳ�Ʊ���
        public static decimal AccountSummaryReport(DateTime startDate, DateTime endDate, int userId,int month,int year)
        {
            decimal totalScore=0; //�ϼ���ɽ��
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
                                int flag = Utils.NvInt(dt.Rows[i]["flag"].ToString());  //֧����ʽ
                                Cable c = CableBusiness.getCalbeByCableId(int.Parse(dt.Rows[i]["��·id"].ToString()));
                                Dateinterval interval = new Dateinterval();
                                //����ʱ����
                                interval.StartDate = DateTime.Parse(dt.Rows[i]["�˵���ʼʱ��"].ToString());
                                interval.EndDate = DateTime.Parse(dt.Rows[i]["�˵�����ʱ��"].ToString());
                                Nullable<DateTime> begindate = DateTime.Parse(dt.Rows[i]["��ɿ�ʼʱ��"].ToString());
                                Nullable<DateTime> enddate = null;
                                if (dt.Rows[i]["��ɽ���ʱ��"] != null && !string.IsNullOrEmpty(dt.Rows[i]["��ɽ���ʱ��"].ToString()))
                                {
                                    enddate = DateTime.Parse(dt.Rows[i]["��ɽ���ʱ��"].ToString());
                                }
                                int days = ScoreBusiness.calCommissionDate(interval, begindate, enddate);//����ʱ�������������ڼ�����Ч����
                                if (days <= 0)
                                {
                                    continue;
                                }
                                int daysInBill = new TimeSpan(DateTime.Parse(dt.Rows[i]["�˵���ʼʱ��"].ToString()).Ticks).Subtract(new TimeSpan(DateTime.Parse(dt.Rows[i]["�˵�����ʱ��"].ToString()).Ticks)).Duration().Days + 1;  //����������//�����˵�������������
                                decimal ratio = decimal.Parse(dt.Rows[i]["��ɱ���"].ToString()) / 100;
                                decimal tax = decimal.Parse(dt.Rows[i]["˰��"].ToString()) / 100;
                                decimal money = decimal.Parse(dt.Rows[i]["��ɽ��"].ToString());
                                if (money == 0)
                                {
                                    continue;
                                }
                                decimal score = 0; //ҵ������
                                if (flag == (int)EnmWriteOffFlag.��������)
                                {
                                    score = ScoreBusiness.calCommissionMoney(daysInBill, days, money, ratio, tax); //ҵ������
                                }
                                else if (flag == (int)EnmWriteOffFlag.������)
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

        #region ���㲹����ɽ��
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
        //                DateTime Settlementstart = DateTime.Parse(dt.Rows[i]["Settlementstart"].ToString());  //���㿪ʼʱ��
        //                DateTime Settlementend = DateTime.Parse(dt.Rows[i]["Settlementend"].ToString());      //�����ֹʱ��
        //                int cableId = Utils.NvInt(dt.Rows[i]["cableid"].ToString());
        //                if (cableId > 0)
        //                {
        //                    Cable cable = CableBusiness.getCalbeByCableId(cableId);
        //                    if (cable != null)
        //                    {
        //                        int payType = cable.Paytype;  //֧����ʽ
        //                        //����֧����ʽͳ�ƻ�ò������ɽ�
        //                        decimal writeoff = Utils.NvDecimal(dt.Rows[i]["writeoff"].ToString());  //���˽��

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

        #region ��дҵ����datatable
        private static void writeScoreTable(int year, int month, string cusName, string cablenumber, string salernumber, decimal score, DataTable tbScore, 
            string cableType, decimal contractMoney, string paytype, decimal money, Nullable<DateTime> startDate, Nullable<DateTime> endDate, 
            decimal proxy, decimal receivable,decimal ratio,decimal tax,int days)
        {
            DataRow[] r = tbScore.Select("���=" + year + " and �¶�=" + month + " and �ͻ�='" + cusName + "' and ��������='" + salernumber + "' and ��·����='" + cablenumber + "' and ��ɱ���='" + ratio + "' and ˰��='" + tax + "'"); //������������datatable��Ӧ������
            if (r.Length > 0)  //����ҵ������У�����ɽ���ۼ�
            {
                r[0]["��ɽ��"] = Utils.NvDecimal(r[0]["��ɽ��"].ToString()) + score;  //�ۼ���ɽ��
            }
            else   //����������
            {
                DataRow row = tbScore.NewRow();
                row["���"] = year;
                row["�¶�"] = month;
                row["��·����"] = cablenumber;
                row["�ͻ�"] = cusName;
                row["Ӧ�������"] = receivable;
                row["���ʽ"] = paytype;
                row["��·����"] = cableType;
                row["��������"] = salernumber;
                row["���˽��"] = money;
                row["������ʼ����"] = startDate;
                row["�����ֹ����"] = endDate;
                row["�����"] = proxy;
                row["��ɱ���"] = ratio * 100;
                row["˰��"] = tax;
                row["ʵ������"] = days;
                row["��ɽ��"] = score;
                tbScore.Rows.Add(row);
            }
        }
        #endregion
        #endregion

        #region ����������ѯҵ����Ϣ
        public static DataTable getScoreByCondition(Hashtable hs)
        {
            try
            {
                string sql = "select c.cablenumber as ��·����, cu.customerName as �ͻ�����,u.logname as ����������,c.completeTime as �깤����, RemoveTime as �������,"
                        + "c.contractType as ��·����,  c.money as ��ͬ���,s.writeoff as ���˽��,s.month as �¶�,s.year as ���, s.Settlementstart as ������ʼ����, s.Settlementend as �����ֹ����,"
                        + "(case c.paytype when " + (int)EnmPayType.һ���Ը� + " then 'һ���Ը�' when " + (int)EnmPayType.���� + " then '����' when " + (int)EnmPayType.���긶 + " then '���긶' "
                        + "when " + (int)EnmPayType.�¸� + " then '�¸�' end) as ��������,c.Limittime as ��ͬ���� "
                        + "from salebills as s left join cable as c on s.cableid = c.id left join customer as cu on s.customerid = cu.id "
                        + "left join tb_user as u on c.userid = u.id where 1=1 ";
                string condition = "";
                string orderby = "";
                if (hs.Count > 0)
                {
                    if (hs.ContainsKey("�ͻ�"))
                    {
                        condition += "and cu.CustomerName like '%" + hs["�ͻ�"].ToString() + "%' ";
                    }
                    if (hs.ContainsKey("�깤��ʼ"))
                    {
                        condition += "and c.CompleteTime >='" + hs["�깤��ʼ"].ToString() + "' ";
                    }
                    if (hs.ContainsKey("�깤��ֹ"))
                    {
                        condition += "and c.CompleteTime <='" + hs["�깤��ֹ"].ToString() + "' ";
                    }
                    if (hs.ContainsKey("���㷽ʽ"))
                    {
                        condition += "and s.flag = " + Utils.NvInt(hs["���㷽ʽ"]) + " ";
                    }
                    if (hs.ContainsKey("������ʼ���"))
                    {
                        condition += " and ((s.year>=" + Utils.NvInt(hs["������ʼ���"]) + " ";
                    }
                    if (hs.ContainsKey("������ʼ�¶�"))
                    {
                        condition += " and s.month>=" + Utils.NvInt(hs["������ʼ�¶�"]) + ") ";
                    }
                    if (hs.ContainsKey("�����ֹ���"))
                    {
                        condition += " and (s.year<=" + Utils.NvInt(hs["�����ֹ���"]) + " ";
                    }
                    if (hs.ContainsKey("�����ֹ�¶�"))
                    {
                        condition += " and s.month<=" + Utils.NvInt(hs["�����ֹ�¶�"]) + ")) ";
                    }
                    condition += " and s.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " and cu.Isdeleted =" + (int)EnmIsdeleted.ʹ���� + " and c.Isdeleted=" + (int)EnmIsdeleted.ʹ���� + " ";
                    orderby = "order by s.month , s.year desc";
                }
                using (DbHelper db = new DbHelper())
                {
                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql + condition + orderby, paramlist).Tables[0]; //��ѯ�����
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��ѯ�˵���������");
                return null;
            }
        }
        #endregion   

        #region ��ѯ������ʵ���������
        public static DataTable getActual(string condition, DateTime endTime)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select b.year as ���,b.month as �¶�,c.cablenumber as ��·����, cu.customername as �ͻ�����, "
                    + "c.ContractType as ��·����, u.name as ��������, c.CompleteTime as �깤����,b.writeoff as ��ͬ���,b.receivable as Ӧ���˿�, "
                    + "cm.ratio as ��ɱ���,cm.tax as ˰��, b.settlementstart as ��ʼ����, b.settlementend as ��ֹ����,b.proxy as �����, "
                    + "c.paytype as ��������,cm.beginTime ,cm.endtime,'' as ��ɽ��  from  salebills as b left join cable as c on b.cableid = c.id "
                    + "left join commission as cm on cm.CableId = b.cableid left join tb_User as u on cm.userid = u.id left join customer as cu on b.customerid = cu.id "
                    + "where c.isDeleted=" + (int)EnmIsdeleted.ʹ���� + " and u.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and b.writeoff>0 "
                    + "and cu.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and c.CableStatus<>" + (int)EnmCableStatus.δ�깤 + " and c.CableStatus<>" + (int)EnmCableStatus.�Ѳ�� + ""
                    + condition + " order by year,month ";
                    DbParameter[] paramlist = { };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                    DataTable result = new DataTable();
                    if (tb.Rows.Count > 0)
                    {
                        //result = calActualMoney(tb, endTime);  //������ɽ��
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
        //            string sql = "select b.year as ���,b.month as �¶�,c.cablenumber as ��·����, cu.customername as �ͻ�����, "
        //            + "c.ContractType as ��·����, u.name as ��������, c.CompleteTime as �깤����,b.writeoff as ��ͬ���,b.receivable as Ӧ���˿�, "
        //            + "cm.ratio as ��ɱ���,cm.tax as ˰��, b.settlementstart as ��ʼ����, b.settlementend as ��ֹ����,b.proxy as �����, "
        //            + "c.paytype as ��������,cm.beginTime ,cm.endtime,'' as ��ɽ��  from  salebills as b left join cable as c on b.cableid = c.id "
        //            + "left join commission as cm on cm.CableId = b.cableid left join tb_User as u on cm.userid = u.id left join customer as cu on b.customerid = cu.id "
        //            + "where c.isDeleted=" + (int)EnmIsdeleted.ʹ���� + " and u.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and b.writeoff>0 "
        //            + "and cu.isDeleted =" + (int)EnmIsdeleted.ʹ���� + " and c.CableStatus<>" + (int)EnmCableStatus.δ�깤 + " and c.CableStatus<>" + (int)EnmCableStatus.�Ѳ�� + " and c.CableStatus<>" + (int)EnmCableStatus.ȡ�� + " "
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
        //                result = calActualMoney(tb,endTime);  //������ɽ��
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

        #region ���������Ч����
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="begindate">�˵����㿪ʼʱ��</param>
        /// <param name="enddate">�˵������ֹʱ��</param>
        /// <returns></returns>
        private static int calCommissionDate(Dateinterval interval, Nullable<DateTime> begindate, Nullable<DateTime> enddate)
        {           
            DateTime startinterval = interval.StartDate; //���俪ʼ����
            DateTime endinterval = interval.EndDate;  //�����ֹ����

            //Nullable<DateTime> begindate = cm.Begintime;  //��ɿ�ʼʱ��;
            //Nullable<DateTime> enddate = cm.Endtime;  //��ɽ�ֹʱ��
            int days = 0;
            if (begindate == null)
            {
                return days;
            }
            if (begindate > endinterval)
            {
                return days;
            }
            //�ж������Ч����
            if (begindate <= startinterval)
            {
                if (enddate == null || enddate >= endinterval)  //û�С���ɽ�ֹ���ڡ����ߡ���ɽ�ֹ���ڡ����ڵ��ڡ������ֹ���ڡ�
                {
                    days = calDays(startinterval, endinterval); //�á����俪ʼ���ڡ��͡������ֹ���ڡ�����������
                }
                else if (enddate < endinterval && enddate>startinterval)   //����ɽ�ֹ���ڡ����ڡ������ֹ���ڡ�
                {
                    days = calDays(startinterval, enddate.Value);   //�á������ʼ���ڡ��͡���ɽ�ֹ���ڡ�����������
                }
            }
            else if(begindate>startinterval)  //"�����ʼ����" ����"������ʼ����"
            {
                if (enddate > endinterval && begindate < endinterval)    //"��ɽ�ֹ����" ����"�����ֹ����"
                {
                    days = calDays(begindate.Value, endinterval);   //�á������ʼ���ڡ��͡������ֹ���ڡ�����������
                }
                else if (enddate <= endinterval)   //"��ɽ�ֹ����" ���ڵ�����"�����ֹ����"
                {
                    days = calDays(begindate.Value, enddate.Value);   //�á������ʼ���ڡ��͡���ɽ�ֹ���ڡ�����������
                }
                else if (enddate == null && begindate <= startinterval) //δ���á���ɽ�ֹ���ڡ����ҡ������ʼ���ڡ����ڵ��ڡ�������ʼ���ڡ�
                {
                    days = calDays(begindate.Value, endinterval);   //�á������ʼ���ڡ��͡������ֹ���ڡ�����������
                }
                else if (enddate == null && begindate > startinterval) //δ���á���ɽ�ֹ���ڡ����ҡ������ʼ���ڡ����ڵ��ڡ�������ʼ���ڡ�
                {
                    days = calDays(begindate.Value, endinterval);   //�á������ʼ���ڡ��͡������ֹ���ڡ�����������
                }
            }
            //else if (enddate<startinterval) //����ɽ�ֹ���ڡ����ڡ�������ʼ���ڡ�
            //{
            //    return days;
            //}
            //else if (begindate > endinterval) //�������ʼ���ڡ����ڡ������ֹ���ڡ�
            //{
            //    return days;
            //}
            return days;
        }
        #endregion

        #region ��ʼ��datatable��
        private static DataTable createCol()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("���", typeof(int));
            tb.Columns.Add("�¶�", typeof(int));
            tb.Columns.Add("�ͻ�", typeof(string));
            tb.Columns.Add("��·����", typeof(string));
            tb.Columns.Add("��·����", typeof(string));            
            tb.Columns.Add("��ͬ���", typeof(decimal));
            tb.Columns.Add("���ʽ", typeof(string));
            tb.Columns.Add("Ӧ�������", typeof(decimal));
            tb.Columns.Add("���˽��", typeof(decimal));
            tb.Columns.Add("������ʼ����", typeof(string));
            tb.Columns.Add("�����ֹ����", typeof(string));
            tb.Columns.Add("��������", typeof(string));
            tb.Columns.Add("�����",typeof(decimal));
            tb.Columns.Add("��ɱ���", typeof(decimal));
            tb.Columns.Add("˰��", typeof(decimal));
            tb.Columns.Add("ʵ������", typeof(int));
            tb.Columns.Add("��ɽ��", typeof(decimal));
            
            return tb;
        }
        #endregion

        #region ��������������
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate">��ʼʱ��</param>
        /// <param name="endDate">��ֹʱ��</param>
        /// <returns></returns>
        private static int calDays(DateTime startDate, DateTime endDate)
        {
            int days = new TimeSpan(startDate.Ticks).Subtract(new TimeSpan(endDate.Ticks)).Duration().Days + 1;  //����������
            return days;
        }
        #endregion

        #region ������ɽ��
        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthdays">����(����)����</param>
        /// <param name="days">ʵ������</param>
        /// <param name="money">ÿ�·���</param>
        /// <param name="ratio">�����</param>
        /// <param name="tax">˰��</param>
        /// <returns></returns>
        public static decimal calCommissionMoney(int daysInMonth, int days, decimal money, decimal ratio, decimal tax)
        {
            return Math.Round(decimal.Parse(days.ToString()) / daysInMonth * money * ratio * (1 - tax),2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthdays">����(����)����</param>
        /// <param name="days">ʵ������</param>
        /// <param name="money">ÿ�·���</param>
        /// <param name="ratio">�����</param>
        /// <param name="tax">˰��</param>
        /// <returns></returns>
        public static decimal calCommissionMoney(decimal money, decimal ratio, decimal tax)
        {
            return money * ratio * (1 - tax);
        }
        #endregion

        #region ���ݺ�ͬ�����ж��Ƿ���Ԥ����
        /// <summary>
        /// ���ݺ�ͬ�����ж��Ƿ���Ԥ����
        /// </summary>
        /// <param name="cable">��·��ͬ</param>
        /// <param name="year">�������</param>
        /// <param name="month">�����¶�</param>
        /// <returns></returns>
        public static decimal isHaveWriteoff(DateTime startDate,DateTime endDate, int paytype, decimal money, int year, int month)
        {
            //DateTime contractStart = DateTime.Parse(cable.Startdate.ToString());   //��ͬ��ʼ����
            //DateTime contractEnd = DateTime.Parse(cable.Enddate.ToString());       //��ͬ��ֹ����
            ////int paytype = cable.Paytype;
            //decimal money = cable.Money;   //��·��ͬ���
            //�ж��Ƿ���Ҫ������ʼ��
            switch (paytype)
            {
                case (int)EnmPayType.�¸�:
                    return money / 12;
                case (int)EnmPayType.����:                  
                    for (DateTime tmpDate = startDate; tmpDate < endDate; tmpDate = tmpDate.AddMonths(3))
                    {
                        if (tmpDate.Year == year && tmpDate.Month == month)
                        {
                            return money / 4;
                        }                       
                    }
                    return 0;
                case (int)EnmPayType.һ���Ը�:
                    if (startDate.Year == year && startDate.Month == month)
                    {
                        return money;
                    }
                    else
                    {
                        return 0;
                    }                  
                case (int)EnmPayType.���긶:                   
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
