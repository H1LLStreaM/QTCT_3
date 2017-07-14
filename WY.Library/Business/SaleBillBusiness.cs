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
    /// �����˵�
    /// </summary>
    public class SaleBillBusiness
    {
        public static Hashtable losths;  //ȱʧ��·������Ϣ        
        public static Hashtable errhs;  //Excel������Ϣ        
        public static List<Tmpsalebills> list;  //Excel��ʱ�˵���Ϣ
        public static List<int> delRowIndex;  //ͳ��ȱʧ���뼰�����˵���Ϣ���б�
        public static Hashtable addhs;  //�˵������·������Ϣ

        #region �жϵ�ǰ�¶��˵��Ƿ��Ѿ�����
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
                MessageHelper.ShowMessage("E999", "��ȡ"+year.ToString()+"��"+month.ToString()+"�¶��˵���������");
                return -1;
            }
        }
        #endregion

        #region ɾ��ѡ���¶ȵ������˵�
        public static bool delAllBillsInDate(int year,int month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update salebills set Isdeleted="+(int)EnmIsdeleted.��ɾ��+" where year=@year and month=@month ";
                    DbParameter[] param = { db.CreateParameter("@year", year), db.CreateParameter("@month", month) };
                    db.ExecuteNonQuery(sql,param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "ɾ��"+year.ToString()+"��"+month.ToString()+"�¶��˵���������");
                return false;
            }
        }
        #endregion

        #region ��ȡ�˵�������Ϣ
        public static void readAllExcelBills(DataTable bill)
        {
            string errorItem = "";
            for (int i = Global.row_Startline; i < bill.Rows.Count - Global.row_Startline; i++)
            {
                try
                {
                    errorItem = "Ӧ���˿�";
                    decimal receivable = Utils.NvDecimal(bill.Rows[i][Global.col_Receivable].ToString());  //Ӧ���˿�
                    errorItem = "���˽��";
                    decimal writeoff = Utils.NvDecimal(bill.Rows[i][Global.col_Writeoff].ToString());      //���˽��
                    string excalbenumber = Utils.NvStr(bill.Rows[i][Global.col_exCalbenumber].ToString());   //ԭ��·����
                    string contract = Utils.NvStr(bill.Rows[i][Global.col_contractnumber].ToString());   //��ͬ���
                    string speed = Utils.NvStr(bill.Rows[i][Global.col_Speed].ToString());   //������
                    string exspeed = Utils.NvStr(bill.Rows[i][Global.col_exSpeed].ToString());   //ԭ����
                    string nature = Utils.NvStr(bill.Rows[i][Global.col_cableNature].ToString());  //��·����
                    string range = Utils.NvStr(bill.Rows[i][Global.col_Range].ToString());   //ͨ�ŷ�Χ
                    string completedate = Utils.NvStr(bill.Rows[i][Global.col_Completedate].ToString());  //�깤����
                    string removedata = Utils.NvStr(bill.Rows[i][Global.col_Removedate].ToString());  //�������
                    string settlementstart = Utils.NvStr(bill.Rows[i][Global.col_Settlementstart].ToString());  //������ʼ����
                    string settlementend = Utils.NvStr(bill.Rows[i][Global.col_Settlementend].ToString());  //�����ֹ����
                }
                catch
                {
                    errhs.Add(i, errorItem+"����");
                    continue;
                }
            }
        }
        #endregion

        #region ����˵���Ϣ
        /// <summary>
        /// ����˵���Ϣ
        /// </summary>
        /// <param name="bill">����ĵ����˵���</param>
        public static void checkBillInfo(DataTable bill,int year,int month)
        {
            list = new List<Tmpsalebills>();
            for (int i = Global.row_Startline; i < bill.Rows.Count; i++)
            {
                string cabelNumber = Utils.NvStr(bill.Rows[i][Global.col_Calbenumber].ToString());  //��·����
                string cusName = Utils.NvStr(bill.Rows[i][Global.col_Cusname].ToString());     //�ͻ�����
                cusName = cusName.Replace("��", "(");
                cusName = cusName.Replace("��", ")");
                if (string.IsNullOrEmpty(cabelNumber) && string.IsNullOrEmpty(cusName))  //�����·���뼰�ͻ����ƶ�Ϊ�գ���ΪExcel�˵�������ĩ�в�ֹͣ��ȡ
                {
                    break;
                }
                #region ��ȡ�˵���Ϣ
                string receivable = Utils.NvStr(bill.Rows[i][Global.col_Receivable].ToString());  //Ӧ���˿�
                string writeoff = Utils.NvStr(bill.Rows[i][Global.col_Writeoff].ToString());      //���˽��
                string excalbenumber = Utils.NvStr(bill.Rows[i][Global.col_exCalbenumber].ToString());   //ԭ��·����
                string contract = Utils.NvStr(bill.Rows[i][Global.col_contractnumber].ToString());   //��ͬ���
                string speed = Utils.NvStr(bill.Rows[i][Global.col_Speed].ToString());   //������
                string exspeed = Utils.NvStr(bill.Rows[i][Global.col_exSpeed].ToString());   //ԭ����
                string nature = Utils.NvStr(bill.Rows[i][Global.col_cableNature].ToString());  //��·����
                string range = Utils.NvStr(bill.Rows[i][Global.col_Range].ToString());   //ͨ�ŷ�Χ
                string completedate = Utils.NvStr(bill.Rows[i][Global.col_Completedate].ToString());  //�깤����
                string removedata = Utils.NvStr(bill.Rows[i][Global.col_Removedate].ToString());  //�������
                string settlementstart = Utils.NvStr(bill.Rows[i][Global.col_Settlementstart].ToString());  //������ʼ����
                string settlementend = Utils.NvStr(bill.Rows[i][Global.col_Settlementend].ToString());  //�����ֹ����
                string ratio = Utils.NvStr(bill.Rows[i][Global.col_Ratio].ToString());  //�������?? ��Ҫ�ж�
                string proxy = Utils.NvStr(bill.Rows[i][Global.col_Proxy].ToString());  //�����
                string writeoffStatus = Utils.NvStr(bill.Rows[i][Global.col_WriteoffStatus].ToString());  //�������               
                Tmpsalebills tmpbill = new Tmpsalebills();  //�����˵���
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
                tmpbill.Settlementend = settlementend;  //������ʼ����
                tmpbill.Settlementstart = settlementstart;  //�����ֹ����
                tmpbill.Receivable = receivable;
                tmpbill.Writeoff = writeoff;
                tmpbill.Ratio = ratio;
                tmpbill.Proxy = proxy;
                tmpbill.Writeoffstatus = writeoffStatus;
                #endregion

                #region �жϴ�����Ϣ
                //DateTime dtStart;   //�˵�������ʼ����
                //DateTime dtEnd;     //�˵������ֹ����     
                //if (string.IsNullOrEmpty(cabelNumber) || string.IsNullOrEmpty(cusName))  //�����·���뼰�ͻ����ƶ�Ϊ�գ���ΪExcel�˵�������ĩ�в�ֹͣ��ȡ
                //{
                //    errhs.Add(i, "�ͻ������·���벻��Ϊ�ա������º˶ԡ�");
                //    tmpbill.Errinfo = "�ͻ������·���벻��Ϊ�ա������º˶ԡ�";
                //    list.Add(tmpbill);
                //    continue;
                //}
                ////�жϿͻ�������·�����Ƿ��Ѿ�¼��
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
                //            errhs.Add(i, "������ʼ���ڸ�ʽ�����������º˶ԡ�");
                //            tmpbill.Errinfo = "������ʼ���ڸ�ʽ�����������º˶ԡ�";
                //            list.Add(tmpbill);
                //            continue;
                //        }
                //        try
                //        {
                //            dtEnd = DateTime.Parse(settlementend);
                //        }
                //        catch
                //        {
                //            errhs.Add(i, "�˵������ֹ���ڸ�ʽ�����������º˶ԡ�");
                //            tmpbill.Errinfo = "�˵������ֹ���ڸ�ʽ�����������º˶ԡ�";
                //            list.Add(tmpbill);
                //            continue;
                //        }

                //        string tmp = ratio.Substring(0, ratio.Length - 1);  //ȥ�������������%��
                //        decimal tmpratio = 0;
                //        try
                //        {
                //            tmpratio = decimal.Parse(tmp);
                //        }
                //        catch
                //        {
                //            errhs.Add(i, "�˵����������ʽ������������ȷ��");  //�����·�������������Ҫ���ֱ������ѭ����һ����¼
                //            tmpbill.Errinfo = "�˵����������ʽ������������ȷ��";
                //            list.Add(tmpbill);
                //            continue;
                //        }

                //        int cusId = 0;  //�ͻ�ID
                //        int cableId = 0; //��·ID
                //        int salerid = 0;   //������ID
                //        DataTable tb = getID(cusName, cabelNumber);  //��ѯ��·ID���ͻ�ID��������ԱID
                //        if (tb.Rows.Count > 0)
                //        {
                //            cusId = Utils.NvInt(tb.Rows[0]["id"].ToString());
                //            cableId = Utils.NvInt(tb.Rows[0]["id1"].ToString());
                //            salerid = Utils.NvInt(tb.Rows[0]["userid"].ToString());
                //            if (tb.Rows[0]["startdate"].ToString() == "")
                //            {
                //                errhs.Add(i, "��·δ�鵽�깤���ڣ������º˶ԡ�");
                //                tmpbill.Errinfo = "��·δ�鵽�깤���ڣ������º˶ԡ�";
                //                list.Add(tmpbill);
                //                continue;
                //            }
                //            DateTime contractStartData = DateTime.Parse(tb.Rows[0]["startdate"].ToString()).Date;  //�깤��·�����ͬ��ʼ����
                //            if (contractStartData > dtStart)
                //            {
                //                errhs.Add(i, "�˵���������ʼ�����ڵ�·��ͬ��ʼ���ڣ������º˶ԡ�");
                //                tmpbill.Errinfo = "�˵���������ʼ�����ڵ�·��ͬ��ʼ���ڣ������º˶ԡ�";
                //                list.Add(tmpbill);
                //                continue;
                //            }
                //            //�����˵������Ա��깤¼���·���ʣ���������򱨾����������ɱ����¼�������˵����и�ֵ�ֿ�
                //            Cable cb = CableBusiness.getCalbeByCableId(cableId);
                //            if (tmpratio > cb.Writeoffratio)
                //            {
                //                errhs.Add(i, "�˵�����������ڵ�·�����趨���ʣ���鿴��Ӧ��¼���������Խ��ᱣ�浽���ݿ⡣");
                //            }
                //        }
                //        string result = isAddBillinfo(year, month, dtStart.Year, dtStart.Month, i);
                //        if (result == "����")
                //        {
                //            //�ж��Ƿ��β���
                //            DateTime starttime = DateTime.Parse(tmpbill.Settlementstart).Date;
                //            DateTime endtime = DateTime.Parse(tmpbill.Settlementend).Date;
                //            //if (isHaveBillsInfo(cusId, cableId, starttime,endtime)) //���ݽ�����ʼ�����жϵ�·�����Ƿ����ظ�
                //            //{
                //            //    tmpbill.Flag = ((int)EnmRepeat.��β�����).ToString();
                //            //}
                //            //else
                //            //{
                //            //    tmpbill.Flag = ((int)EnmRepeat.������).ToString();
                //            //}
                //            addhs.Add(i, tmpbill);  //�����·��Ϣ
                //            continue;
                //        }
                //        else if (result == "����")
                //        {
                //            continue;
                //        }
                //        getLostCable(losths, cusName, cabelNumber, i);
                //        //if (getLostCable(losths, cusName, cabelNumber, i))
                //        //{
                //        //    //list.Add(tmpbill);   //������������˵�
                //        //}
                //    }
                //    else
                //    {
                //        errhs.Add(i, "���ݵ�·���뼰�ͻ����Ʋ�ѯ������깤��Ϣ�������º˶ԡ�");
                //        tmpbill.Errinfo = "���ݵ�·���뼰�ͻ����Ʋ�ѯ������깤��Ϣ�������º˶ԡ�";
                //        list.Add(tmpbill);
                //        //delRowIndex.Add(i);
                //        continue;
                //    }
                //}
                //else
                //{
                //    errhs.Add(i, "���ݵ�·���뼰�ͻ�����û�в鵽�깤��Ϣ�������º˶ԡ�");
                //    tmpbill.Errinfo = "���ݵ�·���뼰�ͻ�����û�в鵽�깤��Ϣ�������º˶ԡ�";
                //    list.Add(tmpbill);
                //    continue;
                //}
                #endregion
                list.Add(tmpbill);
            } 


            ////�Ȼ�ȡ����ε����¶�����Ч���깤��·����
            //if(CableBusiness.getCompleteCalbe(year,month)!=null)
            //{
            //    Cable[] cb = CableBusiness.getCompleteCalbe(year,month);                
            //    for (int i = 0; i < cb.Length; i++)
            //    {                   
            //        losths.Add(cb[i].Cablenumber+cb[i].Customer.Customername, cb[i]);  //Ԥ��������깤��·���룬���ں���ͳ��ȱʧ��·������Ϣ
            //    }
            //}
            ////ѭ���˵���Ϣ�Ա���Ϣ
            //if (Global.row_Startline < 0)
            //{
            //    MessageHelper.ShowMessage("E038");
            //    return;
            //}
                   
        }
        #endregion

        #region �жϵ�·����Ϳͻ������Ƿ��Ѿ�¼��
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
                                               db.CreateParameter("@del1",(int)EnmIsdeleted.ʹ����),db.CreateParameter("@del2",(int)EnmIsdeleted.ʹ����) };

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
                                               db.CreateParameter("@del1",(int)EnmIsdeleted.ʹ����),db.CreateParameter("@del2",(int)EnmIsdeleted.ʹ����) };

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

        #region ���ݽ�����ʼ�����жϵ�·�����Ƿ����ظ�
        public static bool isHaveBillsInfo(int cusid, int cableid, DateTime start, DateTime end)
        {
            try
            {
                //string date = year.ToString() + "/" + month.ToString() + "/";
                Salebills[] bills = SalebillsDao.FindAll(new EqExpression("Settlementstart", start), new EqExpression("Settlementend",end), 
                                                         new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), new EqExpression("Cableid", cableid));
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

        #region ��ȡ�ͻ�����·ID
        public static DataTable getIDs(string cusname, string cablenumber)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Select ch.customerid as �ͻ�id,b.cableid as ��·id,cb.userid as ���������� "
                               + "FROM customerhistory AS ch INNER JOIN cablehistory AS b ON ch.customerid = b.customerid  "
                               +" LEFT JOIN customer AS c ON c.id=ch.customerid "
                               +" LEFT JOIN cable AS cb ON cb.id=b.cableid "
                               + "where ch.CustomerName=@cusname and b.CableNumber=@cablenumber and ch.Isdeleted=@del1 and b.Isdeleted=@del2 "
                               + "AND c.Isdeleted=0 AND cb.isdeleted=0 "
                               + "and b.cablestatus<>@cablestatus and b.cablestatus<>@cablestatus2 ";
                    DbParameter[] paramlist = { db.CreateParameter("@cusname", cusname), db.CreateParameter("@cablenumber", cablenumber),
                                                db.CreateParameter("@del1",(int)EnmIsdeleted.ʹ����),db.CreateParameter("@del2",(int)EnmIsdeleted.ʹ����), 
                                                db.CreateParameter("@cablestatus",(int)EnmCableStatus.ȡ��),db.CreateParameter("@cablestatus2",(int)EnmCableStatus.δ�깤)};

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

        #region ����ȱʧ��·�������
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

        #region �ж��Ƿ�Ϊ�����˵���Ϣ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">�������</param>
        /// <param name="month">�����¶�</param>
        /// <param name="tmpyear">�˵��������</param>
        /// <param name="tmpmonth">�˵������¶�</param>
        /// <param name="rowindex">�б�</param>
        /// <returns></returns>
        private static string isAddBillinfo(int year, int month, int tmpyear, int tmpmonth,int rowindex)
        {
            if (year > tmpyear)
            {
                return "����";
            }
            else if (year == tmpyear)
            {
                if (month > tmpmonth)
                {
                    return "����";
                }
                else if (month == tmpmonth)
                {
                    return "����";
                }
                else
                {
                    errhs.Add(rowindex, "��·�����˵�����ʱ����ڵ����趨ʱ�䣬�����º˶�");
                    return "����";
                }
            }
            else
            {
                errhs.Add(rowindex, "��·�����˵�����ʱ����ڵ����趨ʱ�䣬�����º˶�");
                return "����";
            }
        }
        #endregion

        #region ���������˵���Ϣ
        public static void saveBill(Salebills bill)
        {
            try
            {
                bill.Save();               
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "�˵���Ϣ����ʧ�ܡ�");
            }
        }
        #endregion

        #region ���油���˵���Ϣ
        public static void saveAddBill(Salebills bill)
        {
            try
            {
                //���ݽ�����ʼ�����жϵ�·�����Ƿ����ظ�
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

        #region ��ѯ�˵���Ϣ
        public static Salebills getBillByCondition(int cusid,int cableid,int month,int year)
        {
            try
            {
                Salebills bills = SalebillsDao.FindFirst(new EqExpression("Cableid", cableid), new EqExpression("Customerid", cusid), new EqExpression("Year", year),
                                    new EqExpression("Month", month), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
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
                
                if (!string.IsNullOrEmpty(cable))  //���ݵ�·����ģ����ѯ
                {
                    cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)),
                                               new LikeExpression("Cablenumber", "%" + cable + "%"));
                }
                else //����ȫ����·����
                {
                    cb = CableDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.δ�깤)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.�Ѳ��)),
                                               new NotExpression(new EqExpression("Cablestatus", (int)EnmCableStatus.ȡ��)));                                              
                }
                if (cb != null || cb.Length > 0)
                {
                    object[] cableids = new object[cb.Length];
                    for (int i = 0; i < cb.Length; i++)
                    {
                        cableids[i]=cb[i].Id;
                    }
                    InExpression inexpress = new InExpression("Cableid", cableids);  //�����е�·������Ϊ��ѯ����
                    
                    if (!string.IsNullOrEmpty(customername))
                    {
                        cus = CustomerBusiness.findCustomerByName(customername);
                        if (cus != null)
                        {
                            EqExpression cuseqex = new EqExpression("Customerid", cus.Id);
                            //ֻ�����������Ԥ������˵���Ϣ
                            bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                                         new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.��������), new EqExpression("Flag", (int)EnmWriteOffFlag.Ԥ����)),
                                                         new EqExpression("Year", year),
                                                         new EqExpression("Month", month),
                                                         inexpress, cuseqex);
                        }
                        else
                        {
                            //ֻ�����������Ԥ������˵���Ϣ
                            bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                                         new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.��������), new EqExpression("Flag", (int)EnmWriteOffFlag.Ԥ����)),
                                                         new EqExpression("Year", year),
                                                         new EqExpression("Month", month),
                                                         inexpress);
                        }
                    }
                    else
                    {
                        //ֻ�����������Ԥ�����Excel�˵���Ϣ
                        bills = SalebillsDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����),
                                                     new OrExpression(new EqExpression("Flag", (int)EnmWriteOffFlag.��������), new EqExpression("Flag", (int)EnmWriteOffFlag.Ԥ����)),
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

        #region ������ҵid��ѯ�Ƿ��е�����˵�
        public static int getCountByCustomerId(int customerid)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select count(1) from salebills where customerid=@cusid and isdeleted=@del";
                    DbParameter[] paramlist = { db.CreateParameter("@cusid", customerid), db.CreateParameter("@del", (int)EnmIsdeleted.ʹ����) };
                    return int.Parse(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
                                               
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region ��ѯ��˾��ҵ����ҵ��
        public static DataTable getCompanyTrendcy(int startYear, int endYear, int startMonth, int endMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select  sum(writeoff) as amount,CONCAT(year,'��',month,'��') as yearmonth" +
                                            " from SaleBills " +
                                            " where (year>@syear or (year=@syear and month>=@smonth)) and (year<@eyear or (year=@eyear and month<=@emonth))" +
                                            " and isDeleted=@del " +
                                            " group by CONCAT(year,'��',month,'��')" +
                                            " order by year,month ";
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.ʹ����),
                                               db.CreateParameter("@eyear",endYear),db.CreateParameter("@emonth",endMonth) };

                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "��������ͼ����ʧ�ܡ�");
                    return new DataTable();
                }

            }

        }
        #endregion     



        #region ������Ӷ��ҵ��ģ��
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
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.ʹ����),
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
                    DbParameter[] paramlist = { db.CreateParameter("@syear", startYear), db.CreateParameter("@smonth", startMonth),db.CreateParameter("@del",(int)EnmIsdeleted.ʹ����),
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
