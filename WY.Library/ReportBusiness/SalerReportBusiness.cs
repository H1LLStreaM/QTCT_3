using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Aspose.Cells;
using WY.Common.Framework;
using WY.Library.Model;
using WY.Library.Business;
using WY.Common.Utility;
using System.Windows.Forms;
using WY.Common.Message;
using WY.Common;

namespace WY.Library.ReportBusiness
{
    public class SalerReportBusiness
    {
        private const int SALESDATA_STARTLINE_INDEX = 4;

        private Worksheet sheet;

        private Style dataStyle;

        public SalerReportBusiness()
        {
            Workbook book = new Workbook();
            book.Open(Application.StartupPath + "/templet/salers_report_templet.xls");
            sheet = book.Worksheets[0];
            dataStyle = sheet.Cells[4, 1].Style;
        }

        public void saveReport(string savePath,int year,int month)
        {
            Workbook wk = new Workbook();
            TB_User[] sales = UserBusiness.getAllSalersAndWrite();  //��ȡ���������������깤¼����Ϣ
            if (Global.g_usergroupid != (int)EnmUserRole.���� && Global.g_usergroupid != (int)EnmUserRole.ȫ�� &&  Global.g_usergroupid != (int)EnmUserRole.�����ܼ�)
            {
                TB_User u = UserBusiness.findUserById(Global.g_userid);
                List<TB_User> list = new List<TB_User>();
                list.Add(u);
                sales = list.ToArray();
            }
            string str = year.ToString() + "-" + month.ToString() + "-01";
            DateTime startdate = DateTime.Parse(str).Date;
            DateTime enddate = DateTime.Parse(str).AddMonths(1).AddDays(-1).Date;
            for (int i = 0; i < sales.Length; i++)
            {
                DataTable salesData = ScoreBusiness.makeActualScore(startdate,enddate,"",sales[i].Id,"");
                if (salesData.Rows.Count > 0)
                {
                    writeSalesReport(wk, i, salesData, year, month, sales[i].USER_NAME);
                    try
                    {
                        wk.Password = DES.Decode(sales[i].PASSWORD, Global.DB_PWDKEY);
                        wk.Save(savePath + "//" + sales[i].USER_NAME + " "+ year + "-" + month + "�¶�����嵥1.xls");
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowMessage("E999", "����" + ex.Message + "\n���ļ�����ʧ�ܣ������ļ��Խ�����������ѡĿ¼��");
                    }
                }
            }
            MessageHelper.ShowMessage("I007");
        }

        private void writeSalesReport(Workbook wk, int sheetIndex, DataTable salesData, int year, int month,string logname)
        {
            //Worksheet salesSheet = wk.Worksheets[0];
            //salesSheet.Copy(this.sheet);
            Worksheet salesSheet = wk.Worksheets[0];
            salesSheet.Copy(this.sheet);
            salesSheet.Cells[1, 7].PutValue(year.ToString() + "��" + month + "�¶�");
            int i = 0;
            decimal totalSalesAmount = 0;
            if (salesData.Rows.Count > 0)
            {
                salesSheet.Cells[1, 2].PutValue(logname);
                for (i = 0; i < salesData.Rows.Count; i++)
                {
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 1].PutValue(salesData.Rows[i]["�ͻ�"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 1].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 2].PutValue(salesData.Rows[i]["��·����"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 2].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 3].PutValue(DateTime.Parse(salesData.Rows[i]["������ʼ����"].ToString()).ToString("yyyy-MM-dd"));
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 3].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 4].PutValue(DateTime.Parse(salesData.Rows[i]["�����ֹ����"].ToString()).ToString("yyyy-MM-dd"));
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 4].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 5].PutValue(salesData.Rows[i]["Ӧ�������"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 5].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 6].PutValue(salesData.Rows[i]["���˽��"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 6].Style.Copy(this.dataStyle);
                    decimal ratio = Utils.NvDecimal(salesData.Rows[i]["��ɱ���"].ToString());
                    decimal tax = Utils.NvDecimal(salesData.Rows[i]["˰��"].ToString());
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 7].PutValue(ratio);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 7].Style.Copy(this.dataStyle);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 8].PutValue(tax);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 8].Style.Copy(this.dataStyle);

                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 9].PutValue(salesData.Rows[i]["ʵ������"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 9].Style.Copy(this.dataStyle);

                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 10].PutValue(salesData.Rows[i]["��ɽ��"]);
                    salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX, 10].Style.Copy(this.dataStyle);
                    totalSalesAmount += Utils.NvDecimal(salesData.Rows[i]["��ɽ��"]);
                    //totalCommission += Math.Round(money * ratio / 100 - money * ratio / 100 * tax / 100, 2);
                }
                salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 9].PutValue("��ɺϼ�:");
                salesSheet.Cells[i + SALESDATA_STARTLINE_INDEX + 1, 10].PutValue(totalSalesAmount);
            }
        }
    }
}
